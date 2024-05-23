Imports System.Data
Imports System.Web.Services
Imports System.IO
Imports System.Xml
Imports System.Drawing

Partial Class MassQtyMove
    Inherits System.Web.UI.Page

    Dim oWS As SLWebServices.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim dt As DataTable
    Dim LocWMS As String
    Dim LocMoveIn As String
    Dim LocMoveOut As String

    Private Shared ParmSite As String
    Private Shared Whse As String
    'Private Shared _SessionID As Guid

    Dim LenPointQty As Integer = 0
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

        LenPointQty = UnitQtyFormat()

        If Not Page.IsPostBack Then

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
            PSite = GetSite()
            PWhse = GetDefWhse()
            sItemLotTracked.Value = "0"
            sItemDesc.Value = ""
            sItemUM.Value = ""
            RowPointerSelected.Value = ""
            CommandName.Value = ""
            sItem.Value = ""
            sLot.Value = ""
            sQty.Value = ""
            sVendLot.Value = ""
            sItemQtyOnHand.Value = ""

            txtformsite.Text = PSite
            txttosite.Text = PSite
            txttransdate.Text = Date.Now.ToString("dd/MM/yyyy")
            txtreftransdate.Text = Date.Now.ToString("dd/MM/yyyy")

            ListCategoryCode()
            ListWhse()
            ListFromLocation()
            ListToLocation()
            ListJob()

            Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter></Parameters>"

            oWS = New SLWebServices.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_EX_MV_Trans", "PPCC_Ex_DeleteMVTranSp", Parms)

            BindGrid()

        End If

    End Sub

    Protected Sub ddlfromwhse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlfromwhse.SelectedIndexChanged

        If ddlfromwhse.SelectedItem.Value <> "" Then

            If ddlitem.Items.Count > 0 Then
                ddlitem.Items.Clear()
            End If

            ListFromLocation()

        End If

    End Sub

    Protected Sub ddltowhse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddltowhse.SelectedIndexChanged

        If ddltowhse.SelectedItem.Value <> "" Then

            'If ddlitem.Items.Count > 0 Then
            '    ddlitem.Items.Clear()
            'End If

            ListToLocation()

        End If

    End Sub


    Protected Sub ddlfromloc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlfromloc.SelectedIndexChanged

        If ddlfromloc.SelectedItem.Value <> "" Then

            GetWhseLocWMS(ddlfromwhse.SelectedItem.Value, LocWMS, LocMoveIn, LocMoveOut)

            If (ddlfromloc.SelectedItem.Value = LocWMS Or ddlfromloc.SelectedItem.Value = LocMoveIn Or ddlfromloc.SelectedItem.Value = LocMoveOut) Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "ShowSweetAlert('PPCC','You selected Location for WMS', 'warning');", True)
            End If

        End If

    End Sub

    Protected Sub ddltoloc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddltoloc.SelectedIndexChanged

        If ddltoloc.SelectedItem.Value <> "" Then

            GetWhseLocWMS(ddltowhse.SelectedItem.Value, LocWMS, LocMoveIn, LocMoveOut)

            If (ddltoloc.SelectedItem.Value = LocWMS Or ddltoloc.SelectedItem.Value = LocMoveIn Or ddltoloc.SelectedItem.Value = LocMoveOut) Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "ShowSweetAlert('PPCC','You selected Location for WMS', 'warning');", True)
            End If

        End If

    End Sub

    'Protected Sub txtjob_TextChanged(sender As Object, e As System.EventArgs) Handles txtjob.TextChanged

    '    Dim ListItem As ListItem = ddljob.Items.FindByValue(txtjob.Text)
    '    If ListItem IsNot Nothing Then
    '        ddljob.SelectedValue = txtjob.Text
    '    End If

    'End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        ListItem()
    End Sub

    Protected Sub ddlitem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlitem.SelectedIndexChanged

        sItem.Value = ddlitem.SelectedItem.Value

        If GridView1.Rows.Count = 0 Then
            ListFromLocation()
            ListToLocation()
        End If

        GetItemDesc(ddlitem.SelectedItem.Value, sItemDesc.Value, sItemLotTracked.Value, sItemUM.Value)

        'If sItemLotTracked.Value = "1" Then
        '    ListLot(ddlitem.SelectedItem.Value)
        'Else
        '    ddllot.Items.Insert(0, New ListItem("", ""))
        'End If

    End Sub

    Protected Sub ddllot_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim ItemModel, ItemDescModel, LotTrakcedModel, UmModel, QtyOnHandModel, QtyModel, VendLotModel As String

        If CommandName.Value = "Add" Then
            GetQtyOnHandItemLoc(sItem.Value, ddlfromwhse.SelectedItem.Value, ddlfromloc.SelectedItem.Value, sItemQtyOnHand.Value)
            ItemModel = sItem.Value
        Else
            GetItemMVTrans(RowPointerSelected.Value, "SelectLot", sItem.Value, sItemDesc.Value, sLot.Value, sQty.Value, sItemUM.Value, sVendLot.Value, sItemQtyOnHand.Value, sItemLotTracked.Value)
            ItemModel = sItem.Value
        End If

        ItemDescModel = sItemDesc.Value
        LotTrakcedModel = sItemLotTracked.Value
        UmModel = sItemUM.Value
        QtyOnHandModel = FormatNumber(sItemQtyOnHand.Value, LenPointQty)

        If sItemLotTracked.Value = "0" Then

            QtyModel = FormatNumber(sItemQtyOnHand.Value, LenPointQty)
            VendLotModel = ""
        Else

            If ddllot.SelectedItem.Value <> "" Then

                GetQtyOnHandLotLoc(sItem.Value, ddlfromwhse.SelectedItem.Value, ddlfromloc.SelectedItem.Value, ddllot.SelectedItem.Value, sQty.Value)
                GetVendLot(sItem.Value, ddllot.SelectedItem.Value, sVendLot.Value)

                QtyModel = FormatNumber(sQty.Value, LenPointQty)
                VendLotModel = sVendLot.Value
            Else

            QtyModel = ""
            VendLotModel = ""

        End If

        End If

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModal('" & ItemModel & "', '" & ItemDescModel & "','" & LotTrakcedModel & "','" & UmModel & "','" & QtyOnHandModel & "','" & QtyModel & "','" & VendLotModel & "','','SelectLot');", True)

    End Sub

    Protected Sub btnConfirmModel_Click(sender As Object, e As EventArgs) Handles btnConfirmModel.Click

        'btnConfirmModel.Attributes.Add("onclick", "javascript:disableclosemodel();")
        'btnCloseModel.Attributes.Add("disabled", "disabled")

        Dim Stat, MsgErr, MsgType As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        Parms = "<Parameters><Parameter>" & txtformsite.Text & "</Parameter>" &
                            "<Parameter>" & txttosite.Text & "</Parameter>" &
                            "<Parameter>" & ddlfromwhse.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddltowhse.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddlfromloc.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddltoloc.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & txttransdate.Text & "</Parameter>" &
                            "<Parameter>" & txtreftransdate.Text & "</Parameter>" &
                            "<Parameter>" & ddlcategorycode.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & ddljob.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & txtremark.Text & "</Parameter>" &
                            "<Parameter>" & sItem.Value & "</Parameter>" &
                            "<Parameter>" & ddllot.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & txtQty.Text & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter>" & Session("SessionID").ToString & "</Parameter>" &
                            "<Parameter>" & CommandName.Value & "</Parameter>" &
                            "<Parameter>" & RowPointerSelected.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "</Parameters>"

        oWS = New SLWebServices.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_EX_MV_Trans", "PPCC_Ex_ConfirmMassQtyMoveSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 19 Then
                Stat = node.InnerText

            ElseIf i = 20 Then
                MsgType = node.InnerText

            ElseIf i = 21 Then
                MsgErr = node.InnerText

            End If

            i += 1

        Next

        If sItemLotTracked.Value = "1" Then
            ddllot.SelectedIndex = ddllot.Items.IndexOf(ddllot.Items.FindByValue(""))
        End If

        If Stat = "FALSE" Then

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            MsgType = "Error [" & MsgType & "]"

            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

        Else

            BindGrid()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Pop", "hideModel();", True)

        End If

        CommandName.Value = ""
        RowPointerSelected.Value = ""

    End Sub

    Protected Sub btnCloseModel_Click(sender As Object, e As EventArgs) Handles btnCloseModel.Click

        CommandName.Value = ""
        RowPointerSelected.Value = ""

        If sItemLotTracked.Value = "1" Then
            ddllot.SelectedIndex = ddllot.Items.IndexOf(ddllot.Items.FindByValue(""))
        End If

    End Sub

    'Protected Sub GridView1_OnSelectedIndexChanged(sender As Object, e As EventArgs)

    '    'For Each row As GridViewRow In GridView1.Rows
    '    '    If row.RowIndex = GridView1.SelectedIndex Then
    '    '        row.BackColor = ColorTranslator.FromHtml("#A1DCF2")
    '    '        Dim lblRowPointer As Label = CType(row.FindControl("lblRowPointer"), Label)
    '    '        RowPointerSelected.Value = lblRowPointer.Text
    '    '    Else
    '    '        row.BackColor = ColorTranslator.FromHtml("#FFFFFF")
    '    '    End If
    '    'Next

    'End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridView1.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            'e.Row.Attributes.Add("class", "clickable")
            'e.Row.Attributes.Add("rowNumber", e.Row.RowIndex.ToString())
            'e.Row.Attributes("onmouseover") = "javascript:setMouseOverColor(this);"
            'e.Row.Attributes("onmouseout") = "javascript:setMouseOutColor(this);"

            'e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackClientHyperlink(Me.GridView1, "javascript:GetSelectedRow(this)"))

            'Page.ClientScript.GetPostBackClientHyperlink(Me.GridView1, "Select$" & e.Row.RowIndex)
        End If

    End Sub

    'Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging

    '    GridView1.PageIndex = e.NewPageIndex
    '    BindGrid()

    'End Sub

    Protected Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click

        Dim Stat, MsgErr, MsgType As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        Parms = "<Parameters><Parameter>" & RowPointerSelected.Value & "</Parameter>" &
                            "<Parameter>" & "DELETE" & "</Parameter>" &
                            "<Parameter>" & Session("SessionID").ToString & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter>" & ddlcategorycode.SelectedItem.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "</Parameters>"

        oWS = New SLWebServices.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_EX_MV_Trans", "PPCC_Ex_MVTranCommandSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 7 Then
                Stat = node.InnerText

            ElseIf i = 8 Then
                MsgType = node.InnerText

            ElseIf i = 9 Then
                MsgErr = node.InnerText

            End If

            i += 1

        Next

        If Stat = "TRUE" Then

            RowPointerSelected.Value = ""
            BindGrid()

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "ShowSweetAlert('Success','" & MsgErr & "', 'success');", True)


        End If

    End Sub


    Protected Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btncopy.Click

        If RowPointerSelected.Value <> "" Or Not String.IsNullOrEmpty(RowPointerSelected.Value) Then

            CommandName.Value = "Copy"

            ddllot.Items.Clear()

            If ddllot.Items.Count = 0 Then

                If sItemLotTracked.Value = "1" Then
                    ListLot(ddlitem.SelectedItem.Value)
                Else
                    ddllot.Items.Insert(0, New ListItem("", ""))
                End If

            End If

            Dim ItemModel, ItemDescModel, LotTrakcedModel, UmModel, QtyOnHandModel, QtyModel, VendLotModel As String

            GetItemMVTrans(RowPointerSelected.Value, CommandName.Value, sItem.Value, sItemDesc.Value, sLot.Value, sQty.Value, sItemUM.Value, sVendLot.Value, sItemQtyOnHand.Value, sItemLotTracked.Value)

            ItemModel = sItem.Value
            ItemDescModel = sItemDesc.Value
            LotTrakcedModel = sItemLotTracked.Value
            UmModel = sItemUM.Value
            QtyOnHandModel = FormatNumber(sItemQtyOnHand.Value, LenPointQty)
            QtyModel = ""
            VendLotModel = ""

            If sItemLotTracked.Value = "1" Then
                ddllot.SelectedIndex = ddllot.Items.IndexOf(ddllot.Items.FindByValue(""))
            End If

            'ClientScript.RegisterStartupScript(Me.[GetType](), "Pop", "openModal('" & ItemModel & "', '" & ItemDescModel & "','" & LotTrakcedModel & "','" & UmModel & "','" & QtyOnHandModel & "','" & QtyModel & "','" & VendLotModel & "','','Click');", True)

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModal('" & ItemModel & "', '" & ItemDescModel & "','" & LotTrakcedModel & "','" & UmModel & "','" & QtyOnHandModel & "','" & QtyModel & "','" & VendLotModel & "','','Click');", True)

        End If


    End Sub

    Protected Sub btnedit_Click(sender As Object, e As EventArgs) Handles btnedit.Click

        If RowPointerSelected.Value <> "" Or Not String.IsNullOrEmpty(RowPointerSelected.Value) Then

            CommandName.Value = "Edit"

            ddllot.Items.Clear()

            If ddllot.Items.Count = 0 Then

                If sItemLotTracked.Value = "1" Then
                    ListLot(ddlitem.SelectedItem.Value)
                Else
                    ddllot.Items.Insert(0, New ListItem("", ""))
                End If

            End If

            Dim ItemModel, ItemDescModel, LotTrakcedModel, UmModel, QtyOnHandModel, QtyModel, VendLotModel, LotModel As String

            GetItemMVTrans(RowPointerSelected.Value, CommandName.Value, sItem.Value, sItemDesc.Value, sLot.Value, sQty.Value, sItemUM.Value, sVendLot.Value, sItemQtyOnHand.Value, sItemLotTracked.Value)

            ItemModel = sItem.Value
            ItemDescModel = sItemDesc.Value
            LotTrakcedModel = sItemLotTracked.Value
            UmModel = sItemUM.Value
            QtyOnHandModel = FormatNumber(sItemQtyOnHand.Value, LenPointQty)
            QtyModel = sQty.Value
            VendLotModel = sVendLot.Value
            LotModel = sLot.Value

            'If sItemLotTracked.Value = "1" Then
            '    ddllot.SelectedIndex = ddllot.Items.IndexOf(ddllot.Items.FindByValue(""))
            'End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModal('" & ItemModel & "', '" & ItemDescModel & "','" & LotTrakcedModel & "','" & UmModel & "','" & QtyOnHandModel & "','" & QtyModel & "','" & VendLotModel & "','" & LotModel & "','Edit');", True)

        End If

    End Sub

    Protected Sub btnprintsticker_Click(sender As Object, e As EventArgs) Handles btnprintsticker.Click
        Dim URL As String = ""

        URL = "?Print=Qty Move&DocNum=" & txtdocumentnum.Text

        Response.Redirect("PrintStickerBarcode.aspx" & URL)

    End Sub

    Protected Sub btnprocess_Click(sender As Object, e As EventArgs) Handles btnprocess.Click

        Dim Stat, MsgErr, MsgType, DocNum As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""
        DocNum = ""

        Parms = "<Parameters><Parameter>" & RowPointerSelected.Value & "</Parameter>" &
                            "<Parameter>" & "MOVE" & "</Parameter>" &
                            "<Parameter>" & Session("SessionID").ToString & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter>" & ddlcategorycode.SelectedItem.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "</Parameters>"

        oWS = New SLWebServices.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_EX_MV_Trans", "PPCC_Ex_MVTranCommandSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 6 Then
                DocNum = node.InnerText

            ElseIf i = 7 Then
                Stat = node.InnerText

            ElseIf i = 8 Then
                MsgType = node.InnerText

            ElseIf i = 9 Then
                MsgErr = node.InnerText

            End If

            i += 1

        Next

        BindGrid()

        If Stat = "TRUE" Then

            txtdocumentnum.Text = DocNum

            If GridView1.Rows.Count = 0 Then
                Clear()
            End If

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "ShowSweetAlert('Success','" & MsgErr & "', 'success');", True)
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Success','" & MsgErr & "', 'success');", True)

        End If


    End Sub

    Sub Clear()
        'SessionID = Guid.NewGuid
        ddlfromwhse.SelectedValue = PWhse.ToString
        ddltowhse.SelectedValue = PWhse.ToString
        ddlitem.Items.Clear()
        ddlfromloc.SelectedIndex = ddlfromloc.Items.IndexOf(ddlfromloc.Items.FindByValue(""))
        ddltoloc.SelectedIndex = ddltoloc.Items.IndexOf(ddltoloc.Items.FindByValue(""))
        ListFromLocation()
        ListToLocation()
        txttransdate.Text = Date.Now.ToString("dd/MM/yyyy")
        txtreftransdate.Text = Date.Now.ToString("dd/MM/yyyy")
        ddlcategorycode.SelectedIndex = ddlcategorycode.Items.IndexOf(ddlcategorycode.Items.FindByValue(""))
        ddljob.SelectedIndex = ddljob.Items.IndexOf(ddljob.Items.FindByValue(""))
        txtremark.Text = String.Empty
        txtsearchitem.Text = String.Empty
        sItemLotTracked.Value = "0"
        sItemDesc.Value = ""
        sItemUM.Value = ""
        RowPointerSelected.Value = ""
        CommandName.Value = ""
        sItem.Value = ""
        sLot.Value = ""
        sQty.Value = ""
        sVendLot.Value = ""
        sItemQtyOnHand.Value = ""
        GridView1.DataSource = Nothing
        GridView1.DataBind()

        'Get New Session
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

    End Sub


    Sub Display(ByVal sender As Object, ByVal e As EventArgs)

        If ddlfromloc.SelectedItem.Value <> "" And ddlfromwhse.SelectedItem.Value <> "" And ddlitem.SelectedItem.Value <> "" _
            And ddltoloc.SelectedItem.Value <> "" And ddltowhse.SelectedItem.Value <> "" And ddlcategorycode.SelectedItem.Value <> "" Then

            CommandName.Value = "Add"

            ddllot.Items.Clear()

            If ddllot.Items.Count = 0 Then

                If sItemLotTracked.Value = "1" Then
                    ListLot(ddlitem.SelectedItem.Value)
                Else
                    ddllot.Items.Insert(0, New ListItem("", ""))
                End If

            End If

            GetQtyOnHandItemLoc(ddlitem.SelectedItem.Value, ddlfromwhse.SelectedItem.Value, ddlfromloc.SelectedItem.Value, sItemQtyOnHand.Value)

            Dim ItemModel, ItemDescModel, LotTrakcedModel, UmModel, QtyOnHandModel, QtyModel, VendLotModel As String

            ItemModel = ddlitem.SelectedItem.Value
            ItemDescModel = sItemDesc.Value
            LotTrakcedModel = sItemLotTracked.Value
            UmModel = sItemUM.Value
            QtyOnHandModel = FormatNumber(sItemQtyOnHand.Value, LenPointQty)

            If sItemLotTracked.Value = "0" Then

                QtyModel = FormatNumber(sItemQtyOnHand.Value, LenPointQty)
                VendLotModel = ""
            Else

                QtyModel = ""
                VendLotModel = ""

            End If

            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "Pop", "openModal('" & ItemModel & "', '" & ItemDescModel & "','" & LotTrakcedModel & "','" & UmModel & "','" & QtyOnHandModel & "','" & QtyModel & "','" & VendLotModel & "','','Click');", True)
            'ClientScript.RegisterStartupScript(Me.[GetType](), "Pop", "openModal('" & ItemModel & "', '" & ItemDescModel & "','" & LotTrakcedModel & "','" & UmModel & "','" & QtyOnHandModel & "','" & QtyModel & "','" & VendLotModel & "','','Click');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModal('" & ItemModel & "', '" & ItemDescModel & "','" & LotTrakcedModel & "','" & UmModel & "','" & QtyOnHandModel & "','" & QtyModel & "','" & VendLotModel & "','','Click');", True)

        Else

            If ddlfromloc.SelectedItem.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "ShowSweetAlert('Error','From Location must not be blank', 'error');", True)
                Exit Sub
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','From Location must not be blank', 'error');", True)
            End If

            If ddlfromwhse.SelectedItem.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "ShowSweetAlert('Error','From Warehouse must not be blank', 'error');", True)
                Exit Sub
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','From Warehouse must not be blank', 'error');", True)
            End If

            If ddlitem.SelectedItem.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "ShowSweetAlert('Error','Item must not be blank', 'error');", True)
                Exit Sub
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','Item must not be blank', 'error');", True)
            End If

            If ddltoloc.SelectedItem.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "ShowSweetAlert('Error','To Location must not be blank', 'error');", True)
                Exit Sub
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','To Location must not be blank', 'error');", True)
            End If

            If ddltowhse.SelectedItem.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "ShowSweetAlert('Error','To Warehouse must not be blank', 'error');", True)
                Exit Sub
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','To Warehouse must not be blank', 'error');", True)
            End If

            If ddlcategorycode.SelectedItem.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "ShowSweetAlert('Error','Category Code must not be blank', 'error');", True)
                Exit Sub
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','To Warehouse must not be blank', 'error');", True)
            End If


        End If


    End Sub



    Sub BindGrid()

        Dim Propertie As String

        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = New DataSet
        Filter = "SessionID = '" & Session("SessionID").ToString & "' and UserCode = '" & Session("UserName").ToString & "' AND Moved = 0"
        Propertie = "Item ,ItemDescription, Lot, Uf_ppccmvtran_Vend_lot, Qty, UM, ErrMsg, RowPointer"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_MV_Trans", Propertie, Filter, "CreateDate DESC", "", 0)

        GridView1.DataSource = ds.Tables(0)
        GridView1.DataBind()

        If GridView1.Rows.Count > 0 Then
            ddlfromwhse.Attributes.Add("disabled", "disabled")
            ddltowhse.Attributes.Add("disabled", "disabled")
            ddlfromloc.Attributes.Add("disabled", "disabled")
            ddltoloc.Attributes.Add("disabled", "disabled")
        Else
            ddlfromwhse.Attributes.Remove("disabled")
            ddltowhse.Attributes.Remove("disabled")
            ddlfromloc.Attributes.Remove("disabled")
            ddltoloc.Attributes.Remove("disabled")
        End If

    End Sub


    Sub ListCategoryCode()

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "TAK_CategoryCodes", "CategoryCode, CategoryDesc", "Series ='MassQuantitiesMove'", "CategoryCode", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlcategorycode.Items.Add(New ListItem(dRow("CategoryCode") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("CategoryDesc"), UCase(dRow("CategoryCode"))))
        Next

        ddlcategorycode.Items.Insert(0, New ListItem("", ""))
    End Sub

    Sub ListWhse()

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLWhseAlls", "Whse, Name", "SiteRef = '" & txtformsite.Text & "' AND VendNum IS NULL AND WhseStat = 'A'", "Whse", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlfromwhse.Items.Add(New ListItem(dRow("Whse") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Name"), UCase(dRow("Whse"))))
            ddltowhse.Items.Add(New ListItem(dRow("Whse") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Name"), UCase(dRow("Whse"))))
        Next

        ddlfromwhse.Items.Insert(0, New ListItem("", ""))
        ddltowhse.Items.Insert(0, New ListItem("", ""))

        If PWhse.ToString <> "" Then
            ddlfromwhse.SelectedValue = PWhse.ToString
            ddltowhse.SelectedValue = PWhse.ToString
        End If

    End Sub

    Sub ListFromLocation()

        Dim OldFromLoc As String = ""

        If ddlfromloc.Items.Count > 0 Then
            OldFromLoc = ddlfromloc.SelectedItem.Value
        End If

        ddlfromloc.Items.Clear()
        'ddlitem.Items.Clear()

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet


        If ddlitem.Items.Count = 0 Then
            ds = oWS.LoadDataSet(Session("Token").ToString,
                                 "SLLocations", "Loc, Description",
                                 "LocType = 'S'",
                                 "Loc", "", 0)

            For Each dRow As DataRow In ds.Tables(0).Rows
                ddlfromloc.Items.Add(New ListItem(dRow("Loc") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Description"), UCase(dRow("Loc"))))
            Next
        Else

            If ddlitem.SelectedItem.Value <> "" Then

                ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemlocs", _
                                "Loc, Whse, LocDescription", _
                                "Whse = '" & ddlfromwhse.SelectedItem.Value & "' And Item = '" & ddlitem.SelectedItem.Value & "' And LocType = 'S' And QtyOnHand > 0", _
                                "Loc", "", 0)

                dt = New DataTable
                dt = ds.Tables(0).DefaultView.ToTable(True, "Loc", "Whse", "LocDescription")

                For Each dRow As DataRow In dt.Rows
                    ddlfromloc.Items.Add(New ListItem(dRow("Loc") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("LocDescription"), UCase(dRow("Loc"))))
                Next

            Else
                ds = oWS.LoadDataSet(Session("Token").ToString, _
                                 "SLLocations", "Loc, Description", _
                                 "LocType = 'S'", _
                                 "Loc", "", 0)

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddlfromloc.Items.Add(New ListItem(dRow("Loc") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Description"), UCase(dRow("Loc"))))
                Next


            End If

        End If

        ddlfromloc.Items.Insert(0, New ListItem("", ""))

        If OldFromLoc <> "" Then
            ddlfromloc.SelectedIndex = ddlfromloc.Items.IndexOf(ddlfromloc.Items.FindByValue(OldFromLoc))
        End If

    End Sub

    Sub ListToLocation()

        Dim OldToLoc As String = ""

        If ddltoloc.Items.Count > 0 Then
            OldToLoc = ddltoloc.SelectedItem.Value
        End If

        ddltoloc.Items.Clear()
        'ddlitem.Items.Clear()

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        If ddlitem.Items.Count = 0 Then
            ds = oWS.LoadDataSet(Session("Token").ToString, _
                                 "SLLocations", "Loc, Description", _
                                 "LocType = 'S'", _
                                 "Loc", "", 0)

            For Each dRow As DataRow In ds.Tables(0).Rows
                ddltoloc.Items.Add(New ListItem(dRow("Loc") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Description"), UCase(dRow("Loc"))))
            Next

        Else

            If ddlitem.SelectedItem.Value <> "" Then

                ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemlocs", _
                                "Loc, Whse, LocDescription", _
                                "Whse = '" & ddltowhse.SelectedItem.Value & "' And Item = '" & ddlitem.SelectedItem.Value & "' And LocType = 'S'", _
                                "Loc", "", 0)

                dt = New DataTable
                dt = ds.Tables(0).DefaultView.ToTable(True, "Loc", "Whse", "LocDescription")

                For Each dRow As DataRow In dt.Rows
                    ddltoloc.Items.Add(New ListItem(dRow("Loc") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("LocDescription"), UCase(dRow("Loc"))))
                Next

            Else

                ds = oWS.LoadDataSet(Session("Token").ToString, _
                                 "SLLocations", "Loc, Description", _
                                 "LocType = 'S'", _
                                 "Loc", "", 0)

                For Each dRow As DataRow In ds.Tables(0).Rows
                    ddltoloc.Items.Add(New ListItem(dRow("Loc") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Description"), UCase(dRow("Loc"))))
                Next


            End If

        End If

        ddltoloc.Items.Insert(0, New ListItem("", ""))

        If OldToLoc <> "" Then
            ddltoloc.SelectedIndex = ddltoloc.Items.IndexOf(ddltoloc.Items.FindByValue(OldToLoc))
        End If

    End Sub

    Sub ListItem()

        ddlitem.Items.Clear()

        Dim strSearch As String = txtsearchitem.Text
        Dim QtyOnHand As Decimal = 0.0
        Dim ddlText As String = ""
        Dim i As Integer
        Dim WhereItem As String = ""

        If strSearch <> String.Empty Then

            'strSearch = strSearch.Replace(",", "")
            'strSearch = strSearch.Replace("*", "%")
            'strSearch = strSearch.Replace("%%", "%")
            
            Dim arrSearch As String()
            arrSearch = strSearch.Split(New Char() {","c})

            For i = 0 To arrSearch.Length - 1

                If arrSearch.Length > 0 Then

                    WhereItem = WhereItem & " And Item Like '" & arrSearch(i).Replace("*", "%") & "'"
                End If

            Next

            WhereItem = Mid(WhereItem, 5)

        End If

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        If ddlfromloc.SelectedItem.Value <> "" Then

            ds = oWS.LoadDataSet(Session("Token").ToString, _
                                 "PPCC_Ex_SLItemLocs", _
                                 "Item, ItmDescription", _
                                 WhereItem & " And Whse = '" & ddlfromwhse.SelectedItem.Value & "' And Loc = '" & ddlfromloc.SelectedItem.Value & "' And ItemStat In ('A', 'S') And QtyOnHand > 0", _
                                 "Item", "", 0)

            For Each dRow As DataRow In ds.Tables(0).Rows
                ddlitem.Items.Add(New ListItem(dRow("Item") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("ItmDescription"), UCase(dRow("Item"))))
            Next

        Else
            ds = oWS.LoadDataSet(Session("Token").ToString, _
                                 "PPCC_Ex_SLItemLocs", _
                                 "Item, QtyOnHand, Loc", _
                                 WhereItem & "' And Whse = '" & ddlfromwhse.SelectedItem.Value & "' And ItemStat In ('A', 'S') And QtyOnHand > 0", _
                                 "Item", "", 0)

            For Each dRow As DataRow In ds.Tables(0).Rows
                ddlitem.Items.Add(New ListItem(dRow("Item") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("Loc") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & FormatNumber(dRow("QtyOnHand"), LenPointQty), UCase(dRow("Item"))))
            Next

        End If

        ddlitem.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub ListJob()

        ddljob.Items.Clear()

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "Job", "Suffix = 0 And Type = 'J'", "Job", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddljob.Items.Add(New ListItem(dRow("Job"), UCase(dRow("Job"))))
        Next

        ddljob.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub ListLot(ByVal Item As String)

        ddllot.Items.Clear()

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        Filter = "Item = '" & Item & "' AND Loc = '" & ddlfromloc.SelectedItem.Value & "' AND Whse = '" & ddlfromwhse.SelectedItem.Value & "' And QtyOnHand > 0"

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLLotLocs", "Lot, QtyOnHand", Filter, "Lot", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddllot.Items.Add(New ListItem(dRow("Lot") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") & dRow("QtyOnHand"), UCase(dRow("Lot"))))
        Next

        ddllot.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetItemDesc(ByVal Item As String, ByRef ItemDesc As String, ByRef LotTracked As String, ByRef UM As String)

        ItemDesc = ""
        LotTracked = ""

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLItems", "LotTracked, Description, UM", "Item = '" & Item & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            ItemDesc = ds.Tables(0).Rows(0)("Description").ToString
            LotTracked = IIf(IsDBNull(ds.Tables(0).Rows(0)("LotTracked")), "0", ds.Tables(0).Rows(0)("LotTracked").ToString)
            UM = ds.Tables(0).Rows(0)("UM").ToString
        End If

    End Sub

    Sub GetQtyOnHandItemLoc(ByVal Item As String, ByVal FromWhse As String, ByVal FromLoc As String, ByRef QtyOnHand As String)

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        Filter = "Item = '" & Item & "' And Whse = '" & FromWhse & "' And Loc = '" & FromLoc & "'"

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemLocs", "QtyOnHand", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            QtyOnHand = ds.Tables(0).Rows(0)("QtyOnHand").ToString
        End If

    End Sub

    Sub GetQtyOnHandLotLoc(ByVal Item As String, ByVal FromWhse As String, ByVal FromLoc As String, ByVal Lot As String, ByRef QtyOnHand As String)

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLLotLocs", "QtyOnHand", "Item = '" & Item & "' And Whse = '" & FromWhse & "' And Loc = '" & FromLoc & "' AND Lot = '" & Lot & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            QtyOnHand = ds.Tables(0).Rows(0)("QtyOnHand").ToString
        End If

    End Sub

    Sub GetItemMVTrans(ByVal RowPointer As String, ByVal Type As String, ByRef Item As String, ByRef ItemDesc As String, _
                           ByRef Lot As String, ByRef Qty As String, ByRef UM As String, ByRef VendLot As String, _
                           ByRef QtyOnHand As String, ByRef LotTracked As String)

        Dim Propertie As String

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        Propertie = "Item, ItemDescription, Lot, Qty, UM, Uf_ppccmvtran_Vend_lot, DerQtyOnHand, LotTracked"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_MV_Trans", Propertie, "RowPointer = '" & RowPointer & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Item = ds.Tables(0).Rows(0)("Item").ToString
            ItemDesc = ds.Tables(0).Rows(0)("ItemDescription").ToString
            Lot = ds.Tables(0).Rows(0)("Lot").ToString
            Qty = ds.Tables(0).Rows(0)("Qty").ToString
            UM = ds.Tables(0).Rows(0)("UM").ToString
            VendLot = ds.Tables(0).Rows(0)("Uf_ppccmvtran_Vend_lot").ToString
            QtyOnHand = ds.Tables(0).Rows(0)("DerQtyOnHand").ToString
            LotTracked = ds.Tables(0).Rows(0)("LotTracked").ToString
        End If

        If LotTracked = "1" And Type <> "SelectLot" Then
            ListLot(Item)
        End If

    End Sub

    Sub GetVendLot(ByVal Item As String, ByVal Lot As String, ByRef VendLot As String)

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLLots", "VendLot", "Item = '" & Item & "' AND Lot = '" & Lot & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            VendLot = ds.Tables(0).Rows(0)("VendLot").ToString
        End If

    End Sub

    Sub GetWhseLocWMS(ByVal Whse As String, ByRef LocWMS As String, ByRef LocMoveIn As String, ByRef LocMoveOut As String)

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLWhses", "whsUf_Whse_LocWMS, whsUf_Whse_LocMoveIn, whsUf_Whse_LocMoveOut", "Whse = '" & Whse & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            LocWMS = ds.Tables(0).Rows(0)("whsUf_Whse_LocWMS").ToString
            LocMoveIn = ds.Tables(0).Rows(0)("whsUf_Whse_LocMoveIn").ToString
            LocMoveOut = ds.Tables(0).Rows(0)("whsUf_Whse_LocMoveOut").ToString
        End If

    End Sub

    Function GetExitsItemToLoc(ByVal Item As String, ByVal LocTo As String) As String

        GetExitsItemToLoc = "0"

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemLocs", "Loc", "Item = '" & Item & "' AND Loc = '" & LocTo & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetExitsItemToLoc = "1"
        End If

    End Function

    Function GetSite() As String

        GetSite = ""

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLParms", "Site", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetSite = ds.Tables(0).Rows(0)("Site").ToString
        End If

        Return GetSite

    End Function

    Function GetDefWhse() As String

        GetDefWhse = ""
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token"), "SLInvparms", "DefWhse", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetDefWhse = ds.Tables(0).Rows(0)("DefWhse").ToString
        End If

        Return GetDefWhse

    End Function

    Public Function UnitQtyFormat() As Integer

        Dim strUnitQtyFormat As String = ""
        Dim PointQty As String = ""

        UnitQtyFormat = 0

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLInvparms", "QtyUnitFormat", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            strUnitQtyFormat = ds.Tables(0).Rows(0)("QtyUnitFormat").ToString

            Dim words As String() = strUnitQtyFormat.Split(New Char() {"."c})

            For Each word As String In words
                PointQty = words(1)
                Exit For
            Next

            UnitQtyFormat = Len(PointQty)

        End If

        Return UnitQtyFormat

    End Function

End Class
