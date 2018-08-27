<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NewConnection
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtPort = New System.Windows.Forms.MaskedTextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtHost = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtJoin = New System.Windows.Forms.TextBox()
        Me.chkJoin = New System.Windows.Forms.CheckBox()
        Me.txtRealName = New System.Windows.Forms.TextBox()
        Me.lstMode = New System.Windows.Forms.CheckedListBox()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.chkUser = New System.Windows.Forms.CheckBox()
        Me.txtNick = New System.Windows.Forms.TextBox()
        Me.chkNick = New System.Windows.Forms.CheckBox()
        Me.txtPass = New System.Windows.Forms.TextBox()
        Me.chkPass = New System.Windows.Forms.CheckBox()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ErrorProvider = New GUI.ErrorProvider()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtPort)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtHost)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(372, 45)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Connection"
        '
        'txtPort
        '
        Me.txtPort.AllowPromptAsInput = False
        Me.txtPort.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ErrorProvider.SetIconAlignment(Me.txtPort, System.Windows.Forms.ErrorIconAlignment.MiddleLeft)
        Me.txtPort.Location = New System.Drawing.Point(316, 19)
        Me.txtPort.Mask = "00000"
        Me.txtPort.Name = "txtPort"
        Me.txtPort.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.txtPort.Size = New System.Drawing.Size(50, 20)
        Me.txtPort.TabIndex = 3
        Me.txtPort.Text = "6667"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(300, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(10, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = ":"
        '
        'txtHost
        '
        Me.txtHost.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtHost.Location = New System.Drawing.Point(50, 19)
        Me.txtHost.MaxLength = 254
        Me.txtHost.Name = "txtHost"
        Me.txtHost.Size = New System.Drawing.Size(244, 20)
        Me.txtHost.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Host:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtUsername)
        Me.GroupBox2.Controls.Add(Me.txtJoin)
        Me.GroupBox2.Controls.Add(Me.chkJoin)
        Me.GroupBox2.Controls.Add(Me.txtRealName)
        Me.GroupBox2.Controls.Add(Me.lstMode)
        Me.GroupBox2.Controls.Add(Me.TextBox5)
        Me.GroupBox2.Controls.Add(Me.chkUser)
        Me.GroupBox2.Controls.Add(Me.txtNick)
        Me.GroupBox2.Controls.Add(Me.chkNick)
        Me.GroupBox2.Controls.Add(Me.txtPass)
        Me.GroupBox2.Controls.Add(Me.chkPass)
        Me.GroupBox2.Location = New System.Drawing.Point(0, 51)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(372, 137)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "First Commands"
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(68, 71)
        Me.txtUsername.MaxLength = 50
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(100, 20)
        Me.txtUsername.TabIndex = 5
        Me.ToolTip.SetToolTip(Me.txtUsername, "Username")
        '
        'txtJoin
        '
        Me.txtJoin.Location = New System.Drawing.Point(68, 111)
        Me.txtJoin.MaxLength = 505
        Me.txtJoin.Name = "txtJoin"
        Me.txtJoin.Size = New System.Drawing.Size(192, 20)
        Me.txtJoin.TabIndex = 11
        Me.ToolTip.SetToolTip(Me.txtJoin, "Channels to join, separated by commas.")
        '
        'chkJoin
        '
        Me.chkJoin.AutoSize = True
        Me.chkJoin.Location = New System.Drawing.Point(6, 113)
        Me.chkJoin.Name = "chkJoin"
        Me.chkJoin.Size = New System.Drawing.Size(50, 17)
        Me.chkJoin.TabIndex = 10
        Me.chkJoin.Text = "JOIN"
        Me.chkJoin.UseVisualStyleBackColor = True
        '
        'txtRealName
        '
        Me.txtRealName.Location = New System.Drawing.Point(266, 71)
        Me.txtRealName.MaxLength = 500
        Me.txtRealName.Name = "txtRealName"
        Me.txtRealName.Size = New System.Drawing.Size(100, 20)
        Me.txtRealName.TabIndex = 8
        Me.txtRealName.Text = "QuickIRC User"
        Me.ToolTip.SetToolTip(Me.txtRealName, "Real Name")
        '
        'lstMode
        '
        Me.lstMode.FormattingEnabled = True
        Me.lstMode.Items.AddRange(New Object() {"invisible", "wallops"})
        Me.lstMode.Location = New System.Drawing.Point(174, 71)
        Me.lstMode.Name = "lstMode"
        Me.lstMode.Size = New System.Drawing.Size(60, 34)
        Me.lstMode.TabIndex = 6
        '
        'TextBox5
        '
        Me.TextBox5.Enabled = False
        Me.TextBox5.Location = New System.Drawing.Point(240, 71)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(20, 20)
        Me.TextBox5.TabIndex = 7
        Me.TextBox5.Text = "*"
        '
        'chkUser
        '
        Me.chkUser.AutoSize = True
        Me.chkUser.Checked = True
        Me.chkUser.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUser.Location = New System.Drawing.Point(6, 73)
        Me.chkUser.Name = "chkUser"
        Me.chkUser.Size = New System.Drawing.Size(56, 17)
        Me.chkUser.TabIndex = 4
        Me.chkUser.Text = "USER"
        Me.chkUser.UseVisualStyleBackColor = True
        '
        'txtNick
        '
        Me.txtNick.Location = New System.Drawing.Point(68, 45)
        Me.txtNick.Name = "txtNick"
        Me.txtNick.Size = New System.Drawing.Size(192, 20)
        Me.txtNick.TabIndex = 3
        Me.ToolTip.SetToolTip(Me.txtNick, "Nicknames to try, separated by commas.")
        '
        'chkNick
        '
        Me.chkNick.AutoSize = True
        Me.chkNick.Checked = True
        Me.chkNick.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNick.Location = New System.Drawing.Point(6, 47)
        Me.chkNick.Name = "chkNick"
        Me.chkNick.Size = New System.Drawing.Size(51, 17)
        Me.chkNick.TabIndex = 2
        Me.chkNick.Text = "NICK"
        Me.chkNick.UseVisualStyleBackColor = True
        '
        'txtPass
        '
        Me.txtPass.Location = New System.Drawing.Point(68, 19)
        Me.txtPass.Name = "txtPass"
        Me.txtPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(8226)
        Me.txtPass.Size = New System.Drawing.Size(100, 20)
        Me.txtPass.TabIndex = 1
        '
        'chkPass
        '
        Me.chkPass.AutoSize = True
        Me.chkPass.Location = New System.Drawing.Point(6, 21)
        Me.chkPass.Name = "chkPass"
        Me.chkPass.Size = New System.Drawing.Size(54, 17)
        Me.chkPass.TabIndex = 0
        Me.chkPass.Text = "PASS"
        Me.chkPass.UseVisualStyleBackColor = True
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(297, 194)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(75, 23)
        Me.btnConnect.TabIndex = 2
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'NewConnection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnConnect)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "NewConnection"
        Me.Size = New System.Drawing.Size(372, 217)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtHost As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtPort As MaskedTextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents txtPass As TextBox
    Friend WithEvents txtNick As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents lstMode As CheckedListBox
    Friend WithEvents txtRealName As TextBox
    Friend WithEvents txtJoin As TextBox
    Friend WithEvents btnConnect As Button
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents ErrorProvider As ErrorProvider
    Friend WithEvents chkPass As CheckBox
    Friend WithEvents chkNick As CheckBox
    Friend WithEvents chkUser As CheckBox
    Friend WithEvents chkJoin As CheckBox
    Friend WithEvents txtUsername As TextBox
End Class
