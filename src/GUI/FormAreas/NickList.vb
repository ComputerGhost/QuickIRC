Public Class NickList

    Sub New()
        InitializeComponent()
        _Chat_NicksUpdated = AddressOf Chat_NicksUpdated
        _Chat_NickAdded = AddressOf Chat_NickAdded
        _Chat_NickChanged = AddressOf Chat_NickChanged
        _Chat_NickModeChanged = AddressOf Chat_NickModeChanged
        _Chat_NickRemoved = AddressOf Chat_NickRemoved
    End Sub

    Sub BindToChat(chat As ChannelChatStorage)

        If BoundData IsNot Nothing Then
            RemoveHandler BoundData.NicksUpdated, _Chat_NicksUpdated
            RemoveHandler BoundData.NickAdded, _Chat_NickAdded
            RemoveHandler BoundData.NickChanged, _Chat_NickChanged
            RemoveHandler BoundData.NickModeChanged, _Chat_NickModeChanged
            RemoveHandler BoundData.NickRemoved, _Chat_NickRemoved
        End If

        BoundData = chat
        SetNicks(BoundData.Nicks)

        If BoundData IsNot Nothing Then
            AddHandler BoundData.NicksUpdated, _Chat_NicksUpdated
            AddHandler BoundData.NickAdded, _Chat_NickAdded
            AddHandler BoundData.NickChanged, _Chat_NickChanged
            AddHandler BoundData.NickModeChanged, _Chat_NickModeChanged
            AddHandler BoundData.NickRemoved, _Chat_NickRemoved
        End If

    End Sub


#Region "Bound chat"

    Private BoundData As ChannelChatStorage

    Private _Chat_NicksUpdated As ChannelChatStorage.NicksUpdatedEventHandler
    Private Sub Chat_NicksUpdated(nicknames As SortedDictionary(Of String, String))

        If InvokeRequired Then
            Invoke(Sub() Chat_NicksUpdated(nicknames))
            Exit Sub
        End If

        SetNicks(nicknames)

    End Sub

    Private _Chat_NickAdded As ChannelChatStorage.NickAddedEventHandler
    Private Sub Chat_NickAdded(nickname As String)

        If InvokeRequired Then
            Invoke(Sub() Chat_NickAdded(nickname))
            Exit Sub
        End If

        lstNicks.Items.Add(New ListViewItem With {
            .Text = nickname,
            .Name = nickname})

    End Sub

    Private _Chat_NickChanged As ChannelChatStorage.NickChangedEventHandler
    Private Sub Chat_NickChanged(old_nick As String, new_nick As String)

        If InvokeRequired Then
            Invoke(Sub() Chat_NickChanged(old_nick, new_nick))
            Exit Sub
        End If

        Dim item = lstNicks.Items.Item(old_nick)
        item.Text = item.Text.Replace(old_nick, new_nick)
        item.Name = new_nick

    End Sub

    Private _Chat_NickModeChanged As ChannelChatStorage.NickModeChangedEventHandler
    Private Sub Chat_NickModeChanged(nickname As String, prefix As String)

        If InvokeRequired Then
            Invoke(Sub() Chat_NickModeChanged(nickname, prefix))
            Exit Sub
        End If

        Dim item = lstNicks.Items.Item(nickname)
        item.Text = prefix & nickname

    End Sub

    Private _Chat_NickRemoved As ChannelChatStorage.NickRemovedEventHandler
    Private Sub Chat_NickRemoved(nickname As String)

        If InvokeRequired Then
            Invoke(Sub() Chat_NickRemoved(nickname))
            Exit Sub
        End If

        lstNicks.Items.RemoveByKey(nickname)

    End Sub

#End Region

#Region "Internals"

    Private Sub SetNicks(nicknames As SortedDictionary(Of String, String))

        lstNicks.Items.Clear()

        If nicknames Is Nothing Then
            Exit Sub
        End If

        lstNicks.BeginUpdate()
        For Each nickname In nicknames
            lstNicks.Items.Add(New ListViewItem With {
                .Text = nickname.Value & nickname.Key,
                .Name = nickname.Key})
        Next
        lstNicks.EndUpdate()

    End Sub

#End Region

End Class
