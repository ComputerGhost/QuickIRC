Public Class ClientArea

    ReadOnly Property Connection As IRC.Connection

    Sub New(connection_info As ConnectionInfo)
        InitializeComponent()

        Connection = New IRC.Connection(
            connection_info.Server,
            connection_info.Port)

        Connection.RegisterChat(New IRC.StandardHandler() With {
            .OnChannelJoined = AddressOf HandleChannelJoined,
            .OnChannelParted = AddressOf HandleChannelParted})
        Connection.RegisterChat(connection_info.Connector)

        AddChat("Server", "server", New ServerChatStorage(New IRC.ServerChat()))
        AddChat("Server", "raw", New ServerChatStorage(New IRC.ChatBase()))

        Connection.Connect()

    End Sub

    Sub AddChat(group_name As String, chat_name As String, chat_info As ChatStorageBase)

        If InvokeRequired Then
            Invoke(Sub() AddChat(group_name, chat_name, chat_info))
            Exit Sub
        End If

        Connection.RegisterChat(chat_info.Chat)

        Dim list_item = New ListViewItem({chat_name}) With {
            .Group = lstChannels.Groups(group_name),
            .Name = If(group_name = "Server", Nothing, chat_name),
            .Tag = chat_info}
        lstChannels.Items.Add(list_item)

        list_item.Selected = True

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

        If channel_name.Length = 0 Then Exit Sub

        Dim prefixes = Connection.ServerLimits.ChanTypes
        If prefixes.Contains(channel_name(0)) Then
            Dim chat_info = New ChannelChatStorage(New IRC.ChannelChat(channel_name))
            AddChat("Channels", channel_name, chat_info)

        Else
            Dim chat_info = New UserChatStorage(New IRC.UserChat(channel_name))
            AddHandler chat_info.NickChanged, AddressOf ChangeChat
            AddChat("Users", channel_name, chat_info)
        End If

    End Sub

    Private Sub HandleChannelParted(channel_name As String)
        RemoveChat(channel_name)
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

    Private Sub lstChannels_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstChannels.SelectedIndexChanged

    End Sub

    Private Sub lstChannels_Resize(sender As Object, e As EventArgs) Handles lstChannels.Resize

    End Sub

#End Region

End Class
