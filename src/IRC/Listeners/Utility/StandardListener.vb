'
' Handles auto-responding to some commands and notifications of basic events.
'
Imports IRC

Public Class StandardListener
    Inherits ListenerBase


    Delegate Sub OnChannelJoinedHandler(channel_name As String)
    Delegate Sub OnChannelPartedHandler(channel_name As String)
    Property OnChannelJoined As OnChannelJoinedHandler
    Property OnChannelParted As OnChannelPartedHandler

    ' The message will not be sent to new UserListeners, so handle it here.
    Delegate Sub OnUserMessageHandler(message As Message)
    Property OnUserMessage As OnUserMessageHandler


    ' These are sent in response to CTCP requests
    Property ClientVersion = "QuickIRC v0.2"
    Property ClientSource = "https://github.com/ComputerGhost/QuickIRC"


    Overrides Sub HandleMessageReceived(ByRef message As Message)
        If Not message.IsValid Then
            Exit Sub
        End If

        Select Case message.Verb

            Case "001" ' <sender> 001 <my_nickname> <message>

                Dim nickname = message.Parameters(0)
                Connection.Nickname = nickname

            Case "JOIN" ' <sender> JOIN <channel>

                Dim channel = message.Parameters(0)
                If message.Source.Name = Connection.Nickname Then
                    OnChannelJoined?.Invoke(channel)
                End If

            Case "NICK" ' <sender> NICK <new_nick>

                Dim new_nick = message.Parameters(0)

                If message.Source.Name = Connection.Nickname Then
                    Connection.Nickname = new_nick
                    Connection.UserHost = message.Source.Raw
                End If

            Case "NOTICE" ' <sender> NOTICE <target> <text>

                Dim target = message.Parameters(0)
                If target = Connection.Nickname Then
                    OnUserMessage?.Invoke(message)
                End If

            Case "PART" ' <sender> PART <channel> [comment]

                Dim channel = message.Parameters(0)
                If message.Source.Name = Connection.Nickname Then
                    OnChannelParted?.Invoke(channel)
                End If

            Case "PING" ' <sender> PING <server> [server0]

                Connection.SendLine("PONG " & message.Parameters(0))

            Case "PRIVMSG" ' <sender> PRIVMSG <target> <text>

                Dim target = message.Parameters(0)
                Dim text = message.Parameters(1)

                If target = Connection.Nickname Then
                    OnUserMessage?.Invoke(message)
                End If

                If text.StartsWith(ChrW(1)) Then
                    HandleCTCPRequest(message.Source.Name, target, text)
                End If

        End Select

    End Sub

    Public Overrides Sub HandleMessageSent(ByRef message As Message)
        If Not message.IsValid Then
            Exit Sub
        End If
        Select Case message.Verb
            Case "NOTICE", "PRIVMSG"
                OnUserMessage?.Invoke(message)
        End Select
    End Sub


#Region "Internals"

    Private Sub HandleCTCPRequest(sender_nick As String, target As String, text As String)

        Dim tokenizer As New Tokenizer(text)
        Debug.Assert(tokenizer.Skip(ChrW(1)))

        Dim command = tokenizer.ReadWord().ToUpper().TrimEnd(ChrW(1))
        Select Case command

            Case "CLIENTINFO"

                Dim listing = String.Join(" ", {
                    "ACTION", "CLIENTINFO", "PING",
                    "SOURCE", "VERSION"})
                Connection.SendCTCPResponse(sender_nick, "CLIENTINFO", listing)

            Case "PING"

                Dim timestamp = tokenizer.ReadRemaining()
                If timestamp.EndsWith(ChrW(1)) Then
                    timestamp = timestamp.Substring(0, timestamp.Length - 1)
                End If
                Connection.SendCTCPResponse(sender_nick, "PING", timestamp)

            Case "SOURCE"

                If String.IsNullOrWhiteSpace(ClientSource) Then
                    Throw New ExpectationFailed("ClientSource must be set.")
                End If
                Connection.SendCTCPResponse(sender_nick, "SOURCE", ClientSource)

            Case "VERSION"

                If String.IsNullOrWhiteSpace(ClientVersion) Then
                    Throw New ExpectationFailed("ClientVersion must be set.")
                End If
                Connection.SendCTCPResponse(sender_nick, "VERSION", ClientVersion)

        End Select

    End Sub

#End Region

End Class
