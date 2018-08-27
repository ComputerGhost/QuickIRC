'
' Handles commands within the context with a user private message session.
'
' NOTE: It is assumed that incoming commands have the correct syntax. Use a 
' StandardHandler to ensure that this prerequisite is met.
'
Public Class UserChat
    Inherits ChatBase


    Delegate Sub UserChangedDelegate(old_nick As String, new_nick As String)
    Delegate Sub UserQuitDelegate()
    Property OnUserChanged As UserChangedDelegate
    Property OnUserQuit As UserQuitDelegate


    Property UserName As String


    Sub New(user_name As String)
        MyBase.New()
        UserName = user_name
    End Sub


    Public Overrides Sub ProcessAndSend(text As String)

        Dim tokenizer As New Tokenizer(Connection.ServerLimits, text)

        ' If it's not a command, then it's a message
        If Not tokenizer.Skip("/"c) Then
            Connection.SendLine(String.Format("PRIVMSG {0} :{1}", UserName, text))
            Exit Sub
        End If

        Select Case tokenizer.ReadCommand()
            Case "SAY" ' SAY <message>
                Dim message = If(tokenizer.ReadRemaining(), "")
                Connection.SendLine(String.Format("PRIVMSG {0} :{1}", UserName, message))
            Case Else
                MyBase.ProcessAndSend(text)
        End Select

    End Sub

#Region "Internals"

    Private Function ShouldProcess(message As Message) As Boolean

        ' Commands that have us as a target
        Static TargetCommands As New HashSet(Of String) From {
            "NOTICE", "PRIVMSG"}

        ' Other commands that we are interested in
        Static OtherCommands As New HashSet(Of String) From {
            "AWAY", "NICK", "QUIT"}

        Dim command = message.Verb

        If UserName <> message.Source.Name Then
            Return False
        ElseIf TargetCommands.Contains(command) Then
            Return (Connection.Nickname = message.Parameters(0))
        ElseIf OtherCommands.Contains(command) Then
            Return True
        Else
            Return False
        End If

    End Function

    Protected Friend Overrides Sub HandleMessageReceived(message As Message)

        Dim sender = message.Source.Name

        Select Case message.Verb
            Case "NICK" ' <sender> NICK <new_nick>
                UserName = message.Parameters(0)
                OnUserChanged?.Invoke(sender, UserName)
            Case "QUIT" ' <sender> QUIT [reason]
                OnUserQuit?.Invoke()
        End Select

        MyBase.HandleMessageReceived(message)

    End Sub

    Protected Friend Overrides Sub HandleMessageSent(message As Message)
        If ShouldProcess(message) Then
            MyBase.HandleMessageSent(message)
        End If
    End Sub

#End Region

End Class
