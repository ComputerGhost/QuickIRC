'
' While ThreadLock exists, its targets cannot be accessed by other threads.
'
Public Class ThreadLock
    Implements IDisposable

    Private Targets() As ILockable

    Sub New(ParamArray targets() As ILockable)
        Me.Targets = targets
        For Each target In targets
            target.Lock()
        Next
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        For Each target In Targets
            target.Release()
        Next
    End Sub

End Class
