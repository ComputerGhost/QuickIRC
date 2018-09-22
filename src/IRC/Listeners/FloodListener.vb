'
' Protects us against DOS attacks that rely upon flood mechanics.
'
Public Class FloodListener
    Inherits ListenerBase

    Property FloodSeconds As Integer = 10   ' Within a number of seconds:
    Property MaxCTCPs As Integer = 3        ' - max CTCPs per nick
    Property MaxMessages As Integer = 8     ' - max sent messages of our own

    Property ForgiveAfter As Integer = 45       ' seconds to ignore spammers
    Property AdditionalPenalty As Integer = 15  ' penalty if they continue


    Overrides Sub HandleMessageReceived(ByRef message As Message)

        CleanupDOS()

        If Not message.IsValid Then
            Exit Sub
        End If

        Select Case message.Verb

            Case "NICK" ' keep track of suspects' nick changes
                Dim old_nick = message.Source.Name
                Dim new_nick = message.Parameters(0)
                If IgnoredNicks.ContainsKey(old_nick) Then
                    IgnoredNicks.Add(new_nick, IgnoredNicks(old_nick))
                    IgnoredNicks.Remove(old_nick)
                End If
                If WatchedNicks.ContainsKey(old_nick) Then
                    WatchedNicks.Add(new_nick, WatchedNicks(new_nick))
                    WatchedNicks.Remove(old_nick)
                End If

            Case "PRIVMSG" ' watch for CTCP requests

                Dim tokenizer As New Tokenizer(message.Parameters(1))
                If Not tokenizer.Skip(ChrW(1)) Then
                    Exit Sub
                End If

                Dim sender = message.Source.Name
                Dim command = tokenizer.ReadWord().ToUpper()
                Dim params = tokenizer.ReadRemaining().TrimEnd(ChrW(1))

                If command = "ACTION" Then
                    Exit Sub
                End If

                Suspect(sender) ' This may add sender to ignore list.

                If IgnoredNicks.ContainsKey(sender) Then
                    Throw New FloodException(String.Format(
                        "{0} is flooding, so their {1} request will be ignored.",
                        sender, command))
                End If

                If SentMessages.Count > MaxCTCPs Then
                    Throw New FloodException(String.Format(
                        "We are flooding, so {0}'s {1} request will be ignored.",
                        sender, command))
                End If

        End Select

    End Sub

    Overrides Sub HandleMessageSent(ByRef message As Message)
        CleanupDOS()
    End Sub


#Region "Internals"

    Private IgnoredNicks As New Dictionary(Of String, Date)
    Private WatchedNicks As New Dictionary(Of String, List(Of Date))
    Private SentMessages As New List(Of Date)


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

#End Region

End Class
