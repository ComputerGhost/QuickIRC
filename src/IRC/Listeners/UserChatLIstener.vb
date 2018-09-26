'
' Listens for chats to private messages
'
Public Class UserChatListener
    Inherits ListenerBase


    Delegate Sub MessageDelegate(message As Message)
    Property OnMessage As MessageDelegate

    Delegate Sub UserChangedDelegate(new_nick As String)
    Delegate Sub UserQuitDelegate()
    Property OnUserChanged As UserChangedDelegate
    Property OnUserQuit As UserQuitDelegate

    Property UserName As String


    Sub New(user_name As String)
        UserName = user_name
    End Sub


    Overrides Sub HandleMessageReceived(ByRef message As Message)
        If Not ShouldProcess(message) Then
            Exit Sub
        End If

        Select Case message.Verb
            Case "NICK" ' <sender> NICK <new_nick>
                UserName = message.Parameters(0)
                OnUserChanged?.Invoke(UserName)
            Case "QUIT" ' <sender> QUIT [reason]
                OnUserQuit?.Invoke()
        End Select

        OnMessage?.Invoke(message)

    End Sub


#Region "Internals"

    ' Commands that have us/them as a target
    Shared TargetCommands As New HashSet(Of String) From {
        "NOTICE", "PRIVMSG"}

    ' Other commands that we are interested in
    Shared OtherCommands As New HashSet(Of String) From {
        "AWAY", "NICK", "QUIT"}


    Private Function ShouldProcess(message As Message) As Boolean

        If Not message.IsValid Then
            Return False
        End If

        Dim command = message.Verb

        If UserName <> message.Source.Name Then
            Return False
        ElseIf TargetCommands.Contains(command) Then
            Return (Connection.Nickname = message.Parameters(0))
        ElseIf OtherCommands.Contains(command) Then
            Return True
        Else
            Return False
        End If

    End Function

#End Region

End Class
