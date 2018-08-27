<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MessageList
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
        Me.MessagesView = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'MessagesView
        '
        Me.MessagesView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.MessagesView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MessagesView.Location = New System.Drawing.Point(0, 0)
        Me.MessagesView.Multiline = True
        Me.MessagesView.Name = "MessagesView"
        Me.MessagesView.ReadOnly = True
        Me.MessagesView.Size = New System.Drawing.Size(420, 338)
        Me.MessagesView.TabIndex = 0
        '
        'MessageList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.MessagesView)
        Me.Name = "MessageList"
        Me.Size = New System.Drawing.Size(420, 338)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MessagesView As TextBox
End Class
