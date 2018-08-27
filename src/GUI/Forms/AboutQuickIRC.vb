Public Class AboutQuickIRC

    Private Sub Me_Load() Handles Me.Load

        Dim app_info = My.Application.Info

        Text = "About " & app_info.Title

        lblProductName.Text = String.Format("{0} Version {1}", app_info.ProductName, app_info.Version.ToString())
        lblCopyright.Text = String.Format("{0} {1}", app_info.Copyright, app_info.CompanyName)
        lblDescription.Text = app_info.Description

    End Sub

    Private Sub btnOkay_Click() Handles btnOkay.Click
        Close()
    End Sub

End Class