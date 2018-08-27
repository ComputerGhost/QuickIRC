'
' Anything implementing this can use the ThreadLock class to ensure that no 
' other threads can access the object.
'
Public Interface ILockable

    Sub Lock()
    Sub Release()

End Interface
