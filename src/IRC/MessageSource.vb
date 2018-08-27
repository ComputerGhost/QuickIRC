Imports System.Text.RegularExpressions

Public Structure MessageSource

    Property Raw As String
    Property Name As String
    Property User As String
    Property Host As String


    Shared Function Parse(unparsed As String) As MessageSource

        Dim groups = Regex.Match(unparsed, "^(.*?)(?:!(.*?))?(?:@(.*?))?$").Groups
        If Not groups(0).Success Then
            Throw New FormatException("Could not parse source.")
        End If

        Return New MessageSource With {
            .Raw = unparsed,
            .Name = groups(1).Value,
            .User = groups(2).Value,
            .Host = groups(3).Value}

    End Function


    Sub New(nickname As String)
        Raw = nickname
        Name = nickname
    End Sub


End Structure
