<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ChannelChatView
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
        Me.SplitContainer1 = New QuickIRC.SplitContainer()
        Me.lstMessages = New QuickIRC.MessageList()
        Me.lstNicks = New QuickIRC.NickList()
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.ErrorProvider = New QuickIRC.ErrorProvider()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer1.FixedPanelMaxSize = 200
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstMessages)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lstNicks)
        Me.SplitContainer1.Size = New System.Drawing.Size(527, 327)
        Me.SplitContainer1.SplitterDistance = 377
        Me.SplitContainer1.SplitterWidth = 6
        Me.SplitContainer1.TabIndex = 0
        '
        'lstMessages
        '
        Me.lstMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstMessages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstMessages.Location = New System.Drawing.Point(0, 0)
        Me.lstMessages.Name = "lstMessages"
        Me.lstMessages.Size = New System.Drawing.Size(377, 327)
        Me.lstMessages.TabIndex = 0
        '
        'lstNicks
        '
        Me.lstNicks.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstNicks.Location = New System.Drawing.Point(0, 0)
        Me.lstNicks.Name = "lstNicks"
        Me.lstNicks.Size = New System.Drawing.Size(144, 327)
        Me.lstNicks.TabIndex = 0
        '
        'txtMessage
        '
        Me.txtMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMessage.Location = New System.Drawing.Point(0, 328)
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(527, 20)
        Me.txtMessage.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.QuickIRC.My.Resources.Resources.icon_close
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.Location = New System.Drawing.Point(528, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(23, 23)
        Me.btnClose.TabIndex = 2
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'ChannelChat
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "ChannelChat"
        Me.Size = New System.Drawing.Size(551, 348)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents txtMessage As TextBox
    Friend WithEvents btnClose As Button
    Friend WithEvents lstMessages As MessageList
    Friend WithEvents lstNicks As NickList
    Friend WithEvents ErrorProvider As ErrorProvider
End Class
