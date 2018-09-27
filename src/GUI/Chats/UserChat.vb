Imports Algorithms

Public Class UserChat
    Inherits ChatBase

    '
    ' Message management
    '

    Event MessageAdded(message As IRC.Message)

    ReadOnly Property Messages As New CircularBuffer(Of IRC.Message)
    Property PendingMessage As String = ""

    '
    ' User management
    '

    Event UserChanged(new_nick As String)
    Event UserQuit()

    ReadOnly Property Nickname As String


    '
    ' Methods
    '

    Sub New(connection As IRC.Connection, nickname As String)
        MyBase.New(
            New IRC.UserChatListener(nickname),
            New IRC.UserChatWriter(connection, nickname))
        Me.Nickname = nickname
        With DirectCast(Listener, IRC.UserChatListener)
            .OnMessage = AddressOf HandleMessage
            .OnUserChanged = AddressOf HandleUserChanged
            .OnUserQuit = AddressOf HandleUserQuit
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

    Private Sub HandleUserChanged(new_nick As String)
        RaiseEvent UserChanged(new_nick)
        _Nickname = new_nick
    End Sub

    Private Sub HandleUserQuit()
        RaiseEvent UserQuit()
    End Sub

#End Region

End Class
