'
' Listens for everything.
' Be aware that invalid messages may be picked up with this one.
'
Public Class RawChatListener
    Inherits ListenerBase

    Delegate Sub MessageDelegate(message As Message)
    Property OnMessage As MessageDelegate

    Overrides Sub HandleMessageReceived(ByRef message As Message)
        OnMessage?.Invoke(message)
    End Sub

    Overrides Sub HandleMessageSent(ByRef message As Message)
        OnMessage?.Invoke(message)
    End Sub

End Class
