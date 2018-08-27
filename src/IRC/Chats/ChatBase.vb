Public Class ChatBase

    Delegate Sub ConnectionOpenedDelegate()
    Delegate Sub ConnectionFailedDelegate(ex As Exception)
    Delegate Sub ConnectionClosedDelegate()
    Property OnConnectionOpened As ConnectionOpenedDelegate
    Property OnConnectionFailed As ConnectionFailedDelegate
    Property OnConnectionClosed As ConnectionClosedDelegate

    Delegate Sub MessageReceivedDelegate(message As Message)
    Delegate Sub MessageSentDelegate(message As Message)
    Property OnMessageReceived As MessageReceivedDelegate
    Property OnMessageSent As MessageSentDelegate

    Delegate Sub ExceptionHappenedDelegate(ex As Exception)
    Property OnExceptionHappened As ExceptionHappenedDelegate


    Overridable Sub ProcessAndSend(text As String)

        Dim tokenizer As New Tokenizer(Connection.ServerLimits, text)

        If Not tokenizer.Skip("/"c) Then
            Throw New ExpectationFailed("Command expected.")
        End If
        Dim command = tokenizer.ReadCommand()

        ' Whitelisted commands are sent on to the server.
        If ForwardedCommands.ContainsKey(command) Then

            Dim params As New List(Of String)

            Dim middles = ForwardedCommands(command)
            For i = 1 To middles.Max
                Dim param = tokenizer.ReadWord()
                If param Is Nothing Then
                    If i > middles.Min Then Exit For
                    Throw New SyntaxError("Command expects more parameters.")
                End If
                params.Add(param)
            Next

            Dim trailing = tokenizer.ReadRemaining()
            If trailing IsNot Nothing Then
                params.Add(trailing)
            End If

            Connection.SendMessage(New Message With {
                .Verb = command,
                .Parameters = params})

            Exit Sub
        End If

        ' These commands require special handling
        Select Case command

            Case "CTCP" ' CTCP <target> <command> [parameters]

                Dim target = tokenizer.ReadTarget()
                Dim ctcp = tokenizer.ReadCommand()
                If (target Is Nothing) OrElse (ctcp Is Nothing) Then
                    Throw New SyntaxError("CTCP expects at least two parameters.")
                End If

                ' Whitelisted CTCP commands are sent on the server
                If ForwardedCTCP.Contains(ctcp) Then
                    Connection.SendCTCPRequest(target, ctcp, tokenizer.ReadRemaining())
                    Exit Sub
                End If

                ' Others need some special handling
                Select Case command
                    Case "PING"
                        Connection.SendCTCPRequest(target, "PING", DateTime.UtcNow.Ticks)
                    Case Else
                        Throw New NotImplementedException("CTCP " & ctcp)
                End Select

            Case "MSG" ' MSG <target> <message>

                Dim target = tokenizer.ReadTarget()
                Dim message = tokenizer.ReadRemaining()
                If (target Is Nothing) OrElse (message Is Nothing) Then
                    Throw New SyntaxError("MSG expects two parameters.")
                End If

                Connection.SendLine(String.Format("PRIVMSG {0} :{1}", target, message))


            Case "OPER" ' OPER <username> <password>

                Dim username = tokenizer.ReadWord()
                Dim password = tokenizer.ReadWord()
                If (username Is Nothing) OrElse (password Is Nothing) OrElse (Not tokenizer.IsEnd()) Then
                    Throw New SyntaxError("OPER expects two parameters.")
                End If

                ' Send the correct, log the redacted
                Connection.SendLine(String.Format("OPER {0} {1}", username, password), False)
                Connection.InjectSentLine(String.Format("OPER {0} [redacted]", username))

            Case "PASS" ' PASS <password>

                Dim password = tokenizer.ReadWord()
                If (password Is Nothing) OrElse (Not tokenizer.IsEnd()) Then
                    Throw New SyntaxError("PASS expects one parameter.")
                End If

                ' Send the correct, log the redacted
                Connection.SendLine("PASS " & password, False)
                Connection.InjectSentLine("PASS [redacted]")

            Case "RAW" ' RAW <message>

                Connection.SendLine(tokenizer.ReadRemaining())

            Case Else

                Throw New NotImplementedException(command)

        End Select

    End Sub


    '
    ' These can be overridden to receive the events.
    '

    Protected Friend Overridable Sub HandleConnectionOpened()
        OnConnectionOpened?.Invoke()
    End Sub

    Protected Friend Overridable Sub HandleConnectionFailed(ex As Exception)
        OnConnectionFailed?.Invoke(ex)
    End Sub

    Protected Friend Overridable Sub HandleConnectionClosed()
        OnConnectionClosed?.Invoke()
    End Sub

    Protected Friend Overridable Sub HandleExceptionHappened(ex As Exception)
        OnExceptionHappened?.Invoke(ex)
    End Sub

    Protected Friend Overridable Sub HandleMessageReceived(message As Message)
        OnMessageReceived?.Invoke(message)
    End Sub

    Protected Friend Overridable Sub HandleMessageSent(message As Message)
        OnMessageSent?.Invoke(message)
    End Sub


#Region "Internals"

    ' This should only be set from the Connection class. It will have a value
    ' after registering with a connection.
    Protected ReadOnly Property Connection As Connection = Nothing

    ' This should only be called from the Connection class
    Friend Sub Register(Connection As Connection)
        If Me.Connection IsNot Nothing Then
            Throw New ExpectationFailed("Chat cannot be re-registered with another Connection.")
        End If
        Me._Connection = Connection
    End Sub

#End Region

#Region "Data"

    ' Number of middle params, i.e. not including the last text param
    Private Structure MiddleInfo

        Property Min As Integer
        Property Max As Integer

        WriteOnly Property Count As Integer
            Set(value As Integer)
                Min = value
                Max = value
            End Set
        End Property

    End Structure

    ' These whitelisted commands are forwarded straight to the server
    Private Shared ForwardedCommands As New Dictionary(Of String, MiddleInfo) From {
        {"ADMIN", New MiddleInfo With {.Min = 0, .Max = 1}},    ' ADMIN [server]
        {"AWAY", New MiddleInfo With {.Count = 0}},             ' AWAY [message]
        {"CONNECT", New MiddleInfo With {.Min = 1, .Max = 3}},  ' CONNECT <server> [port [remote]]
        {"DIE", New MiddleInfo With {.Count = 0}},              ' DIE
        {"INFO", New MiddleInfo With {.Min = 0, .Max = 1}},     ' INFO [server]
        {"INVITE", New MiddleInfo With {.Count = 2}},           ' INVITE <username> <channel>
        {"ISON", New MiddleInfo With {.Min = 1, .Max = 177}},   ' ISON <nicknames>
        {"JOIN", New MiddleInfo With {.Min = 1, .Max = 2}},     ' JOIN <channels> [keys]
        {"KICK", New MiddleInfo With {.Count = 2}},             ' KICK <channel> <user> [comment]
        {"KILL", New MiddleInfo With {.Count = 1}},             ' KILL <nick> <comment>
        {"LINKS", New MiddleInfo With {.Min = 0, .Max = 2}},    ' LINKS [[remote] mask]
        {"LIST", New MiddleInfo With {.Min = 0, .Max = 2}},     ' LIST [channels [server]]
        {"LUSERS", New MiddleInfo With {.Min = 0, .Max = 2}},   ' LUSERS [mask [target]]
        {"MODE", New MiddleInfo With {.Min = 1, .Max = 7}},     ' MODE <target> <lots-of-params>
        {"MOTD", New MiddleInfo With {.Min = 0, .Max = 1}},     ' MOTD <server>
        {"NAMES", New MiddleInfo With {.Count = 1}},            ' NAMES <channels>
        {"NICK", New MiddleInfo With {.Min = 1, .Max = 1}},     ' NICK <nickname> [hopcount]
        {"NOTICE", New MiddleInfo With {.Count = 1}},           ' NOTICE <target> <text>
        {"PART", New MiddleInfo With {.Count = 1}},             ' PART <channels> [reason]
        {"PONG", New MiddleInfo With {.Min = 1, .Max = 2}},     ' PONG <daemon> [daemon]
        {"PRIVMSG", New MiddleInfo With {.Count = 1}},          ' PRIVMSG <targets> <text>
        {"REHASH", New MiddleInfo With {.Count = 0}},           ' REHASH
        {"RESTART", New MiddleInfo With {.Count = 0}},          ' RESTART
        {"QUIT", New MiddleInfo With {.Min = 0, .Max = 0}},     ' QUIT [reason]
        {"SERVICE", New MiddleInfo With {.Count = 5}},          ' SERVICE <nick> <res> <dist> <type> <res> <info>
        {"SERVLIST", New MiddleInfo With {.Min = 0, .Max = 2}}, ' SERVLIST [mask [type]]
        {"SQUERY", New MiddleInfo With {.Count = 1}},           ' SQUERY <service> <text>
        {"SQUIT", New MiddleInfo With {.Count = 2}},            ' SQUIT <server> <comment>
        {"SUMMON", New MiddleInfo With {.Min = 1, .Max = 3}},   ' SUMMON <user> [server [channel]]
        {"STATS", New MiddleInfo With {.Min = 0, .Max = 2}},    ' STATS [query [server]]
        {"TIME", New MiddleInfo With {.Min = 0, .Max = 1}},     ' TIME [server]
        {"TOPIC", New MiddleInfo With {.Count = 1}},            ' TOPIC <channel> [topic]
        {"TRACE", New MiddleInfo With {.Min = 0, .Max = 1}},    ' TRACE [server]
        {"USER", New MiddleInfo With {.Count = 3}},             ' USER <username> <varies> <varies> <realname>
        {"USERHOST", New MiddleInfo With {.Min = 1, .Max = 5}}, ' USERHOST <nicknames>
        {"USERS", New MiddleInfo With {.Min = 0, .Max = 1}},    ' USERS [server]
        {"VERSION", New MiddleInfo With {.Min = 0, .Max = 1}},  ' VERSION [server]
        {"WALLOPS", New MiddleInfo With {.Count = 0}},          ' WALLOPS <text>
        {"WHO", New MiddleInfo With {.Min = 0, .Max = 2}},      ' WHO [<name> ["o"]]
        {"WHOIS", New MiddleInfo With {.Min = 1, .Max = 2}},    ' WHOIS [server] <masks>
        {"WHOWAS", New MiddleInfo With {.Min = 1, .Max = 3}},   ' WHOWAS <nick> [count [server]]
        {"end-of-list", Nothing}
        }

    ' These whitelisted CTCP commands are forwarded straight to the target
    Dim ForwardedCTCP As New List(Of String) From {
        "ACTION",
        "CLIENTINFO",
        "FINGER",
        "SOURCE",
        "USERINFO",
        "TIME",
        "VERSION"}

#End Region

End Class
