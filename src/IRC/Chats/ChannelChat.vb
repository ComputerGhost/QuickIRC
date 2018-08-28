Imports Algorithms

'
' Handles commands within the context of a channel.
'
' NOTE: It is assumed that incoming commands have the correct syntax. Use a 
' StandardHandler to ensure that this prerequisite is met.
'
Public Class ChannelChat
    Inherits ChatBase


    Delegate Sub UsersUpdatedDelegate(nicknames As SortedSet(Of String))
    Delegate Sub UserJoinedDelegate(nickname As String)
    Delegate Sub UserChangedDelegate(old_nick As String, new_nick As String)
    Delegate Sub UserPartedDelegate(nickname As String)
    Property OnUsersUpdated As UsersUpdatedDelegate
    Property OnUserJoined As UserJoinedDelegate
    Property OnUserChanged As UserChangedDelegate
    Property OnUserParted As UserPartedDelegate

    Delegate Sub TopicChangedDelegate(Text As String)
    Property OnTopicChanged As TopicChangedDelegate


    Property ChannelName As String
    Property Topic As String

    Property Users As New SortedSet(Of String)


    Sub New(channel_name As String)
        MyBase.New()
        ChannelName = channel_name
    End Sub


    Public Overrides Sub ProcessAndSend(text As String)

        Dim tokenizer As New Tokenizer(Connection.ServerLimits, text)

        ' If it's not a command, then it's a message
        If Not tokenizer.Skip("/c") Then
            Connection.SendLine(String.Format("PRIVMSG {0} :{1}", ChannelName, text))
            Exit Sub
        End If

        Dim command = tokenizer.ReadCommand()
        Select Case command

            Case "INVITE" ' INVITE <username> [channel]

                Dim username = tokenizer.ReadWord()

                If tokenizer.IsChannel() Then
                    MyBase.ProcessAndSend(text)
                    Exit Sub
                End If

                If (username Is Nothing) OrElse (Not tokenizer.IsEnd()) Then
                    Throw New SyntaxError("INVITE expects one or two parameters.")
                End If

                Connection.SendLine(String.Format("INVITE {0} {1}", username, ChannelName))

            Case "KICK" ' KICK [channel] <user> [comment]

                If tokenizer.IsChannel() Then
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

                If tokenizer.IsChannel() Then
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

                If tokenizer.IsChannel() Then
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

#Region "Internals"

    Private Function ShouldProcess(message As Message) As Boolean

        ' Commands that have channel as a target in #1 parameter
        Static TargetCommands1 As New HashSet(Of String) From {
            "JOIN", "MODE", "NOTICE", "PART", "PRIVMSG", "TOPIC"}

        ' Commands that have a channel as target in #2 parameter
        Static TargetCommands2 As New HashSet(Of String) From {
            "328", "332", "333", "366"}

        ' Commands from a sender in the channel
        Static SenderCommands As New HashSet(Of String) From {
            "AWAY", "NICK"}

        ' Other commands we're interested in
        Static OtherCommands As New HashSet(Of String) From {
            "353"}

        Dim command = message.Verb

        If TargetCommands1.Contains(command) Then
            Return (ChannelName = message.Parameters(0))
        ElseIf TargetCommands2.Contains(command) Then
            Return (ChannelName = message.Parameters(1))
        ElseIf SenderCommands.Contains(command) Then
            Using lock As New ThreadLock(Users)
                Return Users.Contains(message.Source.Name)
            End Using
        Else
            Return OtherCommands.Contains(command)
        End If

    End Function

    Protected Friend Overrides Sub HandleMessageReceived(message As Message)

        If Not ShouldProcess(message) Then
            Exit Sub
        End If

        Dim sender = message.Source.Name

        Select Case message.Verb

            Case "332" ' <sender> 332 <my_nick> <channel> <topic>

                If ChannelName <> message.Parameters(1) Then
                    Exit Sub
                End If

                Topic = message.Parameters(2)
                OnTopicChanged?.Invoke(Topic)

            Case "353" ' <sender> 353 <me> [type] <channel> <users>

                If ChannelName <> message.Parameters(1) And ChannelName <> message.Parameters(2) Then
                    Exit Sub
                End If

                Dim tokenizer As New Tokenizer(Connection.ServerLimits, message.Parameters.Last())
                Using lock As New ThreadLock(Users)
                    While Not tokenizer.IsEnd
                        Dim user = tokenizer.ReadWord()
                        If user IsNot Nothing Then
                            Users.Add(user)
                        End If
                    End While
                    OnUsersUpdated?.Invoke(Users)
                End Using

            Case "AWAY" ' <sender> AWAY [message]

                Using lock As New ThreadLock(Users)
                    If Not Users.Contains(sender) Then
                        Exit Sub
                    End If
                End Using

            Case "JOIN" ' <sender> JOIN <channel>

                Using lock As New ThreadLock(Users)
                    If Users.Count = ClientLimits.UserCount Then
                        Throw New LimitException("UserCount")
                    End If
                    Users.Add(sender)
                End Using
                OnUserJoined?.Invoke(sender)

            Case "NICK" ' <sender> NICK <new_nick>

                Dim new_nick = message.Parameters(0)

                Using lock As New ThreadLock(Users)
                    Users.Add(new_nick)
                    Users.Remove(sender)
                End Using

                OnUserChanged?.Invoke(sender, new_nick)

            Case "PART" ' <sender> PART <target> [comment]

                Using lock As New ThreadLock(Users)
                    Users.Remove(sender)
                End Using
                OnUserParted?.Invoke(sender)

            Case "TOPIC" ' TOPIC <target> <text>

                Topic = message.Parameters(1)
                OnTopicChanged?.Invoke(Topic)

        End Select

        MyBase.HandleMessageReceived(message)

    End Sub

    Protected Friend Overrides Sub HandleMessageSent(message As Message)
        If ShouldProcess(message) Then
            MyBase.HandleMessageSent(message)
        End If
    End Sub

#End Region

End Class
