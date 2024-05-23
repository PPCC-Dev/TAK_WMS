Imports System.Data

Partial Class signin
    Inherits System.Web.UI.Page
    Dim oWS As SLWebServices.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim MsgErr As String = ""

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            If Not IsNothing(Session("ErrorUserModule")) Then
                UserNameTextBox.Text = Session("UserName").ToString
                Session.Remove("ErrorUserModule")
            End If

            oWS = New SLWebServices.DOWebServiceSoapClient
            For Each str As String In oWS.GetConfigurationNames
                ConfigurationDropDown.Items.Add(New ListItem(str, str))
            Next

            Dim sl_config As String = System.Configuration.ConfigurationManager.AppSettings("Configuration")
            Dim lst_config As ListItem = ConfigurationDropDown.Items.FindByValue(Convert.ToString(sl_config))

            If sl_config Is Nothing Or lst_config Is Nothing Then
                ConfigurationDropDown.Enabled = True
            End If

            If lst_config IsNot Nothing Then
                ConfigurationDropDown.SelectedValue = sl_config.ToString
                ConfigurationDropDown.Attributes.Add("disabled", "disabled")
            End If

        End If

    End Sub

    Protected Sub SignInButton_Click(sender As Object, e As System.EventArgs) Handles SignInButton.Click

        Dim Config As String = System.Configuration.ConfigurationManager.AppSettings("Configuration")

        Try
            Dim iRetry As Integer = 0
            Dim iLoginFail As Integer = 0
            Dim Token As String = ""
            Dim UserName As String = Trim(UserNameTextBox.Text)
            Dim Password As String = PasswordTextBox.Text
            Dim strPasswordExpDate As String = ""
            Dim PasswordWarning As String = "0"
            Dim PasswordNeverExpires As String = ""
            Dim PasswordExpDate, WarningDate As Date
            Dim CurrDate As DateTime = Date.Now.ToString("dd/MM/yyyy")
            Dim CurrDateDiff As Integer

            oWS = New SLWebServices.DOWebServiceSoapClient
            Token = oWS.CreateSessionToken(UserName, Password, Config)

            Session("Token") = Token
            Session("UserName") = UserName
            Session("Config") = Config

            Dim time As String = System.Configuration.ConfigurationManager.AppSettings("PageTimeOut")
            If time Is Nothing Then
                time = "30"
            End If

            Session("Token") = Token
            Session.Timeout = time


            ds = New DataSet
            oWS = New SLWebServices.DOWebServiceSoapClient
            ds = oWS.LoadDataSet(Session("Token").ToString, "UserNames", "PasswordExpirationDate, PasswordNeverExpires", "Username='" & Session("UserName").ToString & "'", "", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then

                strPasswordExpDate = Convert.ToDateTime(ds.Tables(0).Rows(0)("PasswordExpirationDate").ToString).ToString("dd/MM/yyyy")
                PasswordNeverExpires = ds.Tables(0).Rows(0)("PasswordNeverExpires").ToString

                If PasswordNeverExpires = "0" Then

                    If strPasswordExpDate <> "" Then

                        ds = New DataSet
                        oWS = New SLWebServices.DOWebServiceSoapClient
                        ds = oWS.LoadDataSet(Session("Token").ToString, "PasswordParameters", "PasswordWarning", "ParmKey = 0", "", "", 0)

                        If ds.Tables(0).Rows.Count > 0 Then

                            PasswordExpDate = Date.Parse(strPasswordExpDate)

                            PasswordWarning = ds.Tables(0).Rows(0)("PasswordWarning").ToString
                            WarningDate = DateAdd(DateInterval.Day, (Integer.Parse(PasswordWarning) * -1), PasswordExpDate)
                            CurrDateDiff = DateDiff(DateInterval.Day, CurrDate, PasswordExpDate)

                            If CurrDate >= WarningDate Then

                                MsgErr = "Your password will expire in " & CurrDateDiff & " days"

                                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlertWarning('Warning','" & MsgErr & "', 'warning');", True)

                                ConfigurationDropDown.SelectedValue = Config

                            Else
                                Response.Redirect("Menu.aspx")

                            End If

                        End If

                    Else
                        Response.Redirect("Menu.aspx")

                    End If
                Else

                    Response.Redirect("Menu.aspx")

                End If

            End If

            'Response.Redirect("Menu.aspx")

        Catch ex As Exception

            ConfigurationDropDown.SelectedValue = Config

            'NotificationPanel.Visible = True

            Dim end_pos As Integer = InStr(ex.Message, "at IDOWebService.ThrowSoapException")
            If end_pos > 0 And InStr(ex.Message, "System.Web.Services.Protocols.SoapException:") > 0 Then
                MsgErr = Replace(Left(ex.Message, IIf(end_pos = 0, 0, end_pos - 1)).Replace("System.Web.Services.Protocols.SoapException:", ""), "InvalidCredentials", "Invalid Credentials")

            Else
                MsgErr = ex.Message
            End If

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br/>")

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','" & MsgErr & "', 'error');", True)

        End Try


    End Sub

    
End Class
