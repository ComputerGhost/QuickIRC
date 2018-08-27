Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

'
' This class is separate from Connect because Connect needs to forget about 
' its connection in order to be reused immediately. Unfortunately, 
' disconnecting may not happen immediately. This class pulls out that logic 
' so that it will work right.
'
' InternalConnection is thread-friendly but not thread-safe. The methods can 
' be called from any thread but not at the same time. The exceptions are the 
' properties and the Listen method, which are thread safe and may be used at 
' any time.
'
' All methods may be called after Dispose, but they will have no effect.
' 
Friend Class InternalConnection
    Implements IDisposable

    Delegate Sub LineReceivedDelegate(line As String)

    ReadOnly Property IsConnected As Boolean
        Get
            Return If(Client?.Connected, False)
        End Get
    End Property


    Public Sub Dispose() Implements IDisposable.Dispose
        Disconnect()
    End Sub


    Public Function Connect(server As String, port As Integer) As Boolean
        Client?.ConnectAsync(server, port)?.Wait()
        Return If(Client?.Connected, False)
    End Function

    ' Disconnects and cancels running operations, though this may complete 
    ' some time after the method returns.
    Public Sub Disconnect()

        Client?.Dispose()
        Client = Nothing

        Canceler?.Cancel()
        Canceler?.Dispose()
        Canceler = Nothing

    End Sub

    Public Sub SendLine(line As String)

        If Client Is Nothing Then
            Exit Sub
        End If

        Dim bytes = Encoding.UTF8.GetBytes(line & vbCrLf)
        Dim stream = Client.GetStream()
        stream.Write(bytes, 0, bytes.Length)

    End Sub

    ' Listen for text until the connection closes.
    Public Sub Listen(receipt_handler As LineReceivedDelegate)

        Dim stream = Client?.GetStream()
        Dim decoder = Encoding.UTF8.GetDecoder()
        Dim partial_message As String = ""

        Const BUFFER_SIZE = 1024
        Dim byte_buffer(BUFFER_SIZE) As Byte
        Dim char_buffer(BUFFER_SIZE) As Char

        While Not CancelToken.IsCancellationRequested

            ' Wait until the stream has data available or was canceled. This 
            ' is needed because the simpler stream.Read* will only unblock 
            ' after a network event is processed.
            Const POLL_INTERVAL = 100
            While Not stream.DataAvailable
                Task.Delay(POLL_INTERVAL, CancelToken).Wait()
                If CancelToken.IsCancellationRequested Then
                    Exit Sub
                End If
            End While

            ' Read returns 0 if the connection closes gracefully.
            Dim byte_count = stream.Read(byte_buffer, 0, BUFFER_SIZE)
            If byte_count = 0 Then
                Exit Sub
            End If

            ' Decoder automatically stores partial chars for the next round
            Dim char_count = decoder.GetChars(byte_buffer, 0, byte_count, char_buffer, 0)

            ' Process lines read in
            Dim builder As New StringBuilder(partial_message)
            For i = 0 To char_count - 1

                Dim current_char = char_buffer(i)

                ' The standard requires CR-LF but some implementations don't.
                ' So we catch both and only send non-empty messages.
                If {ChrW(10), ChrW(13)}.Contains(current_char) Then
                    If builder.Length Then
                        receipt_handler.Invoke(builder.ToString())
                    End If
                    builder.Clear()
                ElseIf builder.Length > ClientLimits.LineLength Then
                    Throw New LimitException("ClientLineLength")
                Else
                    builder.Append(current_char)
                End If

            Next

            ' We need to store partial lines until the next round
            partial_message = builder.ToString()

        End While

    End Sub

#Region "Internals"

    Private Client As New TcpClient
    Private Canceler As New CancellationTokenSource
    Private CancelToken As CancellationToken

#End Region

End Class
