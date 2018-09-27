Public Class UserChatView

    Sub New()
        InitializeComponent()
        _Chat_MessageAdded = AddressOf Chat_MessageAdded
    End Sub

    Sub BindToChat(chat As UserChat)

        If BoundChat IsNot Nothing Then
            BoundChat.PendingMessage = txtMessage.Text
            RemoveHandler BoundChat.MessageAdded, _Chat_MessageAdded
        End If

        ' clear to defaults
        lstMessages.Clear()
        txtMessage.Enabled = False

        BoundChat = chat

        If BoundChat IsNot Nothing Then
            lstMessages.AddMessages(BoundChat.Messages)
            txtMessage.Enabled = True
            txtMessage.Text = BoundChat.PendingMessage
            AddHandler BoundChat.MessageAdded, _Chat_MessageAdded
        End If

    End Sub

    Overloads Sub Focus()
        txtMessage.Select()
    End Sub

#Region "Internals"

    Private BoundChat As UserChat

    Private _Chat_MessageAdded As UserChat.MessageAddedEventHandler
    Private Sub Chat_MessageAdded(message As IRC.Message)
        If InvokeRequired Then
            Invoke(Sub() Chat_MessageAdded(message))
            Exit Sub
        End If
        lstMessages.AddMessage(message)
    End Sub

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
