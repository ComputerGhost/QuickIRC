'
' Handles the initial commands when connecting to an IRC server.
'
Public Class ConnectListener
    Inherits ListenerBase


    ' Lines sent to register, and lines sent after registration
    Property PreRegistration As List(Of String) = Nothing
    Property PostRegistration As List(Of String) = Nothing

    ' If we get a nickname error, alternatives to try
    Property AlternativeNicks As List(Of String) = Nothing

    ' Status
    ReadOnly Property IsRegistered As Boolean = False


    Overrides Sub HandleConnected()
        If PreRegistration IsNot Nothing Then
            For Each line In PreRegistration
                Connection.SendLine(line)
            Next
        End If
    End Sub

    Overrides Sub HandleDisconnected()
        _IsRegistered = False
    End Sub

    Overrides Sub HandleMessageReceived(ByRef message As Message)
        If Not message.IsValid Then
            Exit Sub
        End If

        Select Case message.Verb

            Case "004" ' Last of 4 messages sent upon registering
                _IsRegistered = True
                If PostRegistration Is Nothing Then Exit Sub
                For Each line In PostRegistration
                    Connection.SendLine(line)
                Next

            Case "431", "432", "433", "436" ' Nickname errors
                If IsRegistered Then Exit Sub
                If AlternativeIndex < AlternativeNicks?.Count Then
                    Dim nickname = AlternativeNicks(AlternativeIndex)
                    Connection.SendLine("NICK " & nickname)
                    AlternativeIndex += 1
                End If

        End Select

    End Sub

#Region "Internals"

    ' Index of next alternate nick to try
    Private AlternativeIndex As Integer = 0

#End Region

End Class
