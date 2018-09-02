Public Class UserChatStorage
    Inherits ChatStorageBase

    Event ChannelParted(username As String)
    Event NickChanged(old_nick As String, new_nick As String)
    Event UserQuit()


    Sub New(chat As IRC.UserChat)
        MyBase.New(chat)
        chat.OnPart = AddressOf HandleParted
        chat.OnUserChanged = AddressOf HandleUserChanged
        chat.OnUserQuit = AddressOf HandleUserQuit
    End Sub


#Region "Internals"

    Private Sub HandleParted(username As String)
        RaiseEvent ChannelParted(username)
    End Sub

    Private Sub HandleUserChanged(old_nick As String, new_nick As String)
        RaiseEvent NickChanged(old_nick, new_nick)
    End Sub

    Private Sub HandleUserQuit()
        RaiseEvent UserQuit()
    End Sub

#End Region

End Class
