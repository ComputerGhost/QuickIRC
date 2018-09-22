Public Class ServerChatWriter
    Inherits WriterBase

    Sub New(connection As Connection)
        MyBase.New(connection)
    End Sub

    Overrides Sub ProcessAndSend(text As String)
        If text.StartsWith("/"c) Then
            MyBase.ProcessAndSend(text)
        Else
            MyBase.ProcessAndSend("/"c & text)
        End If
    End Sub

End Class
