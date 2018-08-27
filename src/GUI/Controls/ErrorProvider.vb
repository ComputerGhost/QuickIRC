Public Class ErrorProvider
    Inherits Windows.Forms.ErrorProvider


    ReadOnly Property HasErrors As Boolean
        Get
            Return Controls.Count > 0
        End Get
    End Property


    Overloads Sub SetError(c As Control, text As String)
        If text Is Nothing Then
            Controls.Remove(c)
        ElseIf Not Controls.Contains(c) Then
            Controls.Add(c)
        End If
        MyBase.SetError(c, text)
    End Sub


#Region "Internals"

    Private Controls As New List(Of Control)

#End Region

End Class
