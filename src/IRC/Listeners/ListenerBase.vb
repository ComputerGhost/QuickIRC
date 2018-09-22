Public Class ListenerBase

    Protected ReadOnly Property Connection As Connection

    ' The listener has been registered with the Connection.
    Sub HandleRegistration(connection As Connection)
        If Me.Connection IsNot Nothing Then
            Throw New ExpectationFailed("Listener cannot be registered twice.")
        End If
        Me._Connection = connection
    End Sub

    ' The connection has been opened.
    Overridable Sub HandleConnected()
    End Sub

    ' The connection has been lost
    Overridable Sub HandleDisconnected()
    End Sub

    ' A message has been received
    Overridable Sub HandleMessageReceived(ByRef message As Message)
    End Sub

    ' A message has been sent
    Overridable Sub HandleMessageSent(ByRef message As Message)
    End Sub

End Class
