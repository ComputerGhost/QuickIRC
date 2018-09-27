Imports System.Text
Imports System.Text.RegularExpressions
Imports Algorithms

Public Class MessageList

    Property DisplayFriendlyMessages As Boolean = True

    Property DateFormat As String = "D"
    Property TimeFormat As String = "HH:mm:ss"

    ' Message parts' colors
    Property DirectionColor = Color.IndianRed
    Property TimeColor = Color.LightSlateGray
    Property VerbColor = Color.DarkGreen

    ' Types of sources
    Property BotColor = Color.MediumSlateBlue
    Property ClientColor = Color.DarkSlateBlue
    Property MeColor = Color.MediumSlateBlue
    Property UserColor = Color.MediumSlateBlue

    ' Types of messages
    Property CTCPColor = Color.MediumVioletRed
    Property JoinColor = Color.SlateGray
    Property MessageColor = Color.Black
    Property NickColor = Color.SlateBlue
    Property PartColor = Color.SlateGray


    Sub AddMessage(message As IRC.Message)
        If Not ShouldProcess(message) Then
            Exit Sub
        End If
        AppendMessage(message)
        rtfMessages.Select(rtfMessages.TextLength, 0)
        rtfMessages.ScrollToCaret()
    End Sub

    Sub AddMessages(messages As CircularBuffer(Of IRC.Message))
        Using lock As New ThreadLock(messages)
            For Each message In messages
                AddMessage(message)
            Next
        End Using
        rtfMessages.Select(rtfMessages.TextLength, 0)
        rtfMessages.ScrollToCaret()
    End Sub

    Sub Clear()
        rtfMessages.Rtf = "{\rtf1\ansi\deff0}"
        LastDate = Date.MinValue
    End Sub

#Region "Internals"

    Private LastDate As Date = Date.MinValue

    Private Sub AppendMessage(message As IRC.Message)

        Dim builder As New RtfBuilder
        builder.ParagraphSpacing = 72
        builder.LineIndent = 936
        builder.FirstLineIndent = -936
        builder.LineSpacing = 1.25

        ' Since our timestamp doesn't go above hours, notify when switching days
        If LastDate <> Today Then
            LastDate = Today
            builder.StartParagraph()
            builder.AlignCenter()
            builder.SetColor(TimeColor)
            builder.AppendFormat("— {0} —", Today.ToString(DateFormat))
            builder.EndParagraph()
        End If

        ' Start the message line
        builder.StartParagraph()

        ' direction
        builder.SetColor(DirectionColor)
        Select Case message.Direction
            Case IRC.MessageDirection.Incoming
                builder.Append("-> ")
            Case IRC.MessageDirection.Outgoing
                builder.Append("<- ")
            Case Else
                builder.Append("-!- ")
        End Select

        ' timestamp
        builder.SetColor(TimeColor)
        builder.Append(message.TimeStamp.ToString(TimeFormat))
        builder.Append(" "c)

        ' message
        If DisplayFriendlyMessages Then
            AppendFriendlyMessage(builder, message)
        Else
            AppendRawMessage(builder, message)
        End If

        ' End paragraph and add in the RTF
        builder.EndParagraph()
        rtfMessages.AppendRtf(builder.ToString())

    End Sub

    Private Sub AppendFriendlyMessage(builder As RtfBuilder, message As IRC.Message)
        Select Case message.Verb

            Case "JOIN"
                builder.SetColor(JoinColor)
                builder.AppendFormat(
                    "{0} ({1}) has joined the channel.",
                    message.Source.Name, message.Source.Raw)

            Case "NICK"
                builder.SetColor(NickColor)
                builder.AppendFormat(
                    "{0} is now known as {1}.",
                    message.Source.Name,
                    message.Parameters(0))

            Case "PRIVMSG", "NOTICE"

                Dim text = message.Parameters(1)

                If text.StartsWith(ChrW(1) & "ACTION") Then
                    builder.SetColor(GetSourceColor(message))
                    builder.AppendFormat("{0} ", message.Source.Name)
                    builder.SetColor(MessageColor)
                    builder.Append(text.Substring(7).TrimEnd(ChrW(1)))

                ElseIf text.StartsWith(ChrW(1)) Then

                    builder.SetColor(GetSourceColor(message))
                    builder.Append(message.Source.Name)
                    builder.SetColor(MessageColor)
                    builder.Append(": ")

                    ' <\01><command> [params][\01]
                    Dim tokenizer As New IRC.Tokenizer(text)
                    tokenizer.Skip(ChrW(1))
                    Dim verb = tokenizer.ReadWord().ToUpper().TrimEnd(ChrW(1))
                    tokenizer.Skip(" "c)
                    text = tokenizer.ReadRemaining()?.TrimEnd(ChrW(1))

                    If verb IsNot Nothing Then
                        builder.SetColor(CTCPColor)
                        builder.Append(verb)
                    End If
                    If text IsNot Nothing Then
                        builder.SetColor(MessageColor)
                        builder.AppendFormat(" {0}", text)
                    End If

                Else
                    builder.SetColor(GetSourceColor(message))
                    builder.Append(If(message.Source.Name, ""))
                    builder.SetColor(MessageColor)
                    builder.AppendFormat(": {0}", text)
                End If

            Case Nothing ' Message from our code
                builder.SetColor(ClientColor)
                builder.Append(message.Parameters(0))

            Case Else

                If IsNumeric(message.Verb) Then ' server message

                    builder.SetColor(VerbColor)
                    builder.AppendFormat("{0}", message.Verb)

                    builder.SetColor(MessageColor)
                    For i = 1 To message.Parameters.Count - 1
                        builder.AppendFormat(" {0}", message.Parameters(i))
                    Next

                Else ' other command
                    AppendRawMessage(builder, message)
                End If

        End Select
    End Sub

    Private Sub AppendRawMessage(builder As RtfBuilder, message As IRC.Message)

        If message.Source.Name IsNot Nothing Then
            builder.SetColor(GetSourceColor(message))
            builder.AppendFormat("{0} ", message.Source.Name)
        End If

        builder.SetColor(VerbColor)
        builder.AppendFormat("{0}", message.Verb)

        builder.SetColor(MessageColor)
        For i = 0 To message.Parameters.Count - 1
            builder.AppendFormat(" {0}", message.Parameters(i))
        Next

    End Sub

    Private Function GetSourceColor(message As IRC.Message)
        Select Case message.Direction
            Case IRC.MessageDirection.Client
                Return ClientColor
            Case IRC.MessageDirection.Incoming
                If message.Verb = "NOTICE" Then
                    Return BotColor
                Else
                    Return UserColor
                End If
            Case IRC.MessageDirection.Outgoing
                Return MeColor
            Case Else
                Return Color.Red
        End Select
    End Function

    Private Function ShouldProcess(message As IRC.Message)

        If Not DisplayFriendlyMessages Then
            Return True
        End If

        Dim ignored = {"PING", "PONG"}
        Return Not ignored.Contains(message.Verb)

    End Function

#End Region

End Class
