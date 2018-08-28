Public Class UserChatStorage
    Inherits ChatStorageBase


    Event NickChanged(old_nick As String, new_nick As String)
    Event UserQuit()


    Sub New(chat As IRC.UserChat)
        MyBase.New(chat)
        chat.OnUserChanged = AddressOf HandleUserChanged
        chat.OnUserQuit = AddressOf HandleUserQuit
    End Sub


#Region "Internals"

    Private Sub HandleUserChanged(old_nick As String, new_nick As String)
        RaiseEvent NickChanged(old_nick, new_nick)
    End Sub

    Private Sub HandleUserQuit()
        RaiseEvent UserQuit()
    End Sub

#End Region

End Class
