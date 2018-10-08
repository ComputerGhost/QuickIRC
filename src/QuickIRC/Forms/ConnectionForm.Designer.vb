<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ConnectionForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConnectionForm))
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.ddlBookmark = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtConnection = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtPass = New System.Windows.Forms.TextBox()
        Me.txtChannels = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtRealName = New System.Windows.Forms.TextBox()
        Me.lstMode = New System.Windows.Forms.CheckedListBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtNickname = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ErrorProvider = New QuickIRC.ErrorProvider()
        Me.lblNewChat = New System.Windows.Forms.Label()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnConnect
        '
        Me.btnConnect.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnConnect.Location = New System.Drawing.Point(227, 201)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(75, 23)
        Me.btnConnect.TabIndex = 10
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'ddlBookmark
        '
        Me.ddlBookmark.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ddlBookmark.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ddlBookmark.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ddlBookmark.FormattingEnabled = True
        Me.ddlBookmark.Location = New System.Drawing.Point(76, 12)
        Me.ddlBookmark.MaxLength = 255
        Me.ddlBookmark.Name = "ddlBookmark"
        Me.ddlBookmark.Size = New System.Drawing.Size(177, 21)
        Me.ddlBookmark.Sorted = True
        Me.ddlBookmark.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Bookmark:"
        '
        'txtConnection
        '
        Me.txtConnection.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtConnection.Location = New System.Drawing.Point(82, 45)
        Me.txtConnection.MaxLength = 260
        Me.txtConnection.Name = "txtConnection"
        Me.txtConnection.Size = New System.Drawing.Size(220, 20)
        Me.txtConnection.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.txtConnection, "<server>[:<port>]")
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(12, 48)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 13)
        Me.Label11.TabIndex = 35
        Me.Label11.Text = "Connection:"
        '
        'txtUsername
        '
        Me.txtUsername.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUsername.Location = New System.Drawing.Point(55, 129)
        Me.txtUsername.MaxLength = 30
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(81, 20)
        Me.txtUsername.TabIndex = 6
        '
        'txtPass
        '
        Me.txtPass.Location = New System.Drawing.Point(55, 77)
        Me.txtPass.MaxLength = 248
        Me.txtPass.Name = "txtPass"
        Me.txtPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(8226)
        Me.txtPass.Size = New System.Drawing.Size(98, 20)
        Me.txtPass.TabIndex = 4
        '
        'txtChannels
        '
        Me.txtChannels.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtChannels.Location = New System.Drawing.Point(55, 169)
        Me.txtChannels.MaxLength = 283
        Me.txtChannels.Name = "txtChannels"
        Me.txtChannels.Size = New System.Drawing.Size(247, 20)
        Me.txtChannels.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.txtChannels, "[<channel>][,<channel>]*")
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 172)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(31, 13)
        Me.Label8.TabIndex = 51
        Me.Label8.Text = "JOIN"
        '
        'txtRealName
        '
        Me.txtRealName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRealName.Location = New System.Drawing.Point(202, 129)
        Me.txtRealName.MaxLength = 243
        Me.txtRealName.Name = "txtRealName"
        Me.txtRealName.Size = New System.Drawing.Size(100, 20)
        Me.txtRealName.TabIndex = 8
        '
        'lstMode
        '
        Me.lstMode.FormattingEnabled = True
        Me.lstMode.Items.AddRange(New Object() {"invisible", "wallops"})
        Me.lstMode.Location = New System.Drawing.Point(139, 129)
        Me.lstMode.Name = "lstMode"
        Me.lstMode.Size = New System.Drawing.Size(60, 34)
        Me.lstMode.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 132)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 48
        Me.Label4.Text = "USER"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 45
        Me.Label2.Text = "PASS"
        '
        'txtNickname
        '
        Me.txtNickname.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNickname.Location = New System.Drawing.Point(55, 103)
        Me.txtNickname.MaxLength = 526
        Me.txtNickname.Name = "txtNickname"
        Me.txtNickname.Size = New System.Drawing.Size(247, 20)
        Me.txtNickname.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.txtNickname, "<nickname>[:<pass>][,<alternate>]*")
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 106)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(32, 13)
        Me.Label7.TabIndex = 46
        Me.Label7.Text = "NICK"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Enabled = False
        Me.btnSave.Image = Global.QuickIRC.My.Resources.Resources.baseline_save_black_18dp
        Me.btnSave.Location = New System.Drawing.Point(279, 11)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(23, 23)
        Me.btnSave.TabIndex = 3
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.Image = Global.QuickIRC.My.Resources.Resources.baseline_delete_forever_black_18dp
        Me.btnDelete.Location = New System.Drawing.Point(256, 11)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(23, 23)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 0
        Me.ToolTip1.AutoPopDelay = 10000
        Me.ToolTip1.InitialDelay = 550
        Me.ToolTip1.ReshowDelay = 110
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.ToolTip1.ToolTipTitle = "Format"
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'lblNewChat
        '
        Me.lblNewChat.AutoSize = True
        Me.lblNewChat.Location = New System.Drawing.Point(46, 206)
        Me.lblNewChat.Name = "lblNewChat"
        Me.lblNewChat.Size = New System.Drawing.Size(175, 13)
        Me.lblNewChat.TabIndex = 52
        Me.lblNewChat.Text = "This will create a new chat window:"
        '
        'ConnectionForm
        '
        Me.AcceptButton = Me.btnConnect
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(314, 236)
        Me.Controls.Add(Me.lblNewChat)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.txtPass)
        Me.Controls.Add(Me.txtChannels)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtRealName)
        Me.Controls.Add(Me.lstMode)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtNickname)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtConnection)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ddlBookmark)
        Me.Controls.Add(Me.btnConnect)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "ConnectionForm"
        Me.Text = "New Connection"
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnConnect As Button
    Friend WithEvents ErrorProvider As ErrorProvider
    Friend WithEvents ddlBookmark As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtConnection As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtUsername As TextBox
    Friend WithEvents txtPass As TextBox
    Friend WithEvents txtChannels As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents txtRealName As TextBox
    Friend WithEvents lstMode As CheckedListBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtNickname As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents lblNewChat As Label
End Class
