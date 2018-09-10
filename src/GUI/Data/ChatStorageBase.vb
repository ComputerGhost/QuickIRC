Imports Algorithms

Public Class ChatStorageBase

    Event ConnectedChanged(is_connected As Boolean)
    Event MessageAdded(message As IRC.Message)

    ReadOnly Property Chat As IRC.ChatBase
    ReadOnly Property Messages As New CircularBuffer(Of IRC.Message)
    Property Message As String = ""


    Sub New(chat As IRC.ChatBase)
        Me.Chat = chat
        chat.OnConnectionOpened = AddressOf HandleConnectionOpened
        chat.OnConnectionFailed = AddressOf HandleConnectionFailed
        chat.OnConnectionClosed = AddressOf HandleConnectionClosed
        chat.OnMessageReceived = AddressOf HandleMessageReceived
        chat.OnMessageSent = AddressOf HandleMessageReceived
    End Sub


#Region "Internals"

    Private Sub HandleConnectionOpened()

        Dim message As New IRC.Message With {
            .Direction = IRC.MessageDirection.Client,
            .Source = New IRC.MessageSource("QuickIRC"),
            .Parameters = {"Connection established."}.ToList()}
        Using lock As New ThreadLock(Messages)
            Messages.Add(message)
        End Using

        RaiseEvent MessageAdded(message)
        RaiseEvent ConnectedChanged(True)

    End Sub

    Private Sub HandleConnectionFailed(ex As Exception)

        Dim message_text = String.Format("Connection failed. ({0})", ex.Message)
        Dim message As New IRC.Message With {
            .Direction = IRC.MessageDirection.Client,
            .Source = New IRC.MessageSource("QuickIRC"),
            .Parameters = {message_text}.ToList()}
        Using lock As New ThreadLock(Messages)
            Messages.Add(message)
        End Using

        RaiseEvent MessageAdded(message)
        RaiseEvent ConnectedChanged(False)

    End Sub

    Private Sub HandleConnectionClosed()

        Dim message As New IRC.Message With {
            .Direction = IRC.MessageDirection.Client,
            .Source = New IRC.MessageSource("QuickIRC"),
            .Parameters = {"Connection closed."}.ToList()}
        Using lock As New ThreadLock(Messages)
            Messages.Add(message)
        End Using

        RaiseEvent MessageAdded(message)
        RaiseEvent ConnectedChanged(False)

    End Sub

    Private Sub HandleMessageReceived(message As IRC.Message)
        Using lock As New ThreadLock(Messages)
            Messages.Add(message)
        End Using
        RaiseEvent MessageAdded(message)
    End Sub

#End Region

End Class
