Public Class BookmarksModel

    Public Shared CurrentFormatVersion As New VersionModel(1, 0, 0)

    Property FormatVersion As VersionModel = CurrentFormatVersion

    Property Bookmarks As New List(Of BookmarkModel)

End Class
