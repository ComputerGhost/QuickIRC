Public Class ChatForm

    ReadOnly Property Connection As IRC.Connection

    Sub Connect(connection_info As ConnectionModel)

        ' Get the connection ready
        _Connection = New IRC.Connection(connection_info.Address, connection_info.Port)
        Connection.RegisterListener(New IRC.ConnectListener With {
            .PreRegistration = connection_info.PreCommands,
            .AlternativeNicks = connection_info.NickAlternates,
            .PostRegistration = connection_info.PostCommands})

        ' Attach to our UI
        Me.Text = connection_info.Name
        ClientArea.AttachConnection(Connection)

        ' Now we start
        Connection.Connect()

    End Sub

#Region "Menu item events"

#End Region

End Class