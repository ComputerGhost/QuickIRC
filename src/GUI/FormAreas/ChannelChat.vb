Public Class ChannelChat


    Sub BindToChat(chat As ChatStorage)

        If BoundChat IsNot Nothing Then
            BoundChat.Message = txtMessage.Text
        End If

        BoundChat = chat

        If BoundChat IsNot Nothing Then
            txtMessage.Enabled = True
            txtMessage.Text = BoundChat.Message
        Else
            txtMessage.Enabled = False
        End If

        lstMessages.BindToChat(BoundChat)
        lstNicks.BindToChat(BoundChat)

    End Sub


#Region "Internals"

    Private BoundChat As ChatStorage

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Dim chat = DirectCast(BoundChat.Chat, IRC.ChannelChat)
        chat.ProcessAndSend("/PART " & chat.ChannelName)
    End Sub

    Private Sub txtMessage_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMessage.KeyPress
        Select Case e.KeyChar

            Case ChrW(13)   ' ENTER
                Try
                    BoundChat.Chat.ProcessAndSend(txtMessage.Text)
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
