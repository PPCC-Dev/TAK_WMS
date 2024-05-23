Imports System.Data
Imports System.Web.Services
Imports System.IO
Imports System.Xml

Partial Class PrintStickerBarcode
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
                Response.Redirect("MenuPrintSticker.aspx")            
            End If

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

            'SessionID = Guid.NewGuid
            txtbarcodedate.Text = Date.Now.ToString("dd/MM/yyyy")
            'ListCondition()

            Parms = "<Parameters><Parameter>" & Session("SessionID").ToString & "</Parameter>" &
                                "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                                "<Parameter>" & Request.QueryString("Print").ToString & "</Parameter>" &
                                "</Parameters>"

            oWS = New SLWebServices.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_EX_SortPrintBarcodes", "PPCC_EX_ListSourcePrintStickerSp", Parms)



            If Request.QueryString("Print") = "GRN" Then

                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "GrnNum")
                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "PoNum")
                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "Vendor")

            ElseIf Request.QueryString("Print") = "Misc. Receipt" Then

                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "DocNum")


            ElseIf Request.QueryString("Print") = "Qty Move" Then

                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "DocNum")

                


            ElseIf Request.QueryString("Print") = "RMA" Then

                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "DocNum")


            ElseIf Request.QueryString("Print") = "Unposted Job" Then

                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "DocNum")
                ListSort(Session("SessionID").ToString, Request.QueryString("Print").ToString, "Job")
                'ListProductionEmp()
                'ListQCEmp()
                'txtstartdate.Text = Date.Now.ToString("dd/MM/yyyy")
                'txtenddate.Text = Date.Now.ToString("dd/MM/yyyy")

                vMultipleWays.Value = GetMultipleWays(RTrim(txtstartjobtran.Text))

                If vMultipleWays.Value = "1" Then
                    rdoworkoneway.Checked = True
                Else
                    rdoworkmanyway.Checked = False
                End If

            End If

        End If

    End Sub

    Protected Sub ddlstartjob_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlstartjob.SelectedIndexChanged

        GetCoreWeightAndRoll(RTrim(ddlstartjob.SelectedItem.Value), _
                                      RTrim(ddlendjob.SelectedItem.Value), _
                                      txtstartsuffix.Text, txtendsuffix.Text, _
                                      txtcoreweight.Text, vBarcodeType.Value)

        If txtcoreweight.Text <> "" Then
            txtcoreweight.Text = FormatNumber(txtcoreweight.Text, 3)
        End If

        ChangQtyMaltiWay(RTrim(ddlstartjob.SelectedItem.Value), _
                         RTrim(ddlstartjob.SelectedItem.Value), _
                         txtstartsuffix.Text, txtendsuffix.Text)

        'BindGrid()

        'MsgBox(vBarcodeType.Value)

    End Sub

    Protected Sub ddlendjob_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlendjob.SelectedIndexChanged

        GetCoreWeightAndRoll(RTrim(ddlstartjob.SelectedItem.Value), _
                                      RTrim(ddlendjob.SelectedItem.Value), _
                                      txtstartsuffix.Text, txtendsuffix.Text, _
                                      txtcoreweight.Text, vBarcodeType.Value)

        If txtcoreweight.Text <> "" Then
            txtcoreweight.Text = FormatNumber(txtcoreweight.Text, 3)
        Else
            txtcoreweight.Text = FormatNumber("0.000", 3)
        End If

        ChangQtyMaltiWay(RTrim(ddlstartjob.SelectedItem.Value), _
                         RTrim(ddlendjob.SelectedItem.Value), _
                         txtstartsuffix.Text, txtendsuffix.Text)

        'BindGrid()

        'MsgBox(vBarcodeType.Value)

    End Sub


    Protected Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click
        Response.Redirect("MenuPrintSticker.aspx")
    End Sub

    Protected Sub btnpreview_Click(sender As Object, e As EventArgs) Handles btnpreview.Click

        If rdoworkoneway.Checked = True Then
            vMultipleWays.Value = "1"
        ElseIf rdoworkmanyway.Checked = True Then
            vMultipleWays.Value = "2"
        End If

        If Request.QueryString("Print") = "Unposted Job" Then

            If chkeditqty.Checked = True And rdoworkmanyway.Checked = True Then

                If GridView1.Rows.Count > 0 Then

                    For Each row As GridViewRow In GridView1.Rows

                        Dim lblRowPointer As Label = CType(row.FindControl("lblRowPointer"), Label)
                        Dim txtQty As TextBox = CType(row.FindControl("txtQty"), TextBox)

                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Parms = "<Parameters><Parameter>" & lblRowPointer.Text & "</Parameter>" &
                                            "<Parameter>" & txtQty.Text & "</Parameter>" &
                                            "</Parameters>"

                        oWS = New SLWebServices.DOWebServiceSoapClient
                        oWS.CallMethod(Session("Token").ToString, "PPCC_EX_SortPrintBarcodes", "PPCC_Ex_ChangeQtyPrintBarcodeSp", Parms)

                    Next

                End If

            End If

            Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter></Parameters>"

            oWS = New SLWebServices.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "SP!", "PPCC_WMSCalChangQtyMaltiWaySp", Parms)

            If chklastunposted.Checked = True Then
                vMultipleWays.Value = CInt(vMultipleWays.Value) + 5
            End If

        End If


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

        'If ddlproductionemp.Items.Count = 0 Then
        '    ddlproductionemp.Items.Insert(0, New ListItem("", ""))
        'End If

        'If ddlqcemp.Items.Count = 0 Then
        '    ddlqcemp.Items.Insert(0, New ListItem("", ""))
        'End If

        If ddlstartgrn.Items.Count = 0 Then
            ddlstartgrn.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlendgrn.Items.Count = 0 Then
            ddlendgrn.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlstartpo.Items.Count = 0 Then
            ddlstartpo.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlendpo.Items.Count = 0 Then
            ddlendpo.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlstartvendnum.Items.Count = 0 Then
            ddlstartvendnum.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlendvendnum.Items.Count = 0 Then
            ddlendvendnum.Items.Insert(0, New ListItem("", ""))
        End If


        Dim TaskParms As String = ""
        TaskParms = "~LIT~(" & ddlcondition.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & CDate(txtbarcodedate.Text).ToString("yyyy-MM-dd") & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartdocnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlenddocnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartjob.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendjob.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartsuffix.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendsuffix.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartjobtran.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendjobtran.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartdate.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtenddate.Text & "),"
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & "~LIT~(" & txtproductionemp.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtqty.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtqcemp.Text & "),"
        TaskParms = TaskParms & "~LIT~(0),"
        TaskParms = TaskParms & "~LIT~(" & txtcoreweight.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartgrn.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendgrn.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartgrnline.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendgrnline.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartpo.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendpo.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartpoline.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendpoline.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartvendnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendvendnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & vMultipleWays.Value & "),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~()"


        'TaskParms = ddlcondition.SelectedItem.Value & "," & CDate(txtbarcodedate.Text).ToString("yyyy-MM-dd") & "," & ddlstartdocnum.SelectedItem.Value & ","
        'TaskParms = TaskParms & ddlenddocnum.SelectedItem.Value & "," & ddlstartjob.SelectedItem.Value & "," & ddlendjob.SelectedItem.Value & ","
        'TaskParms = TaskParms & txtstartsuffix.Text & "," & txtendsuffix.Text & "," & txtstartjobtran.Text & "," & txtendjobtran.Text & ","
        'TaskParms = TaskParms & txtstartdate.Text & "," & txtenddate.Text & ",,,,,,,,," & ddlproductionemp.SelectedItem.Value & ","
        'TaskParms = TaskParms & txtqty.Text & "," & ddlqcemp.SelectedItem.Value & ",0," & txtcoreweight.Text & ","
        'TaskParms = TaskParms & ddlstartgrn.SelectedItem.Value & "," & ddlendgrn.SelectedItem.Value & "," & txtstartgrnline.Text & ","
        'TaskParms = TaskParms & txtendgrnline.Text & "," & ddlstartpo.SelectedItem.Value & "," & ddlendpo.SelectedItem.Value & ","
        'TaskParms = TaskParms & txtstartpoline.Text & "," & txtendpoline.Text & "," & ddlstartvendnum.SelectedItem.Value & ","
        'TaskParms = TaskParms & ddlendvendnum.SelectedItem.Value & "," & IIf(Request.QueryString("Print") <> "Unposted Job", IIf(rdoworkoneway.Checked = True, 1, 2), vMultipleWays.Value) & "," & ",,,"

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

        If Request.QueryString("Print") = "Unposted Job" Then

            If chklastunposted.Checked = True Then
                vMultipleWays.Value = CInt(vMultipleWays.Value) - 5
            End If

            ChangQtyMaltiWay(RTrim(ddlstartjob.SelectedItem.Value), _
                         RTrim(ddlstartjob.SelectedItem.Value), _
                         txtstartsuffix.Text, txtendsuffix.Text)

        End If

        Dim script As String = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", script, True)

    End Sub

    Protected Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnprint.Click

        If rdoworkoneway.Checked = True Then
            vMultipleWays.Value = "1"
        ElseIf rdoworkmanyway.Checked = True Then
            vMultipleWays.Value = "2"
        End If

        If Request.QueryString("Print") = "Unposted Job" Then

            If chkeditqty.Checked = True And rdoworkmanyway.Checked = True Then

                If GridView1.Rows.Count > 0 Then

                    For Each row As GridViewRow In GridView1.Rows

                        Dim lblRowPointer As Label = CType(row.FindControl("lblRowPointer"), Label)
                        Dim txtQty As TextBox = CType(row.FindControl("txtQty"), TextBox)

                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Parms = "<Parameters><Parameter>" & lblRowPointer.Text & "</Parameter>" &
                                            "<Parameter>" & txtQty.Text & "</Parameter>" &
                                            "</Parameters>"

                        oWS = New SLWebServices.DOWebServiceSoapClient
                        oWS.CallMethod(Session("Token").ToString, "PPCC_EX_SortPrintBarcodes", "PPCC_Ex_ChangeQtyPrintBarcodeSp", Parms)

                    Next

                End If

            End If

            Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter></Parameters>"

            oWS = New SLWebServices.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "SP!", "PPCC_WMSCalChangQtyMaltiWaySp", Parms)

            If chklastunposted.Checked = True Then
                vMultipleWays.Value = CInt(vMultipleWays.Value) + 5
            End If

        End If


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

        'If ddlproductionemp.Items.Count = 0 Then
        '    ddlproductionemp.Items.Insert(0, New ListItem("", ""))
        'End If

        'If ddlqcemp.Items.Count = 0 Then
        '    ddlqcemp.Items.Insert(0, New ListItem("", ""))
        'End If

        If ddlstartgrn.Items.Count = 0 Then
            ddlstartgrn.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlendgrn.Items.Count = 0 Then
            ddlendgrn.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlstartpo.Items.Count = 0 Then
            ddlstartpo.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlendpo.Items.Count = 0 Then
            ddlendpo.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlstartvendnum.Items.Count = 0 Then
            ddlstartvendnum.Items.Insert(0, New ListItem("", ""))
        End If

        If ddlendvendnum.Items.Count = 0 Then
            ddlendvendnum.Items.Insert(0, New ListItem("", ""))
        End If


        Dim TaskParms As String = ""
        TaskParms = "~LIT~(" & ddlcondition.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & CDate(txtbarcodedate.Text).ToString("yyyy-MM-dd") & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartdocnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlenddocnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartjob.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendjob.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartsuffix.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendsuffix.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartjobtran.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendjobtran.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartdate.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtenddate.Text & "),"
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & ","
        TaskParms = TaskParms & "~LIT~(" & txtproductionemp.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtqty.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtqcemp.Text & "),"
        TaskParms = TaskParms & "~LIT~(1),"
        TaskParms = TaskParms & "~LIT~(" & txtcoreweight.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartgrn.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendgrn.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartgrnline.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendgrnline.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartpo.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendpo.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & txtstartpoline.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & txtendpoline.Text & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlstartvendnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & ddlendvendnum.SelectedItem.Value & "),"
        TaskParms = TaskParms & "~LIT~(" & vMultipleWays.Value & "),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~(),"
        TaskParms = TaskParms & "~LIT~()"

        'TaskParms = ddlcondition.SelectedItem.Value & "," & CDate(txtbarcodedate.Text).ToString("yyyy-MM-dd") & "," & ddlstartdocnum.SelectedItem.Value & ","
        'TaskParms = TaskParms & ddlenddocnum.SelectedItem.Value & "," & ddlstartjob.SelectedItem.Value & "," & ddlendjob.SelectedItem.Value & ","
        'TaskParms = TaskParms & txtstartsuffix.Text & "," & txtendsuffix.Text & "," & txtstartjobtran.Text & "," & txtendjobtran.Text & ","
        'TaskParms = TaskParms & txtstartdate.Text & "," & txtenddate.Text & ",,,,,,,,," & ddlproductionemp.SelectedItem.Value & ","
        'TaskParms = TaskParms & txtqty.Text & "," & ddlqcemp.SelectedItem.Value & ",1," & txtcoreweight.Text & ","
        'TaskParms = TaskParms & ddlstartgrn.SelectedItem.Value & "," & ddlendgrn.SelectedItem.Value & "," & txtstartgrnline.Text & ","
        'TaskParms = TaskParms & txtendgrnline.Text & "," & ddlstartpo.SelectedItem.Value & "," & ddlendpo.SelectedItem.Value & ","
        'TaskParms = TaskParms & txtstartpoline.Text & "," & txtendpoline.Text & "," & ddlstartvendnum.SelectedItem.Value & ","
        'TaskParms = TaskParms & ddlendvendnum.SelectedItem.Value & "," & IIf(Request.QueryString("Print") <> "Unposted Job", IIf(rdoworkoneway.Checked = True, 1, 2), vMultipleWays.Value) & "," & ",,,"

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

        If Request.QueryString("Print") = "Unposted Job" Then

            If chklastunposted.Checked = True Then
                vMultipleWays.Value = CInt(vMultipleWays.Value) - 5
            End If

            ChangQtyMaltiWay(RTrim(ddlstartjob.SelectedItem.Value), _
                         RTrim(ddlstartjob.SelectedItem.Value), _
                         txtstartsuffix.Text, txtendsuffix.Text)

        End If


        Dim script As String = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", script, True)



    End Sub

    'Private Sub chkeditqty_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkeditqty.CheckedChanged

    '    If rdoworkmanyway.Checked = True Then
    '        ChangQtyMaltiWay(ddlstartjob.SelectedItem.Value, ddlendjob.SelectedItem.Value, txtstartsuffix.Text, txtendsuffix.Text)
    '    End If

    'End Sub

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


    Sub ListSort(SessionID As String, Condition As String, FieldType As String)

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        Filter = "SessionID = '" & SessionID & "' AND FieldType = '" & FieldType & "' AND Condition = '" & Condition & "'"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_SortPrintBarcodes", "Field", Filter, "Field", "", 0)

        If Request.QueryString("Print") = "GRN" Then

            If FieldType = "GrnNum" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartgrn.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                    ddlendgrn.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                Next

                ddlstartgrn.Items.Insert(0, New ListItem("", ""))
                ddlendgrn.Items.Insert(0, New ListItem("", ""))

            ElseIf FieldType = "PoNum" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartpo.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                    ddlendpo.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                Next

                ddlstartpo.Items.Insert(0, New ListItem("", ""))
                ddlendpo.Items.Insert(0, New ListItem("", ""))

            ElseIf FieldType = "Vendor" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartvendnum.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                    ddlendvendnum.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                Next

                ddlstartvendnum.Items.Insert(0, New ListItem("", ""))
                ddlendvendnum.Items.Insert(0, New ListItem("", ""))

            End If

        ElseIf Request.QueryString("Print") = "Misc. Receipt" Then

            For Each dRow As DataRow In ds.Tables(0).Rows
                ddlstartdocnum.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                ddlenddocnum.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
            Next

            ddlstartdocnum.Items.Insert(0, New ListItem("", ""))
            ddlenddocnum.Items.Insert(0, New ListItem("", ""))

        ElseIf Request.QueryString("Print") = "Qty Move" Then

            For Each dRow As DataRow In ds.Tables(0).Rows
                ddlstartdocnum.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                ddlenddocnum.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
            Next

            ddlstartdocnum.Items.Insert(0, New ListItem("", ""))
            ddlenddocnum.Items.Insert(0, New ListItem("", ""))

            If Not Request.QueryString("DocNum") Is Nothing Then

                ddlstartdocnum.SelectedIndex = ddlstartdocnum.Items.IndexOf(ddlstartdocnum.Items.FindByValue(Request.QueryString("DocNum")))
                ddlenddocnum.SelectedIndex = ddlenddocnum.Items.IndexOf(ddlenddocnum.Items.FindByValue(Request.QueryString("DocNum")))

            End If


        ElseIf Request.QueryString("Print") = "RMA" Then

            For Each dRow As DataRow In ds.Tables(0).Rows
                ddlstartdocnum.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                ddlenddocnum.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
            Next

            ddlstartdocnum.Items.Insert(0, New ListItem("", ""))
            ddlenddocnum.Items.Insert(0, New ListItem("", ""))

        ElseIf Request.QueryString("Print") = "Unposted Job" Then

            If FieldType = "DocNum" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartdocnum.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                    ddlenddocnum.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                Next

                ddlstartdocnum.Items.Insert(0, New ListItem("", ""))
                ddlenddocnum.Items.Insert(0, New ListItem("", ""))

            ElseIf FieldType = "Job" Then

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlstartjob.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                    ddlendjob.Items.Add(New ListItem(dRow("Field"), UCase(dRow("Field"))))
                Next

                ddlstartjob.Items.Insert(0, New ListItem("", ""))
                ddlendjob.Items.Insert(0, New ListItem("", ""))

            End If


        End If


    End Sub


    'Sub ListProductionEmp()

    '    oWS = New SLWebServices.DOWebServiceSoapClient

    '    ds = New DataSet

    '    'Dept like 'PD%'

    '    ds = oWS.LoadDataSet(Session("Token").ToString, "SLEmployees", "EmpNum, Fname, Lname", "Dept like 'PD%'", "", "", 0)

    '    For Each dRow As DataRow In ds.Tables(0).Rows
    '        ddlproductionemp.Items.Add(New ListItem(dRow("EmpNum") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Fname") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("Lname"), UCase(dRow("EmpNum"))))
    '    Next

    '    ddlproductionemp.Items.Insert(0, New ListItem("", ""))


    'End Sub


    'Sub ListQCEmp()

    '    oWS = New SLWebServices.DOWebServiceSoapClient

    '    ds = New DataSet

    '    'Dept like 'QA%' or Dept like 'QC%'

    '    ds = oWS.LoadDataSet(Session("Token").ToString, "SLEmployees", "EmpNum, Fname, Lname", "Dept like 'QA%' or Dept like 'QC%'", "EmpNum", "", 0)

    '    For Each dRow As DataRow In ds.Tables(0).Rows
    '        ddlqcemp.Items.Add(New ListItem(dRow("EmpNum") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Fname") & HttpUtility.HtmlDecode("&nbsp;&nbsp;") & dRow("Lname"), UCase(dRow("EmpNum"))))
    '    Next

    '    ddlqcemp.Items.Insert(0, New ListItem("", ""))

    'End Sub

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

    Protected Sub ChangQtyMaltiWay(ByVal StartJob As String, _
                                   ByVal EndJob As String, _
                                   ByVal StartSuffix As String, _
                                   ByVal EndSuffix As String)

        Parms = "<Parameters><Parameter>" & RTrim(StartJob) & "</Parameter>" &
                            "<Parameter>" & RTrim(EndJob) & "</Parameter>" &
                            "<Parameter>" & StartSuffix & "</Parameter>" &
                            "<Parameter>" & EndSuffix & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "</Parameters>"

        oWS = New SLWebServices.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "SP!", "PPCC_WMSChangQtyMaltiWaySp", Parms)

        BindGrid()

    End Sub

    'Protected Sub DeleteQtyMaltiWay()

    '    Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter></Parameters>"

    '    oWS = New SLWebServices.DOWebServiceSoapClient
    '    oWS.CallMethod(Session("Token").ToString, "SP!", "PPCC_WMSDeleteQtyMaltiWaySp", Parms)

    'End Sub

    Sub BindGrid()

        Dim Propertie As String

        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = New DataSet
        Filter = "total_complete > 0 and CreatedBy = '" & Session("UserName").ToString & "'"
        Propertie = "job ,lot, suffix, output_row, chang_qty, RowPointer"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSChangUnpostJobTransactions", Propertie, Filter, "", "", 0)

        GridView1.DataSource = ds.Tables(0)
        GridView1.DataBind()

    End Sub

    Function GetMultipleWays(startTransNum As String) As String

        GetMultipleWays = "1"

        Dim MultipleWays As String = ""

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobTrans", "jobtUf_JobTran_RowCount", "TransNum = '" & startTransNum & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            MultipleWays = ds.Tables(0).Rows(0)("jobtUf_JobTran_RowCount").ToString

            If CInt(MultipleWays) > 1 Then
                GetMultipleWays = "2"
                rdoworkmanyway.Checked = True
            Else
                GetMultipleWays = "1"
                rdoworkoneway.Checked = True
            End If

        End If

        Return GetMultipleWays

    End Function

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
