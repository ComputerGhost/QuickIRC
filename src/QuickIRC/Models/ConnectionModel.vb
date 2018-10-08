Public Class ConnectionModel

    ' Not useful for connecting but may be used to show the user the name.
    Property Name As String

    ' Connection info
    Property Address As String
    Property Port As Integer = 6667

    ' Commands to send
    Property PreCommands As New List(Of String)
    Property NickAlternates As New List(Of String)
    Property PostCommands As New List(Of String)

End Class
