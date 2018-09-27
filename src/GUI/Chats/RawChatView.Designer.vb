<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RawChatView
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
        Me.lstMessages = New GUI.MessageList()
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.ErrorProvider = New GUI.ErrorProvider()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lstMessages
        '
        Me.lstMessages.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstMessages.BotColor = System.Drawing.Color.MediumSlateBlue
        Me.lstMessages.ClientColor = System.Drawing.Color.DarkSlateBlue
        Me.lstMessages.CTCPColor = System.Drawing.Color.MediumVioletRed
        Me.lstMessages.DateFormat = "D"
        Me.lstMessages.DirectionColor = System.Drawing.Color.IndianRed
        Me.lstMessages.DisplayFriendlyMessages = False
        Me.lstMessages.JoinColor = System.Drawing.Color.SlateGray
        Me.lstMessages.Location = New System.Drawing.Point(0, 0)
        Me.lstMessages.MeColor = System.Drawing.Color.MediumSlateBlue
        Me.lstMessages.MessageColor = System.Drawing.Color.Black
        Me.lstMessages.Name = "lstMessages"
        Me.lstMessages.NickColor = System.Drawing.Color.SlateBlue
        Me.lstMessages.PartColor = System.Drawing.Color.SlateGray
        Me.lstMessages.Size = New System.Drawing.Size(544, 327)
        Me.lstMessages.TabIndex = 1
        Me.lstMessages.TimeColor = System.Drawing.Color.LightSlateGray
        Me.lstMessages.TimeFormat = "HH:mm:ss"
        Me.lstMessages.UserColor = System.Drawing.Color.MediumSlateBlue
        Me.lstMessages.VerbColor = System.Drawing.Color.DarkGreen
        '
        'txtMessage
        '
        Me.txtMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMessage.Location = New System.Drawing.Point(0, 328)
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(544, 20)
        Me.txtMessage.TabIndex = 2
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'RawChatView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.lstMessages)
        Me.Name = "RawChatView"
        Me.Size = New System.Drawing.Size(544, 348)
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstMessages As MessageList
    Friend WithEvents txtMessage As TextBox
    Friend WithEvents ErrorProvider As ErrorProvider
End Class
