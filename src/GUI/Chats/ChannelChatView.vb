Public Class ChannelChatView

    Sub New()
        InitializeComponent()
        _Chat_MessageAdded = AddressOf Chat_MessageAdded
        _Chat_NicksUpdated = AddressOf Chat_NicksUpdated
        _Chat_NickAdded = AddressOf Chat_NickAdded
        _Chat_NickChanged = AddressOf Chat_NickChanged
        _Chat_NickModeChanged = AddressOf Chat_NickModeChanged
        _Chat_NickRemoved = AddressOf Chat_NickRemoved
    End Sub

    Sub BindToChat(chat As ChannelChat)

        If BoundChat IsNot Nothing Then
            BoundChat.PendingMessage = txtMessage.Text
            RemoveHandler BoundChat.MessageAdded, _Chat_MessageAdded
            RemoveHandler BoundChat.NicksUpdated, _Chat_NicksUpdated
            RemoveHandler BoundChat.NickAdded, _Chat_NickAdded
            RemoveHandler BoundChat.NickChanged, _Chat_NickChanged
            RemoveHandler BoundChat.NickModeChanged, _Chat_NickModeChanged
            RemoveHandler BoundChat.NickRemoved, _Chat_NickRemoved
        End If

        ' clear to defaults
        lstMessages.Clear()
        lstNicks.Clear()
        txtMessage.Enabled = False

        BoundChat = chat

        If BoundChat IsNot Nothing Then
            lstMessages.AddMessages(BoundChat.Messages)
            lstNicks.AddNicks(BoundChat.Nicks)
            txtMessage.Enabled = True
            txtMessage.Text = BoundChat.PendingMessage
            AddHandler BoundChat.MessageAdded, _Chat_MessageAdded
            AddHandler BoundChat.NicksUpdated, _Chat_NicksUpdated
            AddHandler BoundChat.NickAdded, _Chat_NickAdded
            AddHandler BoundChat.NickChanged, _Chat_NickChanged
            AddHandler BoundChat.NickModeChanged, _Chat_NickModeChanged
            AddHandler BoundChat.NickRemoved, _Chat_NickRemoved
        End If

    End Sub

    Overloads Sub Focus()
        txtMessage.Select()
    End Sub

#Region "Chat events"

    Private _Chat_MessageAdded As ChannelChat.MessageAddedEventHandler
    Private Sub Chat_MessageAdded(message As IRC.Message)
        If InvokeRequired Then
            Invoke(Sub() Chat_MessageAdded(message))
            Exit Sub
        End If
        lstMessages.AddMessage(message)
    End Sub

    Private _Chat_NicksUpdated As ChannelChat.NicksUpdatedEventHandler
    Private Sub Chat_NicksUpdated(nicknames As SortedDictionary(Of String, String))
        If InvokeRequired Then
            Invoke(Sub() Chat_NicksUpdated(nicknames))
            Exit Sub
        End If
        lstNicks.Clear()
        lstNicks.AddNicks(nicknames)
    End Sub

    Private _Chat_NickAdded As ChannelChat.NickAddedEventHandler
    Private Sub Chat_NickAdded(nickname As String)
        If InvokeRequired Then
            Invoke(Sub() Chat_NickAdded(nickname))
            Exit Sub
        End If
        lstNicks.AddNick(nickname)
    End Sub

    Private _Chat_NickChanged As ChannelChat.NickChangedEventHandler
    Private Sub Chat_NickChanged(old_nick As String, new_nick As String)
        If InvokeRequired Then
            Invoke(Sub() Chat_NickChanged(old_nick, new_nick))
            Exit Sub
        End If
        lstNicks.ChangeNick(old_nick, new_nick)
    End Sub

    Private _Chat_NickModeChanged As ChannelChat.NickModeChangedEventHandler
    Private Sub Chat_NickModeChanged(nickname As String, prefix As String)
        If InvokeRequired Then
            Invoke(Sub() Chat_NickModeChanged(nickname, prefix))
            Exit Sub
        End If
        lstNicks.ChangeMode(nickname, prefix)
    End Sub

    Private _Chat_NickRemoved As ChannelChat.NickRemovedEventHandler
    Private Sub Chat_NickRemoved(nickname As String)
        If InvokeRequired Then
            Invoke(Sub() Chat_NickRemoved(nickname))
            Exit Sub
        End If
        lstNicks.RemoveNick(nickname)
    End Sub

#End Region

#Region "Internals"

    Private BoundChat As ChannelChat

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        BoundChat.Writer.ProcessAndSend("/PART")
    End Sub

    Private Sub txtMessage_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMessage.KeyPress
        Select Case e.KeyChar

            Case ChrW(13)   ' ENTER
                Try
                    BoundChat.Writer.ProcessAndSend(txtMessage.Text)
                    txtMessage.Text = ""
                Catch ex As NotImplementedException
                    ErrorProvider.SetError(txtMessage, "Command not recognized.")
                Catch ex As IRC.SyntaxError
                    ErrorProvider.SetError(txtMessage, "Syntax error.")
                End Try
                e.Handled = True

            Case ChrW(27)   ' ESC
                txtMessage.Text = ""
                e.Handled = True

        End Select
    End Sub

    Private Sub txtMessage_TextChanged() Handles txtMessage.TextChanged
        ErrorProvider.SetError(txtMessage, Nothing)
    End Sub

#End Region

End Class
