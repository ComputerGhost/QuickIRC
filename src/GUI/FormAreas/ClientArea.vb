Public Class ClientArea

    ReadOnly Property Connection As IRC.Connection

    Sub New(connection_info As ConnectionInfo)
        InitializeComponent()

        Connection = New IRC.Connection(
            connection_info.Server,
            connection_info.Port)

        Connection.RegisterChat(New IRC.StandardHandler() With {
            .OnChannelJoined = AddressOf HandleChannelJoined,
            .OnChannelParted = AddressOf HandleChannelParted,
            .OnUserMessage = AddressOf HandleUserMessage})
        Connection.RegisterChat(connection_info.Connector)

        Dim server_chat = New IRC.ServerChat()
        Connection.RegisterChat(server_chat)
        lstChannels.Items.Add(New ListViewItem({"server"}) With {
            .Group = lstChannels.Groups("Server"),
            .Selected = True,
            .Tag = New ServerChatStorage(server_chat)})

        Dim raw_chat = New IRC.ChatBase()
        Connection.RegisterChat(raw_chat)
        lstChannels.Items.Add(New ListViewItem({"raw"}) With {
            .Group = lstChannels.Groups("Server"),
            .Tag = New ServerChatStorage(raw_chat)})

        Connection.Connect()

    End Sub

    Sub ChangeChat(old_name As String, new_name As String)

        If InvokeRequired Then
            Invoke(Sub() ChangeChat(old_name, new_name))
            Exit Sub
        End If

        Dim item_index = lstChannels.Items.IndexOfKey(old_name)
        Dim list_item = lstChannels.Items(item_index)
        list_item.Name = new_name
        list_item.Text = new_name

    End Sub

    Sub RemoveChat(chat_name As String)

        If InvokeRequired Then
            Invoke(Sub() RemoveChat(chat_name))
            Exit Sub
        End If

        Dim item_index = lstChannels.Items.IndexOfKey(chat_name)
        Dim list_item = lstChannels.Items(item_index)
        Dim chat_info = DirectCast(list_item.Tag, ChatStorageBase)
        Dim chat = chat_info.Chat

        Connection.UnregisterChat(chat)
        lstChannels.Items.Remove(list_item)

    End Sub


#Region "Chat Events"

    Private Sub HandleChannelJoined(channel_name As String)

        If InvokeRequired Then
            Invoke(Sub() HandleChannelJoined(channel_name))
            Exit Sub
        End If

        If channel_name.Length = 0 Then Exit Sub

        Dim channel_chat = New IRC.ChannelChat(channel_name)
        Connection.RegisterChat(channel_chat)
        lstChannels.Items.Add(New ListViewItem({channel_name}) With {
            .Group = lstChannels.Groups("Channels"),
            .Name = channel_name,
            .Selected = True,
            .Tag = New ChannelChatStorage(channel_chat)})

    End Sub

    Private Sub HandleChannelParted(channel_name As String)
        RemoveChat(channel_name)
    End Sub

    Private Sub HandleUserMessage(other_nick As String, message As IRC.Message)

        If InvokeRequired Then
            Invoke(Sub() HandleUserMessage(other_nick, message))
            Exit Sub
        End If

        If lstChannels.Items.ContainsKey(other_nick) Then
            Exit Sub
        End If

        Dim user_chat = New IRC.UserChat(other_nick)
        Connection.RegisterChat(user_chat)

        Dim chat_info = New UserChatStorage(user_chat)
        chat_info.Messages.Add(message)
        AddHandler chat_info.NickChanged, AddressOf ChangeChat
        AddHandler chat_info.ChannelParted, AddressOf HandleChannelParted

        Dim list_item = New ListViewItem({other_nick}) With {
            .Group = lstChannels.Groups("Users"),
            .Name = other_nick,
            .Tag = chat_info}
        lstChannels.Items.Add(list_item)

    End Sub

#End Region

#Region "UI Events"

    ' Fixes a "limitation" of child controls not getting the resize event.
    Private Sub Me_SizeChanged() Handles MyBase.Resize
        SplitContainer1.Width += 1
        SplitContainer1.Width -= 1
    End Sub


    Private Sub lstChannels_SelectedIndexChanged() Handles lstChannels.SelectedIndexChanged

        If lstChannels.SelectedIndices.Count = 0 Then
            Exit Sub
        End If
        Dim index = lstChannels.SelectedIndices(0)
        Dim item = lstChannels.Items(index)

        ServerChat.Visible = False
        ChannelChat.Visible = False
        UserChat.Visible = False
        Select Case item.Group.Name
            Case "Server"
                ServerChat.BindToChat(item.Tag)
                ServerChat.Visible = True
            Case "Channels"
                ChannelChat.BindToChat(item.Tag)
                ChannelChat.Visible = True
            Case "Users"
                UserChat.BindtoChat(item.Tag)
                UserChat.Visible = True
            Case Else
                Throw New Exception("Group name not recognized.")
        End Select

    End Sub

    Private Sub lstChannels_Resize() Handles lstChannels.Resize
        lstChannels.Columns(0).Width = lstChannels.ClientSize.Width
    End Sub

#End Region

End Class
