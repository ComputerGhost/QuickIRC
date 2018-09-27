Public Class ClientArea

    ReadOnly Property Connection As IRC.Connection

    Sub New(connection_info As ConnectionInfo)
        InitializeComponent()

        Connection = New IRC.Connection(connection_info.Server, connection_info.Port)

        Connection.RegisterListener(New IRC.StandardListener() With {
            .OnChannelJoined = AddressOf HandleChannelJoined,
            .OnChannelParted = AddressOf HandleChannelParted,
            .OnUserMessage = AddressOf HandleUserMessage})
        Connection.RegisterListener(connection_info.Connector)

        lstChannels.Items.Add(New ListViewItem({"server"}) With {
            .Group = lstChannels.Groups("Server"),
            .Selected = True,
            .Tag = New ServerChat(Connection)})
        lstChannels.Items.Add(New ListViewItem({"raw"}) With {
            .Group = lstChannels.Groups("Server"),
            .Tag = New RawChat(Connection)})

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

        Dim item_index = lstChannels.Items.IndexOfKey(chat_name)
        Dim list_item = lstChannels.Items(item_index)
        Dim chat_data = DirectCast(list_item.Tag, ChatBase)

        Connection.UnregisterListener(chat_data.Listener)
        lstChannels.Items.Remove(list_item)

    End Sub


#Region "Chat Events"

    Private Sub HandleChannelJoined(channel_name As String)

        If InvokeRequired Then
            Invoke(Sub() HandleChannelJoined(channel_name))
            Exit Sub
        End If

        If channel_name.Length = 0 Then
            Exit Sub
        End If

        lstChannels.Items.Add(New ListViewItem({channel_name}) With {
            .Group = lstChannels.Groups("Channels"),
            .Name = channel_name,
            .Selected = True,
            .Tag = New ChannelChat(Connection, channel_name)})
        ChannelChatView.Focus()

    End Sub

    Private Sub HandleChannelParted(channel_name As String)
        If InvokeRequired Then
            Invoke(Sub() HandleChannelParted(channel_name))
            Exit Sub
        End If
        RemoveChat(channel_name)
    End Sub

    Private Sub HandleUserMessage(message As IRC.Message)

        If InvokeRequired Then
            Invoke(Sub() HandleUserMessage(message))
            Exit Sub
        End If

        Dim nickname = message.Source.Name

        If lstChannels.Items.ContainsKey(nickname) Then
            Exit Sub
        End If

        Dim chat_info = New UserChat(Connection, nickname)
        AddHandler chat_info.UserChanged, Sub(new_nick) ChangeChat(chat_info.Nickname, new_nick)
        AddHandler chat_info.UserQuit, Sub() HandleChannelParted(chat_info.Nickname)
        chat_info.Messages.Add(message)

        lstChannels.Items.Add(New ListViewItem({nickname}) With {
            .Group = lstChannels.Groups("Users"),
            .Name = nickname,
            .Tag = chat_info})

    End Sub

#End Region

#Region "UI Events"

    ' We want the server chat to have focus first
    Private Sub Me_Load() Handles Me.Load
        BeginInvoke(Sub() ServerChatView.Focus())
    End Sub

    ' Fixes a "limitation" of child controls not getting the resize event.
    Private Sub Me_SizeChanged() Handles Me.Resize
        SplitContainer1.Width += 1
        SplitContainer1.Width -= 1
    End Sub

    ' Otherwise, we lose focus after selection change
    Private Sub lstChannels_MouseUp() Handles lstChannels.MouseUp

        If lstChannels.SelectedItems.Count = 0 Then
            Exit Sub
        End If

        Select Case lstChannels.SelectedItems(0).Group.Name
            Case "Server"
                ServerChatView.Focus()
            Case "Channels"
                ChannelChatView.Focus()
            Case "Users"
                UserChatView.Focus()
        End Select

    End Sub

    Private Sub lstChannels_SelectedIndexChanged() Handles lstChannels.SelectedIndexChanged

        If lstChannels.SelectedIndices.Count = 0 Then
            Exit Sub
        End If
        Dim index = lstChannels.SelectedIndices(0)
        Dim item = lstChannels.Items(index)

        ' Hide all by default
        RawChatView.Visible = False
        ServerChatView.Visible = False
        ChannelChatView.Visible = False
        UserChatView.Visible = False

        Select Case item.Group.Name
            Case "Server"
                If item.Text = "raw" Then
                    RawChatView.BindToChat(item.Tag)
                    RawChatView.Visible = True
                ElseIf item.Text = "server" Then
                    ServerChatView.BindToChat(item.Tag)
                    ServerChatView.Visible = True
                Else
                    Debug.Assert(False, "Chat is not raw or server.")
                End If
            Case "Channels"
                ChannelChatView.BindToChat(item.Tag)
                ChannelChatView.Visible = True
            Case "Users"
                UserChatView.BindToChat(item.Tag)
                UserChatView.Visible = True
            Case Else
                Throw New Exception("Group name not recognized.")
        End Select

    End Sub

    Private Sub lstChannels_Resize() Handles lstChannels.Resize
        lstChannels.Columns(0).Width = lstChannels.ClientSize.Width
    End Sub

    Private Sub lstChannels_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstChannels.SelectedIndexChanged

    End Sub

    Private Sub lstChannels_MouseUp(sender As Object, e As MouseEventArgs) Handles lstChannels.MouseUp

    End Sub

    Private Sub lstChannels_Resize(sender As Object, e As EventArgs) Handles lstChannels.Resize

    End Sub

#End Region

End Class
