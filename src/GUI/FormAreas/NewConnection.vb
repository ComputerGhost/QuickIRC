Public Class NewConnection


    Event ConnectionRequested(connect_info As ConnectionInfo)


    Private Sub NewConnection_Load() Handles MyBase.Load

        ' By default, "invisible" mode is checked
        lstMode.SetItemChecked(0, True)

    End Sub

    Private Sub Me_VisibleChanged() Handles Me.VisibleChanged
        If Me.Visible Then
            txtHost.Select()
        End If
    End Sub

    Private Sub btnConnect_Click() Handles btnConnect.Click

        If Not ValidateChildren() Or ErrorProvider.HasErrors Then
            Exit Sub
        End If

        Dim connect_info = GetConnectInfo()

        RaiseEvent ConnectionRequested(connect_info)

    End Sub


#Region "Input validation"


    Private Sub txtHost_Validating() Handles txtHost.Validating

        txtHost.Text = txtHost.Text.Trim()

        If txtHost.Text.Length = 0 Then
            ErrorProvider.SetError(txtHost, "Host is required.")
        ElseIf txtHost.Text.Count(AddressOf Char.IsWhiteSpace) Then
            ErrorProvider.SetError(txtHost, "Invalid host name.")
        Else
            ErrorProvider.SetError(txtHost, Nothing)
        End If

    End Sub

    Private Sub txtPort_Validating() Handles txtPort.Validating

        txtPort.Text = txtPort.Text.Trim()
        If txtPort.Text.Length = 0 Then
            txtPort.Text = 6667
        End If

        If CInt(txtPort.Text) < 1 OrElse CInt(txtPort.Text) > 65535 Then
            ErrorProvider.SetError(txtPort, "Port is outside of the range 1-65535.")
        Else
            ErrorProvider.SetError(txtPort, Nothing)
        End If

    End Sub

    Private Sub chkUsername_CheckedChanged() Handles chkUser.CheckedChanged

        txtUsername.Text = txtUsername.Text.Trim()

        ErrorProvider.SetError(txtUsername, Nothing)

        Dim wants_username = Not chkUser.Checked
        If wants_username And txtUsername.Text.Count(AddressOf Char.IsWhiteSpace) Then
            ErrorProvider.SetError(txtUsername, "Whitespace is not valid in the username.")
        End If

    End Sub

    Private Sub txtUsername_Validating() Handles txtUsername.Validating

        txtUsername.Text = txtUsername.Text.Trim()

        ErrorProvider.SetError(txtUsername, Nothing)

        If chkUser.Checked Then
            If txtUsername.Text.Length = 0 Then
                ErrorProvider.SetError(txtUsername, "Username is required.")
            ElseIf txtPass.Text.Count(AddressOf Char.IsWhiteSpace) Then
                ErrorProvider.SetError(txtPass, "Whitespace is not valid in the username.")
            End If
        End If

    End Sub


    Private Sub chkPass_CheckedChanged() Handles chkPass.CheckedChanged

        txtPass.Text = txtPass.Text.Trim()

        ErrorProvider.SetError(txtPass, Nothing)

        Dim wants_password = Not chkPass.Checked
        If wants_password And txtPass.Text.Count(AddressOf Char.IsWhiteSpace) Then
            ErrorProvider.SetError(txtPass, "Whitespace is not valid in the password.")
        End If

    End Sub

    Private Sub txtPass_Validating() Handles txtPass.Validating

        txtPass.Text = txtPass.Text.Trim()

        ErrorProvider.SetError(txtPass, Nothing)

        If chkPass.Checked Then
            If txtPass.Text.Length = 0 Then
                ErrorProvider.SetError(txtPass, "Password is expected.")
            ElseIf txtPass.Text.Count(AddressOf Char.IsWhiteSpace) Then
                ErrorProvider.SetError(txtPass, "Whitespace is not valid in the password.")
            End If
        End If

    End Sub


    Private Sub chkNick_CheckedChanged() Handles chkNick.CheckedChanged

        txtNick.Text = txtNick.Text.Trim()

        ErrorProvider.SetError(txtNick, Nothing)

    End Sub

    Private Sub txtNick_Validating() Handles txtNick.Validating

        txtNick.Text = txtNick.Text.Trim()

        ErrorProvider.SetError(txtNick, Nothing)

        If chkNick.Checked And ExtractList(txtNick.Text).Count = 0 Then
            ErrorProvider.SetError(txtNick, "No nicknames specified.")
        End If

    End Sub

    Private Sub txtNick_TextChanged() Handles txtNick.TextChanged
        Static previous_first_nick As String = ""

        Dim first_nick = "" & GetFirstNick()
        If txtUsername.Text = previous_first_nick Then
            txtUsername.Text = first_nick
        End If

        previous_first_nick = first_nick
    End Sub


    Private Sub chkJoin_CheckedChanged() Handles chkJoin.CheckedChanged

        txtJoin.Text = txtJoin.Text.Trim()

        ErrorProvider.SetError(txtJoin, Nothing)

    End Sub

    Private Sub txtJoin_Validating() Handles txtJoin.Validating

        txtJoin.Text = txtJoin.Text.Trim()

        ErrorProvider.SetError(txtJoin, Nothing)

        If chkJoin.Checked And ExtractList(txtJoin.Text).Count = 0 Then
            ErrorProvider.SetError(txtJoin, "No channels specified.")
        End If

    End Sub

#End Region

#Region "Internals"

    ' Our lists can be delimited by comma, space, or both. This parses such a
    ' list and returns the non-empty items.
    Private Function ExtractList(text As String) As String()

        ' We need to split on whitespace, so replace commas with spaces.
        Dim normalized = text.Replace(","c, " "c)

        ' This will split on any whitespace character.
        Dim separators(0) As Char
        Return normalized.Split(separators, StringSplitOptions.RemoveEmptyEntries)

    End Function

    ' Pulls connection info from form. Note: assumes all input is valid.
    Private Function GetConnectInfo() As ConnectionInfo

        ' At this point, it is assumed that all inputs are valid.

        Dim connect_info As New ConnectionInfo With {
            .Name = txtHost.Text,
            .Server = txtHost.Text,
            .Port = txtPort.Text}
        connect_info.Connector = New IRC.ConnectListener With {
            .PreRegistration = New List(Of String),
            .PostRegistration = New List(Of String)}

        ' So we don't have long lists if this.that.something.whatevs
        Dim connector = connect_info.Connector
        Dim pre_commands = connector.PreRegistration
        Dim post_commands = connector.PostRegistration

        ' Set the properties that the user wants.
        If chkPass.Checked Then
            pre_commands.Add("/PASS " & txtPass.Text)
        End If
        If chkUser.Checked Then
            Dim user_name = txtUserName.Text
            Dim user_mode =
                If(lstMode.CheckedIndices.Contains(1), 4, 0) +  ' wallops
                If(lstMode.CheckedIndices.Contains(0), 8, 0)    ' invisible
            Dim real_name = txtRealName.Text
            pre_commands.Add(String.Format("/USER {0} {1} * {2}", user_name, user_mode, real_name))
        End If
        If chkNick.Checked Then
            pre_commands.Add("/NICK " & GetFirstNick())
            connector.AlternativeNicks = GetFallbackNicks()
        End If
        If chkJoin.Checked Then
            Dim channels = ExtractList(txtJoin.Text)
            post_commands.Add("/JOIN " & String.Join(","c, channels))
        End If

        Return connect_info

    End Function

    Private Function GetFirstNick() As String
        Dim nicknames = ExtractList(txtNick.Text)
        Return If(nicknames.Length, nicknames(0), Nothing)
    End Function

    Private Function GetFallbackNicks() As List(Of String)
        Dim nicknames = ExtractList(txtNick.Text).ToList()
        Debug.Assert(nicknames.Count > 0)
        nicknames.RemoveAt(0)
        Return nicknames
    End Function

#End Region

End Class
