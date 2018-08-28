Public Class NickList

    Sub New()
        InitializeComponent()
        _Chat_NicksUpdated = AddressOf Chat_NicksUpdated
        _Chat_NickAdded = AddressOf Chat_NickAdded
        _Chat_NickChanged = AddressOf Chat_NickChanged
        _Chat_NickRemoved = AddressOf Chat_NickRemoved
    End Sub

    Sub BindToChat(chat As ChatStorageBase)

        If BoundData IsNot Nothing Then
            RemoveHandler BoundData.NicksUpdated, _Chat_NicksUpdated
            RemoveHandler BoundData.NickAdded, _Chat_NickAdded
            RemoveHandler BoundData.NickChanged, _Chat_NickChanged
            RemoveHandler BoundData.NickRemoved, _Chat_NickRemoved
        End If

        BoundData = chat

        Nicks.Clear()
        If BoundData IsNot Nothing Then
            AddHandler BoundData.NicksUpdated, _Chat_NicksUpdated
            AddHandler BoundData.NickAdded, _Chat_NickAdded
            AddHandler BoundData.NickChanged, _Chat_NickChanged
            AddHandler BoundData.NickRemoved, _Chat_NickRemoved
        End If

    End Sub


#Region "Bound chat"

    Private BoundData As ChannelChatStorage

    Private _Chat_NicksUpdated As ChannelChatStorage.NicksUpdatedEventHandler
    Private Sub Chat_NicksUpdated(nicknames As SortedSet(Of String))

        If InvokeRequired Then
            Invoke(Sub() Chat_NicksUpdated(nicknames))
            Exit Sub
        End If

        Nicks.BeginUpdate()
        Nicks.Items.Clear()
        For Each nickname In nicknames
            Nicks.Items.Add(New ListViewItem With {
                .Text = nickname,
                .Name = nickname})
        Next
        Nicks.EndUpdate()

    End Sub

    Private _Chat_NickAdded As ChannelChatStorage.NickAddedEventHandler
    Private Sub Chat_NickAdded(nickname As String)

        If InvokeRequired Then
            Invoke(Sub() Chat_NickAdded(nickname))
            Exit Sub
        End If

        Nicks.Items.Add(New ListViewItem With {
            .Text = nickname,
            .Name = nickname})

    End Sub

    Private _Chat_NickChanged As ChannelChatStorage.NickChangedEventHandler
    Private Sub Chat_NickChanged(old_nick As String, new_nick As String)

        If InvokeRequired Then
            Invoke(Sub() Chat_NickChanged(old_nick, new_nick))
            Exit Sub
        End If

        Dim item = Nicks.Items.Item(old_nick)
        item.Text = new_nick
        item.Name = new_nick

    End Sub

    Private _Chat_NickRemoved As ChannelChatStorage.NickRemovedEventHandler
    Private Sub Chat_NickRemoved(nickname As String)

        If InvokeRequired Then
            Invoke(Sub() Chat_NickRemoved(nickname))
            Exit Sub
        End If

        Nicks.Items.RemoveByKey(nickname)

    End Sub

#End Region

End Class
