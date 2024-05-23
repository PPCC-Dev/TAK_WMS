Imports System.Data

Partial Class OldSignin
    Inherits System.Web.UI.Page
    Dim oWS As SLWebServices.DOWebServiceSoapClient
    Dim ds As DataSet

    Protected Sub SignInButton_Click(sender As Object, e As System.EventArgs) Handles SignInButton.Click

        Try
            Dim iRetry As Integer = 0
            Dim iLoginFail As Integer = 0
            Dim Token As String = ""
            Dim UserName As String = Trim(UserNameTextBox.Text)
            Dim Password As String = PasswordTextBox.Text
            Dim Config As String = Convert.ToString(ConfigurationDropDown.SelectedValue)

            oWS = New SLWebServices.DOWebServiceSoapClient
            Token = oWS.CreateSessionToken(UserName, Password, Config)

            Session("Token") = Token
            Session("UserName") = UserName
            Session("Config") = Config

            If RememberCheckBox.Checked Then
                Dim aCookie As New HttpCookie("UserInfo")
                aCookie.Expires = DateTime.Now.AddDays(7)
                aCookie.Values("UserName") = UserName
                aCookie.Values("Config") = Config
                aCookie.Values("Remember") = "1"
                Response.Cookies.Add(aCookie)
            End If

            If Not RememberCheckBox.Checked Then
                If (Not Request.Cookies("UserInfo") Is Nothing) Then
                    Dim myCookie As HttpCookie
                    myCookie = New HttpCookie("UserInfo")
                    myCookie.Expires = DateTime.Now.AddDays(-1D)
                    Response.Cookies.Add(myCookie)
                End If

            End If

            Dim time As String = System.Configuration.ConfigurationManager.AppSettings("PageTimeOut")
            'MsgBox(time)
            If time Is Nothing Then
                time = "30"
            End If

            Session("Token") = Token
            Session.Timeout = time

            Response.Redirect("default.aspx")

        Catch ex As Exception

            NotificationPanel.Visible = True

            Dim end_pos As Integer = InStr(ex.Message, "at IDOWebService.ThrowSoapException")
            If end_pos > 0 And InStr(ex.Message, "System.Web.Services.Protocols.SoapException:") > 0 Then
                Literal1.Text = Replace(Left(ex.Message, IIf(end_pos = 0, 0, end_pos - 1)).Replace("System.Web.Services.Protocols.SoapException:", ""), "InvalidCredentials", "Invalid Credentials")

            Else
                Literal1.Text = ex.Message.Replace("InvalidCredentials", "Invalid Credentials")
            End If

        End Try


    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            If Not IsNothing(Session("ErrorUserModule")) Then
                NotificationPanel.Visible = True
                Literal1.Text = Session("ErrorUserModule").ToString
                UserNameTextBox.Text = Session("UserName").ToString
                Session.Remove("ErrorUserModule")
            End If

            oWS = New SLWebServices.DOWebServiceSoapClient
            For Each str As String In oWS.GetConfigurationNames
                ConfigurationDropDown.Items.Add(New ListItem(str, str))
            Next

            If Not Request.Cookies("UserInfo") Is Nothing Then
                UserNameTextBox.Text = Server.HtmlEncode(Request.Cookies("UserInfo")("UserName"))
                Dim ListItem As ListItem
                ListItem = ConfigurationDropDown.Items.FindByValue(Request.Cookies("UserInfo")("Config"))
                If Not ListItem Is Nothing Then
                    ConfigurationDropDown.SelectedValue = Server.HtmlEncode(Request.Cookies("UserInfo")("Config"))
                End If
                If Request.Cookies("UserInfo")("Remember") = "1" Then
                    RememberCheckBox.Checked = True
                Else
                    RememberCheckBox.Checked = False
                End If
            End If

            Dim sl_config As String = System.Configuration.ConfigurationManager.AppSettings("Configuration")
            Dim lst_config As ListItem = ConfigurationDropDown.Items.FindByValue(Convert.ToString(sl_config))

            If sl_config Is Nothing Or lst_config Is Nothing Then
                ConfigurationDropDown.Enabled = True
            End If

            If lst_config IsNot Nothing Then
                ConfigurationDropDown.SelectedValue = sl_config.ToString
                'ConfigurationDropDown.Enabled = False
            End If


        End If
    End Sub

End Class
