Imports System.Text.RegularExpressions
Imports Algorithms

'
' Processes events for channels.
'
Public Class ChannelChatListener
    Inherits ListenerBase


    Delegate Sub MessageDelegate(message As Message)
    Property OnMessage As MessageDelegate

    Delegate Sub UsersUpdatedDelegate(nicknames As SortedDictionary(Of String, String))
    Delegate Sub UserJoinedDelegate(nickname As String)
    Delegate Sub UserChangedDelegate(old_nick As String, new_nick As String)
    Delegate Sub UserModeChangedDelegate(nickname As String, prefix As String)
    Delegate Sub UserPartedDelegate(nickname As String)
    Property OnUsersUpdated As UsersUpdatedDelegate
    Property OnUserJoined As UserJoinedDelegate
    Property OnUserChanged As UserChangedDelegate
    Property OnUserModeChanged As UserModeChangedDelegate
    Property OnUserParted As UserPartedDelegate

    Delegate Sub TopicChangedDelegate(text As String)
    Property OnTopicChanged As TopicChangedDelegate


    Property ChannelName As String
    Property Topic As String

    Property Users As New SortedDictionary(Of String, String)


    Sub New(channel_name As String)
        ChannelName = channel_name
    End Sub


    Overrides Sub HandleMessageReceived(ByRef message As Message)
        If Not ShouldProcess(message) Then
            Exit Sub
        End If

        Dim sender = message.Source.Name
        Select Case message.Verb

            Case "332" ' <sender> 332 <my_nick> <channel> <topic>

                Topic = message.Parameters(2)
                OnTopicChanged?.Invoke(Topic)

            Case "353" ' <sender> 353 <me> [type] <channel> <users>

                If ChannelName <> message.Parameters(1) And ChannelName <> message.Parameters(2) Then
                    Exit Sub
                End If

                Dim tokenizer As New Tokenizer(message.Parameters.Last())
                Using lock As New ThreadLock(Users)
                    While Not tokenizer.IsEnd
                        Dim user = tokenizer.ReadWord()
                        If user IsNot Nothing Then
                            Dim pattern = String.Format("([{0}]*)(.*)", Regex.Escape(Connection.ServerLimits.Prefix))
                            Dim parts = Regex.Match(user, pattern).Groups
                            Users.Add(parts(2).Value, parts(1).Value)
                        End If
                    End While
                    OnUsersUpdated?.Invoke(Users)
                End Using

            Case "AWAY" ' <sender> AWAY [message]

                Using lock As New ThreadLock(Users)
                    If Not Users.ContainsKey(sender) Then
                        Exit Sub
                    End If
                End Using

            Case "JOIN" ' <sender> JOIN <channel>

                Using lock As New ThreadLock(Users)
                    If Users.Count = ClientLimits.UserCount Then
                        Throw New LimitException("UserCount")
                    End If
                    Users.Add(sender, "")
                End Using
                OnUserJoined?.Invoke(sender)

            Case "MODE" ' MODE <channel> <lots-of-params>

                ' First we need to get a list of changes
                Dim changes As New List(Of String)
                Dim targets As New Queue(Of String)
                For i = 1 To message.Parameters.Count - 1
                    Dim parameter = message.Parameters(i)
                    If parameter.StartsWith("+"c) Then
                        For j = 1 To parameter.Length - 1
                            changes.Add("+" & parameter(j))
                        Next
                    ElseIf parameter.StartsWith("-"c) Then
                        For j = 1 To parameter.Length - 1
                            changes.Add("-" & parameter(j))
                        Next
                    Else
                        targets.Enqueue(parameter)
                    End If
                Next

                ' Now we can make those changes
                For Each change In changes

                    Dim do_add = change.StartsWith("+"c)
                    Dim mode = change(1)

                    Dim mode_prefix As Char
                    Select Case mode
                        Case "a"c : mode_prefix = "&"c
                        Case "h"c : mode_prefix = "%"c
                        Case "o"c : mode_prefix = "@"c
                        Case "q"c : mode_prefix = "~"c
                        Case "v"c : mode_prefix = "+"c
                        Case Else : Continue For
                    End Select

                    If targets.Count = 0 Then
                        Throw New SyntaxError("Target expected for MODE command.")
                    End If
                    Dim username = targets.Dequeue()

                    Using lock As New ThreadLock(Users)
                        If Users.ContainsKey(username) Then
                            If do_add Then
                                Users(username) = Users(username) & mode_prefix
                            Else
                                Users(username) = Users(username).Replace(mode_prefix, "")
                            End If
                            OnUserModeChanged?.Invoke(username, Users(username))
                        End If
                    End Using

                Next

            Case "NICK" ' <sender> NICK <new_nick>

                Dim new_nick = message.Parameters(0)

                Using lock As New ThreadLock(Users)
                    If Users.ContainsKey(sender) Then
                        Users.Add(new_nick, Users(sender))
                        Users.Remove(sender)
                    End If
                End Using

                OnUserChanged?.Invoke(sender, new_nick)

            Case "PART" ' <sender> PART <target> [comment]

                Using lock As New ThreadLock(Users)
                    Users.Remove(sender)
                End Using
                OnUserParted?.Invoke(sender)

            Case "QUIT" ' <sender> QUIT [comment]

                Using lock As New ThreadLock(Users)
                    Users.Remove(sender)
                End Using
                OnUserParted?.Invoke(sender)

            Case "TOPIC" ' TOPIC <target> <text>

                Topic = message.Parameters(1)
                OnTopicChanged?.Invoke(Topic)

        End Select
    End Sub

    Overrides Sub HandleMessageSent(ByRef message As Message)
        If ShouldProcess(message) Then
            OnMessage?.Invoke(message)
        End If
    End Sub


#Region "Internals"

    Private Function ShouldProcess(message As Message) As Boolean

        If Not message.IsValid Then
            Return False
        End If

        Dim command = message.Verb

        ' First parameter is the channel
        If {"JOIN", "MODE", "NOTICE", "PART", "PRIVMSG", "TOPIC"}.Contains(command) Then
            Return ChannelName = message.Parameters(0)
        End If

        ' Second parameter is the channel
        If {"328", "332", "333", "366"}.Contains(command) Then
            Return ChannelName = message.Parameters(1)
        End If

        ' Commands from a sender in the channel
        If {"AWAY", "NICK", "QUIT"}.Contains(command) Then
            Using lock As New ThreadLock(Users)
                Return Users.ContainsKey(message.Source.Name)
            End Using
        End If

        ' More complicated. Pass these and look at them later.
        Return {"353"}.Contains(command)

    End Function

#End Region

End Class
