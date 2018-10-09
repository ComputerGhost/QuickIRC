Imports System.IO
Imports Newtonsoft.Json

Public Class ConnectionForm

    ' Form to open the connection in. If unset, a new one is created.
    Property TargetForm As MainForm
        Get
            Return _TargetForm
        End Get
        Set(value As MainForm)
            _TargetForm = value
            lblNewChat.Visible = value Is Nothing
        End Set
    End Property
    Private _TargetForm As MainForm

    Private Sub Form_Load() Handles Me.Load

        ddlBookmark.Items.Add("")
        For Each bookmark In LoadBookmarks()
            ddlBookmark.Items.Add(bookmark)
        Next

        SetFormData(New BookmarkModel)

    End Sub

    Private Sub btnConnect_Click() Handles btnConnect.Click
        If Not ValidateChildren() Or ErrorProvider.HasErrors Then
            Exit Sub
        End If

        ' It is assumed that all inputs are valid at this point.

        ' Extract data from compact view in UI
        Dim connection_parts = txtConnection.Text.Split(":"c)
        Dim nick_list = ExtractList(txtNickname.Text)
        Dim nick_parts = nick_list(0).Split(":"c)
        Dim user_mode =
            If(lstMode.CheckedIndices.Contains(1), 4, 0) +  ' wallops
            If(lstMode.CheckedIndices.Contains(0), 8, 0)    ' invisible
        Dim channels = ExtractList(txtChannels.Text)

        ' Now we build the connection model

        Dim connection As New ConnectionModel

        connection.Name = ddlBookmark.Text
        If String.IsNullOrWhiteSpace(ddlBookmark.Text) Then
            connection.Name = connection_parts(0)
        End If

        connection.Address = connection_parts(0)
        If connection_parts.Length > 1 Then
            connection.Port = connection_parts(1)
        End If

        With connection.PreCommands
            If Not String.IsNullOrWhiteSpace(txtPass.Text) Then
                .Add("PASS " & txtPass.Text)
            End If
            .Add(String.Format("USER {0} {1} * :{2}",
                txtUsername.Text, user_mode, txtRealName.Text))
            .Add("NICK " & nick_parts(0))
        End With

        Dim alt_suffixes = {Nothing, "_", "3", "4", "5"}
        For i = 1 To UBound(alt_suffixes)
            If nick_list.Length <= i Then
                connection.NickAlternates.Add(nick_list(0) & alt_suffixes(i))
            Else
                connection.NickAlternates.Add(nick_list(i))
            End If
        Next

        With connection.PostCommands
            If nick_parts.Length > 1 AndAlso Not String.IsNullOrWhiteSpace(nick_parts(1)) Then
                connection.PostCommands.Add("PRIVMSG NickServ :IDENTIFY " & StoredPassword)
            End If
            If channels.Length Then
                connection.PostCommands.Add("JOIN " & String.Join(","c, channels.ToList()))
            End If
        End With

        ' Setup the chat window and we're done!
        If TargetForm Is Nothing Then
            TargetForm = New MainForm
        End If
        TargetForm.Connect(connection)
        TargetForm.Show()
        Me.Close()

    End Sub

    ' Required fields

    Private Sub txtConnection_Validating() Handles txtConnection.Validating
        ErrorProvider.SetError(txtConnection, Nothing)
        If String.IsNullOrWhiteSpace(txtConnection.Text) Then
            ErrorProvider.SetError(txtConnection, "A server is required.")
        ElseIf txtConnection.Text.Count(AddressOf Char.IsWhiteSpace) Then
            ErrorProvider.SetError(txtConnection, "The connection cannot contain whitespace.")
        End If
    End Sub

    Private Sub txtNickname_Validating() Handles txtNickname.Validating
        ErrorProvider.SetError(txtNickname, Nothing)
        If String.IsNullOrWhiteSpace(txtNickname.Text) Then
            ErrorProvider.SetError(txtNickname, "A nickname is required.")
        End If
    End Sub

    Private Sub txtUsername_Validating() Handles txtUsername.Validating
        ErrorProvider.SetError(txtUsername, Nothing)
        If String.IsNullOrWhiteSpace(txtUsername.Text) Then
            ErrorProvider.SetError(txtUsername, "A username is required.")
        End If
    End Sub

#Region "Bookmarks"

    Private Sub btnDelete_Click() Handles btnDelete.Click
        If ddlBookmark.SelectedIndex > 0 Then
            ddlBookmark.Items.RemoveAt(ddlBookmark.SelectedIndex)
            SaveBookmarks()
        Else
            SetFormData(New BookmarkModel)
        End If
    End Sub

    Private Sub btnSave_Click() Handles btnSave.Click
        If ddlBookmark.SelectedIndex <= 0 Then
            ddlBookmark.Items.Add(GetFormData())
        Else
            ddlBookmark.Items(ddlBookmark.SelectedIndex) = GetFormData()
        End If
        SaveBookmarks()
    End Sub

    ' The first bookmark is not set; the rest are the loaded bookmarks.

    Private Sub ddlBookmark_SelectedIndexChanged() Handles ddlBookmark.SelectedIndexChanged
        If ddlBookmark.SelectedIndex > 0 Then
            SetFormData(ddlBookmark.Items(ddlBookmark.SelectedIndex))
        End If
    End Sub

    Private Sub ddlBookmark_TextChanged() Handles ddlBookmark.TextChanged
        btnSave.Enabled = Not String.IsNullOrWhiteSpace(ddlBookmark.Text)
    End Sub

    ' The bookmarks file is a JSON-serialized BookmarksModel

    Private Function LoadBookmarks() As List(Of BookmarkModel)
        Dim data As BookmarksModel

        ' Load bookmarks from file
        Try
            Using reader As New StreamReader("bookmarks.js")
                data = JsonConvert.DeserializeObject(Of BookmarksModel)(reader.ReadToEnd())
            End Using
            If data.FormatVersion > BookmarksModel.CurrentFormatVersion Then
                BeginInvoke(Sub() Bookmarks_IncompatibleVersion())
            End If
        Catch ex As FileNotFoundException
            data = New BookmarksModel
        Catch ex As JsonSerializationException
            data = New BookmarksModel
            BeginInvoke(Sub() Bookmarks_DeserializationError())
        End Try

        ' Correct any blank fields
        If data.FormatVersion Is Nothing Then
            data.FormatVersion = New VersionModel(0, 0, 0)
        End If
        If data.Bookmarks Is Nothing Then
            data.Bookmarks = New List(Of BookmarkModel)
        End If

        Return data.Bookmarks
    End Function

    Private Sub SaveBookmarks()

        ' Pull list from combobox
        Dim bookmarks As New List(Of BookmarkModel)
        For i = 1 To ddlBookmark.Items.Count - 1
            bookmarks.Add(ddlBookmark.Items(i))
        Next

        ' Write the list to the file
        Using writer As New StreamWriter("bookmarks.js")
            Dim data As New BookmarksModel With {.Bookmarks = bookmarks}
            writer.Write(JsonConvert.SerializeObject(data))
        End Using

    End Sub

    Private Sub Bookmarks_IncompatibleVersion()
        MessageBox.Show(
            "The bookmarks were loaded from a newer format. Saving a bookmark may cause some information to be lost.",
            "Version Incompatibility",
            MessageBoxButtons.OK,
            MessageBoxIcon.Exclamation)
    End Sub

    Private Sub Bookmarks_DeserializationError()
        MessageBox.Show(
            "The bookmarks could not be loaded due to a file format error. Saving a bookmark will reset the file.",
            "Unable to Load",
            MessageBoxButtons.OK,
            MessageBoxIcon.Exclamation)
    End Sub

#End Region

#Region "Nickname special handling"

    ' The nickname will have the NickServ password redacted

    Private StoredPassword As String

    ' Redacts the NickServ password in a csv list of nicks
    Private Function RedactNickServ(nick_list As String) As String

        StoredPassword = Nothing

        ' Convert csv nicks to array
        Dim nicks = ExtractList(nick_list)
        If nicks.Length = 0 Then
            Return ""
        End If

        ' Get nick:pass from first nickname
        Dim nick_parts = nicks(0).Split(":"c)
        If nick_parts.Length > 1 Then
            StoredPassword = nick_parts(1)
            nick_parts(1) = StrDup(nick_parts(1).Length, "•")
        End If

        ' Recombine our modified extracted data
        nicks(0) = String.Join(":"c, nick_parts)
        Return String.Join(", ", nicks)

    End Function

    Private Sub txtNick_GotFocus() Handles txtNickname.GotFocus
        Dim simplified = txtNickname.Text
        While simplified.Contains("••")
            simplified = simplified.Replace("••", "•")
        End While
        txtNickname.Text = simplified.Replace("•", StoredPassword)
    End Sub

    Private Sub txtNick_LostFocus() Handles txtNickname.LostFocus
        txtNickname.Text = RedactNickServ(txtNickname.Text)
    End Sub

    ' The user is based off of the NICK field by default

    Private PreviousNick As String

    Private Sub txtNick_TextChanged() Handles txtNickname.TextChanged
        Dim nicks = ExtractList(txtNickname.Text)
        Dim first_nick = If(nicks.Length, nicks(0), "").Split(":"c)(0)
        If first_nick.Length > 8 Then first_nick = first_nick.Substring(0, 8)
        If txtUsername.Text = PreviousNick Then
            txtUsername.Text = first_nick
        End If
        PreviousNick = txtNickname.Text
    End Sub


#End Region

#Region "Form data helpers"

    Private Function ExtractList(csv As String) As String()
        Dim whitespace As Char() = Nothing
        Dim normalized = csv.Replace(","c, " "c)
        Return normalized.Split(whitespace, StringSplitOptions.RemoveEmptyEntries)
    End Function

    Private Function GetFormData() As BookmarkModel

        Dim server_parts = txtConnection.Text.Split(":")
        Dim server_address = server_parts(0)
        Dim server_port = If(server_parts.Length > 1, CInt(server_parts(1)), 6667)

        Dim pass As String = Nothing
        If Not String.IsNullOrWhiteSpace(txtPass.Text) Then
            pass = txtPass.Text
        End If

        Dim nickname As String = Nothing
        Dim nick_pass As String = StoredPassword
        Dim nick_alternates As List(Of String) = Nothing
        Dim nicks = ExtractList(txtNickname.Text)
        If nicks.Length Then
            nickname = nicks(0).Split(":"c)(0)
        End If
        If nicks.Length > 1 Then
            nick_alternates = nicks.ToList()
            nick_alternates.RemoveAt(0)
        End If

        Dim username As String = Nothing
        If Not String.IsNullOrWhiteSpace(txtUsername.Text) Then
            username = txtUsername.Text
        End If

        Dim user_mode =
            If(lstMode.GetItemChecked(1), 4, 0) +   ' wallops
            If(lstMode.GetItemChecked(0), 8, 0)     ' invisible

        Dim realname As String = Nothing
        If Not String.IsNullOrWhiteSpace(txtRealName.Text) Then
            realname = txtRealName.Text
        End If

        Return New BookmarkModel With {
            .Name = ddlBookmark.Text,
            .ServerAddress = server_address,
            .ServerPort = server_port,
            .ServerPass = pass,
            .Nickname = nickname,
            .NickAlternates = nick_alternates,
            .NickPass = nick_pass,
            .Username = username,
            .UserMode = user_mode,
            .RealName = realname,
            .Channels = ExtractList(txtChannels.Text).ToList()
        }
    End Function

    Private Sub SetFormData(bookmark As BookmarkModel)

        Debug.Assert(ddlBookmark.Text = bookmark.Name)

        Dim connection = bookmark.ServerAddress
        If bookmark.ServerPort <> 6667 Then
            connection &= ":" & bookmark.ServerPort
        End If

        Dim nicks As New List(Of String)
        If bookmark.Nickname IsNot Nothing Then
            If String.IsNullOrWhiteSpace(bookmark.NickPass) Then
                nicks.Add(bookmark.Nickname)
            Else
                nicks.Add(bookmark.Nickname & ":" & bookmark.NickPass)
            End If
        End If
        If If(bookmark.NickAlternates?.Count, 0) Then
            nicks.AddRange(bookmark.NickAlternates)
        End If

        Dim channels As List(Of String) = bookmark.Channels
        If channels Is Nothing Then
            channels = New List(Of String)
        End If

        txtConnection.Text = connection
        txtPass.Text = bookmark.ServerPass
        txtNickname.Text = RedactNickServ(String.Join(", ", nicks))
        txtUsername.Text = bookmark.Username
        lstMode.SetItemChecked(1, bookmark.UserMode And 4)  ' wallops
        lstMode.SetItemChecked(0, bookmark.UserMode And 8)  ' invisible
        txtRealName.Text = bookmark.RealName
        txtChannels.Text = String.Join(", ", channels)

        PreviousNick = bookmark.Nickname

    End Sub

#End Region

End Class