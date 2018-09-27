Imports Algorithms

Public Class ChannelChat
    Inherits ChatBase

    '
    ' Message management
    '

    Event MessageAdded(message As IRC.Message)

    ReadOnly Property Messages As New CircularBuffer(Of IRC.Message)
    Property PendingMessage As String = ""


    '
    ' Nick management
    '

    Event NicksUpdated(nicknames As SortedDictionary(Of String, String))
    Event NickAdded(nickname As String)
    Event NickChanged(old_nick As String, new_nick As String)
    Event NickModeChanged(nickname As String, prefix As String)
    Event NickRemoved(nickname As String)

    ReadOnly Property Nicks As New SortedDictionary(Of String, String)


    '
    ' Methods
    '

    Sub New(connection As IRC.Connection, channel_name As String)
        MyBase.New(
            New IRC.ChannelChatListener(channel_name),
            New IRC.ChannelChatWriter(connection, channel_name))
        With DirectCast(Listener, IRC.ChannelChatListener)
            .OnMessage = AddressOf HandleMessage
            .OnUserChanged = AddressOf HandleUserChanged
            .OnUserJoined = AddressOf HandleUserJoined
            .OnUserModeChanged = AddressOf HandleUserModeChanged
            .OnUserParted = AddressOf HandleUserParted
            .OnUsersUpdated = AddressOf HandleUsersUpdated
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

    Private Sub HandleUserChanged(old_nick As String, new_nick As String)
        Using lock As New ThreadLock(Nicks)
            If Nicks.ContainsKey(old_nick) Then
                Nicks.Add(new_nick, Nicks(old_nick))
                Nicks.Remove(old_nick)
            End If
        End Using
        RaiseEvent NickChanged(old_nick, new_nick)
    End Sub

    Private Sub HandleUserModeChanged(nickname As String, prefix As String)
        Using lock As New ThreadLock(Nicks)
            If Nicks.ContainsKey(nickname) Then
                Nicks(nickname) = prefix
            End If
        End Using
        RaiseEvent NickModeChanged(nickname, prefix)
    End Sub

    Private Sub HandleUserJoined(nickname As String)
        Using lock As New ThreadLock(Nicks)
            Nicks.Add(nickname, "")
        End Using
        RaiseEvent NickAdded(nickname)
    End Sub

    Private Sub HandleUsersUpdated(nicknames As SortedDictionary(Of String, String))
        Using lock As New ThreadLock(Nicks)
            Nicks.Clear()
            For Each nickname In nicknames
                Nicks.Add(nickname.Key, nickname.Value)
            Next
            RaiseEvent NicksUpdated(Nicks)
        End Using
    End Sub

    Private Sub HandleUserParted(nickname As String)
        Using lock As New ThreadLock(Nicks)
            Nicks.Remove(nickname)
        End Using
        RaiseEvent NickRemoved(nickname)
    End Sub

#End Region

End Class
