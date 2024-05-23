Imports System.Data

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Dim oWS As SLWebServices.DOWebServiceSoapClient
    'Protected Sub SMButton_Click(sender As Object, e As System.EventArgs) Handles SMButton.Click
    '    Response.Redirect("tag_sm.aspx")
    'End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'Try
        If Session("Token") Is Nothing Then
            Response.Redirect("signin.aspx")
        Else

            If Session("Token").ToString = "" Then
                Response.Redirect("signin.aspx")
            End If
        End If

        If Not Page.IsPostBack Then

            'If Session("EmployeeName") IsNot Nothing Then
            '    EmployeeLabel.Text = "Employee: " & Session("EmployeeName").ToString & " | "
            'Else
            '    EmployeeLabel.Text = ""
            'End If

            'Session("CanAccessBCPallet") = "0"
            'Session("CanAccessReqMoveIn") = "0"
            'Session("CanAccessReqMoveOut") = "0"
            'Session("CanAccessReqMoveInhouse") = "0"

            'Dim ds As New Data.DataSet
            'oWS = New SLWebServices.DOWebServiceSoapClient

            'Dim PropertyList As String = "UserNamesUf_Users_Access_BCPallet, UserNamesUf_Users_Access_ReqMoveIn, UserNamesUf_Users_Access_ReqMoveOut, UserNamesUf_Users_Access_ReqMoveInhouse"

            'Dim Filter As String = "Username='" & Session("UserName").ToString & "'"
            'ds = oWS.LoadDataSet(Session("Token").ToString, "UserNames", PropertyList, Filter, "", "", 0)

            'If ds.Tables(0).Rows.Count > 0 Then
            '    If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_BCPallet") IsNot DBNull.Value Then
            '        If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_BCPallet").ToString = "1" Then
            '            Session("CanAccessBCPallet") = "1"
            '        End If
            '    End If

            '    If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveIn") IsNot DBNull.Value Then
            '        If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveIn").ToString = "1" Then
            '            Session("CanAccessReqMoveIn") = "1"
            '        End If
            '    End If

            '    If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveOut") IsNot DBNull.Value Then
            '        If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveOut").ToString = "1" Then
            '            Session("CanAccessReqMoveOut") = "1"
            '        End If
            '    End If

            '    If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveInhouse") IsNot DBNull.Value Then
            '        If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveInhouse").ToString = "1" Then
            '            Session("CanAccessReqMoveInhouse") = "1"
            '        End If
            '    End If

            'End If

            'btnBCPallet.Visible = Session("CanAccessBCPallet")
            'btnReqMoveIn.Visible = Session("CanAccessReqMoveIn")
            'btnReqMoveOut.Visible = Session("CanAccessReqMoveOut")
            'btnReqMoveInhouse.Visible = Session("CanAccessReqMoveInhouse")

        End If

        'Catch ex As Exception
        '    Session("ErrorUserModule") = "User is not licensed for UserNames " & Session("UserName").ToString
        '    'MsgBox(Session("ErrorUserModule"))
        '    Response.Redirect("signin.aspx")
        'End Try

    End Sub

    Protected Sub btnMenu_Click(sender As Object, e As System.EventArgs) Handles btnMenu.Click
        Response.Redirect("Menu.aspx")
    End Sub

    'Protected Sub btnBCPallet_Click(sender As Object, e As System.EventArgs) Handles btnBCPallet.Click
    '    Response.Redirect("BCPallet.aspx")
    'End Sub

    'Protected Sub btnReqMoveIn_Click(sender As Object, e As System.EventArgs) Handles btnReqMoveIn.Click
    '    Response.Redirect("RequestMoveIn.aspx")
    'End Sub

    'Protected Sub btnReqMoveOut_Click(sender As Object, e As System.EventArgs) Handles btnReqMoveOut.Click
    '    Response.Redirect("RequestMoveOut.aspx")
    'End Sub

    Protected Sub SignOutButton_Click(sender As Object, e As System.EventArgs) Handles SignOutButton.Click
        Session.Abandon()
        Response.Redirect("signin.aspx")
    End Sub

    'Protected Sub btnReqMoveInhouse_Click(sender As Object, e As System.EventArgs) Handles btnReqMoveInhouse.Click
    '    Response.Redirect("RequestMoveIn_InhouseWhse.aspx")
    'End Sub

    
End Class

