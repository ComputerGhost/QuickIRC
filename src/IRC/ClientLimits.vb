Public Module ClientLimits

    ' Max raw line length to accept. IRC v3.3 has a 5119 limit.
    ' Exceeding this will close the connection.
    Public Const LineLength = 5119

    ' Max users to list in a channel.
    Public Const UserCount = 10

End Module
