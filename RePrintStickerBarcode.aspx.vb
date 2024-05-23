Imports System.Data
Imports System.Web.Services
Imports System.IO
Imports System.Xml

Partial Class RePrintStickerBarcode
    Inherits System.Web.UI.Page

    Dim oWS As SLWebServices.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim dt As DataTable

    Private Shared ParmSite As String
    Private Shared Whse As String
    'Private Shared _SessionID As Guid

    Dim Filter As String = ""
    Dim Parms As String = ""

    Private Shared Property PSite() As String
        Get
            Return ParmSite
        End Get
        Set(value As String)
            ParmSite = value
        End Set
    End Property

    Private Shared Property PWhse() As String
        Get
            Return Whse
        End Get
        Set(value As String)
            Whse = value
        End Set
    End Property

    'Private Shared Property SessionID() As Guid
    '    Get
    '        Return _SessionID
    '    End Get
    '    Set(value As Guid)
    '        _SessionID = value
    '    End Set
    'End Property

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Session("Token") Is Nothing Then
            Response.Redirect("signin.aspx")
        Else
            If Session("Token").ToString = "" Then
                Response.Redirect("signin.aspx")
            End If

        End If

        ListCondition()

        If Not Page.IsPostBack Then

            If Request.QueryString("Print") = "" Or String.IsNullOrEmpty(Request.QueryString("Print")) Then
                Response.Redirect("MenuRePrintSticker.aspx")
            End If

            'SessionID = Guid.NewGuid

            Parms = "<Parameters><Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

            oWS = New SLWebServices.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_EX_MV_Trans", "PPCC_Ex_GetSessionIDSp", Parms)

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)

            Dim i As Integer = 1

            For Each node As XmlNode In doc.DocumentElement

                If i = 1 Then
                    Session("SessionID") = node.InnerText
                End If

                i += 1
            Next

            Parms = "<Parameters><Parameter>" & Session("SessionID").ToString & "</Parameter>" &
                                "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                                "<Parameter>" & Request.QueryString("Print").ToString & "</Parameter>" &
                                "</Parameters>"

            oWS = New SLWebServices.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_EX_SortRePrintBarcodes", "PPCC_EX_ListSourceRePrintStickerSp", Parms)

            If Request.QueryString("Print") = "GRN" Then

                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "GrnNum")
                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "Barcode")

            ElseIf Request.QueryString("Print") = "Misc. Receipt" Then

                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "DocNum")
                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "Barcode")


            ElseIf Request.QueryString("Print") = "Qty Move" Then

                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "DocNum")
                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "Barcode")


            ElseIf Request.QueryString("Print") = "RMA" Then

                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "DocNum")
                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "Barcode")


            ElseIf Request.QueryString("Print") = "Unposted Job" Then

                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "DocNum")
                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "Job")
                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "Lot")
                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "Barcode")


            End If

        End If

    End Sub

    Protected Sub ddlstartjob_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlstartjob.SelectedIndexChanged

        GetCoreWeightAndRoll(RTrim(ddlstartjob.SelectedItem.Value), _
                                      RTrim(ddlendjob.SelectedItem.Value), _
                                      txtstartsuffix.Text, txtendsuffix.Text, _
                                      vCoreweight.Value, vBarcodeType.Value)

    End Sub

    Protected Sub ddlendjob_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlendjob.SelectedIndexChanged

        GetCoreWeightAndRoll(RTrim(ddlstartjob.SelectedItem.Value), _
                                      RTrim(ddlendjob.SelectedItem.Value), _
                                      txtstartsuffix.Text, txtendsuffix.Text, _
                                      vCoreweight.Value, vBarcodeType.Value)

    End Sub

    Protected Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click
        Response.Redirect("MenuRePrintSticker.aspx")
    End Sub

    Protected Sub btnpreview_Click(sender As Object, e As EventArgs) Handles btnpreview.Click

        If Request.QueryString("Print") = "GRN" Then
            vBGTaskName.Value = "PPCC_WMSGRNBarcode"

        ElseIf Request.QueryString("Print") = "Unposted Job" And vBarcodeType.Value = "ENV" Then
            vBGTaskName.Value = "PPCC_WMSJobBarcode_ENV"

        ElseIf Request.QueryString("Print") = "Unposted Job" And vBarcodeType.Value = "ROL" Then
            vBGTaskName.Value = "PPCC_WMSJobBarcode_ROL"
        Else
            vBGTaskName.Value = "PPCC_WMSOtherBarcode"

        End If

        If ddlstartdocnum.Items.Count = 0 Then
            ddlstartdocnum.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlenddocnum.Items.Count = 0 Then
            ddlenddocnum.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlstartjob.Items.Count = 0 Then
            ddlstartjob.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlendjob.Items.Count = 0 Then
            ddlendjob.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlstartgrn.Items.Count = 0 Then
            ddlstartgrn.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlendgrn.Items.Count = 0 Then
            ddlendgrn.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlstartbarcode.Items.Count = 0 Then
            ddlstartbarcode.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlendbarcode.Items.Count = 0 Then
            ddlendbarcode.Items.Insert(0, New ListItem("", ""))
        End If


        Dim TaskParms As String = ""
        TaskParms = "~LIT~(" & ddlcondition.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartdocnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlenddocnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartjob.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendjob.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartsuffix.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendsuffix.Text & "),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(" & txtqty.Text & "),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(0),"
        TaskParms = TaskParms & "~LIT~(" & vCoreweight.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartgrn.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendgrn.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartgrnline.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendgrnline.Text & "),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(3),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartbarcode.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendbarcode.SelectedItem.Value & ") "

        Parms = "<Parameters><Parameter>" & vBGTaskName.Value & "</Parameter>" &
                            "<Parameter>" & TaskParms & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter>" & "Preview" & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & String.Empty & "</Parameter>" &
                            "</Parameters>"

        oWS = New SLWebServices.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSInsertActiveBGTaskForPrintBCSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)
        Dim TaskNumber As String = ""
        Dim i As Integer = 1
        Dim CallTaskCount As Integer = 0

        For Each node As XmlNode In doc.DocumentElement
            If i = 5 Then
                If Trim(node.InnerText) <> String.Empty Then
                    TaskNumber = node.InnerText
                    Call TaskManRunning(TaskNumber, "Preview")
                    CallTaskCount += 1
                End If
            End If
            i += 1
        Next

        If CallTaskCount = 0 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','Report Filed', 'error');", True)
            Exit Sub
        End If

        Dim script As String = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", script, True)

    End Sub

    Protected Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnprint.Click


        If Request.QueryString("Print") = "GRN" Then
            vBGTaskName.Value = "PPCC_WMSGRNBarcode"

        ElseIf Request.QueryString("Print") = "Unposted Job" And vBarcodeType.Value = "ENV" Then
            vBGTaskName.Value = "PPCC_WMSJobBarcode_ENV"

        ElseIf Request.QueryString("Print") = "Unposted Job" And vBarcodeType.Value = "ROL" Then
            vBGTaskName.Value = "PPCC_WMSJobBarcode_ROL"
        Else
            vBGTaskName.Value = "PPCC_WMSOtherBarcode"

        End If

        If ddlstartdocnum.Items.Count = 0 Then
            ddlstartdocnum.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlenddocnum.Items.Count = 0 Then
            ddlenddocnum.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlstartjob.Items.Count = 0 Then
            ddlstartjob.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlendjob.Items.Count = 0 Then
            ddlendjob.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlstartgrn.Items.Count = 0 Then
            ddlstartgrn.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlendgrn.Items.Count = 0 Then
            ddlendgrn.Items.Insert(0, New ListItem("", ""))
        End If

        Dim TaskParms As String = ""
        TaskParms = "~LIT~(" & ddlcondition.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartdocnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlenddocnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartjob.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendjob.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartsuffix.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendsuffix.Text & "),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(" & txtqty.Text & "),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(1),"
        TaskParms = TaskParms & "~LIT~(" & vCoreweight.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartgrn.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendgrn.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartgrnline.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendgrnline.Text & "),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(3),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartbarcode.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendbarcode.SelectedItem.Value & ") "
        

        Parms = "<Parameters><Parameter>" & vBGTaskName.Value & "</Parameter>" &
                            "<Parameter>" & TaskParms & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter>" & "Print" & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & String.Empty & "</Parameter>" &
                            "</Parameters>"

        oWS = New SLWebServices.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSInsertActiveBGTaskForPrintBCSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)
        Dim TaskNumber As String = ""
        Dim i As Integer = 1
        Dim CallTaskCount As Integer = 0

        For Each node As XmlNode In doc.DocumentElement
            If i = 5 Then
                If Trim(node.InnerText) <> String.Empty Then
                    TaskNumber = node.InnerText
                    Call TaskManRunning(TaskNumber, "Print")
                    CallTaskCount += 1
                End If
            End If
            i += 1
        Next

        If Not IsNothing(Session("UrlReport")) Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Success','Report Submitted', 'success');", True)
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','Report Filed', 'error');", True)
            Exit Sub
        End If


        Dim script As String = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", script, True)


    End Sub

    'Sub ListSort(Type As String)

    '    oWS = New SLWebServices.DOWebServiceSoapClient

    '    If Request.QueryString("Print") = "GRN" Then

    '        '----------------------- Dropdownlist GRN -----------------------'

    '        ds = New DataSet

    '        ds = oWS.LoadDataSet(Session("Token").ToString, "SLGrnHdrs", "GrnNum, VendNum", "", "GrnNum", "", 0)

    '        For Each dRow As DataRow In ds.Tables(0).Rows
    '            ddlstartgrn.Items.Add(New ListItem(dRow("GrnNum") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("VendNum"), UCase(dRow("GrnNum"))))
    '            ddlendgrn.Items.Add(New ListItem(dRow("GrnNum") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("VendNum"), UCase(dRow("GrnNum"))))
    '        Next

    '        ddlstartgrn.Items.Insert(0, New ListItem("", ""))
    '        ddlendgrn.Items.Insert(0, New ListItem("", ""))


    '        '----------------------- Dropdownlist Barcode -----------------------'

    '        Filter = "last_print_date is not null And Type = 'R' "
    '        Filter = Filter & "and Convert(nvarchar(10), CreateDate, 120) >= dateadd(MONTH, -6, Convert(nvarchar(10), getdate(), 120)) "
    '        Filter = Filter & "and Convert(nvarchar(10), CreateDate, 120) <= Convert(nvarchar(10), getdate(), 120)"

    '        ds = New DataSet

    '        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSBarcodes", "Barcode", Filter, "Barcode", "", 0)

    '        dt = New DataTable
    '        dt = ds.Tables(0).DefaultView.ToTable(True, "Barcode")

    '        For Each dRow As DataRow In dt.Rows
    '            ddlstartbarcode.Items.Add(New ListItem(dRow("Barcode"), UCase(dRow("Barcode"))))
    '            ddlendbarcode.Items.Add(New ListItem(dRow("Barcode"), UCase(dRow("Barcode"))))
    '        Next

    '        ddlstartbarcode.Items.Insert(0, New ListItem("", ""))
    '        ddlendbarcode.Items.Insert(0, New ListItem("", ""))


    '    ElseIf Request.QueryString("Print") = "Misc. Receipt" Or Request.QueryString("Print") = "Qty Move" Or Request.QueryString("Print") = "RMA" Then

    '        '----------------------- Dropdownlist Document Number -----------------------'

    '        If Request.QueryString("Print") = "Misc. Receipt" Then
    '            Filter = "DocumentNum is not null and DocumentNum <> '' and TransType = 'H' and RefType = 'I' "
    '        ElseIf Request.QueryString("Print") = "Qty Move" Then
    '            Filter = "DocumentNum is not null and DocumentNum <> '' and TransType = 'M' and RefType = 'I' "
    '        ElseIf Request.QueryString("Print") = "RMA" Then
    '            Filter = "DocumentNum is not null and DocumentNum <> '' and TransType = 'W' and RefType = 'R' "
    '        End If

    '        Filter = Filter & "and Convert(nvarchar(10), CreateDate, 120) >= dateadd(MONTH, -6, Convert(nvarchar(10), getdate(), 120)) "
    '        Filter = Filter & "and Convert(nvarchar(10), CreateDate, 120) <= Convert(nvarchar(10), getdate(), 120)"

    '        ds = New DataSet

    '        ds = oWS.LoadDataSet(Session("Token").ToString, "SLMatltrans", "DocumentNum", Filter, "DocumentNum", "", 0)

    '        dt = New DataTable
    '        dt = ds.Tables(0).DefaultView.ToTable(True, "DocumentNum")

    '        For Each dRow As DataRow In dt.Rows
    '            ddlstartdocnum.Items.Add(New ListItem(dRow("DocumentNum"), UCase(dRow("DocumentNum"))))
    '            ddlenddocnum.Items.Add(New ListItem(dRow("DocumentNum"), UCase(dRow("DocumentNum"))))
    '        Next

    '        ddlstartdocnum.Items.Insert(0, New ListItem("", ""))
    '        ddlenddocnum.Items.Insert(0, New ListItem("", ""))


    '        '----------------------- Dropdownlist Barcode -----------------------'

    '        If Request.QueryString("Print") = "Misc. Receipt" Then
    '            Filter = "last_print_date is not null And Type = 'H' "
    '        ElseIf Request.QueryString("Print") = "Qty Move" Then
    '            Filter = "last_print_date is not null And Type = 'M' "
    '        ElseIf Request.QueryString("Print") = "RMA" Then
    '            Filter = "last_print_date is not null And Type = 'W' "
    '        End If

    '        Filter = Filter & "and Convert(nvarchar(10), CreateDate, 120) >= dateadd(MONTH, -6, Convert(nvarchar(10), getdate(), 120)) "
    '        Filter = Filter & "and Convert(nvarchar(10), CreateDate, 120) <= Convert(nvarchar(10), getdate(), 120)"


    '        ds = New DataSet

    '        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSBarcodes", "Barcode", Filter, "Barcode", "", 0)

    '        dt = New DataTable
    '        dt = ds.Tables(0).DefaultView.ToTable(True, "Barcode")

    '        For Each dRow As DataRow In dt.Rows
    '            ddlstartbarcode.Items.Add(New ListItem(dRow("Barcode"), UCase(dRow("Barcode"))))
    '            ddlendbarcode.Items.Add(New ListItem(dRow("Barcode"), UCase(dRow("Barcode"))))
    '        Next

    '        ddlstartbarcode.Items.Insert(0, New ListItem("", ""))
    '        ddlendbarcode.Items.Insert(0, New ListItem("", ""))


    '    ElseIf Request.QueryString("Print") = "Unposted Job" Then

    '        '----------------------- Dropdownlist Job -----------------------'

    '        Filter = "CHARINDEX(job.type, 'J') > 0"

    '        ds = New DataSet

    '        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "Job, Suffix, Item", Filter, "Job, Suffix", "", 0)

    '        For Each dRow As DataRow In ds.Tables(0).Rows
    '            ddlstartjob.Items.Add(New ListItem(dRow("Job") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Suffix") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Item"), UCase(dRow("Job"))))
    '            ddlendjob.Items.Add(New ListItem(dRow("Job") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Suffix") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Item"), UCase(dRow("Job"))))
    '        Next

    '        ddlstartjob.Items.Insert(0, New ListItem("", ""))
    '        ddlendjob.Items.Insert(0, New ListItem("", ""))

    '        '----------------------- Dropdownlist Lot -----------------------'

    '        Filter = "Convert(nvarchar(10), CreateDate, 120) >= dateadd(MONTH, -6, Convert(nvarchar(10), getdate(), 120)) "
    '        Filter = Filter & "and Convert(nvarchar(10), CreateDate, 120) <= Convert(nvarchar(10), getdate(), 120)"

    '        ds = New DataSet

    '        ds = oWS.LoadDataSet(Session("Token").ToString, "SLLots", "Lot, Item", Filter, "Lot", "", 0)

    '        For Each dRow As DataRow In ds.Tables(0).Rows
    '            ddlstartlot.Items.Add(New ListItem(dRow("Lot") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Item"), UCase(dRow("Lot"))))
    '            ddlendlot.Items.Add(New ListItem(dRow("Lot") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Item"), UCase(dRow("Lot"))))
    '        Next

    '        ddlstartlot.Items.Insert(0, New ListItem("", ""))
    '        ddlendlot.Items.Insert(0, New ListItem("", ""))


    '        '----------------------- Dropdownlist Barcode -----------------------'

    '        Filter = "last_print_date is not null And Type = 'F' "
    '        Filter = Filter & "Convert(nvarchar(10), CreateDate, 120) >= dateadd(MONTH, -6, Convert(nvarchar(10), getdate(), 120)) "
    '        Filter = Filter & "and Convert(nvarchar(10), CreateDate, 120) <= Convert(nvarchar(10), getdate(), 120)"

    '        ds = New DataSet

    '        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSBarcodes", "Barcode", Filter, "Barcode", "", 0)

    '        dt = New DataTable
    '        dt = ds.Tables(0).DefaultView.ToTable(True, "Barcode")

    '        For Each dRow As DataRow In dt.Rows
    '            ddlstartbarcode.Items.Add(New ListItem(dRow("Barcode"), UCase(dRow("Barcode"))))
    '            ddlendbarcode.Items.Add(New ListItem(dRow("Barcode"), UCase(dRow("Barcode"))))
    '        Next

    '        ddlstartbarcode.Items.Insert(0, New ListItem("", ""))
    '        ddlendbarcode.Items.Insert(0, New ListItem("", ""))

    '    End If


    'End Sub

    Sub ListSort(SessionID As String, Condition As String, FieldType As String)

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        Filter = "SessionID = '" & SessionID & "' AND FieldType = '" & FieldType & "' AND Condition = '" & Condition & "'"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_SortRePrintBarcodes", "Field1, Field2, Field3", Filter, "Field1, Field2", "", 0)

        If Request.QueryString("Print") = "GRN" Then

            If FieldType = "GrnNum" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartgrn.Items.Add(New ListItem(dRow("Field1") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Field2"), UCase(dRow("Field1"))))
                    ddlendgrn.Items.Add(New ListItem(dRow("Field1") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Field2"), UCase(dRow("Field1"))))
                Next

                ddlstartgrn.Items.Insert(0, New ListItem("", ""))
                ddlendgrn.Items.Insert(0, New ListItem("", ""))

            ElseIf FieldType = "Barcode" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartbarcode.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                    ddlendbarcode.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                Next

                ddlstartbarcode.Items.Insert(0, New ListItem("", ""))
                ddlendbarcode.Items.Insert(0, New ListItem("", ""))

            End If

        ElseIf Request.QueryString("Print") = "Misc. Receipt" Then

            If FieldType = "DocNum" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartdocnum.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                    ddlenddocnum.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                Next

                ddlstartdocnum.Items.Insert(0, New ListItem("", ""))
                ddlenddocnum.Items.Insert(0, New ListItem("", ""))

            ElseIf FieldType = "Barcode" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartbarcode.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                    ddlendbarcode.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                Next

                ddlstartbarcode.Items.Insert(0, New ListItem("", ""))
                ddlendbarcode.Items.Insert(0, New ListItem("", ""))

            End If
            

        ElseIf Request.QueryString("Print") = "Qty Move" Then

            If FieldType = "DocNum" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartdocnum.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                    ddlenddocnum.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                Next

                ddlstartdocnum.Items.Insert(0, New ListItem("", ""))
                ddlenddocnum.Items.Insert(0, New ListItem("", ""))

            ElseIf FieldType = "Barcode" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartbarcode.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                    ddlendbarcode.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                Next

                ddlstartbarcode.Items.Insert(0, New ListItem("", ""))
                ddlendbarcode.Items.Insert(0, New ListItem("", ""))

            End If


        ElseIf Request.QueryString("Print") = "RMA" Then

            If FieldType = "DocNum" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartdocnum.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                    ddlenddocnum.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                Next

                ddlstartdocnum.Items.Insert(0, New ListItem("", ""))
                ddlenddocnum.Items.Insert(0, New ListItem("", ""))

            ElseIf FieldType = "Barcode" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartbarcode.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                    ddlendbarcode.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                Next

                ddlstartbarcode.Items.Insert(0, New ListItem("", ""))
                ddlendbarcode.Items.Insert(0, New ListItem("", ""))

            End If

        ElseIf Request.QueryString("Print") = "Unposted Job" Then

            If FieldType = "DocNum" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartdocnum.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                    ddlenddocnum.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                Next

                ddlstartdocnum.Items.Insert(0, New ListItem("", ""))
                ddlenddocnum.Items.Insert(0, New ListItem("", ""))

            ElseIf FieldType = "Job" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartjob.Items.Add(New ListItem(dRow("Field1") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Field2") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Field3"), UCase(dRow("Field1"))))
                    ddlendjob.Items.Add(New ListItem(dRow("Field1") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Field2") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Field3"), UCase(dRow("Field1"))))
                Next

                ddlstartjob.Items.Insert(0, New ListItem("", ""))
                ddlendjob.Items.Insert(0, New ListItem("", ""))

            ElseIf FieldType = "Lot" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartlot.Items.Add(New ListItem(dRow("Field1") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Field2"), UCase(dRow("Field1"))))
                    ddlendlot.Items.Add(New ListItem(dRow("Field1") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Field2"), UCase(dRow("Field1"))))
                Next

                ddlstartlot.Items.Insert(0, New ListItem("", ""))
                ddlendlot.Items.Insert(0, New ListItem("", ""))

            ElseIf FieldType = "Barcode" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartbarcode.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                    ddlendbarcode.Items.Add(New ListItem(dRow("Field1"), UCase(dRow("Field1"))))
                Next

                ddlstartbarcode.Items.Insert(0, New ListItem("", ""))
                ddlendbarcode.Items.Insert(0, New ListItem("", ""))

            End If


        End If


    End Sub

    Sub ListCondition()

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "UserDefinedTypeValues", "Value", "TypeName = 'PPCC_ConditionBarcode'", "Value", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlcondition.Items.Add(New ListItem(dRow("Value"), dRow("Value")))
        Next

        ddlcondition.Items.Insert(0, New ListItem("", ""))

        ddlcondition.SelectedIndex = ddlcondition.Items.IndexOf(ddlcondition.Items.FindByValue(Request.QueryString("Print")))

    End Sub

    Protected Sub GetCoreWeightAndRoll(ByVal StartJob As String, _
                                       ByVal EndJob As String, _
                                       ByVal StartSuffix As String, _
                                       ByVal EndSuffix As String, _
                                       ByRef CoreWeigth As String, _
                                       ByRef BarcodeType As String)

        Parms = "<Parameters><Parameter>" & RTrim(ddlstartjob.SelectedItem.Value) & "</Parameter>" &
                            "<Parameter>" & RTrim(ddlendjob.SelectedItem.Value) & "</Parameter>" &
                            "<Parameter>" & txtstartsuffix.Text & "</Parameter>" &
                            "<Parameter>" & txtendsuffix.Text & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "</Parameters>"

        oWS = New SLWebServices.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "SP!", "PPCC_WMSGetCoreWeigth", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 5 Then
                CoreWeigth = node.InnerText

            ElseIf i = 6 Then
                BarcodeType = node.InnerText

            End If

            i += 1

        Next

    End Sub

    Function TaskManRunning(ByVal TaskNumber As String, ProcessType As String) As Boolean
        Dim StartDate As String = ""
        Dim CompletionDate As String = ""
        Dim ds As New Data.DataSet
        Dim i As Integer = 0
        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "TaskNumber, SubmissionDate, StartDate, CompletionDate", "TaskNumber=" & TaskNumber, "", "", 0)
        StartDate = ds.Tables(0).Rows(0)("StartDate").ToString

        Dim TaskInterval1 As String = System.Configuration.ConfigurationManager.AppSettings("TaskInterval1")
        Dim TaskInterval2 As String = System.Configuration.ConfigurationManager.AppSettings("TaskInterval2")

        If TaskInterval1 Is Nothing Then
            TaskInterval1 = "240" 'Max 2 Minutes
        End If

        If TaskInterval2 Is Nothing Then
            TaskInterval2 = "600" 'Max 5 Minutes
        End If

        While StartDate = "" And i < Convert.ToInt32(TaskInterval1)
            ds = New DataSet
            ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "StartDate", "TaskNumber=" & TaskNumber, "", "", 0)
            StartDate = ds.Tables(0).Rows(0)("StartDate").ToString
            System.Threading.Thread.Sleep(500)
            i += 1
        End While

        If StartDate <> "" Then
            i = 0
            ds = New DataSet
            ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "CompletionDate", "TaskNumber=" & TaskNumber, "", "", 0)

            While CompletionDate = "" And i < Convert.ToInt32(TaskInterval2)
                ds = New DataSet
                ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "CompletionDate", "TaskNumber=" & TaskNumber, "", "", 0)
                CompletionDate = ds.Tables(0).Rows(0)("CompletionDate").ToString
                System.Threading.Thread.Sleep(500)
                i += 1
            End While
        End If


        Dim FileName As String = ""
        Dim OutputPath As String = ""
        If CompletionDate <> "" Then

            ds = New DataSet
            ds = oWS.LoadDataSet(Session("Token").ToString, "BGTaskHistories", "ReportOutputPath", "TaskNumber=" & TaskNumber, "", "", 0)
            OutputPath = ds.Tables(0).Rows(0)("ReportOutputPath").ToString
            FileName = OutputPath.Substring(OutputPath.LastIndexOf("\"c) + 1)

        End If

        If FileName <> "" Then
            Dim url As String = System.Configuration.ConfigurationManager.AppSettings("ReportAddress")

            If ProcessType = "Preview" Then
                Session("UrlReport") = url & Session("UserName").ToString & "/Preview/" & FileName
            Else
                Session("UrlReport") = url & Session("UserName").ToString & "/" & FileName
            End If

        End If

        Return True

    End Function

End Class

