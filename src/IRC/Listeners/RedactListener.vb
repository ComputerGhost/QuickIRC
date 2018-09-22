Imports System.Text

'
' Redacts sensitive information within commands.
'
Public Class RedactListener
    Inherits ListenerBase

    Overrides Sub HandleMessageSent(ByRef message As Message)
        If Not message.IsValid Then
            Exit Sub
        End If

        Select Case message.Verb

            Case "DIE" : Redact(message, 0)
            Case "OPER" : Redact(message, 1)
            Case "PASS" : Redact(message, 0)
            Case "RESTART" : Redact(message, 0)

            Case "PRIVMSG"

                Dim target = message.Parameters(0).ToLower()
                Dim text = message.Parameters(1)

                If target = "nickserv" Then
                    Dim tokenizer As New Tokenizer(text)
                    Select Case tokenizer.ReadWord().ToUpper()
                        Case "DROP" : RedactService(message, 1)
                        Case "GHOST" : RedactService(message, 1)
                        Case "IDENTIFY" : RedactService(message, 0)
                        Case "RECOVER" : RedactService(message, 1)
                        Case "REGAIN" : RedactService(message, 1)
                        Case "SETPASS" : RedactService(message, 2)

                        Case "SET"
                            If tokenizer.ReadWord().ToUpper() = "PASSWORD" Then
                                RedactService(message, 1)
                            End If

                    End Select

                ElseIf target = "chanserv" Then
                    Dim tokenizer As New Tokenizer(text)
                    Select Case tokenizer.ReadWord().ToUpper()
                        Case "IDENTIFY" : RedactService(message, 1)
                        Case "LOGOUT" : RedactService(message, 1)
                        Case "REGISTER" : RedactService(message, 1)

                        Case "SET"
                            tokenizer.ReadWord() ' ignore word
                            If tokenizer.ReadWord().ToUpper = "PASSWORD" Then
                                RedactService(message, 2)
                            End If

                    End Select
                End If

        End Select

    End Sub

#Region "Internals"

    Private Sub Redact(ByRef message As Message, param_index As Integer)

        If message.Parameters.Count < param_index + 1 Then
            Exit Sub
        End If
        If message.Parameters(param_index) = "[redacted]" Then
            Exit Sub
        End If

        message.Parameters(param_index) = "[redacted]"

    End Sub

    Private Sub RedactService(ByRef message As Message, param_index As Integer)

        Dim tokenizer As New Tokenizer(message.Parameters(1))
        Dim parts As New List(Of String)
        Dim current_word As String

        For i = 0 To param_index - 1
            current_word = tokenizer.ReadWord()
            If current_word Is Nothing Then Exit Sub
            parts.Add(current_word)
        Next

        current_word = tokenizer.ReadWord()
        If current_word Is Nothing Then Exit Sub
        If current_word = "[redacted]" Then Exit Sub
        parts.Add("[redacted]")

        While True
            current_word = tokenizer.ReadWord()
            If current_word Is Nothing Then Exit While
            parts.Add(current_word)
        End While

        message.Parameters(1) = String.Join(" ", parts)

    End Sub

#End Region

End Class
