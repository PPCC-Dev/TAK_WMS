Partial Class SNPRWeb1
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Session("Token") Is Nothing Then
            Response.Redirect("signin_hh.aspx")
        Else
            If Session("Token").ToString = "" Then
                Response.Redirect("signin_hh.aspx")
            Else
                labUserName.Text = Session("UserName")
            End If

        End If

        btnExit.UseSubmitBehavior = False
        btnMainMenu.UseSubmitBehavior = False

    End Sub

    Protected Sub btnMainMenu_Click(sender As Object, e As EventArgs) Handles btnMainMenu.Click
        Response.Redirect("default_hh.aspx")
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Session("Token") = Nothing
        Session("UserName") = Nothing
        Session("Password") = Nothing
        Response.Redirect("signin_hh.aspx")
    End Sub



End Class