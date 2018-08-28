Imports Algorithms

Public Class ChannelChatStorage
    Inherits ChatStorageBase


    Event NicksUpdated(nicknames As SortedSet(Of String))
    Event NickAdded(nickname As String)
    Event NickChanged(old_nick As String, new_nick As String)
    Event NickRemoved(nickname As String)


    ReadOnly Property Nicks As New SortedSet(Of String)


    Sub New(chat As IRC.ChannelChat)
        MyBase.New(chat)
        chat.OnUsersUpdated = AddressOf HandleUsersUpdated
        chat.OnUserJoined = AddressOf HandleUserJoined
        chat.OnUserChanged = AddressOf HandleUserChanged
        chat.OnUserParted = AddressOf HandleUserParted
    End Sub


#Region "Internals"

    Private Sub HandleUsersUpdated(nicknames As SortedSet(Of String))
        Using lock As New ThreadLock(Nicks)
            Nicks.Clear()
            For Each nickname In nicknames
                Nicks.Add(nickname)
            Next
            RaiseEvent NicksUpdated(Nicks)
        End Using
    End Sub

    Private Sub HandleUserJoined(nickname As String)
        Using lock As New ThreadLock(Nicks)
            Nicks.Add(nickname)
        End Using
        RaiseEvent NickAdded(nickname)
    End Sub

    Private Sub HandleUserChanged(old_nick As String, new_nick As String)
        Using lock As New ThreadLock(Nicks)
            If Nicks.Contains(old_nick) Then
                Nicks.Remove(old_nick)
                Nicks.Add(new_nick)
            End If
        End Using
        RaiseEvent NickChanged(old_nick, new_nick)
    End Sub

    Private Sub HandleUserParted(nickname As String)
        Using lock As New ThreadLock(Nicks)
            Nicks.Remove(nickname)
        End Using
        RaiseEvent NickRemoved(nickname)
    End Sub

#End Region

End Class
