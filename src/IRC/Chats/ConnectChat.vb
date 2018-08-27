'
' Handles connecting to an IRC server.
'
' This class is not thread-safe. It may be executed on multiple threads but 
' not at the same time. For this reason, the properties must not be changed 
' once it is registered with a Connection.
'
Public Class ConnectChat
    Inherits ChatBase


    ' Commands sent to register, and commands sent after registration
    Property PreRegistration As List(Of String) = Nothing
    Property PostRegistration As List(Of String) = Nothing
    ReadOnly Property IsRegistered As Boolean = False

    ' If we get a nickname error, alternatives to try
    Property AlternativeNicks As List(Of String) = Nothing


#Region "Internals"

    Private AlternativeIndex As Integer = 0


    Protected Friend Overrides Sub HandleConnectionOpened()
        MyBase.HandleConnectionOpened()
        If PreRegistration IsNot Nothing Then
            For Each cmd In PreRegistration
                ProcessAndSend(cmd)
            Next
        End If
    End Sub

    Protected Friend Overrides Sub HandleConnectionFailed(ex As Exception)
        _IsRegistered = False
        MyBase.HandleConnectionFailed(ex)
    End Sub

    Protected Friend Overrides Sub HandleConnectionClosed()
        _IsRegistered = False
        MyBase.HandleConnectionClosed()
    End Sub


    Protected Friend Overrides Sub HandleMessageReceived(message As Message)
        Select Case message.Verb

            Case "004" ' Last of 4 messages sent upon registering
                _IsRegistered = True
                If PostRegistration IsNot Nothing Then
                    For Each cmd In PostRegistration
                        ProcessAndSend(cmd)
                    Next
                End If

            Case "431", "432", "433", "436" ' Nickname errors
                If IsRegistered Then
                    Exit Sub
                End If
                If AlternativeIndex < AlternativeNicks?.Count Then
                    Dim nickname = AlternativeNicks(AlternativeIndex)
                    Connection.SendLine("NICK " & nickname)
                    AlternativeIndex += 1
                End If

        End Select
        MyBase.HandleMessageReceived(message)
    End Sub

#End Region

End Class
