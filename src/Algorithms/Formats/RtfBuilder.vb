Imports System.Drawing
Imports System.Text

Public Class RtfBuilder

    Property ParagraphSpacing As Integer = 72
    Property FirstLineIndent As Integer = 0
    Property LineIndent As Integer = 0
    Property PagePadding As Integer = 36


    Public Sub AlignCenter()
        TextBuilder.Append("\qc ")
    End Sub

    ' Sanitizes and append character
    Public Sub Append(character As Char)
        If {"\"c, "{"c, "}"c}.Contains(character) Then
            TextBuilder.Append("\"c)
        End If
        TextBuilder.Append(character)
    End Sub

    ' Sanitizes and appends text
    Public Sub Append(text As String)
        For Each character In text
            Append(character)
        Next
    End Sub

    Public Sub AppendFormat(format As String, ParamArray args() As Object)
        Append(String.Format(format, args))
    End Sub

    Public Sub SetColor(color As Color)

        Dim color_index = Colors.IndexOf(color)
        If color_index = -1 Then
            color_index = Colors.Count
            Colors.Add(color)
        End If

        TextBuilder.AppendFormat("\cf{0} ", color_index)

    End Sub

    Public Sub EndParagraph()
        TextBuilder.Append("\par")
    End Sub

    ' Reset formatting for the new paragraph
    Public Sub StartParagraph()
        TextBuilder.AppendFormat(
            "\pard \fi{0}\li{1}\sb{2}\ri{3} ",
            FirstLineIndent + PagePadding,
            LineIndent + PagePadding,
            ParagraphSpacing,
            PagePadding)
    End Sub

    Public Overrides Function ToString() As String

        Dim builder As New StringBuilder
        builder.Append("{\rtf1\ansi\deff0")

        builder.Append("{\colortbl")
        For i = 0 To Colors.Count - 1
            Dim color = Colors(i)
            builder.AppendFormat("\red{0}\green{1}\blue{2};", color.R, color.G, color.B)
        Next
        builder.Append("}")

        builder.Append(TextBuilder.ToString())

        builder.Append("}")
        Return builder.ToString()

    End Function


#Region "Internals"

    Private Colors As New List(Of Color)

    Private TextBuilder As New StringBuilder

#End Region

End Class
