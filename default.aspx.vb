
Partial Class _default
    Inherits System.Web.UI.Page

    Dim oWS As SLWebServices.DOWebServiceSoapClient

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Session("Token") Is Nothing Then
            Response.Redirect("signin.aspx")
        Else
            If Session("Token").ToString = "" Then
                Response.Redirect("signin.aspx")
            End If
        End If

        If Not Page.IsPostBack Then
            Response.Redirect("Menu.aspx")
        End If

    End Sub

End Class
