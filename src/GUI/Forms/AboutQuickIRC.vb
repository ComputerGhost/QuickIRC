Public Class AboutQuickIRC

    Private Sub Me_Load() Handles Me.Load

        Dim app_info = My.Application.Info
        Dim version_info = app_info.Version
        Dim version_string = String.Format("{0}.{1}.{2}", version_info.Major, version_info.Minor, version_info.Revision)

        Text = "About " & app_info.Title

        lblProductName.Text = String.Format("{0} Version {1}", app_info.ProductName, version_string)
        lblCopyright.Text = String.Format("{0} {1}", app_info.Copyright, app_info.CompanyName)
        lblDescription.Text = app_info.Description

    End Sub

    Private Sub btnOkay_Click() Handles btnOkay.Click
        Close()
    End Sub

End Class