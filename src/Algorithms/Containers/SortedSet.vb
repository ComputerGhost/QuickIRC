Imports System.Threading

Public Class SortedSet(Of T)
    Inherits Generic.SortedSet(Of T)
    Implements ILockable


    ' Locks usage to just this thread.
    ' Release *must* *always* be called afterwards.
    Sub Lock() Implements ILockable.Lock
        Monitor.Enter(UseLock)
    End Sub

    ' Release for use by other threads.
    Sub Release() Implements ILockable.Release
        Monitor.Exit(UseLock)
    End Sub


    Private UseLock As New Object

End Class
