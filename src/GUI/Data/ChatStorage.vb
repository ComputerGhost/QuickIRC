Imports Algorithms

Public Class ChatStorage


    Event ConnectedChanged(is_connected As Boolean)
    Event MessageAdded(message As IRC.Message)

    Event NicksUpdated(nicknames As SortedSet(Of String))
    Event NickAdded(nickname As String)
    Event NickChanged(old_nick As String, new_nick As String)
    Event NickRemoved(nickname As String)


    ReadOnly Property Chat As IRC.ChatBase
    ReadOnly Property Messages As New CircularBuffer(Of IRC.Message)
    ReadOnly Property Nicks As New SortedSet(Of String)

    Property Message As String = ""


    Sub New(chat As IRC.ChatBase)

        Me.Chat = chat
        chat.OnConnectionOpened = AddressOf HandleConnectionOpened
        chat.OnConnectionFailed = AddressOf HandleConnectionFailed
        chat.OnConnectionClosed = AddressOf HandleConnectionClosed
        chat.OnMessageReceived = AddressOf HandleMessageReceived
        chat.OnMessageSent = AddressOf HandleMessageReceived

        If TypeOf chat Is IRC.ChannelChat Then
            With DirectCast(chat, IRC.ChannelChat)
                .OnUsersUpdated = AddressOf HandleUsersUpdated
                .OnUserJoined = AddressOf HandleUserJoined
                .OnUserChanged = AddressOf HandleUserChanged
                .OnUserParted = AddressOf HandleUserParted
            End With
        ElseIf TypeOf chat Is IRC.UserChat Then
            With DirectCast(chat, IRC.UserChat)
                .OnUserChanged = AddressOf HandleUserChanged
                .OnUserQuit = Sub() HandleUserParted(.UserName)
            End With
        End If

    End Sub


#Region "Receiving info from IRC"

    Private Sub HandleConnectionOpened()

        Dim message As New IRC.Message With {
            .Source = New IRC.MessageSource("QuickIRC"),
            .Parameters = {"Connection established."}.ToList()}
        Using lock As New ThreadLock(Messages)
            Messages.Add(message)
        End Using
        RaiseEvent MessageAdded(message)

        RaiseEvent ConnectedChanged(True)

    End Sub

    Private Sub HandleConnectionFailed()

        Dim message As New IRC.Message With {
            .Source = New IRC.MessageSource("QuickIRC"),
            .Parameters = {"Connection failed."}.ToList()}
        Using lock As New ThreadLock(Messages)
            Messages.Add(message)
        End Using
        RaiseEvent MessageAdded(message)

        RaiseEvent ConnectedChanged(False)

    End Sub

    Private Sub HandleConnectionClosed()

        Dim message As New IRC.Message With {
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

    Private Sub HandleUsersUpdated(nicknames As SortedSet(Of String))
        Using lock As New ThreadLock(Nicks)
            Nicks.Clear()
            For Each nickname In nicknames
                Nicks.Add(nickname)
            Next
            RaiseEvent NicksUpdated(Nicks)
        End Using
    End Sub

    Private Sub HandleUserJoined(nickname As String)
        Using lock As New ThreadLock(Nicks)
            Nicks.Add(nickname)
        End Using
        RaiseEvent NickAdded(nickname)
    End Sub

    Private Sub HandleUserChanged(old_nick As String, new_nick As String)
        Using lock As New ThreadLock(Nicks)
            If Nicks.Contains(old_nick) Then
                Nicks.Remove(old_nick)
                Nicks.Add(new_nick)
            End If
        End Using
        RaiseEvent NickChanged(old_nick, new_nick)
    End Sub

    Private Sub HandleUserParted(nickname As String)
        Using lock As New ThreadLock(Nicks)
            Nicks.Remove(nickname)
        End Using
        RaiseEvent NickRemoved(nickname)
    End Sub

#End Region

End Class
