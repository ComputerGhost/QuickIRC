'
' Manages a connection to an IRC server.
'
' Chats should be registered to receive events, and their priority depends on
' the order in which they are registered. The first event will be either a 
' connected or failure event. If connected, message events are passed along,
' finishing with either a closed or failure event.
'
' Do note that, while this class is thread-safe, it may call delegates on any
' thread(s). Those delegates need to be thread-safe too.
'
Imports Algorithms

Public Class Connection
    Implements IDisposable


    ReadOnly Property Server As String
    ReadOnly Property Port As String
    Property ServerLimits As ServerLimits

    Property Nickname As String
    Property UserHost As String


    Sub New(server As String, port As Integer)
        Me.Server = server
        Me.Port = port
        ServerLimits = ServerLimits.GetDefault()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        SyncLock ConnectionLock
            InternalConnection.Dispose()
        End SyncLock
    End Sub


    ' Asynchronously connects and starts receiving data
    Sub Connect()
        Task.Run(Sub() InternalConnect(Server, Port))
    End Sub

    ' Disconnects in the background. It is safe to reconnect immediately, but
    ' doing so will stop the disconnected event from being raised.
    Sub Disconnect()
        Task.Run(Sub() InternalDisconnect())
    End Sub

    ' Inject a message, process as if it was sent, but don't send.
    Sub InjectSentMessage(message As Message)
        Task.Run(Sub() InternalInject(message))
    End Sub

    ' Inject a raw message, process as if it was sent, but don't send.
    Sub InjectSentLine(raw As String)
        Dim fake = Message.Parse(raw)
        fake.Source = New MessageSource(Nickname)
        InjectSentMessage(fake)
    End Sub

    ' Sends a CTCP request to a target, and notify listeners of this
    Sub SendCTCPRequest(target As String, command As String, Optional parameters As String = Nothing)
        If parameters Is Nothing Then
            SendLine(String.Format("PRIVMSG {1} :{0}{2}{0}", ChrW(1), target, command))
        Else
            SendLine(String.Format("PRIVMSG {1} :{0}{2} {3}{0}", ChrW(1), target, command, parameters))
        End If
    End Sub

    ' Sends a CTCP response to a target, and notify listeners of this
    Sub SendCTCPResponse(target As String, command As String, Optional parameters As String = Nothing)
        If parameters Is Nothing Then
            SendLine(String.Format("NOTICE {1} :{0}{2}{0}", ChrW(1), target, command))
        Else
            SendLine(String.Format("NOTICE {1} :{0}{2} {3}{0}", ChrW(1), target, command, parameters))
        End If
    End Sub

    ' Send a message to the IRC server, and notify listeners of this
    Sub SendMessage(message As Message, Optional do_notify As Boolean = True)
        Task.Run(Sub() InternalSend(message, do_notify))
    End Sub

    ' Send a raw message to the IRC server and notify listeners of this.
    ' This will throw a SyntaxException if it is invalid.
    Sub SendLine(raw As String, Optional do_notify As Boolean = True)
        Dim outgoing = Message.Parse(raw)
        outgoing.Source = New MessageSource(Nickname)
        SendMessage(outgoing, do_notify)
    End Sub


    '
    ' Chat handler methods
    '

    ' Register a chatroom for this connection.
    Sub RegisterChat(chat As ChatBase)
        Task.Run(Sub() InternalRegister(chat))
    End Sub

    ' Unregister a chatroom for this connection. Please ensure that the chat 
    ' does not interface with Connection after this call.
    Sub UnregisterChat(chat As ChatBase)
        Task.Run(Sub() InternalUnregister(chat))
    End Sub


#Region "Internals"

    Private InternalConnection As InternalConnection
    Private ConnectionLock As New Object

    Private Chats As New Algorithms.List(Of ChatBase)


    Private Sub InternalRegister(chat As ChatBase)
        Using lock As New ThreadLock(Chats)
            chat.Register(Me)
            Chats.Add(chat)
        End Using
    End Sub

    Private Sub InternalUnregister(chat As ChatBase)
        Using lock As New ThreadLock(Chats)
            Chats.Remove(chat)
        End Using
    End Sub

    ' Connects and starts listening
    Private Sub InternalConnect(server As String, port As Integer)
        Dim internal = InternalConnection
        Try
            SyncLock ConnectionLock
                InternalConnection?.Dispose()
                InternalConnection = New InternalConnection()
                internal = InternalConnection
            End SyncLock

            If Not internal.Connect(server, port) Then
                HandleFailedConnect(internal, New Exception("Connection could not be established."))
                Exit Sub
            End If
            HandleConnected(internal)

            Dim receipt_handler = Sub(line As String) HandleLineReceived(internal, line)
            InternalConnection.Listen(receipt_handler)

            HandleDisconnected(internal)

        Catch ex As Exception
            HandleFailedConnect(internal, ex)
        End Try
    End Sub

    Private Sub InternalDisconnect()
        SyncLock ConnectionLock
            InternalConnection?.Disconnect()
        End SyncLock
    End Sub

    Private Sub InternalInject(message As Message)

        message.Direction = MessageDirection.Outgoing
        message.Source = New MessageSource(Nickname)

        Using lock As New ThreadLock(Chats, SentNotificationQueue)
            If QueueSentNotifications Then
                SentNotificationQueue.Enqueue(message)
            Else
                For Each chat In Chats
                    chat.HandleMessageSent(message)
                Next
            End If
        End Using

    End Sub

    Private Sub InternalSend(message As Message, do_notify As Boolean)

        message.Direction = MessageDirection.Outgoing
        message.Source = New MessageSource(Nickname)

        SyncLock ConnectionLock
            InternalConnection?.SendLine(message.Raw)
        End SyncLock

        Using lock As New ThreadLock(Chats, SentNotificationQueue)
            If do_notify Then
                If QueueSentNotifications Then
                    SentNotificationQueue.Enqueue(message)
                Else
                    For Each chat In Chats
                        chat.HandleMessageSent(message)
                    Next
                End If
            End If
        End Using

    End Sub


    '
    ' Defer notifications of sent messages, if in the middle of other event.
    '

    Private SentNotificationQueue As New Algorithms.Queue(Of Message)
    Private QueueSentNotifications As Boolean = False

    ' Starts queueing sent notifications.
    ' Note: Assumes SentNotificationQueue is thread-locked
    Sub StartSentQueue()
        QueueSentNotifications = True
    End Sub

    ' Stops queueing sent notifications and flushes current notifications.
    ' Note: Assumes SentNotificationQueue is thread-locked
    Sub StopSentQueue()
        While SentNotificationQueue.Count
            Dim message = SentNotificationQueue.Dequeue()
            For Each chat In Chats
                chat.HandleMessageSent(message)
            Next
        End While
        QueueSentNotifications = False
    End Sub


    '
    ' Event handling
    '

    ' If we are still on the same connection, signal we are connected
    Private Sub HandleConnected(internal As InternalConnection)
        SyncLock ConnectionLock

            If internal IsNot Me.InternalConnection Then
                Exit Sub
            End If

            Using lock As New ThreadLock(Chats, SentNotificationQueue)
                Try
                    StartSentQueue()
                    For Each chat In Chats
                        chat.HandleConnectionOpened()
                    Next
                Finally
                    StopSentQueue()
                End Try
            End Using

        End SyncLock
    End Sub

    ' If we are still on the same connection, signal connection failed.
    Private Sub HandleFailedConnect(internal As InternalConnection, ex As Exception)
        SyncLock ConnectionLock

            If internal IsNot Me.InternalConnection Then
                Exit Sub
            End If

            Using lock As New ThreadLock(Chats, SentNotificationQueue)
                Try
                    StartSentQueue()
                    For Each chat In Chats
                        chat.HandleConnectionFailed(ex)
                    Next
                Finally
                    StopSentQueue()
                End Try
            End Using

        End SyncLock
    End Sub

    ' If we are still on the same connection, signal disconnected.
    Private Sub HandleDisconnected(internal As InternalConnection)
        SyncLock ConnectionLock

            If internal IsNot Me.InternalConnection Then
                Exit Sub
            End If

            Using lock As New ThreadLock(Chats)
                For Each chat In Chats
                    chat.HandleConnectionClosed()
                Next
            End Using

        End SyncLock
    End Sub

    ' If we are still on the same connection, signal message received.
    Private Sub HandleLineReceived(internal As InternalConnection, line As String)
        SyncLock ConnectionLock

            If internal IsNot Me.InternalConnection Then
                Exit Sub
            End If

            Dim msg = Message.Parse(line)
            msg.Direction = MessageDirection.Incoming

            Using locks As New ThreadLock(Chats, SentNotificationQueue)
                Try
                    StartSentQueue()
                    For Each chat In Chats
                        chat.HandleMessageReceived(msg)
                    Next
                Catch ex As Exception When _
                    TypeOf ex Is FloodException OrElse
                    TypeOf ex Is LimitException OrElse
                    TypeOf ex Is NotImplementedException
                    For Each chat In Chats
                        chat.HandleExceptionHappened(ex)
                    Next
                Finally
                    StopSentQueue()
                End Try
            End Using

        End SyncLock
    End Sub

#End Region

End Class
