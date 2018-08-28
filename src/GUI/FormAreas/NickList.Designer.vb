<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NickList
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
        Me.lstNicks = New System.Windows.Forms.ListView()
        Me.SuspendLayout()
        '
        'lstNicks
        '
        Me.lstNicks.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstNicks.Location = New System.Drawing.Point(0, 0)
        Me.lstNicks.MultiSelect = False
        Me.lstNicks.Name = "lstNicks"
        Me.lstNicks.Size = New System.Drawing.Size(150, 150)
        Me.lstNicks.TabIndex = 0
        Me.lstNicks.UseCompatibleStateImageBehavior = False
        Me.lstNicks.View = System.Windows.Forms.View.List
        '
        'NickList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lstNicks)
        Me.Name = "NickList"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lstNicks As ListView
End Class
