'
' Automatically responds to certain messages, checks syntax of commands, 
' protects against DOS, and manages our state per messages received.
'
' A StandardHandler should be registered with the highest priority.
'
Public Class StandardHandler
    Inherits ChatBase


    '
    ' Set these to get notified of important events
    '

    Delegate Sub OnChannelJoinedHandler(channel_name As String)
    Delegate Sub OnChannelPartedHandler(channel_name As String)
    Property OnChannelJoined As OnChannelJoinedHandler
    Property OnChannelParted As OnChannelPartedHandler


    '
    ' These are sent in response to CTCP requests
    '

    Property ClientVersion = "QuickIRC v0.1.1"
    Property ClientSource = "https://github.com/ComputerGhost/QuickIRC"


    '
    ' DOS Protection
    '

    Delegate Sub OnSpammerIgnoredDelegate(nickname As String, message As Message)
    Delegate Sub OnUserIgnoredDelegate(nickname As String, message As Message)
    Property OnSpammerIgnored As OnSpammerIgnoredDelegate   ' ignored due to them spamming
    Property OnUserIgnored As OnUserIgnoredDelegate         ' ignored due to us spamming

    Property FloodSeconds As Integer = 10   ' Within a number of seconds:
    Property MaxCTCPs As Integer = 3        ' - max CTCPs per nick
    Property MaxMessages As Integer = 8     ' - max sent messages of our own

    Property ForgiveAfter As Integer = 45       ' seconds to ignore spammers
    Property AdditionalPenalty As Integer = 15  ' penalty if they continue


    '
    ' Methods
    '

    Protected Friend Overrides Sub HandleMessageReceived(message As Message)

        Dim sender = message.Source.Name
        Dim command = message.Verb

        ' Syntax-check any incoming commands
        If IncomingCommands.ContainsKey(command) Then
            Dim param_info = IncomingCommands(command)
            Dim param_count = message.Parameters.Count
            If param_count < param_info.Min Or param_count > param_info.Max Then
                Throw New SyntaxError("Incorrect number of parameters for command.")
            End If
        End If

        ' Cleanup our ignore lists
        CleanupDOS()

        ' We are interested in a select few at this level.
        Select Case command

            Case "001" ' <sender> 001 <my_nickname> <message>

                Dim nickname = message.Parameters(0)

                Connection.Nickname = nickname

            Case "JOIN" ' <sender> JOIN <channel>

                Dim channel = message.Parameters(0)
                If Connection.Nickname = message.Source.Name Then
                    OnChannelJoined?.Invoke(channel)
                End If

            Case "NICK" ' <sender> NICK <new_nick>

                Dim new_nick = message.Parameters(0)

                If Connection.Nickname = message.Source.Name Then
                    Connection.Nickname = new_nick
                    Connection.UserHost = message.Source.Raw
                End If

                ' DOS protection
                If IgnoredNicks.ContainsKey(sender) Then
                    IgnoredNicks.Add(new_nick, IgnoredNicks(sender))
                    IgnoredNicks.Remove(sender)
                End If
                If WatchedNicks.ContainsKey(sender) Then
                    WatchedNicks.Add(new_nick, WatchedNicks(new_nick))
                    WatchedNicks.Remove(sender)
                End If

            Case "PART" ' <sender> PART <channel> [comment]

                Dim channel = message.Parameters(0)
                If Connection.Nickname = message.Source.Name Then
                    OnChannelParted?.Invoke(channel)
                End If

            Case "PING" ' <sender> PING <server> [server0]

                Connection.SendLine("PONG " & message.Parameters(0))

            Case "PRIVMSG" ' <sender> PRIVMSG <target> <text>

                Dim target = message.Parameters(0)
                Dim text = message.Parameters(1)

                ' We're only interested in CTCP, since we auto-respond to them.
                Dim tokenizer As New Tokenizer(Connection.ServerLimits, text)
                If Not tokenizer.Skip(ChrW(1)) Then
                    Exit Select
                End If

                ' DOS protection, but ACTION is a safe command.
                If tokenizer.ReadCommand() <> "ACTION" Then
                    Suspect(sender)
                End If

                ' If we are ignoring the sender, then ignore the message
                If IgnoredNicks.ContainsKey(sender) Then
                    OnSpammerIgnored?.Invoke(sender, message)
                    Throw New FloodException(sender)
                End If

                ' If we have been flooding, then don't auto-respond to anyone
                If SentMessages.Count > MaxCTCPs Then
                    OnUserIgnored?.Invoke(sender, message)
                    Throw New FloodException("We have sent too many messages.")
                End If

                ' Not flooding? Handle as usual.
                HandleCTCPRequest(sender, target, text)

        End Select

        MyBase.HandleMessageReceived(message)

    End Sub


#Region "Internals"

    '
    ' DOS protection
    '

    Private IgnoredNicks As New Dictionary(Of String, DateTime)
    Private WatchedNicks As New Dictionary(Of String, List(Of DateTime))
    Private SentMessages As New List(Of DateTime)

    ' Remove older data that we no longer need or want.
    Private Sub CleanupDOS()

        Dim forgivens As New List(Of String)

        ' Only worry about the past few seconds of messages
        Dim leeway = Now.AddSeconds(-1 * FloodSeconds)
        For Each watched In WatchedNicks
            watched.Value.RemoveAll(Function(x) x < leeway)
            If watched.Value.Count = 0 Then
                forgivens.Add(watched.Key)
            End If
        Next
        For Each forgiven In forgivens
            WatchedNicks.Remove(forgiven)
        Next

        ' Forgive spammers after a while
        forgivens.Clear()
        For Each ignored In IgnoredNicks
            If ignored.Value < Now.AddSeconds(-1 * ForgiveAfter) Then
                forgivens.Add(ignored.Key)
            End If
        Next
        For Each forgiven In forgivens
            IgnoredNicks.Remove(forgiven)
        Next

        ' Clear out our older messages
        SentMessages.RemoveAll(Function(x) x < leeway)

    End Sub

    ' Penalize someone for spamming us.
    Private Sub Penalize(nickname As String)
        Dim elapsed = (Now - IgnoredNicks(nickname)).Seconds
        Dim penalty = Math.Min(ForgiveAfter, elapsed + AdditionalPenalty)
        IgnoredNicks(nickname) = Now.AddSeconds(-1 * penalty)
    End Sub

    ' Add a spammer suspect; returns true if we are ignoring them
    Private Sub Suspect(nickname As String)

        If IgnoredNicks.ContainsKey(nickname) Then
            Penalize(nickname)
        End If

        If Not WatchedNicks.ContainsKey(nickname) Then
            WatchedNicks(nickname) = New List(Of DateTime)
        End If
        WatchedNicks(nickname).Add(Now)

        If WatchedNicks(nickname).Count > MaxCTCPs Then
            IgnoredNicks.Add(nickname, Now)
        End If

    End Sub


    '
    ' Other
    '

    Private Sub HandleCTCPRequest(sender_nick As String, target As String, text As String)

        Dim tokenizer As New Tokenizer(Connection.ServerLimits, text)
        Debug.Assert(tokenizer.Skip(ChrW(1)))

        Dim command = tokenizer.ReadCommand().TrimEnd(ChrW(1))
        Select Case command

            Case "CLIENTINFO"

                Dim listing = String.Join(" ", SupportedCTCP)
                Connection.SendCTCPResponse(sender_nick, "CLIENTINFO", listing)

            Case "PING"

                Dim timestamp = tokenizer.ReadRemaining()
                If timestamp.EndsWith(ChrW(1)) Then
                    timestamp = timestamp.Substring(0, timestamp.Length - 1)
                End If
                Connection.SendCTCPResponse(sender_nick, "PING", timestamp)

            Case "SOURCE"

                If String.IsNullOrWhiteSpace(ClientSource) Then
                    Throw New ExpectationFailed("ClientSource must be set.")
                End If
                Connection.SendCTCPResponse(sender_nick, "SOURCE", ClientSource)

            Case "VERSION"

                If String.IsNullOrWhiteSpace(ClientVersion) Then
                    Throw New ExpectationFailed("ClientVersion must be set.")
                End If
                Connection.SendCTCPResponse(sender_nick, "VERSION", ClientVersion)

        End Select

    End Sub

#End Region

#Region "'Constant' Data"


    ' CTCP commands that we support (sent in response to CLIENTINFO)
    Private Shared SupportedCTCP As New List(Of String) From {
        "ACTION",
        "CLIENTINFO",
        "PING",
        "SOURCE",
        "VERSION"}


    ' Number of expected parameters
    Private Structure ParameterInfo
        Property Min As Integer
        Property Max As Integer
        WriteOnly Property Count As Integer
            Set(value As Integer)
                Min = value
                Max = value
            End Set
        End Property
    End Structure

    ' Incoming commands that we'll support and syntax-check
    Private Shared IncomingCommands As New Dictionary(Of String, ParameterInfo) From {
        {"001", New ParameterInfo With {.Count = 2}},           ' 001 <nick> <message>
        {"328", New ParameterInfo With {.Count = 3}},           ' 328 <me> <channel> <service>
        {"332", New ParameterInfo With {.Count = 3}},           ' 332 <me> <channel> <topic>
        {"333", New ParameterInfo With {.Count = 4}},           ' 333 <me> <channel> <user> <timestamp>
        {"353", New ParameterInfo With {.Min = 3, .Max = 4}},   ' 353 <me> <type> <channel> <users>
        {"366", New ParameterInfo With {.Count = 3}},           ' 366 <me> <channel> <message>
        {"JOIN", New ParameterInfo With {.Count = 1}},          ' JOIN <channel>
        {"MODE", New ParameterInfo With {.Min = 1, .Max = 9}},  ' MODE <target> [lots-of-params]
        {"NICK", New ParameterInfo With {.Count = 1}},          ' NICK <nickname>
        {"PART", New ParameterInfo With {.Min = 1, .Max = 2}},  ' PART <target> [message]
        {"PING", New ParameterInfo With {.Min = 1, .Max = 2}},  ' PING <server> [server0]
        {"PRIVMSG", New ParameterInfo With {.Count = 2}},       ' PRIVMSG <target> <text>
        {"TOPIC", New ParameterInfo With {.Count = 2}},         ' TOPIC <channel> <text>
        {"end-of-list", Nothing}
        }

#End Region


End Class
