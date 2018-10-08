'
' This is the same as the .Net SplitContainer, but it allows setting the max 
' size of the fixed panel.
'
' Note: The SplitContainer may not update properly after a resize, especially
' when restoring from a maximized state. This is probably due to a limitation
' of the Windows stack size when dispatching messages. To work around this 
' issue, the form can be sized down 1 pixel and then sized up 1 pixel when 
' restoring from a maximized state. The SplitContainer should also be docked,
' not anchored, to handle weird sizes correctly.
'
Public Class SplitContainer
    Inherits System.Windows.Forms.SplitContainer

    Property FixedPanelMaxSize As Integer = 65536

    Overloads Property FixedPanel As FixedPanel
        Get
            Return _FixedPanel
        End Get
        Set(value As FixedPanel)
            _FixedPanel = value
            MyBase.FixedPanel = value
        End Set
    End Property
    Private _FixedPanel As FixedPanel

    Overloads Property Panel1MinSize As Integer = 25
    Overloads Property Panel2MinSize As Integer = 25


    ' When the size changed, the max panel size needs redone by setting the 
    ' min size of the opposite panel.
    Private Sub Me_SizeChanged() Handles MyBase.Resize

        ' Happens on minimize
        If ClientSize.Width = 0 Or ClientSize.Height = 0 Then
            Exit Sub
        End If

        If FixedPanel = FixedPanel.Panel1 Then

            Dim Min2PerMax1 = ClientSize.Width - FixedPanelMaxSize - SplitterWidth
            Dim Min2PerForm = Math.Max(Panel2MinSize, Min2PerMax1)
            Dim Max1PerMin2 = ClientSize.Width - Min2PerForm - SplitterWidth

            MyBase.Panel2MinSize = Min2PerForm
            If SplitterDistance > Max1PerMin2 Then
                SplitterDistance = Max1PerMin2
            End If

        ElseIf FixedPanel = FixedPanel.Panel2 Then

            Dim Min1PerMax2 = ClientSize.Width - FixedPanelMaxSize - SplitterWidth
            Dim Min1PerForm = Math.Max(Panel1MinSize, Min1PerMax2)
            Dim Max2PerMin1 = ClientSize.Width - Min1PerForm - SplitterWidth

            MyBase.Panel1MinSize = Min1PerForm
            If SplitterDistance < Min1PerForm Then
                SplitterDistance = Min1PerForm
            End If

        End If

    End Sub

    ' If size cannot contain both panels, the fixed panel should size down.
    Private Sub Me_SplitterMoved() Handles MyBase.SplitterMoved

        If FixedPanel = FixedPanel.Panel1 Then

            If Panel2.Width > Panel2MinSize Then
                MyBase.FixedPanel = FixedPanel.Panel1
            Else
                MyBase.FixedPanel = FixedPanel.Panel2
            End If

        ElseIf FixedPanel = FixedPanel.Panel2 Then

            If Panel1.Width > Panel1MinSize Then
                MyBase.FixedPanel = FixedPanel.Panel2
            Else
                MyBase.FixedPanel = FixedPanel.Panel1
            End If

        End If

    End Sub

End Class
