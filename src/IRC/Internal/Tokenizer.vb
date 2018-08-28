Imports System.Text.RegularExpressions

Public Class Tokenizer


    ReadOnly Property ServerLimits As ServerLimits
    ReadOnly Property Text As String
    ReadOnly Property Offset As Integer = 0


    Sub New(server_limits As ServerLimits, text As String)
        Me.ServerLimits = server_limits
        Me.Text = text
    End Sub


    ' Returns whether the next token is a channel
    Function IsChannel() As Boolean
        Dim prefixes = ServerLimits.ChanTypes
        Return Offset <> Text.Length AndAlso prefixes.Contains(Text(0))
    End Function

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


    ' Special cases

    Function ReadCommand() As String
        Return ReadWord().ToUpper()
    End Function

    Function ReadChannels() As String
        If Not IsChannel() Then
            Return Nothing
        Else
            Return ReadWord()
        End If
    End Function

    Function ReadNickname() As String
        Dim start = Offset
        Dim word = ReadWord()
        If Regex.IsMatch(word, "^[A-Za-z\[-`{-}][\w\[-`{-}-]*$") Then
            Return word
        Else
            _Offset = start
            Return Nothing
        End If
    End Function

    Function ReadTarget() As String
        Return ReadWord()
    End Function

End Class
