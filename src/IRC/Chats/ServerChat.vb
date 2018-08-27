Public Class ServerChat
    Inherits ChatBase

    Public Overrides Sub ProcessAndSend(text As String)
        If text.StartsWith("/"c) Then
            MyBase.ProcessAndSend(text)
        Else
            MyBase.ProcessAndSend("/"c & text)
        End If
    End Sub


#Region "Internals"

    Private Shared IgnoredCommands As New HashSet(Of String) From {
        "353",
        "AWAY",
        "JOIN",
        "NICK",
        "NOTICE",
        "PART",
        "PING",
        "PRIVMSG",
        "TOPIC"}

    Protected Friend Overrides Sub HandleMessageReceived(message As Message)
        If Not IgnoredCommands.Contains(message.Verb) Then
            MyBase.HandleMessageReceived(message)
        End If
    End Sub

    Protected Friend Overrides Sub HandleMessageSent(message As Message)
        If Not IgnoredCommands.Contains(message.Verb) Then
            MyBase.HandleMessageSent(message)
        End If
    End Sub

#End Region

End Class
