Public Class WriterBase

    Protected ReadOnly Property Connection As Connection

    Sub New(connection As Connection)
        Me._Connection = connection
    End Sub

    ' If parameter starts with "/", it is a command. Otherwise it's text.
    ' Override this in child classes with context-specific stuff.
    Overridable Sub ProcessAndSend(text As String)

        Dim tokenizer As New Tokenizer(text)
        If Not tokenizer.Skip("/"c) Then
            Throw New ExpectationFailed("Command expected.")
        End If

        Dim command = tokenizer.ReadWord.ToUpper()
        If ForwardedCommands.ContainsKey(command) Then
            ProcessForwarded(command, tokenizer)
        Else
            ProcessSpecial(command, tokenizer)
        End If

    End Sub

#Region "Internals"

    ' Whitelisted commands require minimal processing
    Private Sub ProcessForwarded(command As String, tokenizer As Tokenizer)
        Debug.Assert(ForwardedCommands.ContainsKey(command))

        Dim message As New Message With {.Verb = command}

        ' Middle params, which are single words
        Dim middles = ForwardedCommands(command)
        For i = 1 To middles.Max
            Dim param = tokenizer.ReadWord()
            If param IsNot Nothing Then
                message.Parameters.Add(param)
            ElseIf i > middles.Min Then
                Exit For
            Else
                Throw New SyntaxError(String.Format(
                    "{0} requires more parameters.", command))
            End If
        Next

        ' Remaining is text param, which can have multiple words.
        Dim trailing = tokenizer.ReadRemaining()
        If trailing IsNot Nothing Then
            message.Parameters.Add(trailing)
        End If

        Connection.SendMessage(message)

    End Sub

    ' These commands require special processing
    Private Sub ProcessSpecial(command As String, tokenizer As Tokenizer)
        Select Case command

            Case "CTCP" ' CTCP <target> <command> [parameters]

                Dim target = tokenizer.ReadWord()
                Dim ctcp = tokenizer.ReadWord().ToUpper()
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

                Dim target = tokenizer.ReadWord()
                Dim message = tokenizer.ReadRemaining()
                If (target Is Nothing) OrElse (message Is Nothing) Then
                    Throw New SyntaxError("MSG expects two parameters.")
                End If

                Connection.SendLine(String.Format("PRIVMSG {0} :{1}", target, message))

            Case "RAW" ' RAW <message>

                Connection.SendLine(tokenizer.ReadRemaining())

            Case Else

                Throw New NotImplementedException(String.Format(
                    "{0} is not a recognized command.", command))

        End Select
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
        {"OPER", New MiddleInfo With {.Count = 2}},             ' OPER <username> <password>
        {"PART", New MiddleInfo With {.Count = 1}},             ' PART <channels> [reason]
        {"PASS", New MiddleInfo With {.Count = 1}},             ' PASS <password>
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
