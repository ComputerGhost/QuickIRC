Imports System.ComponentModel

Public Class MainForm

    ReadOnly Property Connection As IRC.Connection = Nothing

    Sub Connect(connection_info As ConnectionModel)

        ' Get the connection ready
        _Connection = New IRC.Connection(connection_info.Address, connection_info.Port)
        Connection.RegisterListener(New IRC.ConnectListener With {
            .PreRegistration = connection_info.PreCommands,
            .AlternativeNicks = connection_info.NickAlternates,
            .PostRegistration = connection_info.PostCommands})

        ' Attach to our UI
        Me.Text = connection_info.Name
        ClientArea.AttachConnection(Connection)

        ' Change out some visibility now that we have a connection
        btnConnect.Visible = False
        ClientArea.Visible = True

        ' Now we start
        Connection.Connect()

    End Sub

    Private Sub btnConnect_Click() Handles btnConnect.Click
        ConnectionForm.TargetForm = Me
        ConnectionForm.ShowDialog()
    End Sub

#Region "Menu item events"

    ' Connection

    Private Sub NewToolStripMenuItem_Click() Handles NewToolStripMenuItem.Click
        If Connection Is Nothing Then
            ConnectionForm.TargetForm = Me
            ConnectionForm.ShowDialog()
        Else
            ConnectionForm.Show()
        End If
    End Sub

    Private Sub CloseToolStripMenuItem_Click() Handles CloseToolStripMenuItem.Click
        Close()
    End Sub

    ' Help

    Private Sub ContentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ContentsToolStripMenuItem.Click
        Try
            Dim file_info = FileIO.FileSystem.GetFileInfo("./help/index.html")
            Process.Start(file_info.FullName)
        Catch ex As Win32Exception
            Dim message = String.Format(
                "The help file could not be opened. The following error message was received:{0}{0}{1}",
                vbCrLf, ex.Message)
            MessageBox.Show(message, "Unable to Open Help", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub AboutQuickIRCToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutQuickIRCToolStripMenuItem.Click
        AboutForm.Show()
    End Sub

#End Region

End Class