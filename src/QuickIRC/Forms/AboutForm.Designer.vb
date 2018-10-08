<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblProductName = New System.Windows.Forms.Label()
        Me.lblCopyright = New System.Windows.Forms.Label()
        Me.btnOkay = New System.Windows.Forms.Button()
        Me.lblDescription = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = Global.QuickIRC.My.Resources.Resources.MainLogo
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(100, 99)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'lblProductName
        '
        Me.lblProductName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblProductName.Location = New System.Drawing.Point(118, 12)
        Me.lblProductName.Name = "lblProductName"
        Me.lblProductName.Size = New System.Drawing.Size(244, 23)
        Me.lblProductName.TabIndex = 1
        Me.lblProductName.Text = "Product Name"
        '
        'lblCopyright
        '
        Me.lblCopyright.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCopyright.Location = New System.Drawing.Point(118, 35)
        Me.lblCopyright.Name = "lblCopyright"
        Me.lblCopyright.Size = New System.Drawing.Size(244, 23)
        Me.lblCopyright.TabIndex = 3
        Me.lblCopyright.Text = "Copyright"
        '
        'btnOkay
        '
        Me.btnOkay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOkay.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnOkay.Location = New System.Drawing.Point(287, 89)
        Me.btnOkay.Name = "btnOkay"
        Me.btnOkay.Size = New System.Drawing.Size(75, 23)
        Me.btnOkay.TabIndex = 5
        Me.btnOkay.Text = "Okay"
        Me.btnOkay.UseVisualStyleBackColor = True
        '
        'lblDescription
        '
        Me.lblDescription.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDescription.Location = New System.Drawing.Point(118, 58)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(244, 28)
        Me.lblDescription.TabIndex = 6
        Me.lblDescription.Text = "Description"
        '
        'AboutQuickIRC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnOkay
        Me.ClientSize = New System.Drawing.Size(374, 124)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.btnOkay)
        Me.Controls.Add(Me.lblCopyright)
        Me.Controls.Add(Me.lblProductName)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutQuickIRC"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Title"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lblProductName As Label
    Friend WithEvents lblCopyright As Label
    Friend WithEvents btnOkay As Button
    Friend WithEvents lblDescription As Label
End Class
