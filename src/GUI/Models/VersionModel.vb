Public Class VersionModel

    Property Major As Integer
    Property Minor As Integer
    Property Patch As Integer

    Sub New(major As Integer, minor As Integer, patch As Integer)
        Me.Major = major
        Me.Minor = minor
        Me.Patch = patch
    End Sub

    Shared Operator <(a As VersionModel, b As VersionModel) As Boolean
        If a.Major < b.Major Then Return True
        If a.Major > b.Major Then Return False
        If a.Minor < b.Minor Then Return True
        If a.Minor > b.Minor Then Return False
        If a.Patch < b.Patch Then Return True
        If a.Patch > b.Patch Then Return False
        Return False ' equal
    End Operator

    Shared Operator >(a As VersionModel, b As VersionModel) As Boolean
        If a.Major > b.Major Then Return True
        If a.Major < b.Major Then Return False
        If a.Minor > b.Minor Then Return True
        If a.Minor < b.Minor Then Return False
        If a.Patch > b.Patch Then Return True
        If a.Patch < b.Patch Then Return False
        Return False ' equal
    End Operator

End Class
