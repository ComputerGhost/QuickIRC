'
' Listens for chats to private messages
'
Imports IRC

Public Class UserChatListener
    Inherits ListenerBase


    Delegate Sub MessageDelegate(message As Message)
    Property OnMessage As MessageDelegate

    Delegate Sub UserChangedDelegate(new_nick As String)
    Delegate Sub UserQuitDelegate()
    Property OnUserChanged As UserChangedDelegate
    Property OnUserQuit As UserQuitDelegate

    Property OtherUser As String


    Sub New(user_name As String)
        OtherUser = user_name
    End Sub


    Overrides Sub HandleMessageReceived(ByRef message As Message)
        If Not ShouldProcess(message) Then
            Exit Sub
        End If

        Select Case message.Verb
            Case "NICK" ' <sender> NICK <new_nick>
                OtherUser = message.Parameters(0)
                OnUserChanged?.Invoke(OtherUser)
            Case "QUIT" ' <sender> QUIT [reason]
                OnUserQuit?.Invoke()
        End Select

        OnMessage?.Invoke(message)

    End Sub

    Public Overrides Sub HandleMessageSent(ByRef message As Message)
        If Not ShouldProcess(message) Then
            Exit Sub
        End If
        OnMessage?.Invoke(message)
    End Sub


#Region "Internals"

    ' Commands that have us/them as a target
    Shared TargetCommands As New HashSet(Of String) From {
        "NOTICE", "PRIVMSG"}

    ' Commands that have us/them as a source
    Shared SourceCommands As New HashSet(Of String) From {
        "AWAY", "NICK", "QUIT"}


    Private Function ShouldProcess(message As Message) As Boolean

        If Not message.IsValid Then
            Return False
        End If

        Dim command = message.Verb
        Dim target = message.Parameters(0)
        Dim source = message.Source.Name

        If TargetCommands.Contains(command) Then
            If source = Connection.Nickname And target = OtherUser Then
                Return True
            End If
            If source = OtherUser And target = Connection.Nickname Then
                Return True
            End If
        ElseIf SourceCommands.Contains(command) Then
            Return source Is Nothing Or source = OtherUser
        End If
        Return False

    End Function

#End Region

End Class
