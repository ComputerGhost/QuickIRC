Public Class UserChatWriter
    Inherits WriterBase

    ' Called when the user closes the user chat.
    ' Please unregister associated listener when this is received.
    Delegate Sub PartDelegate(username As String)
    Property OnPart As PartDelegate

    ReadOnly Property UserName As String


    Sub New(connection As Connection, user_name As String)
        MyBase.New(connection)
        UserName = user_name
    End Sub

    Overrides Sub ProcessAndSend(text As String)

        Dim tokenizer As New Tokenizer(text)

        ' If it's not a command, then it's a message
        If Not tokenizer.Skip("/"c) Then
            Connection.SendLine(String.Format("PRIVMSG {0} :{1}", UserName, text))
            Exit Sub
        End If

        Select Case tokenizer.ReadWord().ToUpper()
            Case "PART" ' PART
                OnPart?.Invoke(UserName)
            Case "SAY" ' SAY <message>
                Dim message = If(tokenizer.ReadRemaining(), "")
                Connection.SendLine(String.Format("PRIVMSG {0} :{1}", UserName, message))
            Case Else
                MyBase.ProcessAndSend(text)
        End Select

    End Sub

End Class
