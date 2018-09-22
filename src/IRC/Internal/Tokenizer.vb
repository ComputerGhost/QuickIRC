Public Class Tokenizer


    ReadOnly Property Text As String
    ReadOnly Property Offset As Integer = 0


    Sub New(text As String)
        Me.Text = text
    End Sub


    Function IsEnd() As Boolean
        Return Offset = Text.Length
    End Function

    ' Skips the next specific character; returns whether it was done.
    Function Skip(what As Char) As Boolean
        If Offset = Text.Length OrElse Text(Offset) <> what Then
            Return False
        End If
        _Offset += 1
        Return True
    End Function

    Function ReadChar() As Char
        If Offset = Text.Length Then
            Return ChrW(0)
        End If
        _Offset += 1
        Return Text(Offset - 1)
    End Function

    Function ReadWord() As String
        If Offset = Text.Length Then
            Return Nothing
        End If

        Dim start = Offset
        While Offset < Text.Length
            _Offset += 1
            Dim current_char = Text(Offset - 1)
            If Char.IsWhiteSpace(current_char) Then
                Return Text.Substring(start, Offset - start - 1)
            End If
        End While

        Return Text.Substring(start)
    End Function

    Function ReadRemaining() As String
        If Offset = Text.Length Then
            Return Nothing
        End If

        Dim start = Offset
        _Offset = Text.Length

        Return Text.Substring(start)
    End Function

    Function Peek() As Char
        If Offset = Text.Length Then
            Return ChrW(0)
        End If
        Return Text(Offset)
    End Function

End Class
