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
        InternalRegister(New SyntaxListener())
        InternalRegister(New RedactListener())
        InternalRegister(New FloodListener())
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
        Task.Run(Sub() InternalInjectSent(message))
    End Sub

    ' Inject a raw message, process as if it was sent, but don't send.
    Sub InjectSentLine(raw As String)
        Dim fake = Message.Parse(raw)
        fake.Source = New MessageSource(Nickname)
        InjectSentMessage(fake)
    End Sub

    ' Post a message from our client to the user
    Sub PostClientMessage(text As String)
        Task.Run(Sub() InternalPost(New Message With {
            .Source = MessageSource.Parse("QuickIRC"),
            .Verb = "CLIENT_MSG",
            .Parameters = {text}.ToList()}))
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
    ' Listener handler methods
    '

    ' Register a listener for this connection.
    Sub RegisterListener(listener As ListenerBase)
        Task.Run(Sub() InternalRegister(listener))
    End Sub

    ' Unregister a listener for this connection. Please ensure that the 
    ' listener does not interface with Connection after this call.
    Sub UnregisterListener(listener As ListenerBase)
        Task.Run(Sub() InternalUnregister(listener))
    End Sub


#Region "Internals"

    Private InternalConnection As InternalConnection
    Private ConnectionLock As New Object

    Private Listeners As New Algorithms.List(Of ListenerBase)


    Private Sub InternalRegister(listener As ListenerBase)
        Using lock As New ThreadLock(Listeners)
            listener.HandleRegistration(Me)
            Listeners.Add(listener)
        End Using
    End Sub

    Private Sub InternalUnregister(listener As ListenerBase)
        Using lock As New ThreadLock(Listeners)
            Listeners.Remove(listener)
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
                ' Note: I don't think this is ever reached.
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

    Private Sub InternalInjectSent(message As Message)

        message.Direction = MessageDirection.Outgoing
        message.Source = New MessageSource(Nickname)

        Using lock As New ThreadLock(Listeners, MessageQueue)
            If QueueMessages Then
                MessageQueue.Enqueue(message)
            Else
                For Each listener In Listeners
                    listener.HandleMessageSent(message)
                Next
            End If
        End Using

    End Sub

    Private Sub InternalPost(message As Message)
        Using lock As New ThreadLock(Listeners, MessageQueue)
            If QueueMessages Then
                MessageQueue.Enqueue(message)
            Else
                For Each listener In Listeners
                    listener.HandleMessageReceived(message)
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

        Using lock As New ThreadLock(Listeners, MessageQueue)
            If do_notify Then
                If QueueMessages Then
                    MessageQueue.Enqueue(message)
                Else
                    For Each listener In Listeners
                        listener.HandleMessageSent(message)
                    Next
                End If
            End If
        End Using

    End Sub


    '
    ' Defer our messages to ourselves, if in the middle of other event.
    '

    Private MessageQueue As New Algorithms.Queue(Of Message)
    Private QueueMessages As Boolean = False

    ' Starts queueing messages by us.
    ' Note: Assumes MessageQueue is thread-locked
    Sub StartQueue()
        QueueMessages = True
    End Sub

    ' Stops queueing messages by us and flushes current ones.
    ' Note: Assumes MessageQueue is thread-locked
    Sub StopQueue()
        While MessageQueue.Count
            Dim message = MessageQueue.Dequeue()
            For Each listener In Listeners
                If message.Direction = MessageDirection.Outgoing Then
                    listener.HandleMessageSent(message)
                Else
                    listener.HandleMessageSent(message)
                End If
            Next
        End While
        QueueMessages = False
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

            Using lock As New ThreadLock(Listeners, MessageQueue)
                Try
                    StartQueue()
                    For Each listener In Listeners
                        listener.HandleConnected()
                    Next
                Finally
                    StopQueue()
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

            Using lock As New ThreadLock(Listeners, MessageQueue)
                Try
                    StartQueue()
                    For Each listener In Listeners
                        listener.HandleDisconnected()
                    Next
                    PostClientMessage(ex.Message)
                Finally
                    StopQueue()
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

            Using lock As New ThreadLock(Listeners)
                For Each listener In Listeners
                    listener.HandleDisconnected()
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

            Using locks As New ThreadLock(Listeners, MessageQueue)
                Try
                    StartQueue()
                    For Each listener In Listeners
                        listener.HandleMessageReceived(msg)
                    Next
                Catch ex As Exception When _
                    TypeOf ex Is FloodException OrElse
                    TypeOf ex Is LimitException OrElse
                    TypeOf ex Is NotImplementedException
                    PostClientMessage(ex.Message)
                Finally
                    StopQueue()
                End Try
            End Using

        End SyncLock
    End Sub

#End Region

End Class
