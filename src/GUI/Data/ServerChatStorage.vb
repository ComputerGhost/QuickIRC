Public Class ServerChatStorage
    Inherits ChatStorageBase

    Property DisplayFriendlyMessages As Boolean = True

    Sub New(chat As IRC.ChatBase, Optional display_friendly As Boolean = True)
        MyBase.New(chat)
        DisplayFriendlyMessages = display_friendly
    End Sub

End Class
