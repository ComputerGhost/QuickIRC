'
' Checks the syntax of incoming command. Register this listener first.
'
Public Class SyntaxListener
    Inherits ListenerBase


    Overrides Sub HandleMessageReceived(ByRef message As Message)
        Dim sender = message.Source.Name
        Dim command = message.Verb
        If IncomingCommands.ContainsKey(command) Then
            Dim param_info = IncomingCommands(command)
            Dim param_count = message.Parameters.Count
            If param_count < param_info.Min Or param_count > param_info.Max Then
                message.IsValid = False
            End If
        End If
    End Sub

    Overrides Sub HandleMessageSent(ByRef message As Message)
        If OutgoingCommands.ContainsKey(Command) Then
            Dim param_info = IncomingCommands(Command)
            Dim param_count = message.Parameters.Count
            If param_count < param_info.Min Or param_count > param_info.Max Then
                message.IsValid = False
            End If
        End If
    End Sub


#Region "Internals"

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

    ' Outgoing commands that we'll syntax-check (after sending)
    Private Shared OutgoingCommands As New Dictionary(Of String, ParameterInfo) From {
        {"DIE", New ParameterInfo With {.Min = 0, .Max = 1}},   ' DIE [password]
        {"OPER", New ParameterInfo With {.Count = 2}},          ' OPER <username> <password>
        {"PASS", New ParameterInfo With {.Count = 1}},          ' PASS <password>
        {"PRIVMSG", New ParameterInfo With {.Count = 2}},       ' PRIVMSG <target> <text>
        {"RESTART", New ParameterInfo With {.Min = 0, .Max = 2}},' RESTART [password] [reason]
        {"end-of-list", Nothing}
        }

#End Region

End Class
