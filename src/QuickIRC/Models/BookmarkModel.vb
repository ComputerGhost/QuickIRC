Public Class BookmarkModel

    Property Name As String

    Property ServerAddress As String
    Property ServerPort As Integer = 6667
    Property ServerPass As String

    Property Nickname As String
    Property NickAlternates As List(Of String)
    Property NickPass As String

    Property Username As String
    Property UserMode As Integer = 8
    Property RealName As String = "QuickIRC User"

    Property Channels As List(Of String)


    Public Overrides Function ToString() As String
        Return Name
    End Function

End Class
