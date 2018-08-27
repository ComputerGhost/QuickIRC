'
' Passed from a new connection input to whatever handles setting up the 
' connection. All properties should be set.
'
Public Structure ConnectionInfo

    ' Not useful for connecting but may be used to show the user the name.
    Property Name As String

    ' Connection info
    Property Server As String
    Property Port As Integer

    ' Please register this with the Connection
    Property Connector As IRC.ConnectChat

End Structure
