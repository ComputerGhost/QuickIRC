Imports Algorithms

Public Class ChatBase

    ReadOnly Property Listener As IRC.ListenerBase
    ReadOnly Property Writer As IRC.WriterBase

    Sub New(listener As IRC.ListenerBase, writer As IRC.WriterBase)
        Me.Listener = listener
        Me.Writer = writer
    End Sub

End Class
