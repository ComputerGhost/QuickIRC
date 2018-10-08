Public Class NickList

    Sub AddNick(nickname As String)
        lstNicks.Items.Add(New ListViewItem With {
            .Text = nickname,
            .Name = nickname})
    End Sub

    Sub AddNicks(nicknames As SortedDictionary(Of String, String))
        lstNicks.BeginUpdate()
        For Each nickname In nicknames
            lstNicks.Items.Add(New ListViewItem With {
                .Text = nickname.Value & nickname.Key,
                .Name = nickname.Key})
        Next
        lstNicks.EndUpdate()
    End Sub

    Sub Clear()
        lstNicks.Items.Clear()
    End Sub

    Sub ChangeMode(nickname As String, prefix As String)
        Dim item = lstNicks.Items.Item(nickname)
        item.Text = prefix & nickname
    End Sub

    Sub ChangeNick(old_nick As String, new_nick As String)
        Dim item = lstNicks.Items.Item(old_nick)
        item.Text = item.Text.Replace(old_nick, new_nick)
        item.Name = new_nick
    End Sub

    Sub RemoveNick(nickname As String)
        lstNicks.Items.RemoveByKey(nickname)
    End Sub


#Region "UI Events"

    Private Sub Me_SizeChanged() Handles Me.SizeChanged
        lstNicks.Columns(0).Width = lstNicks.ClientSize.Width
    End Sub

#End Region

End Class
