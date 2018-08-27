' We have exceeded one of our client's limitations.
Public Class LimitException
    Inherits Exception

    Sub New(limit_name As String)
        MyBase.New(String.Format("Limit $0 exceeded.", limit_name))
    End Sub

End Class
