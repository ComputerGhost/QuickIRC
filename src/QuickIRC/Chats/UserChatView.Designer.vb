<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserChatView
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lstMessages = New QuickIRC.MessageList()
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.ErrorProvider = New QuickIRC.ErrorProvider()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lstMessages
        '
        Me.lstMessages.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstMessages.Location = New System.Drawing.Point(0, 0)
        Me.lstMessages.Name = "lstMessages"
        Me.lstMessages.Size = New System.Drawing.Size(592, 328)
        Me.lstMessages.TabIndex = 0
        '
        'txtMessage
        '
        Me.txtMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMessage.Location = New System.Drawing.Point(0, 329)
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(592, 20)
        Me.txtMessage.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.QuickIRC.My.Resources.Resources.icon_close
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.Location = New System.Drawing.Point(593, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(23, 23)
        Me.btnClose.TabIndex = 2
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'UserChat
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.lstMessages)
        Me.Name = "UserChat"
        Me.Size = New System.Drawing.Size(616, 349)
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstMessages As MessageList
    Friend WithEvents txtMessage As TextBox
    Friend WithEvents ErrorProvider As ErrorProvider
    Friend WithEvents btnClose As Button
End Class
