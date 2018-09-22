Public Class ChannelChatWriter
    Inherits WriterBase

    ReadOnly Property ChannelName As String

    Sub New(connection As Connection, channel_name As String)
        MyBase.New(connection)
        ChannelName = channel_name
    End Sub

    Overrides Sub ProcessAndSend(text As String)

        Dim tokenizer As New Tokenizer(text)

        ' If it's not a command, then it's a message
        If Not tokenizer.Skip("/c") Then
            Connection.SendLine(String.Format("PRIVMSG {0} :{1}", ChannelName, text))
            Exit Sub
        End If

        Dim command = tokenizer.ReadWord().ToUpper
        Select Case command

            Case "INVITE" ' INVITE <username> [channel]

                Dim username = tokenizer.ReadWord()

                Dim channel_prefixes = Connection.ServerLimits.ChanTypes
                If channel_prefixes.Contains(tokenizer.Peek()) Then
                    MyBase.ProcessAndSend(text)
                    Exit Sub
                End If

                If (username Is Nothing) OrElse (Not tokenizer.IsEnd()) Then
                    Throw New SyntaxError("INVITE expects one or two parameters.")
                End If

                Connection.SendLine(String.Format("INVITE {0} {1}", username, ChannelName))

            Case "KICK" ' KICK [channel] <user> [comment]

                Dim channel_prefixes = Connection.ServerLimits.ChanTypes
                If channel_prefixes.Contains(tokenizer.Peek()) Then
                    MyBase.ProcessAndSend(text)
                    Exit Sub
                End If

                Dim username = tokenizer.ReadWord()
                Dim comment = tokenizer.ReadRemaining()

                If username Is Nothing Then
                    Throw New SyntaxError("KICK expects one to three parameters.")
                End If

                If comment Is Nothing Then
                    Connection.SendLine(String.Format("KICK {0} {1}", ChannelName, username))
                Else
                    Connection.SendLine(String.Format("KICK {0} {1} :{2}", ChannelName, username, comment))
                End If

            Case "ME" ' ME [action]

                Dim action_text = tokenizer.ReadRemaining()

                If action_text Is Nothing Then
                    Throw New SyntaxError("ME expects a parameter.")
                End If

                Connection.SendCTCPResponse(ChannelName, "ACTION", action_text)

            Case "PART" ' PART [channel] [message]

                Dim channel_prefixes = Connection.ServerLimits.ChanTypes
                If channel_prefixes.Contains(tokenizer.Peek()) Then
                    MyBase.ProcessAndSend(text)
                    Exit Sub
                End If

                Dim message = tokenizer.ReadRemaining()
                If message Is Nothing Then
                    Connection.SendLine("PART " & ChannelName)
                Else
                    Connection.SendLine(String.Format("PART {0} :{1}", ChannelName, message))
                End If

            Case "SAY" ' SAY <message>

                Dim message = If(tokenizer.ReadRemaining(), "")
                Connection.SendLine(String.Format("PRIVMSG {0} :{1}", ChannelName, message))

            Case "TOPIC" ' TOPIC [channel] [message]

                Dim channel_prefixes = Connection.ServerLimits.ChanTypes
                If channel_prefixes.Contains(tokenizer.Peek()) Then
                    MyBase.ProcessAndSend(text)
                    Exit Sub
                End If

                Dim message = tokenizer.ReadRemaining()
                If message Is Nothing Then
                    Connection.SendLine("TOPIC " & ChannelName)
                Else
                    Connection.SendLine(String.Format("TOPIC {0} :{1}", ChannelName, message))
                End If

            Case Else

                MyBase.ProcessAndSend(text)

        End Select

    End Sub

End Class
