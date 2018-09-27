'
' Listens for chats that are at the server level
'
Public Class ServerChatListener
    Inherits ListenerBase


    Delegate Sub MessageDelegate(message As Message)
    Property OnMessage As MessageDelegate


    Overrides Sub HandleMessageReceived(ByRef message As Message)
        If ShouldProcess(message) Then
            OnMessage?.Invoke(message)
        End If
    End Sub

    Overrides Sub HandleMessageSent(ByRef message As Message)
        If ShouldProcess(message) Then
            OnMessage?.Invoke(message)
        End If
    End Sub


#Region "Internals"

    Private Shared IgnoredCommands As New HashSet(Of String) From {
        "328", "332", "333", "353", "366",
        "AWAY",
        "JOIN",
        "NICK",
        "NOTICE",
        "PART",
        "PING",
        "PRIVMSG",
        "TOPIC"}

    Private Function ShouldProcess(message As Message) As Boolean

        If Not message.IsValid Then
            Return False
        End If

        If IgnoredCommands.Contains(message.Verb) Then
            Return False
        End If

        ' Non-channel MODE commands
        If message.Verb = "MODE" Then
            Dim target = message.Parameters(0)
            Dim chan_prefixes = Connection.ServerLimits.ChanTypes
            Return target.Length = 0 OrElse Not chan_prefixes.Contains(target(0))
        End If

        Return True

    End Function

#End Region

End Class
