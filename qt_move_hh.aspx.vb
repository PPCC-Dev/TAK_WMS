Imports System.Globalization

Partial Class qt_move_hh
    Inherits System.Web.UI.Page
    Dim oWS As SLWebServices.DOWebServiceSoapClient

    Sub LoadEmployeeUser()
        Try
            Literal1.Text = ""

            UserDropDown.Items.Clear()
            Dim Filter As String = ""

            'Load Employee User
            Dim ds As Data.DataSet
            Filter = "UserName='" & Session("UserName").ToString & "'"
            ds = New Data.DataSet
            oWS = New SLWebServices.DOWebServiceSoapClient

            ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Users", "EmpNum, EmpName", Filter, "EmpNum", "", 0)
            For Each dRow As Data.DataRow In ds.Tables(0).Rows
                UserDropDown.Items.Add(New ListItem(dRow("EmpNum") & " -- " & dRow("EmpName"), dRow("EmpNum")))
            Next
            UserDropDown.Items.Insert(0, New ListItem("", ""))

            If Session("Employee") IsNot Nothing Then
                If Session("Employee").ToString <> "" Then
                    Dim ListItem As ListItem = UserDropDown.Items.FindByValue(Session("Employee").ToString)
                    If ListItem IsNot Nothing Then
                        UserDropDown.SelectedValue = Session("Employee").ToString
                    End If
                End If
            End If
        Catch ex As Exception
            Literal1.Text = "LoadEmployeeUser() <br />" & ex.Message
        End Try
    End Sub

    'Load User Info.
    Sub LoadUserInfo()
        Try
            Literal1.Text = ""
            Dim ds As Data.DataSet
            Dim PropertyList As String = ""
            Dim Filter As String = ""
            Dim UserId As Decimal
            Dim DefaultFromLocation As String = ""
            Dim DefaultToLocation As String = ""

            Filter = "Username='" & Session("UserName").ToString & "'"
            PropertyList = "UserId, UserNamesUf_UserNames_Whse, UserNamesUf_UserNames_FromLoc, UserNamesUf_UserNames_ToLoc"
            ds = New Data.DataSet
            oWS = New SLWebServices.DOWebServiceSoapClient
            ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_UserNames", PropertyList, Filter, "", "", 0)
            If ds.Tables(0).Rows.Count > 0 Then
                UserId = ds.Tables(0).Rows(0)("UserId").ToString
                DefaultFromLocation = ds.Tables(0).Rows(0)("UserNamesUf_UserNames_FromLoc").ToString
                DefaultToLocation = ds.Tables(0).Rows(0)("UserNamesUf_UserNames_ToLoc").ToString
                ToWhseTextBox.Text = ds.Tables(0).Rows(0)("UserNamesUf_UserNames_Whse").ToString
                Call LoadWhseLocation(ds.Tables(0).Rows(0)("UserNamesUf_UserNames_Whse").ToString, DefaultToLocation, False)
            End If

            Filter = "UserId='" & UserId & "'"
            PropertyList = "UserCode, Whse"
            ds = New Data.DataSet
            oWS = New SLWebServices.DOWebServiceSoapClient
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLUserLocals", PropertyList, Filter, "", "", 0)
            If ds.Tables(0).Rows.Count > 0 Then
                FromWhseTextBox.Text = ds.Tables(0).Rows(0)("Whse").ToString
                Call LoadWhseLocation(ds.Tables(0).Rows(0)("Whse").ToString, DefaultFromLocation)
            End If

            FromSiteTextBox.Text = Session("Config")
            ToSiteTextBox.Text = Session("Config")
        Catch ex As Exception
            Literal1.Text = "LoadUserInfo() <br />" & ex.Message
        End Try
    End Sub

    Sub LoadWhseLocation(ByVal Whse As String, ByVal DefaultLocation As String, Optional ByVal FromLocation As Boolean = True)
        'SLItemlocAlls: SiteRef = FP(%3) and Whse = FP(%2) and Item = FP(%1) and LocType = 'S'
        Try
            Literal1.Text = ""
            Dim ds As New Data.DataSet
            Dim PropertyList As String = "Loc"
            Dim Filter As String = "SiteRef= '" & Session("Config") & "' And LocType = 'S'"
            oWS = New SLWebServices.DOWebServiceSoapClient
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLLocationAlls", PropertyList, Filter, "Loc", "", 0)

            If FromLocation Then
                FromLocationDropDown.Items.Clear()
                For Each dRow As Data.DataRow In ds.Tables(0).DefaultView.ToTable(True, "Loc").Rows
                    FromLocationDropDown.Items.Add(New ListItem(dRow("Loc"), dRow("Loc")))
                Next
                Dim ListItem As ListItem = FromLocationDropDown.Items.FindByValue(DefaultLocation)
                If ListItem IsNot Nothing Then
                    FromLocationDropDown.SelectedValue = DefaultLocation
                    FromLocationTextBox.Text = DefaultLocation
                Else
                    FromLocationTextBox.Text = ""
                End If
            Else
                ToLocationDropDown.Items.Clear()
                For Each dRow As Data.DataRow In ds.Tables(0).DefaultView.ToTable(True, "Loc").Rows
                    ToLocationDropDown.Items.Add(New ListItem(dRow("Loc"), dRow("Loc")))
                Next
                Dim ListItem As ListItem = ToLocationDropDown.Items.FindByValue(DefaultLocation)
                If ListItem IsNot Nothing Then
                    ToLocationDropDown.SelectedValue = DefaultLocation
                    ToLocationTextBox.Text = DefaultLocation
                Else
                    ToLocationTextBox.Text = ""
                End If

            End If
        Catch ex As Exception
            Literal1.Text = "LoadWhseLocation() <br />" & ex.Message
        End Try

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            Literal1.Text = ""

            'Verify Token
            If Session("Token") Is Nothing Then
                Response.Redirect("signin_hh.aspx")
            Else
                If Session("Token").ToString = "" Then
                    Response.Redirect("signin_hh.aspx")
                End If
            End If

            'Verify Employee
            If Session("Employee") Is Nothing Then
                Response.Redirect("default_hh.aspx")
            Else
                If Session("Employee").ToString = "" Then
                    Response.Redirect("default_hh.aspx")
                End If
            End If


            If Not Page.IsPostBack Then
                Dim en As New CultureInfo("en-US")
                TransDateTextBox.Text = Now.Date.ToString("dd/MM/yyyy", en)
                UserNameLabel.Text = "ผู้ใช้งาน: " & Session("UserName").ToString

                Call LoadUserInfo()
                Call LoadEmployeeUser()

            End If
            Call RebindGrid()

            If Trim(FromLocationTextBox.Text) <> "" Then
                ToLocationTextBox.Focus()
            Else
                FromLocationTextBox.Focus()
            End If

            'If Trim(ToLocationTextBox.Text) <> "" Then
            '    BarCodeTextBox.Focus()
            'Else
            '    ToLocationTextBox.Focus()
            'End If

        Catch ex As Exception
            Literal1.Text = "Page_Load() <br />" & ex.Message
        End Try

    End Sub

    Protected Sub QtyMoveGridView_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles QtyMoveGridView.RowCommand
        Try
            Literal1.Text = ""

            If e.CommandName = "DeleteMove" Then
                Dim pointer As String = e.CommandArgument
                oWS = New SLWebServices.DOWebServiceSoapClient
                Dim res As Object
                Dim Parms As String = "<Parameters><Parameter>" & pointer & "</Parameter></Parameters>"

                res = oWS.CallMethod(Session("Token").ToString, "PPCC_MoveTrans", "PPCC_WSDeleteMoveTranSp", Parms)

                Call RebindGrid()
            End If
        Catch ex As Exception
            Literal1.Text = "QtyMoveGridView_RowCommand() <br />" & ex.Message
        End Try
    End Sub

    Protected Sub QtyMoveGridView_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles QtyMoveGridView.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.TableSection = TableRowSection.TableHeader
        End If
    End Sub

    Sub RebindGrid()
        Try
            Literal1.Text = ""
            oWS = New SLWebServices.DOWebServiceSoapClient
            Dim Filter As String = "UserCode= '" & Session("UserName").ToString & "' And Moved = 0"
            Dim PropList As String = "Item, Description, Lot, Qty, UM, ErrFlag, ErrMsg, RowPointer, TagID, ModelName"
            Dim ds As New Data.DataSet
            ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_MoveTrans", PropList, Filter, "CreateDate Desc", "", 0)

            QtyMoveGridView.DataSource = ds.Tables(0)
            QtyMoveGridView.DataBind()
        Catch ex As Exception
            Literal1.Text = "RebindGrid() <br />" & ex.Message
        End Try

    End Sub

    Protected Sub BarCodeTextBox_TextChanged(sender As Object, e As System.EventArgs) Handles BarCodeTextBox.TextChanged
        Try
            If Trim(BarCodeTextBox.Text) <> "" Then
                Literal1.Text = ""
                Dim TagID As String = ""
                Dim TagItem As String = ""
                Dim TagWhse As String = ""
                Dim TagLoc As String = ""
                Dim TagQty As String = ""
                Dim TagLot As String = ""

                Dim ds As New Data.DataSet
                Dim Filter As String = "TagID='" & Trim(BarCodeTextBox.Text) & "' And Stat <> 'O' And Stat <> 'C' And TagQty > 0"
                Dim PropertyList As String = "TagID, Stat, Item, Whse, Loc, TagQty, Lot"
                oWS = New SLWebServices.DOWebServiceSoapClient
                ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Tags", PropertyList, Filter, "TagID", "", 0)

                '1. Check Exist Bar Code
                If ds.Tables(0).Rows.Count = 0 Then
                    Literal1.Text = "ไม่พบรหัสบาร์โค้ดนี้"
                    BarCodeTextBox.Text = ""
                    Exit Sub
                Else
                    TagItem = ds.Tables(0).Rows(0)("Item").ToString
                    TagWhse = ds.Tables(0).Rows(0)("Whse").ToString
                    TagLoc = ds.Tables(0).Rows(0)("Loc").ToString
                    TagQty = ds.Tables(0).Rows(0)("TagQty").ToString
                    TagLot = ds.Tables(0).Rows(0)("Lot").ToString
                End If

                '2. Check Match From Warehouse
                If Trim(TagWhse) <> Trim(FromWhseTextBox.Text) Then
                    Literal1.Text = "จากคลัง: คลังที่เลือกไม่ตรงกับคลังของบาร์โค้ด"
                    BarCodeTextBox.Text = ""
                    Exit Sub
                End If

                '3. Check Match From Location
                If Trim(TagLoc) <> Trim(FromLocationTextBox.Text) Then
                    Literal1.Text = "จาก ที่จัดเก็บ: ที่จัดเก็บที่เลือกไม่ตรงกับที่จัดเก็บของบาร์โค้ด"
                    BarCodeTextBox.Text = ""
                    Exit Sub
                End If

                '4. Check From Item/Warehouse
                ds = New Data.DataSet
                Filter = "Item='" & TagItem & "' And Whse='" & FromWhseTextBox.Text & "'"
                oWS = New SLWebServices.DOWebServiceSoapClient
                ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemWhses", "Item", Filter, "Item", "", 0)
                If ds.Tables(0).Rows.Count = 0 Then
                    Literal1.Text = "จากคลัง: ไม่พบข้อมูลที่ Item/Warehouse"
                    BarCodeTextBox.Text = ""
                    Exit Sub
                End If

                '5. Check To Item/Warehouse
                ds = New Data.DataSet
                Filter = "Item='" & TagItem & "' And Whse='" & ToWhseTextBox.Text & "'"
                oWS = New SLWebServices.DOWebServiceSoapClient
                ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemWhses", "Item", Filter, "Item", "", 0)
                If ds.Tables(0).Rows.Count = 0 Then
                    Literal1.Text = "ไปคลัง: ไม่พบข้อมูลที่ Item/Warehouse"
                    BarCodeTextBox.Text = ""
                    Exit Sub
                End If

                '6. Check From Item Stockroom Locations
                ds = New Data.DataSet
                Filter = "Item='" & TagItem & "' And Whse='" & FromWhseTextBox.Text & "' And Loc ='" & FromLocationTextBox.Text & "'"
                oWS = New SLWebServices.DOWebServiceSoapClient
                ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemLocs", "Item", Filter, "Item", "", 0)
                If ds.Tables(0).Rows.Count = 0 Then
                    Literal1.Text = "จาก ที่จัดเก็บ: ไม่พบข้อมูลที่ Item Stockroom Locations"
                    BarCodeTextBox.Text = ""
                    Exit Sub
                End If

                '7. Check To Item Stockroom Locations
                ds = New Data.DataSet
                Filter = "Item='" & TagItem & "' And Whse='" & ToWhseTextBox.Text & "' And Loc ='" & ToLocationTextBox.Text & "'"
                oWS = New SLWebServices.DOWebServiceSoapClient
                ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemLocs", "Item", Filter, "Item", "", 0)
                If ds.Tables(0).Rows.Count = 0 Then
                    Literal1.Text = "ไป ที่จัดเก็บ: ไม่พบข้อมูลที่ Item Stockroom Locations"
                    BarCodeTextBox.Text = ""
                    Exit Sub
                End If

                '8. Check On hand
                Dim OnhandQty As String = ""
                ds = New Data.DataSet
                Filter = "Item='" & TagItem & "'"
                oWS = New SLWebServices.DOWebServiceSoapClient
                ds = oWS.LoadDataSet(Session("Token").ToString, "SLItems", "Item, LotTracked", Filter, "Item", "", 0)
                If ds.Tables(0).Rows.Count > 0 Then
                    If ds.Tables(0).Rows(0)("LotTracked").ToString = "1" Then 'Lot Tracked
                        ds = New Data.DataSet
                        Filter = "Whse='" & TagWhse & "' And Loc='" & TagLoc & "' And Lot='" & TagLot & "' And Item='" & TagItem & "'"
                        oWS = New SLWebServices.DOWebServiceSoapClient
                        ds = oWS.LoadDataSet(Session("Token").ToString, "SLLotLocs", "Item, QtyOnHand", Filter, "Item", "", 0)
                        If ds.Tables(0).Rows.Count > 0 Then
                            OnhandQty = ds.Tables(0).Rows(0)("QtyOnHand").ToString
                        Else
                            OnhandQty = "0"
                        End If

                        If Convert.ToDecimal(TagQty) > Convert.ToDecimal(OnhandQty) Then
                            Literal1.Text = "จำนวนไม่พอ ไม่สามารถทำรายการได้"
                            BarCodeTextBox.Text = ""
                            Exit Sub
                        End If
                    Else
                        ds = New Data.DataSet
                        Filter = "Whse='" & TagWhse & "' And Loc='" & TagLoc & "' And Item='" & TagItem & "'"
                        oWS = New SLWebServices.DOWebServiceSoapClient
                        ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemLocs", "Item, QtyOnHand", Filter, "Item", "", 0)
                        If ds.Tables(0).Rows.Count > 0 Then
                            OnhandQty = ds.Tables(0).Rows(0)("QtyOnHand").ToString
                        Else
                            OnhandQty = "0"
                        End If

                        If Convert.ToDecimal(TagQty) > Convert.ToDecimal(OnhandQty) Then
                            Literal1.Text = "จำนวนไม่พอ ไม่สามารถทำรายการได้"
                            BarCodeTextBox.Text = ""
                            Exit Sub
                        End If
                    End If
                End If


                If DeleteCheckBox.Checked Then
                    For Each row As GridViewRow In QtyMoveGridView.Rows
                        Dim RowPointerHiddenField As HiddenField = DirectCast(row.FindControl("RowPointerHidden"), HiddenField)
                        Dim TagIDLabel As Label = DirectCast(row.FindControl("TagLabel"), Label)

                        If Trim(Trim(BarCodeTextBox.Text)) = TagIDLabel.Text Then
                            oWS = New SLWebServices.DOWebServiceSoapClient
                            Dim res As Object
                            Dim Parms As String = "<Parameters><Parameter>" & RowPointerHiddenField.Value & "</Parameter></Parameters>"

                            res = oWS.CallMethod(Session("Token").ToString, "PPCC_MoveTrans", "PPCC_WSDeleteMoveTranSp", Parms)
                            BarCodeTextBox.Text = ""
                        End If
                    Next


                    DeleteCheckBox.Checked = False
                Else
                    oWS = New SLWebServices.DOWebServiceSoapClient
                    Dim Parms As String = "<Parameters><Parameter>" & BarCodeTextBox.Text & "</Parameter>" & _
                                                "<Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                                                "<Parameter>" & FromSiteTextBox.Text & "</Parameter>" & _
                                                "<Parameter>" & ToSiteTextBox.Text & "</Parameter>" & _
                                                "<Parameter>" & TransDateTextBox.Text & "</Parameter>" & _
                                                "<Parameter>" & FromWhseTextBox.Text & "</Parameter>" & _
                                                "<Parameter>" & ToWhseTextBox.Text & "</Parameter>" & _
                                                "<Parameter>" & FromLocationTextBox.Text & "</Parameter>" & _
                                                "<Parameter>" & ToLocationTextBox.Text & "</Parameter>" & _
                                                "<Parameter>" & UserDropDown.SelectedValue & "</Parameter></Parameters>"
                    Dim res As Object
                    oWS = New SLWebServices.DOWebServiceSoapClient
                    res = oWS.CallMethod(Session("Token").ToString, "PPCC_MoveTrans", "PPCC_WSInsertMoveTranSp", Parms)

                    BarCodeTextBox.Text = ""
                End If

            End If

            Call RebindGrid()
            BarCodeTextBox.Focus()

        Catch ex As Exception
            Literal1.Text = "BarCodeTextBox_TextChanged() <br />" & ex.Message
        End Try

    End Sub

    Protected Sub ProcessButton_Click(sender As Object, e As System.EventArgs) Handles ProcessButton.Click
        Try
            Literal1.Text = ""
            oWS = New SLWebServices.DOWebServiceSoapClient
            Dim Parms As String = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                                "<Parameter>" & TransDateTextBox.Text & "</Parameter>" & _
                                "<Parameter>" & UserDropDown.SelectedValue & "</Parameter></Parameters>"

            Dim res As Object
            oWS = New SLWebServices.DOWebServiceSoapClient
            res = oWS.CallMethod(Session("Token").ToString, "PPCC_MoveTrans", "PPCC_WSDoMoveTransSp", Parms)

            Call RebindGrid()
        Catch ex As Exception
            Literal1.Text = "ProcessButton_Click() <br />" & ex.Message
        End Try

    End Sub

    Protected Sub MainMenuButton_Click(sender As Object, e As System.EventArgs) Handles MainMenuButton.Click
        Response.Redirect("default_hh.aspx")
    End Sub

    Protected Sub SignoutButton_Click(sender As Object, e As System.EventArgs) Handles SignoutButton.Click
        Session.Abandon()
        Response.Redirect("signin_hh.aspx")
    End Sub

    Protected Sub SwitchButton_Click(sender As Object, e As System.EventArgs) Handles SwitchButton.Click
        Try
            Literal1.Text = ""
            Dim FromSite As String = FromSiteTextBox.Text
            Dim ToSite As String = ToSiteTextBox.Text
            Dim FromWhse As String = FromWhseTextBox.Text
            Dim ToWhse As String = ToWhseTextBox.Text
            Dim FromLocation As String = FromLocationTextBox.Text
            Dim ToLocation As String = ToLocationTextBox.Text

            FromSiteTextBox.Text = ToSite
            ToSiteTextBox.Text = FromSite
            FromWhseTextBox.Text = ToWhse
            ToWhseTextBox.Text = FromWhse

            Dim FromLocArr As New ArrayList
            Dim ToLocArr As New ArrayList

            For Each List As ListItem In FromLocationDropDown.Items
                FromLocArr.Add(List.Value)
            Next
            For Each List As ListItem In ToLocationDropDown.Items
                ToLocArr.Add(List.Value)
            Next

            FromLocationDropDown.Items.Clear()
            ToLocationDropDown.Items.Clear()

            For Each Str As String In ToLocArr
                FromLocationDropDown.Items.Add(New ListItem(Str, Str))
            Next
            For Each Str As String In FromLocArr
                ToLocationDropDown.Items.Add(New ListItem(Str, Str))
            Next

            Dim ListItem As ListItem = FromLocationDropDown.Items.FindByValue(ToLocation)
            If ListItem IsNot Nothing Then
                FromLocationTextBox.Text = ToLocation
            End If

            ToLocationTextBox.Text = FromLocation

        Catch ex As Exception
            Literal1.Text = "SwitchButton_Click() <br />" & ex.Message
        End Try
       
    End Sub

    Protected Sub CancelAllButton_Click(sender As Object, e As System.EventArgs) Handles CancelAllButton.Click
        Try
            Literal1.Text = ""
            oWS = New SLWebServices.DOWebServiceSoapClient
            Dim Parms As String = "<Parameters><Parameter>" & DBNull.Value & "</Parameter>" & _
                                "<Parameter>" & UserDropDown.SelectedValue & "</Parameter></Parameters>"

            Dim res As Object
            oWS = New SLWebServices.DOWebServiceSoapClient
            res = oWS.CallMethod(Session("Token").ToString, "PPCC_MoveTrans", "PPCC_WSDeleteMoveTranSp", Parms)

            Call RebindGrid()
        Catch ex As Exception
            Literal1.Text = "CancelAllButton_Click() <br />" & ex.Message
        End Try
        
    End Sub

    Protected Sub PickDateButton_Click(sender As Object, e As System.EventArgs) Handles PickDateButton.Click
        TransactionDateCalendar.Visible = Not TransactionDateCalendar.Visible
    End Sub

    Protected Sub TransactionDateCalendar_SelectionChanged(sender As Object, e As System.EventArgs) Handles TransactionDateCalendar.SelectionChanged
        Dim en As New CultureInfo("en-US")
        TransDateTextBox.Text = TransactionDateCalendar.SelectedDate.ToString("dd/MM/yyyy", en)
        TransactionDateCalendar.Visible = False
    End Sub

    Protected Sub ToLocationTextBox_TextChanged(sender As Object, e As System.EventArgs) Handles ToLocationTextBox.TextChanged
        Dim ListItem As ListItem = ToLocationDropDown.Items.FindByValue(ToLocationTextBox.Text)
        If ListItem IsNot Nothing Then
            BarCodeTextBox.Focus()
        Else
            Literal1.Text = "ไม่พบ Location " & ToLocationTextBox.Text
            ToLocationTextBox.Text = ""
            ToLocationTextBox.Focus()
        End If
    End Sub

    Protected Sub FromLocationTextBox_TextChanged(sender As Object, e As System.EventArgs) Handles FromLocationTextBox.TextChanged
        Dim ListItem As ListItem = FromLocationDropDown.Items.FindByValue(FromLocationTextBox.Text)
        If ListItem IsNot Nothing Then
            ToLocationTextBox.Focus()
        Else
            Literal1.Text = "ไม่พบ Location " & FromLocationTextBox.Text
            FromLocationTextBox.Text = ""
            FromLocationTextBox.Focus()
        End If
    End Sub
End Class
