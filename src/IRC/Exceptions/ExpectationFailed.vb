'
' Use in place of Debug.Assert when input can come from other compilation 
' units. This way, it is not required to link against a Debug version of us 
' while testing.
'
Public Class ExpectationFailed
    Inherits Exception

    Sub New(message As String)
        MyBase.New(message)
    End Sub

End Class
