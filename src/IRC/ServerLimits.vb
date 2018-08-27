Public Structure ServerLimits

    ReadOnly Property ChanTypes As String
    ReadOnly Property MessageLen As Integer
    ReadOnly Property NickLen As Integer


    Shared Function GetDefault() As ServerLimits
        Return New ServerLimits With {
            ._ChanTypes = "&#+!",
            ._MessageLen = 512,
            ._NickLen = 9}
    End Function


End Structure
