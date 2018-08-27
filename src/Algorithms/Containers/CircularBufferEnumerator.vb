Public Class CircularBufferEnumerator(Of T)
    Implements IEnumerator(Of T)


    Friend Sub New(buffer() As T, Start As Integer, Count As Integer)
        Me.Buffer = buffer
        Me.Start = Start
        Me.Count = Count
        Me.Offset = Start - 1
    End Sub

#Region "Internals"

    Private Buffer() As T
    Private Start As Integer
    Private Count As Integer
    Private Offset As Integer

#End Region

#Region "IEnumerator interface"

    Public ReadOnly Property Current As T Implements IEnumerator(Of T).Current
        Get
            Return Buffer(Offset Mod Buffer.Length)
        End Get
    End Property

    Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
        Get
            Return Buffer(Offset Mod Buffer.Length)
        End Get
    End Property

    Public Sub Reset() Implements IEnumerator.Reset
        Offset = Start - 1
    End Sub

    Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
        If Offset - Start + 1 = Count Then
            Return False
        Else
            Offset += 1
            Return True
        End If
    End Function

#End Region

#Region "IDisposable interface"

    ' I don't know why VS generated the mess of code below, but I reckon I'll
    ' leave it as it. It looks super important!


    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class
