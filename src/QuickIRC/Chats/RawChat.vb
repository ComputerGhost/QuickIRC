Imports Algorithms

Public Class RawChat
    Inherits ChatBase

    '
    ' Message management
    '

    Event MessageAdded(message As IRC.Message)

    ReadOnly Property Messages As New CircularBuffer(Of IRC.Message)
    Property PendingMessage As String = ""


    '
    ' Methods
    '

    Sub New(connection As IRC.Connection)
        MyBase.New(
            New IRC.RawChatListener(),
            New IRC.WriterBase(connection))
        With DirectCast(Listener, IRC.RawChatListener)
            .OnMessage = AddressOf HandleMessage
        End With
        connection.RegisterListener(Listener)
    End Sub

#Region "Internals"

    Private Sub HandleMessage(message As IRC.Message)
        Using lock As New ThreadLock(Messages)
            Messages.Add(message)
        End Using
        RaiseEvent MessageAdded(message)
    End Sub

#End Region

End Class
