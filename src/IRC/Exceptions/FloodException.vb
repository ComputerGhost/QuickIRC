' User was caught flooding us in a potential DOS attempt.
Public Class FloodException
    Inherits Exception

    Sub New(message As String)
        MyBase.New(message)
    End Sub

End Class
