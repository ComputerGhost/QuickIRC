<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ClientArea
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
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Server", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Channels", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Users", System.Windows.Forms.HorizontalAlignment.Left)
        Me.SplitContainer1 = New GUI.SplitContainer()
        Me.lstChannels = New System.Windows.Forms.ListView()
        Me.ChatName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.UserChatView = New GUI.UserChatView()
        Me.ChannelChatView = New GUI.ChannelChatView()
        Me.ServerChatView = New GUI.ServerChatView()
        Me.RawChatView = New GUI.RawChatView()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.FixedPanelMaxSize = 200
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstChannels)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.SplitContainer1.Size = New System.Drawing.Size(660, 368)
        Me.SplitContainer1.SplitterDistance = 150
        Me.SplitContainer1.SplitterWidth = 6
        Me.SplitContainer1.TabIndex = 0
        '
        'lstChannels
        '
        Me.lstChannels.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChatName})
        Me.lstChannels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstChannels.FullRowSelect = True
        ListViewGroup1.Header = "Server"
        ListViewGroup1.Name = "Server"
        ListViewGroup2.Header = "Channels"
        ListViewGroup2.Name = "Channels"
        ListViewGroup3.Header = "Users"
        ListViewGroup3.Name = "Users"
        Me.lstChannels.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3})
        Me.lstChannels.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lstChannels.HideSelection = False
        Me.lstChannels.Location = New System.Drawing.Point(0, 0)
        Me.lstChannels.MultiSelect = False
        Me.lstChannels.Name = "lstChannels"
        Me.lstChannels.Size = New System.Drawing.Size(150, 368)
        Me.lstChannels.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstChannels.TabIndex = 0
        Me.lstChannels.UseCompatibleStateImageBehavior = False
        Me.lstChannels.View = System.Windows.Forms.View.Details
        '
        'ChatName
        '
        Me.ChatName.Text = "ChatName"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(504, 368)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RawChatView)
        Me.Panel1.Controls.Add(Me.UserChatView)
        Me.Panel1.Controls.Add(Me.ChannelChatView)
        Me.Panel1.Controls.Add(Me.ServerChatView)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(498, 362)
        Me.Panel1.TabIndex = 0
        '
        'UserChatView
        '
        Me.UserChatView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UserChatView.Location = New System.Drawing.Point(0, 0)
        Me.UserChatView.Name = "UserChatView"
        Me.UserChatView.Size = New System.Drawing.Size(498, 362)
        Me.UserChatView.TabIndex = 2
        Me.UserChatView.Visible = False
        '
        'ChannelChatView
        '
        Me.ChannelChatView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ChannelChatView.Location = New System.Drawing.Point(0, 0)
        Me.ChannelChatView.Name = "ChannelChatView"
        Me.ChannelChatView.Size = New System.Drawing.Size(498, 362)
        Me.ChannelChatView.TabIndex = 1
        Me.ChannelChatView.Visible = False
        '
        'ServerChatView
        '
        Me.ServerChatView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ServerChatView.Location = New System.Drawing.Point(0, 0)
        Me.ServerChatView.Name = "ServerChatView"
        Me.ServerChatView.Size = New System.Drawing.Size(498, 362)
        Me.ServerChatView.TabIndex = 0
        Me.ServerChatView.Visible = False
        '
        'RawChatView
        '
        Me.RawChatView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RawChatView.Location = New System.Drawing.Point(0, 0)
        Me.RawChatView.Name = "RawChatView"
        Me.RawChatView.Size = New System.Drawing.Size(498, 362)
        Me.RawChatView.TabIndex = 3
        Me.RawChatView.Visible = False
        '
        'ClientArea
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "ClientArea"
        Me.Size = New System.Drawing.Size(660, 368)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents lstChannels As ListView
    Friend WithEvents ChatName As ColumnHeader
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents UserChatView As UserChatView
    Friend WithEvents ChannelChatView As ChannelChatView
    Friend WithEvents ServerChatView As ServerChatView
    Friend WithEvents RawChatView As RawChatView
End Class
