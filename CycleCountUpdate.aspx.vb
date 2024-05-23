Imports System.Xml

Public Class CycleCountUpdate
    Inherits System.Web.UI.Page
    Dim oWS As SLWebServices.DOWebServiceSoapClient


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Verify Employee
        If Session("Employee") Is Nothing Then
            Response.Redirect("default_hh.aspx")
        Else
            If Session("Employee").ToString = "" Then
                Response.Redirect("default_hh.aspx")
            End If
        End If

        If Session("Token") Is Nothing Then
            Response.Redirect("signin_hh.aspx")
        Else
            If Session("Token").ToString = "" Then
                Response.Redirect("signin_hh.aspx")
            Else
                RebindGrid()
                If IsPostBack = False Then
                    labError.Text = ""
                    LoadUserInfo()
                    LoadEmployeeUser()
                    Session("ConfirmClearAll") = 0
                End If
            End If
        End If
        btnClearAll.UseSubmitBehavior = False
        btnProcess.UseSubmitBehavior = False
        changeCaptionClearAll()
    End Sub

    'Load User Info.
    Sub LoadUserInfo()
        'Dim ds As Data.DataSet
        'Dim PropertyList As String = ""
        'Dim Filter As String = ""
        'Dim UserId As Decimal

        'Filter = "Username='" & Session("UserName").ToString & "'"
        'PropertyList = "UserId, Whse"
        'ds = New Data.DataSet
        'oWS = New SLWebServices.DOWebServiceSoapClient
        'ds = oWS.LoadDataSet(Session("Token").ToString, "SLUserLocals", PropertyList, Filter, "", "", 0)
        'If ds.Tables(0).Rows.Count > 0 Then
        '    UserId = ds.Tables(0).Rows(0)("UserId")
        '    txtWhse.Text = ds.Tables(0).Rows(0)("Whse")
        'End If

        Dim ds As Data.DataSet
        Dim PropertyList As String = ""
        Dim Filter As String = ""
        Dim UserId As Decimal

        Filter = "Username='" & Session("UserName").ToString & "'"
        PropertyList = "UserId, UserNamesUf_UserNames_Whse"
        ds = New Data.DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_UserNames", PropertyList, Filter, "", "", 0)
        If ds.Tables(0).Rows.Count > 0 Then
            UserId = ds.Tables(0).Rows(0)("UserId").ToString
        End If

        Filter = "UserId='" & UserId & "'"
        PropertyList = "UserCode, Whse"
        ds = New Data.DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLUserLocals", PropertyList, Filter, "", "", 0)
        If ds.Tables(0).Rows.Count > 0 Then
            txtWhse.Text = ds.Tables(0).Rows(0)("Whse").ToString
            txtWhse.Enabled = False
        End If
    End Sub

    Sub LoadEmployeeUser()
        ddlEmployee.Items.Clear()
        Dim Filter As String = ""

        'Load Employee User
        Dim ds As Data.DataSet
        Filter = "UserName='" & Session("UserName").ToString & "'"
        ds = New Data.DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Users", "EmpNum, EmpName", Filter, "EmpNum", "", 0)
        For Each dRow As Data.DataRow In ds.Tables(0).Rows
            ddlEmployee.Items.Add(New ListItem(dRow("EmpNum") & " -- " & dRow("EmpName"), dRow("EmpNum")))
        Next

        Dim ListItem As ListItem = ddlEmployee.Items.FindByValue(Session("Employee").ToString)
        If ListItem IsNot Nothing Then
            ddlEmployee.SelectedValue = Session("Employee").ToString
        End If
    End Sub

    Protected Sub txtBarcode_Change(sender As Object, e As System.EventArgs) Handles txtBarcode.TextChanged

        If txtBarcode.Text = "" Then
            Exit Sub
        End If

        Dim CancelStatus As String = ""
        If chkCancel.Checked = True Then
            CancelStatus = "T"
        Else
            CancelStatus = "F"
        End If

        oWS = New SLWebServices.DOWebServiceSoapClient
        Dim Parms As String = "<Parameters><Parameter>" & txtWhse.Text & "</Parameter>" & _
                                    "<Parameter>" & ddlEmployee.SelectedValue & "</Parameter>" & _
                                    "<Parameter>" & txtBarcode.Text & "</Parameter>" & _
                                    "<Parameter>" & CancelStatus & " </Parameter>" & _
                                    "<Parameter ByRef='Y'>" & "" & "</Parameter>" & _
                                    "<Parameter ByRef='Y'>" & "" & "</Parameter></Parameters>"
        Dim res As Object
        oWS = New SLWebServices.DOWebServiceSoapClient
        res = oWS.CallMethod(Session("Token").ToString, "PPCC_CycleCounts", "ppcc_cyclecountinsert", Parms)
        labError.Text = ""

        If res = 0 Then
            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)
            Dim i As Integer = 1
            For Each node As XmlNode In doc.DocumentElement
                If i = 5 Then
                    If node.InnerText <> "0" Then
                        labError.ForeColor = Drawing.Color.Red
                    Else
                        labError.ForeColor = Drawing.Color.Black
                    End If
                End If
                If i = 6 Then
                    labError.Text = node.InnerText
                End If
                i += 1
            Next
        End If

        txtBarcode.Text = ""
        ddlEmployee.Enabled = False
        RebindGrid()
        txtBarcode.Focus()
    End Sub

    Sub RebindGrid()
        oWS = New SLWebServices.DOWebServiceSoapClient
        Dim Filter As String = "emp_num='" & ddlEmployee.SelectedValue & "'"
        Dim PropList As String = "tag_id, item, description, u_m, lot, tag_qty, loc"
        Dim ds As New Data.DataSet
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_CycleCounts", PropList, Filter, "tag_id", "", 0)

        grdCycleCount.DataSource = ds.Tables(0)
        grdCycleCount.DataBind()
    End Sub

    Protected Sub btnClearAll_Click(sender As Object, e As System.EventArgs) Handles btnClearAll.Click

        If Session("ConfirmClearAll") = 0 Then
            Session("ConfirmClearAll") = 1
        Else
            oWS = New SLWebServices.DOWebServiceSoapClient
            Dim Parms As String = "<Parameters><Parameter>" & txtWhse.Text & "</Parameter>" & _
                                        "<Parameter>" & ddlEmployee.SelectedValue & "</Parameter>" & _
                                        "<Parameter ByRef='Y'>" & "" & "</Parameter>" & _
                                        "<Parameter ByRef='Y'>" & "" & "</Parameter></Parameters>"
            Dim res As Object
            oWS = New SLWebServices.DOWebServiceSoapClient
            res = oWS.CallMethod(Session("Token").ToString, "PPCC_CycleCounts", "ppcc_cyclecountclearall", Parms)

            If res = 0 Then
                Dim doc As XmlDocument = New XmlDocument()
                doc.LoadXml(Parms)
                Dim i As Integer = 1
                For Each node As XmlNode In doc.DocumentElement
                    If i = 3 Then
                        If node.InnerText <> "0" Then
                            labError.ForeColor = Drawing.Color.Red
                        Else
                            labError.ForeColor = Drawing.Color.Black
                        End If
                    End If
                    If i = 4 Then
                        labError.Text = node.InnerText
                    End If
                    i += 1
                Next
            End If

            txtBarcode.Text = ""
            ddlEmployee.Enabled = True
            RebindGrid()
            ddlEmployee.Focus()
            Session("ConfirmClearAll") = 0
        End If
        changeCaptionClearAll()
    End Sub

    Protected Sub btnScan_Click(sender As Object, e As System.EventArgs) Handles btnScan.Click

        txtBarcode_Change(sender, e)

    End Sub

    Sub changeCaptionClearAll()
        If Session("ConfirmClearAll") = 0 Then
            btnClearAll.Text = "Clear All"
        Else
            btnClearAll.Text = "Confirm"
        End If
    End Sub

    Protected Sub btnProcess_Click(sender As Object, e As System.EventArgs) Handles btnProcess.Click

        oWS = New SLWebServices.DOWebServiceSoapClient
        Dim Parms As String = "<Parameters><Parameter>" & txtWhse.Text & "</Parameter>" & _
                                    "<Parameter>" & ddlEmployee.SelectedValue & "</Parameter>" & _
                                    "<Parameter ByRef='Y'>" & "" & "</Parameter>" & _
                                    "<Parameter ByRef='Y'>" & "" & "</Parameter></Parameters>"
        Dim res As Object
        oWS = New SLWebServices.DOWebServiceSoapClient
        res = oWS.CallMethod(Session("Token").ToString, "PPCC_CycleCounts", "PPCC_CyclecountProcess", Parms)

        If res = 0 Then
            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)
            Dim i As Integer = 1
            For Each node As XmlNode In doc.DocumentElement
                If i = 3 Then
                    If node.InnerText <> "0" Then
                        labError.ForeColor = Drawing.Color.Red
                    Else
                        labError.ForeColor = Drawing.Color.Black
                    End If
                End If
                If i = 4 Then
                    labError.Text = node.InnerText
                End If
                i += 1
            Next
        End If

        txtBarcode.Text = ""
        ddlEmployee.Enabled = True
        RebindGrid()
        txtBarcode.Focus()
    End Sub

End Class