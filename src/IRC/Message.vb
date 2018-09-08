Imports System.Text
Imports System.Text.RegularExpressions

Public Class Message

    Property TimeStamp As Date = Now
    Property Direction As MessageDirection
    Property Source As MessageSource
    Property Verb As String
    Property Parameters As New List(Of String)

    ReadOnly Property Raw As String
        Get
            Return If(_Raw, GetRaw())
        End Get
    End Property
    Private _Raw As String


    Shared Function Parse(line As String, direction As MessageDirection) As Message

        ' [tags] [source] <command> [parameters]
        Dim groups = Regex.Match(line, "^(?:@([^ ]+) )?(?::([^ ]+) )?([^ ]+) ?(.*?)$").Groups
        If Not groups(0).Success Then
            Throw New SyntaxError("Could not parse message.")
        End If

        Dim ret = New Message With {
            ._Raw = line,
            .Direction = direction,
            .Source = MessageSource.Parse(groups(2).Value),
            .Verb = groups(3).Value.ToUpper()}

        Dim current_param As New StringBuilder
        Dim colon_encountered = False
        For Each current_char In groups(4).Value
            If colon_encountered Then
                current_param.Append(current_char)
            ElseIf current_char = ":"c Then
                colon_encountered = True
            ElseIf current_char = " "c Then
                ret.Parameters.Add(current_param.ToString())
                current_param.Clear()
            Else
                current_param.Append(current_char)
            End If
        Next
        If current_param.Length Or colon_encountered Then
            ret.Parameters.Add(current_param.ToString())
        End If

        Return ret

    End Function


#Region "Internals"

    ' Formats for sending it over the network.
    Private Function GetRaw() As String

        Dim raw As New StringBuilder()

        raw.Append(Verb)
        raw.Append(" "c)

        ' Middle parameters
        For i = 0 To Parameters.Count - 2
            raw.Append(Parameters(i))
            raw.Append(" "c)
        Next

        ' Last parameter, prefixed with ':' if text
        For Each c In Parameters.Last()
            If Char.IsWhiteSpace(c) Then
                raw.Append(":"c)
                Exit For
            End If
        Next
        raw.Append(Parameters.Last())

        Return raw.ToString()

    End Function

#End Region


End Class
