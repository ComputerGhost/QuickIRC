Imports System.ComponentModel

Public Class MainForm


    Private Sub NewConnection_ConnectionRequested(connection_info As ConnectionInfo) Handles NewConnection.ConnectionRequested

        Dim page As New TabPage(connection_info.Name)
        page.Controls.Add(New ClientArea(connection_info) With {
            .Dock = DockStyle.Fill})

        ' Add the page second-to-last and activate it
        Dim tabs = Connections.TabPages
        tabs.Insert(tabs.Count - 1, page)
        Connections.SelectedIndex = tabs.Count - 2

    End Sub

#Region "Menu events"

    Private Sub AboutQuickIRCToolStripMenuItem_Click() Handles AboutQuickIRCToolStripMenuItem.Click
        AboutQuickIRC.ShowDialog()
    End Sub

    Private Sub ContentsToolStripMenuItem_Click() Handles ContentsToolStripMenuItem.Click
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

    Private Sub ExitToolStripMenuItem_Click() Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click() Handles NewToolStripMenuItem.Click

        ' The last tab is the connection tab.
        Connections.SelectedIndex = Connections.TabPages.Count - 1

    End Sub

#End Region

End Class