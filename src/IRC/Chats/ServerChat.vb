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

        If IgnoredCommands.Contains(message.Verb) Then
            Return False
        End If

        If message.Verb = "MODE" Then
            Dim target = message.Parameters(0)
            Dim chan_prefixes = Connection.ServerLimits.ChanTypes
            Return target.Length = 0 OrElse Not chan_prefixes.Contains(target(0))
        End If

        Return True

    End Function

    Protected Friend Overrides Sub HandleMessageReceived(message As Message)
        If ShouldProcess(message) Then
            MyBase.HandleMessageReceived(message)
        End If
    End Sub

    Protected Friend Overrides Sub HandleMessageSent(message As Message)
        If ShouldProcess(message) Then
            MyBase.HandleMessageSent(message)
        End If
    End Sub

#End Region

End Class
