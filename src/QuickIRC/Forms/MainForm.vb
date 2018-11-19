Imports System.ComponentModel
Imports System.Net.Cache
Imports System.Runtime.InteropServices
Imports FaviconFetcher

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
        _LoadFaviconAsync(connection_info)

        ' Change out some visibility now that we have a connection
        btnConnect.Visible = False
        ClientArea.Visible = True

        ' Now we start
        Connection.Connect()

    End Sub

    Private Sub Form_Closing() Handles Me.FormClosing
        If Connection IsNot Nothing Then
            Connection.Disconnect()
            _Connection = Nothing
        End If
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

#Region "Helpers"

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(hWnd As IntPtr, Msg As UInteger, wParam As Integer, lParam As IntPtr) As IntPtr
    End Function
    Private Const WM_SETICON As UInteger = &H80
    Private Const ICON_SMALL As Integer = 0
    Private Const ICON_BIG As Integer = 1

    Private Sub _LoadFaviconAsync(connection_info As ConnectionModel)
        Task.Run(Sub() _LoadFavicon(connection_info))
    End Sub

    Private Sub _LoadFavicon(connection_info As ConnectionModel)

        Dim address = connection_info.Address.ToLower()
        If address.StartsWith("irc.") Then
            address = address.Substring(4)
        End If

        Dim websiteUri = New Uri("http://" & address)

        Dim source = New HttpSource() With {
            .CachePolicy = New RequestCachePolicy(RequestCacheLevel.CacheIfAvailable),
            .UserAgent = "QuickIRC"}
        Dim fetcher = New Fetcher(source)

        Using image = fetcher.FetchClosest(websiteUri, New Size(16, 16))
            If image IsNot Nothing Then
                Invoke(Sub() _SetIcon(image))
            End If
        End Using

    End Sub

    Private Sub _SetIcon(image As Image)
        If Connection Is Nothing Then
            Exit Sub
        End If
        Using bitmap = New Bitmap(image)
            Dim smallIcon = Icon.FromHandle(bitmap.GetHicon())
            SendMessage(Handle, WM_SETICON, ICON_SMALL, smallIcon.Handle)
        End Using
    End Sub

#End Region

End Class