Imports System.Runtime.CompilerServices

Module RichTextBoxExtensions

    <Extension()>
    Sub AppendRtf(target As RichTextBox, rtf As String)
        target.Select(target.TextLength, 0)
        target.SelectedRtf = rtf
    End Sub

End Module
