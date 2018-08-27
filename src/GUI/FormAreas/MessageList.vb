Imports System.Runtime.InteropServices
Imports System.Text

Public Class MessageList

    Sub New()
        InitializeComponent()
        _Chat_MessageAdded = AddressOf Chat_MessageAdded
    End Sub

    Sub BindToChat(chat As ChatStorage)

        If BoundData IsNot Nothing Then
            RemoveHandler BoundData.MessageAdded, _Chat_MessageAdded
        End If

        BoundData = chat

        MessagesView.Clear()
        If BoundData IsNot Nothing Then
            AddMessages(BoundData.Messages)
            AddHandler BoundData.MessageAdded, _Chat_MessageAdded
        End If

    End Sub


#Region "Bound chat"

    Private BoundData As ChatStorage

    Private _Chat_MessageAdded As ChatStorage.MessageAddedEventHandler
    Private Sub Chat_MessageAdded(message As IRC.Message)

        If InvokeRequired Then
            Invoke(Sub() Chat_MessageAdded(message))
            Exit Sub
        End If

        AddMessage(message)

    End Sub

#End Region

#Region "Internals"

    Private Sub AddMessage(message As IRC.Message)

        If MessagesView.Text.Length Then
            MessagesView.AppendText(vbCrLf)
        End If

        MessagesView.AppendText(FormatMessage(message))

    End Sub

    Private Sub AddMessages(messages As IEnumerable(Of IRC.Message))

        If MessagesView.Text.Length Then
            MessagesView.AppendText(vbCrLf)
        End If

        Dim builder As New StringBuilder()
        For Each message In messages
            FormatMessage(builder, message)
            builder.Append(vbCrLf)
        Next
        MessagesView.AppendText(builder.ToString())

    End Sub

    Private Function FormatMessage(message As IRC.Message)
        Dim builder As New StringBuilder()
        FormatMessage(builder, message)
        Return builder.ToString()
    End Function

    Private Sub FormatMessage(builder As StringBuilder, message As IRC.Message)
        builder.Append(message.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss "))
        builder.Append(message.Raw)
    End Sub

#End Region

End Class
