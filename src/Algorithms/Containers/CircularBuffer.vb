Imports System.Threading

Public Class CircularBuffer(Of T)
    Implements IList(Of T), ILockable


    ReadOnly Property Capacity As Integer
        Get
            Return Buffer.Length
        End Get
    End Property


    Sub New()
        ReDim Buffer(1000 - 1)
    End Sub

    Sub New(capacity As Integer)
        ReDim Buffer(capacity - 1)
    End Sub

    Sub New(other As CircularBuffer(Of T))
        ReDim Buffer(other.Capacity)
        For Each element In other
            Add(element)
        Next
    End Sub


    ' Locks usage to just this thread.
    ' Release *must* *always* be called afterwards.
    Sub Lock() Implements ILockable.Lock
        Monitor.Enter(UseLock)
    End Sub

    ' Release for use by other threads.
    Sub Release() Implements ILockable.Release
        Monitor.Exit(UseLock)
    End Sub



#Region "Internals"

    Private Buffer() As T
    Private Head As Integer = -1    ' end, where we're writing
    Private Tail As Integer = 0     ' start
    Private _Count As Integer = 0

    Private UseLock As New Object

#End Region

#Region "IList interface"

    Default Public Property Item(index As Integer) As T Implements IList(Of T).Item
        Get
            If index >= _Count Then
                Throw New IndexOutOfRangeException()
            End If
            Return Buffer((Tail + index) Mod Capacity)
        End Get
        Set(value As T)
            If index >= _Count Then
                Buffer((Tail + index) Mod Capacity) = value
            End If
        End Set
    End Property

    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
        Get
            Return _Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Public Function IndexOf(item As T) As Integer Implements IList(Of T).IndexOf
        Throw New NotImplementedException()
    End Function

    Public Sub Insert(index As Integer, item As T) Implements IList(Of T).Insert
        Throw New NotImplementedException()
    End Sub

    Public Sub RemoveAt(index As Integer) Implements IList(Of T).RemoveAt
        Throw New NotImplementedException()
    End Sub

    Public Sub Add(item As T) Implements ICollection(Of T).Add
        Head = (Head + 1) Mod Capacity
        Buffer(Head) = item
        If Count = Capacity Then
            Tail = (Tail + 1) Mod Capacity
        Else
            _Count += 1
        End If
    End Sub

    Public Sub Clear() Implements ICollection(Of T).Clear
        _Count = 0
        Head = -1
        Tail = 0
    End Sub

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        Throw New NotImplementedException()
    End Function

    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        Throw New NotImplementedException()
    End Sub

    Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        Throw New NotImplementedException()
    End Function

    Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return New CircularBufferEnumerator(Of T)(Buffer, Tail, Count)
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return New CircularBufferEnumerator(Of T)(Buffer, Tail, Count)
    End Function

#End Region

End Class
