Imports System.Data
Imports System.Drawing
Imports System.Web.Services
Imports System.Xml

Partial Class RequestMoveIn
    Inherits System.Web.UI.Page

    Dim oWS As SLWebServices.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim dt As DataTable
    Dim res As Object
    Dim Rpt As String = "PPCC_WMSGenerateFileMoveIn"
    Private Shared iSave As Integer = 0
    Private Shared iPrint As Integer = 0

    Private Shared _SessionID As Guid

    Private Shared Property SessionID() As Guid
        Get
            Return _SessionID
        End Get
        Set(value As Guid)
            _SessionID = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Session("Token") Is Nothing Then
            Response.Redirect("signin.aspx")
        Else
            If Session("Token").ToString = "" Then
                Response.Redirect("signin.aspx")
            End If

        End If

        If Session("CanAccessReqMoveIn") <> "1" Then Response.Redirect("default.aspx")

        If Not IsNothing(Session("UserName")) Then
            'Response.Redirect("signin.aspx")
            LoadUserInfo()

            If IsNothing(Session("UserWhse")) Then
                Response.Redirect("signin.aspx")
            End If
            'MsgBox(Session("UserWhse"))
        End If


        If Not Page.IsPostBack Then
            SessionID = Guid.NewGuid
            txtBarcode.Focus()
            txtBarcode.AutoCompleteType = AutoCompleteType.Disabled
        End If

    End Sub

    Function CheckBarcodePallet(barcodeID As String) As Integer

        Dim iCheck As Integer = 0

        Dim ds As DataSet

        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSBarcodes", "BarcodePallet", "BarcodePallet ='" & barcodeID & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Dim View As New DataView(ds.Tables(0))
            iCheck = View.ToTable(True, "BarcodePallet").Rows.Count


        End If

        Return iCheck

    End Function

    Function CheckBarcodeSubmit(barcodeID As String) As Integer

        Dim iSubmit As Integer = 0

        Dim ds As DataSet

        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSBarcodes", "Submited", "BarcodePallet ='" & barcodeID & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            iSubmit = ds.Tables(0).Rows(0)("Submited").ToString
        End If

        Return iSubmit

    End Function

    Protected Sub txtBarcode_TextChanged(sender As Object, e As System.EventArgs) Handles txtBarcode.TextChanged
        NotPassNotifyPanel.Visible = False
        PassNotifyPanel.Visible = False

        Dim iCheck As Integer = 0
        Dim iCount As Object = 0
        Dim TaskNumber As String = ""
        Dim infobar As String = ""
        Dim iExist As Integer = 0
        Dim iSubmit As Object = 0
        iCheck = CheckBarcodePallet(txtBarcode.Text)

        Try
            If txtBarcode.Text <> String.Empty Then

                txtBarcodetmp.Text = txtBarcode.Text

                txtBarcode.Attributes.Add("onchange", "showProgress();")

                If iCheck > 0 Then

                    Dim ds As DataSet
                    Dim Parms As String = ""

                    '---------------------Check Submited ---------------------'
                    ds = New DataSet
                    oWS = New SLWebServices.DOWebServiceSoapClient

                    Parms = "<Parameters><Parameter>" & txtBarcodetmp.Text & "</Parameter>" & _
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

                    res = New Object
                    res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSCheckPalletSubmitSp", Parms)

                    If res = "0" Then

                        Dim doc As XmlDocument = New XmlDocument()
                        doc.LoadXml(Parms)
                        Dim i As Integer = 1
                        For Each node As XmlNode In doc.DocumentElement
                            If i = 2 Then

                                If node.InnerText <> String.Empty Or node.InnerText <> "" Then
                                    NotPassNotifyPanel.Visible = True
                                    NotPassText.Text = node.InnerText
                                    iSubmit = 1
                                    txtBarcode.Text = String.Empty
                                    txtBarcode.Focus()
                                    Exit For
                                End If

                            End If
                            i += 1
                        Next

                    End If
                    '---------------------End Check Submited ---------------------'

                    If iSubmit = 0 Then

                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
                                "<Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                                "<Parameter>" & txtBarcodetmp.Text & "</Parameter>" & _
                                "<Parameter>" & "I" & "</Parameter>" & _
                                "<Parameter>" & Session("UserWhse").ToString & "</Parameter>" & _
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

                        res = New Object
                        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSHistoryReqMoves", "PPCC_WMSProcessReqMoveSp", Parms)

                        BindGridDetail()

                        If res = "0" Then

                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(Parms)
                            Dim i As Integer = 1
                            For Each node As XmlNode In doc.DocumentElement
                                If i = 6 Then

                                    If node.InnerText <> String.Empty Or node.InnerText <> "" Then
                                        iCount += 1
                                        NotPassNotifyPanel.Visible = True
                                        NotPassText.Text = node.InnerText
                                        txtBarcode.Text = String.Empty
                                        txtBarcode.Focus()
                                        Exit Sub
                                    End If

                                End If
                                i += 1
                            Next

                        End If

                        'GridDetail.AllowPaging = False


                        'If GridDetail.Rows.Count >= 0 Then

                        '    For Each Row As GridViewRow In GridDetail.Rows
                        '        If Replace(Row.Cells(7).Text, "&nbsp;", "") <> "" Then
                        '            iCount += 1
                        '            NotPassNotifyPanel.Visible = True
                        '            NotPassText.Text = "Please, Check Error Message"
                        '            txtBarcode.Text = String.Empty
                        '            txtBarcode.Focus()

                        '            'GridDetail.AllowPaging = True
                        '            'BindGridDetail()

                        '            Exit Sub
                        '        End If
                        '    Next

                        'End If

                        If iCount = 0 Then

                            Dim CallTaskCount As Integer = 0
                            Dim CountNoTask As Integer = 0
                            Dim TaskName As String = ""

                            oWS = New SLWebServices.DOWebServiceSoapClient
                            ds = New Data.DataSet
                            ds = oWS.LoadDataSet(Session("Token"), "BGTaskDefinitions", "TaskName", "TaskName='" & Rpt.ToString & "'", "", "", 0)
                            If ds.Tables(0).Rows.Count = 0 Then
                                CountNoTask += 1
                            Else
                                TaskName = ds.Tables(0).Rows(0)("TaskName").ToString
                            End If

                            Dim TaskParms As String = ""
                            TaskParms = SessionID.ToString & "," & Session("UserName").ToString & "," & "I,,,"

                            If CountNoTask = 0 Then
                                'Qty Move
                                oWS = New SLWebServices.DOWebServiceSoapClient
                                Parms = "<Parameters>"
                                Parms &= "<Parameter>" & SessionID.ToString & "</Parameter>"
                                Parms &= "<Parameter>" & Session("UserName").ToString & "</Parameter>"
                                Parms &= "<Parameter>" & "I" & "</Parameter>"
                                Parms &= "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>"
                                Parms &= "</Parameters>"

                                oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSAutoMoveTmpSp", Parms)

                                BindGridDetail()

                                If res = "0" Then

                                    Dim _doc As XmlDocument = New XmlDocument()
                                    _doc.LoadXml(Parms)
                                    Dim j As Integer = 1
                                    For Each node As XmlNode In _doc.DocumentElement
                                        If j = 6 Then

                                            If node.InnerText <> String.Empty Or node.InnerText <> "" Then
                                                iCount += 1
                                                NotPassNotifyPanel.Visible = True
                                                NotPassText.Text = node.InnerText
                                                txtBarcode.Text = String.Empty
                                                txtBarcode.Focus()
                                                Exit Sub
                                            End If

                                        End If
                                        j += 1
                                    Next

                                End If

                                'For Each Row As GridViewRow In GridDetail.Rows
                                '    If Replace(Row.Cells(7).Text, "&nbsp;", "") <> "" Then
                                '        iCount += 1
                                '        NotPassNotifyPanel.Visible = True
                                '        NotPassText.Text = "Please, Check Error Message"
                                '        txtBarcode.Text = String.Empty
                                '        txtBarcode.Focus()

                                '        GridDetail.AllowPaging = True
                                '        BindGridDetail()

                                '        Exit Sub
                                '    End If
                                'Next

                                'GridDetail.AllowPaging = True
                                'BindGridDetail()

                                If iCount = 0 Then

                                    oWS = New SLWebServices.DOWebServiceSoapClient
                                    Parms = "<Parameters>"
                                    Parms &= "<Parameter>" & TaskName.ToString & "</Parameter>"
                                    Parms &= "<Parameter>" & TaskParms & "</Parameter>"
                                    Parms &= "<Parameter>" & Session("UserName").ToString & "</Parameter>"
                                    Parms &= "<Parameter>" & "Print" & "</Parameter>"
                                    Parms &= "<Parameter ByRef='Y'>" & String.Empty & "</Parameter>"
                                    Parms &= "</Parameters>"

                                    oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSInsertActiveBGTaskForPrintBCSp", Parms)

                                    Dim doc As XmlDocument = New XmlDocument()
                                    doc.LoadXml(Parms)
                                    Dim i As Integer = 1

                                    For Each node As XmlNode In doc.DocumentElement
                                        If i = 5 Then
                                            If Trim(node.InnerText) <> String.Empty Then
                                                TaskNumber = node.InnerText
                                                CallTaskCount += 1
                                            End If
                                        End If
                                        i += 1
                                    Next

                                    If CallTaskCount > 0 Then

                                        'Update Req Stat
                                        oWS = New SLWebServices.DOWebServiceSoapClient
                                        Parms = "<Parameters>"
                                        Parms &= "<Parameter>" & SessionID.ToString & "</Parameter>"
                                        Parms &= "<Parameter>" & TaskNumber & "</Parameter>"
                                        Parms &= "<Parameter>" & txtBarcodetmp.Text & "</Parameter>"
                                        Parms &= "<Parameter>" & Session("UserName").ToString & "</Parameter>"
                                        Parms &= "<Parameter>" & "I" & "</Parameter>"
                                        Parms &= "<Parameter ByRef='Y'>" & String.Empty & "</Parameter>"
                                        Parms &= "</Parameters>"

                                        res = New Object
                                        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSHistoryReqMoves", "PPCC_WMSUpdateReqMoveStatSp", Parms)

                                        BindGridSummary()
                                        SessionID = Guid.NewGuid
                                        'System.Threading.Thread.Sleep(500)

                                    End If

                                End If

                                If CallTaskCount = 0 Or CountNoTask > 0 Then
                                    NotPassNotifyPanel.Visible = True
                                    NotPassText.Text = "Undefined background task definition"
                                    txtBarcode.Text = String.Empty
                                    txtBarcode.Focus()
                                    Exit Sub
                                End If

                            End If


                        End If

                    Else
                        NotPassNotifyPanel.Visible = True
                        NotPassText.Text = "Barcode [" & txtBarcode.Text & "] already submited"
                        txtBarcode.Text = String.Empty
                        txtBarcode.Focus()
                        Exit Sub
                    End If

                Else
                    NotPassNotifyPanel.Visible = True
                    NotPassText.Text = "Barcode [" & txtBarcode.Text & "] not exist"
                    txtBarcode.Text = String.Empty
                    txtBarcode.Focus()
                    Exit Sub

                End If

            End If

        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            Exit Sub
        End Try

        txtBarcode.Text = String.Empty
        txtBarcode.Focus()

    End Sub

    Sub BindGridDetail()
        Dim ds As DataSet
        ds = New DataSet

        Dim Filter As String = ""
        Dim Propertie As String = ""

        Filter = "SessionID = '" & SessionID.ToString & "' and OrderNumber = '" & Left(txtBarcodetmp.Text, 10) & "' and ReqStat is null"

        Propertie = "OrderNumber, Item, Lot, Qty, UM, QtyConv, FromUM, Msg"

        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token"), "PPCC_WMSHistoryReqMoves", Propertie, Filter, "", "", 0)

        GridDetail.DataSource = ds.Tables(0)
        GridDetail.DataBind()

    End Sub

    'Protected Sub btnUpload_Click(sender As Object, e As EventArgs)

    'End Sub

    Sub BindGridSummary()
        Dim ds As DataSet
        ds = New DataSet

        Dim Filter As String = ""
        Dim Propertie As String = ""

        Filter = "SessionID = '" & SessionID.ToString & "' And ReqType = 'S'"

        Propertie = "OrderNumber, ReqStat"

        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token"), "PPCC_WMSHistoryReqMoves", Propertie, Filter, "", "", 0)

        Dim distinctDT As DataTable = ds.Tables(0).DefaultView.ToTable(True, "OrderNumber", "ReqStat")

        GridSummary.DataSource = distinctDT
        GridSummary.DataBind()

        'If GridSummary.Rows.Count > 0 Then

        'For i As Integer = 0 To GridSummary.Rows.Count - 1

        'If GridSummary.Rows(GridSummary.Rows.Count - 1).Cells(0).Text = Left(txtBarcodetmp.Text, 10) And _
        '    GridSummary.Rows(GridSummary.Rows.Count - 1).Cells(1).Text = "S" Then
        '    SessionID = Guid.NewGuid
        'End If

        'Next

        'End If

    End Sub

    Protected Sub GridDetail_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridDetail.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblDerQtyConv As Label = DirectCast(e.Row.FindControl("lblDerQtyConv"), Label)
            If Not IsNothing(lblDerQtyConv) Then
                If IsDBNull(e.Row.DataItem("QtyConv")) Then
                    lblDerQtyConv.Text = String.Empty
                End If
            End If
        End If

    End Sub

    Protected Sub GridSummary_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridSummary.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblReqMoveStaus As Label = DirectCast(e.Row.FindControl("lblReqMoveStaus"), Label)

            If Not IsNothing(lblReqMoveStaus) Then
                If lblReqMoveStaus.Text = "S" Then
                    lblReqMoveStaus.Text = "Submitted"
                Else
                    lblReqMoveStaus.Text = "Failed"
                End If
            End If

        End If

    End Sub

    Sub ShowPageCommand(sender As Object, e As GridViewPageEventArgs) Handles GridDetail.PageIndexChanging
        GridDetail.PageIndex = e.NewPageIndex
        BindGridDetail()
    End Sub

    'Function TaskManRunning(ByVal TaskNumber As String) As Boolean
    '    Dim StartDate As String = ""
    '    Dim CompletionDate As String = ""
    '    Dim ds As New Data.DataSet
    '    Dim i As Integer = 0
    '    oWS = New SLWebServices.DOWebServiceSoapClient

    '    ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "TaskNumber, SubmissionDate, StartDate, CompletionDate", "TaskNumber=" & TaskNumber, "", "", 0)
    '    StartDate = ds.Tables(0).Rows(0)("StartDate").ToString

    '    Dim TaskInterval1 As String = System.Configuration.ConfigurationManager.AppSettings("TaskInterval1")
    '    Dim TaskInterval2 As String = System.Configuration.ConfigurationManager.AppSettings("TaskInterval2")

    '    If TaskInterval1 Is Nothing Then
    '        TaskInterval1 = "240" 'Max 2 Minutes
    '    End If

    '    If TaskInterval2 Is Nothing Then
    '        TaskInterval2 = "600" 'Max 5 Minutes
    '    End If

    '    While StartDate = "" And i < Convert.ToInt32(TaskInterval1)
    '        ds = New DataSet
    '        ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "StartDate", "TaskNumber=" & TaskNumber, "", "", 0)
    '        StartDate = ds.Tables(0).Rows(0)("StartDate").ToString
    '        System.Threading.Thread.Sleep(500)
    '        i += 1
    '    End While

    '    If StartDate <> "" Then
    '        i = 0
    '        ds = New DataSet
    '        ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "CompletionDate", "TaskNumber=" & TaskNumber, "", "", 0)

    '        While CompletionDate = "" And i < Convert.ToInt32(TaskInterval2)
    '            ds = New DataSet
    '            ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "CompletionDate", "TaskNumber=" & TaskNumber, "", "", 0)
    '            CompletionDate = ds.Tables(0).Rows(0)("CompletionDate").ToString
    '            System.Threading.Thread.Sleep(500)
    '            i += 1
    '        End While

    '    End If

    '    Dim FileName As String = ""
    '    Dim TaskErrorMsg As String = ""
    '    If CompletionDate <> "" Then

    '        ds = New DataSet
    '        ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "TaskErrorMsg", "TaskNumber=" & TaskNumber, "", "", 0)
    '        TaskErrorMsg = ds.Tables(0).Rows(0)("TaskErrorMsg").ToString

    '    End If

    '    Session("TaskErrorMsg") = TaskErrorMsg

    '    Return True

    'End Function

    Sub LoadUserInfo()

        NotPassNotifyPanel.Visible = False
        PassNotifyPanel.Visible = False

        Dim ds As DataSet
        Dim PropertyList As String = ""
        Dim Filter As String = ""
        Dim UserId As Decimal

        Filter = "Username='" & Session("UserName").ToString & "'"
        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "UserNames", "UserId", Filter, "", "", 0)
        If ds.Tables(0).Rows.Count > 0 Then
            UserId = ds.Tables(0).Rows(0)("UserId").ToString
        End If

        'Filter = "UserId='" & UserId & "'"
        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLInvparms", "invUf_Invparms_WhseWMS", "", "", "", 0)
        If ds.Tables(0).Rows.Count > 0 Then

            Session("UserWhse") = ds.Tables(0).Rows(0)("invUf_Invparms_WhseWMS").ToString

            'If Session("UserWhse").ToString = "" Then
            '    Session("UserWhse") = "BC01"
            'End If

        End If

    End Sub

End Class
