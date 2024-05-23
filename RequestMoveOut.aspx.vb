Imports System.Data
Imports System.Web.Services
Imports System.Drawing
Imports System.Xml

Partial Class RequestMoveOut
    Inherits System.Web.UI.Page

    Dim oWS As SLWebServices.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim res As Object
    Dim distinctDT As DataTable
    Dim Rpt As String = "PPCC_WMSGenerateFileMoveOut"

    Private Shared _SessionID As Guid
    Private Shared _JobTable As DataSet
    Private Shared _CoNumTable As DataSet
    Private Shared _ItemTable As DataSet
    Private Shared _LotTable As DataSet
    Private Shared _SuffixTable As DataSet

    Private Shared Property SessionID() As Guid
        Get
            Return _SessionID
        End Get
        Set(value As Guid)
            _SessionID = value
        End Set
    End Property

    Private Property JobTable As DataSet
        Get
            Return _JobTable
        End Get
        Set(value As DataSet)
            _JobTable = value
        End Set
    End Property

    Private Property CoNumTable As DataSet
        Get
            Return _CoNumTable
        End Get
        Set(value As DataSet)
            _CoNumTable = value
        End Set
    End Property

    Private Property LotTable As DataSet
        Get
            Return _LotTable
        End Get
        Set(value As DataSet)
            _LotTable = value
        End Set
    End Property

    Private Property ItemTable As DataSet
        Get
            Return _ItemTable
        End Get
        Set(value As DataSet)
            _ItemTable = value
        End Set
    End Property

    Private Property SuffixTable As DataSet
        Get
            Return _SuffixTable
        End Get
        Set(value As DataSet)
            _SuffixTable = value
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

        If Session("CanAccessReqMoveOut") <> "1" Then Response.Redirect("default.aspx")

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
            LoadCriteria()

            oWS = New SLWebServices.DOWebServiceSoapClient
            Dim obj As Object
            Dim Parms As String = ""
            Parms = "<Parameters>"
            Parms &= "<Parameter>" & Session("UserName").ToString & "</Parameter>"
            Parms &= "<Parameter>" & SessionID.ToString & "</Parameter>"
            Parms &= "</Parameters>"
            obj = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSDeleteHistoryMoveSp", Parms)

        End If

        If IsPostBack Then
            GetData()
        End If

    End Sub

    Sub ShowPageCommand(sender As Object, e As GridViewPageEventArgs) Handles GridReqMove.PageIndexChanging
        GridReqMove.PageIndex = e.NewPageIndex
        BindGrid()
        SetData()

        For i As Integer = 0 To GridReqMove.Rows.Count - 1
            Dim SelectCheckBox As CheckBox = DirectCast(GridReqMove.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)

            If GridReqMove.Rows(i).Cells(9).Text = "1" Then
                SelectCheckBox.Checked = True

            End If

        Next

    End Sub

    Sub BindGrid()

        Dim ds As DataSet
        ds = New DataSet

        Dim Filter As String = ""

        If rdoSelectdJob.Checked = True Or rdoSelectdCo.Checked = True Then
            Filter = "SessionID ='" & SessionID.ToString & "' And Username = '" & Session("UserName").ToString & "' " & _
                 "And RefType = '" & IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")) & "' " & _
                 "And RefNum = '" & IIf(rdoSelectdJob.Checked = True, txtJobSearch.Text, IIf(rdoSelectdCo.Checked = True, txtCoSearch.Text, "")) & "'"
        Else
            Filter = "SessionID ='" & SessionID.ToString & "' And Username = '" & Session("UserName").ToString & "' " & _
                 "And RefType = '" & IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")) & "'"
        End If

        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "Item, Lot, QtyReq, UM, RowPointer, Msg, Selected, RefLine, QtyIssued", Filter, "", "", 0)

        GridReqMove.DataSource = ds.Tables(0)
        GridReqMove.DataBind()


        For i As Integer = 0 To GridReqMove.Rows.Count - 1
            Dim SelectCheckBox As CheckBox = DirectCast(GridReqMove.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)

            If GridReqMove.Rows(i).Cells(9).Text = "1" Then
                SelectCheckBox.Checked = True

            End If

        Next


    End Sub

    Sub LoadCriteria()
        BindCriteriaJob()
        BindCriteriaCO()
    End Sub

    Sub BindCriteriaJob()

        oWS = New SLWebServices.DOWebServiceSoapClient
        JobTable = New DataSet

        JobTable = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "Job", "(Stat = 'R' Or Stat = 'S') And Type = 'J'", "Job", "", 0)

        Dim distinctDT As DataTable = JobTable.Tables(0).DefaultView.ToTable(True, "Job")

        For Each dRow As DataRow In distinctDT.Rows
            ddlJob.Items.Add(New ListItem(dRow("Job"), dRow("Job")))
        Next

        ddlJob.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub BindCriteriaCO()

        CoNumTable = New DataSet

        CoNumTable = oWS.LoadDataSet(Session("Token").ToString, "SLCos", "CoNum", "(Type = 'R' Or Type = 'B') And Stat = 'O'", "CoNum", "", 0)
        For Each dRow As DataRow In CoNumTable.Tables(0).Rows
            ddlCo.Items.Add(New ListItem(dRow("CoNum"), dRow("CoNum")))
        Next

        ddlCo.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub BindCriteriaSuffixJob()

        SuffixTable = New DataSet

        SuffixTable = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "Suffix", "Job = '" & txtJobSearch.Text & "' AND Stat IN ('R','S')", "Suffix", "", 0)

        For Each dRow As DataRow In SuffixTable.Tables(0).Rows
            ddlsuffix.Items.Add(New ListItem(dRow("Suffix"), dRow("Suffix")))
        Next

        ddlsuffix.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub BindCriteriaAdd(ByVal RefType As String, ByVal RefNum As String, suffix As String)

        If RefType = "J" Then

            ItemTable = New DataSet

            ItemTable = oWS.LoadDataSet(Session("Token").ToString, "SLJobmatls", "Item", "Job = '" & RefNum & "' And Suffix ='" & suffix & "'", "Item", "", -1)

            distinctDT = New DataTable
            distinctDT = ItemTable.Tables(0).DefaultView.ToTable(True, "Item")

            For Each dRow As DataRow In distinctDT.Rows
                ddlItem.Items.Add(New ListItem(dRow("Item"), dRow("Item")))
            Next

            ddlItem.Items.Insert(0, New ListItem("", ""))

        ElseIf RefType = "C" Then

            ItemTable = New DataSet

            ItemTable = oWS.LoadDataSet(Session("Token").ToString, "SLCoitems", "Item", "CoNum = '" & RefNum & "'", "Item", "", -1)

            distinctDT = New DataTable
            distinctDT = ItemTable.Tables(0).DefaultView.ToTable(True, "Item")

            For Each dRow As DataRow In distinctDT.Rows
                ddlItem.Items.Add(New ListItem(dRow("Item"), dRow("Item")))
            Next

            ddlItem.Items.Insert(0, New ListItem("", ""))

        Else

            ItemTable = New DataSet

            ItemTable = oWS.LoadDataSet(Session("Token").ToString, "SLItems", "Item", "Stat <> 'O'", "Item", "", -1)

            distinctDT = New DataTable
            distinctDT = ItemTable.Tables(0).DefaultView.ToTable(True, "Item")

            For Each dRow As DataRow In distinctDT.Rows
                ddlItem.Items.Add(New ListItem(dRow("Item"), dRow("Item")))
            Next

            ddlItem.Items.Insert(0, New ListItem("", ""))


        End If

    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click

        Try
            NotPassNotifyPanel.Visible = False
            PassNotifyPanel.Visible = False

            Dim iCount As Object = 0
            Dim Parms As String = ""
            Dim TaskNumber As String = ""
            Dim infobar As String = ""

            If txtOrder.Text <> String.Empty Then

                GridReqMove.AllowPaging = False
                BindGrid()

                'Generate File & Move
                If GridReqMove.Rows.Count > 0 Then

                    Dim count As Integer = 0
                    Dim arr As ArrayList = DirectCast(ViewState("SelectedRecordsMoveOut"), ArrayList)
                    count = arr.Count


                    For i As Integer = 0 To GridReqMove.Rows.Count - 1

                        If GridReqMove.Rows(i).Cells(9).Text = "1" Then
                            If Replace(GridReqMove.Rows(i).Cells(7).Text, "&nbsp;", "") <> "" Then
                                iCount += 1
                                NotPassNotifyPanel.Visible = True
                                NotPassText.Text = "Please, Check Error Message"
                                GridReqMove.AllowPaging = True
                                BindGrid()
                                Exit Sub
                            End If

                        End If

                    Next

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
                        TaskParms = SessionID.ToString & "," & Session("UserName").ToString & "," & "O," & _
                                    txtOrder.Text & "," & IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")) & "," & _
                                    IIf(rdoSelectdJob.Checked = True, txtJobSearch.Text, IIf(rdoSelectdCo.Checked = True, txtCoSearch.Text, ""))


                        If CountNoTask = 0 Then
                            'Qty Move
                            'oWS = New SLWebServices.DOWebServiceSoapClient
                            'Parms = "<Parameters>"
                            'Parms &= "<Parameter>" & SessionID.ToString & "</Parameter>"
                            'Parms &= "<Parameter>" & Session("UserName").ToString & "</Parameter>"
                            'Parms &= "<Parameter>" & "O" & "</Parameter>"
                            'Parms &= "<Parameter ByRef='Y'>" & String.Empty & "</Parameter>"
                            'Parms &= "</Parameters>"

                            'oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSAutoMoveTmpSp", Parms)
                            ''CallTaskCount = 1
                            'BindGrid()

                            ''Check Error
                            'For Each Row As GridViewRow In GridReqMove.Rows
                            '    If Replace(Row.Cells(5).Text, "&nbsp;", "") <> "" Then
                            '        NotPassNotifyPanel.Visible = True
                            '        NotPassText.Text = "Please, Check Error Message"
                            '        Exit Sub
                            '    End If
                            'Next

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

                            'End If

                            If CallTaskCount > 0 Then

                                'Update Req Stat
                                oWS = New SLWebServices.DOWebServiceSoapClient
                                Parms = "<Parameters>"
                                Parms &= "<Parameter>" & SessionID.ToString & "</Parameter>"
                                Parms &= "<Parameter>" & TaskNumber & "</Parameter>"
                                Parms &= "<Parameter>" & txtOrder.Text & "</Parameter>"
                                Parms &= "<Parameter>" & Session("UserName").ToString & "</Parameter>"
                                Parms &= "<Parameter>" & "O" & "</Parameter>"
                                Parms &= "<Parameter ByRef='Y'>" & String.Empty & "</Parameter>"
                                Parms &= "</Parameters>"

                                oWS.CallMethod(Session("Token").ToString, "PPCC_WMSHistoryReqMoves", "PPCC_WMSUpdateReqMoveStatSp", Parms)

                                Dim _doc As XmlDocument = New XmlDocument()
                                _doc.LoadXml(Parms)
                                Dim j As Integer = 1

                                For Each node As XmlNode In _doc.DocumentElement
                                    If j = 6 Then
                                        If Trim(node.InnerText) <> String.Empty Then
                                            infobar = node.InnerText
                                        End If
                                    End If
                                    j += 1
                                Next

                                If infobar <> "" Then
                                    NotPassNotifyPanel.Visible = True
                                    NotPassText.Text = infobar
                                    GridReqMove.AllowPaging = True
                                    BindGrid()
                                    Exit Sub
                                Else
                                    PassNotifyPanel.Visible = True
                                    PassText.Text = "Submit successful"
                                End If

                            End If

                            If CallTaskCount = 0 Or CountNoTask > 0 Then
                                NotPassNotifyPanel.Visible = True
                                NotPassText.Text = "Undefined background task definition"
                                GridReqMove.AllowPaging = True
                                BindGrid()
                                Exit Sub
                            End If

                        End If

                    End If


                    GridReqMove.AllowPaging = True
                    BindGrid()
                    ClearAfterSubmit()
                    SessionID = Guid.NewGuid

                End If

            Else

                NotPassNotifyPanel.Visible = True
                NotPassText.Text = "You must be save before submit process"
                GridReqMove.AllowPaging = True
                BindGrid()
                Exit Sub
            End If
        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            GridReqMove.AllowPaging = True
            BindGrid()
            Exit Sub
        End Try

    End Sub

    Protected Sub rdoSelectdJob_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoSelectdJob.CheckedChanged

        If rdoSelectdJob.Checked = True Then

            txtJobSearch.Enabled = True
            ddlsuffix.Enabled = True
            txtCoSearch.Enabled = False
            txtCoSearch.Text = String.Empty
            txtAddItem.Text = String.Empty
            txtAddQtyReq.Text = String.Empty
            ddlAddLot.Items.Clear()
            ddlsuffix.Items.Clear()
            lblum.Text = String.Empty

            GridReqMove.DataSource = Nothing
            GridReqMove.DataBind()

        End If

    End Sub

    Protected Sub rdoSelectdCo_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoSelectdCo.CheckedChanged

        If rdoSelectdCo.Checked = True Then

            txtJobSearch.Enabled = False
            ddlsuffix.Enabled = False
            txtCoSearch.Enabled = True
            txtJobSearch.Text = String.Empty
            txtAddItem.Text = String.Empty
            txtAddQtyReq.Text = String.Empty
            ddlAddLot.Items.Clear()
            ddlsuffix.Items.Clear()
            lblum.Text = String.Empty

            GridReqMove.DataSource = Nothing
            GridReqMove.DataBind()

        End If

    End Sub

    Protected Sub rdoSelectdOther_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoSelectdOther.CheckedChanged

        If rdoSelectdOther.Checked = True Then

            txtJobSearch.Enabled = False
            ddlsuffix.Enabled = False
            txtCoSearch.Enabled = False
            txtJobSearch.Text = String.Empty
            txtCoSearch.Text = String.Empty
            txtAddItem.Text = String.Empty
            txtAddQtyReq.Text = String.Empty
            ddlAddLot.Items.Clear()
            ddlsuffix.Items.Clear()
            lblum.Text = String.Empty

            BindCriteriaAdd(IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")),
                                IIf(rdoSelectdJob.Checked = True, txtJobSearch.Text, IIf(rdoSelectdCo.Checked = True, txtCoSearch.Text, "")), "")

            GridReqMove.DataSource = Nothing
            GridReqMove.DataBind()

        End If

    End Sub

    Protected Sub txtJobSearch_TextChanged(sender As Object, e As System.EventArgs) Handles txtJobSearch.TextChanged

        ddlsuffix.Items.Clear()
        ddlItem.Items.Clear()
        ddlAddLot.Items.Clear()

        ddlsuffix.SelectedIndex = ddlsuffix.Items.IndexOf(ddlsuffix.Items.FindByValue("0"))


        Dim CheckRefNum As Integer = 0

        CheckRefNum = CheckOrder("J", txtJobSearch.Text)

        If CheckRefNum <> 1 Then

            NotPassNotifyPanel.Visible = True
            NotPassText.Text = txtJobSearch.Text & " is not a valid Job"
            Exit Sub

        Else
            ddlsuffix.Enabled = True
            BindCriteriaSuffixJob()
        End If

    End Sub

    Function LenSuffix(suffix As String) As String

        Dim strSuffix As String = ""
        Dim iLen As Integer = 0

        iLen = Len(suffix)

        Select Case iLen
            Case 1
                strSuffix = "000" & suffix
            Case 2
                strSuffix = "00" & suffix
            Case 3
                strSuffix = "0" & suffix
            Case 4
                strSuffix = suffix
        End Select

        Return strSuffix

    End Function

    Protected Sub ddlsuffix_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlsuffix.SelectedIndexChanged

        GridReqMove.DataSource = Nothing
        GridReqMove.DataBind()

        Dim CheckPrint As Integer = 0

        CheckPrint = CheckPrintPickList("J", txtJobSearch.Text, ddlsuffix.SelectedItem.Value)

        If CheckPrint <> 1 Then
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = "Job [" & txtJobSearch.Text & "] and Suffix [" & LenSuffix(ddlsuffix.SelectedItem.Value) & "] haven't printed pick list"
            Exit Sub
        End If


        Try
            If Not IsNothing(Session("UserWhse")) Then
                oWS = New SLWebServices.DOWebServiceSoapClient

                Dim Parms As String = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
                                      "<Parameter>" & IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")) & "</Parameter>" & _
                                      "<Parameter>" & IIf(rdoSelectdJob.Checked = True, txtJobSearch.Text, IIf(rdoSelectdCo.Checked = True, txtCoSearch.Text, DBNull.Value)) & "</Parameter>" & _
                                      "<Parameter>" & ddlsuffix.SelectedItem.Value & "</Parameter>" & _
                                      "<Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                                      "<Parameter>" & Session("UserWhse").ToString & "</Parameter></Parameters>"

                Dim res As Object
                oWS = New SLWebServices.DOWebServiceSoapClient
                res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSInsertTmpReqMoveOutSp", Parms)

                BindGrid()

                BindCriteriaAdd(IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")),
                                IIf(rdoSelectdJob.Checked = True, txtJobSearch.Text, IIf(rdoSelectdCo.Checked = True, txtCoSearch.Text, "")), ddlsuffix.SelectedItem.Value)

            Else
                NotPassNotifyPanel.Visible = True
                NotPassText.Text = "Warehouse is not valid"
                Exit Sub
            End If

        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            Exit Sub
        End Try

    End Sub

    Protected Sub txtCoSearch_TextChanged(sender As Object, e As System.EventArgs) Handles txtCoSearch.TextChanged

        ddlItem.Items.Clear()
        ddlAddLot.Items.Clear()

        Dim CheckRefNum As Integer = 0
        Dim CheckPrint As Integer = 0

        CheckRefNum = CheckOrder("C", txtCoSearch.Text)
        CheckPrint = CheckPrintPickList("C", txtCoSearch.Text, "")

        If CheckRefNum <> 1 Then
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = txtCoSearch.Text & " is not a valid Customer Order"
            Exit Sub
        End If

        If CheckPrint <> 1 Then
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = "Customer Order [" & txtCoSearch.Text & "] haven't printed pick list"
            Exit Sub
        End If


        Try
            If Not IsNothing(Session("UserWhse")) Then

                Dim Parms As String = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
                                       "<Parameter>" & IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")) & "</Parameter>" & _
                                       "<Parameter>" & IIf(rdoSelectdJob.Checked = True, txtJobSearch.Text, IIf(rdoSelectdCo.Checked = True, txtCoSearch.Text, DBNull.Value)) & "</Parameter>" & _
                                       "<Parameter>" & DBNull.Value & "</Parameter>" & _
                                       "<Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                                       "<Parameter>" & Session("UserWhse").ToString & "</Parameter></Parameters>"

                Dim res As Object
                oWS = New SLWebServices.DOWebServiceSoapClient
                res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSInsertTmpReqMoveOutSp", Parms)

                BindGrid()

                BindCriteriaAdd(IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")),
                                IIf(rdoSelectdJob.Checked = True, txtJobSearch.Text, IIf(rdoSelectdCo.Checked = True, txtCoSearch.Text, "")), "")

            Else
                NotPassNotifyPanel.Visible = True
                NotPassText.Text = "Warehouse is not valid"
                Exit Sub
            End If

        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            Exit Sub
        End Try



    End Sub

    Function CheckOrder(ByVal RefType As String, ByVal RefNum As String) As Integer
        Dim Result As Integer = 0
        Dim ds As DataSet
        Dim Filter As String = ""

        oWS = New SLWebServices.DOWebServiceSoapClient

        If RefType = "J" Then

            ds = New DataSet
            Filter = "Job = '" & RefNum & "' And" & " (Stat = 'R' Or Stat = 'S') And Type = 'J'"
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "Job", Filter, "Job", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then
                Result = 1
            Else
                Result = 0
            End If


        ElseIf RefType = "C" Then

            ds = New DataSet
            Filter = "CoNum = '" & RefNum & "' And CoStat = 'O' And (CoType = 'R' Or CoType = 'B') And Stat = 'O'"
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLCoitems", "CoNum", Filter, "CoNum", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then
                Result = 1
            Else
                Result = 0
            End If

        End If

        Return Result

    End Function


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

    Sub SelectCheckBox_OnCheckedChanged(sender As Object, e As System.EventArgs)

        For i As Integer = 0 To GridReqMove.Rows.Count - 1
            Dim SelectCheckBox As CheckBox = DirectCast(GridReqMove.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)

            If SelectCheckBox.Checked = True Then
                If Replace(GridReqMove.Rows(i).Cells(7).Text, "&nbsp;", "") <> "" Then
                    Dim message As String = "alert('Please, Check Error Message')"
                    ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), [GetType](), "alert", message, True)

                    Dim arr As ArrayList = DirectCast(ViewState("SelectedRecordsMoveOut"), ArrayList)
                    If arr.Contains(GridReqMove.DataKeys(i).Value) Then
                        arr.Remove(GridReqMove.DataKeys(i).Value)
                    End If

                    SelectCheckBox.Checked = False

                End If
            End If

        Next

    End Sub

    Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        NotPassNotifyPanel.Visible = False
        PassNotifyPanel.Visible = False

        Dim ds As DataSet
        Dim res As Object
        Dim Parms As String = ""
        Dim PropertyList As String = ""
        Dim Filter As String = ""
        Dim icount As Integer = 0

        Dim count As Integer = 0
        SetData()

        GridReqMove.AllowPaging = False
        BindGrid()

        Dim arr As ArrayList = DirectCast(ViewState("SelectedRecordsMoveOut"), ArrayList)
        count = arr.Count

        If arr.Count = 0 Then
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = "Please, Selected Transactions"
            GridReqMove.AllowPaging = True
            BindGrid()
            Exit Sub
        End If

        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient

        If (rdoSelectdJob.Checked = True And txtJobSearch.Text <> String.Empty) Or (rdoSelectdCo.Checked = True And txtCoSearch.Text <> String.Empty) Or (rdoSelectdOther.Checked = True) Then

            'For Each Row As GridViewRow In GridReqMove.Rows

            '    Dim SelectCheckBox As CheckBox = DirectCast(Row.FindControl("SelectCheckBox"), CheckBox)

            '    If SelectCheckBox.Checked = True Then

            '        If Replace(Row.Cells(7).Text, "&nbsp;", "") <> "" Then
            '            icount += 1
            '            NotPassNotifyPanel.Visible = True
            '            NotPassText.Text = "Please, Check Error Message"
            '            GridReqMove.AllowPaging = True
            '            BindGrid()
            '            Exit Sub
            '        End If

            '    End If

            'Next

            'For i As Integer = 0 To GridReqMove.Rows.Count - 1
            '    Dim SelectCheckBox As CheckBox = DirectCast(GridReqMove.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)

            '    If SelectCheckBox.Checked = True Then

            '        If Replace(GridReqMove.Rows(i).Cells(7).Text, "&nbsp;", "") <> "" Then
            '            icount += 1
            '            NotPassNotifyPanel.Visible = True
            '            NotPassText.Text = "Please, Check Error Message"
            '            GridReqMove.AllowPaging = True
            '            BindGrid()
            '            Exit Sub

            '        End If

            '    End If

            'Next

            If icount = 0 Then

                Try

                    If txtOrder.Text = String.Empty Then

                        Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
                        "<Parameter>" & IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")) & "</Parameter>" & _
                        "<Parameter>" & IIf(rdoSelectdJob.Checked = True, txtJobSearch.Text, IIf(rdoSelectdCo.Checked = True, txtCoSearch.Text, DBNull.Value)) & "</Parameter>" & _
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & _
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

                        oWS = New SLWebServices.DOWebServiceSoapClient
                        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSGenerateOrderNoSp", Parms)

                        If res = "0" Then
                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(Parms)
                            Dim i As Integer = 1
                            For Each node As XmlNode In doc.DocumentElement
                                If i = 5 Then

                                    If node.InnerText <> String.Empty Then
                                        txtOrder.Text = node.InnerText
                                    End If

                                ElseIf i = 6 Then

                                    If node.InnerText <> String.Empty Then
                                        NotPassNotifyPanel.Visible = True
                                        NotPassText.Text = node.InnerText
                                        GridReqMove.AllowPaging = True
                                        BindGrid()
                                        Exit Sub
                                    End If

                                End If
                                i += 1
                            Next
                        End If

                    End If

                Catch ex As Exception
                    NotPassNotifyPanel.Visible = True
                    NotPassText.Text = ex.Message
                    GridReqMove.AllowPaging = True
                    BindGrid()
                    Exit Sub
                End Try

                Try
                    If txtOrder.Text <> String.Empty Then

                        Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
                                "<Parameter>" & Session("UserName").ToString & "</Parameter></Parameters>"

                        oWS = New SLWebServices.DOWebServiceSoapClient
                        oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSDeleteHistoryReqMoveBeforeInsertSp", Parms)

                        For i As Integer = 0 To GridReqMove.Rows.Count - 1

                            If arr.Contains(GridReqMove.DataKeys(i).Value) Then

                                res = New Object
                                Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
                                        "<Parameter>" & txtOrder.Text & "</Parameter>" & _
                                        "<Parameter>" & IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")) & "</Parameter>" & _
                                        "<Parameter>" & IIf(rdoSelectdJob.Checked = True, txtJobSearch.Text, IIf(rdoSelectdCo.Checked = True, txtCoSearch.Text, DBNull.Value)) & "</Parameter>" & _
                                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                                        "<Parameter>" & GridReqMove.DataKeys(i).Value.ToString() & "</Parameter></Parameters>"

                                oWS = New SLWebServices.DOWebServiceSoapClient
                                res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSInsertHistoryReqMoveSp", Parms)

                                arr.Remove(GridReqMove.DataKeys(i).Value)

                            End If

                        Next

                        If res = "0" Then
                            PassNotifyPanel.Visible = True
                            PassText.Text = "Save successful"
                        Else
                            NotPassNotifyPanel.Visible = True
                            NotPassText.Text = "Save not successful"
                            GridReqMove.AllowPaging = True
                            BindGrid()
                            Exit Sub
                        End If

                        ViewState("SelectedRecordsMoveOut") = arr
                        hfCount.Value = "0"

                        GridReqMove.AllowPaging = True
                        BindGrid()
                        btnSubmit.Enabled = True


                        'For Each Row As GridViewRow In GridReqMove.Rows
                        '    Dim CheckSelect As CheckBox = DirectCast(Row.FindControl("SelectCheckBox"), CheckBox)

                        '    If CheckSelect.Checked = True Then
                        '        ds = New DataSet
                        '        oWS = New SLWebServices.DOWebServiceSoapClient

                        '        Parms = "<Parameters><Parameter>" & Row.Cells(8).Text & "</Parameter>" & _
                        '                "<Parameter>" & "S" & "</Parameter></Parameters>"

                        '        oWS = New SLWebServices.DOWebServiceSoapClient
                        '        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSProcessSaveOrDelSp", Parms)
                        '    End If

                        'Next

                        'Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
                        '                       "<Parameter>" & txtOrder.Text & "</Parameter>" & _
                        '                       "<Parameter>" & IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")) & "</Parameter>" & _
                        '                       "<Parameter>" & IIf(rdoSelectdJob.Checked = True, txtJobSearch.Text, IIf(rdoSelectdCo.Checked = True, txtCoSearch.Text, DBNull.Value)) & "</Parameter>" & _
                        '                       "<Parameter>" & Session("UserName").ToString & "</Parameter></Parameters>"

                        'oWS = New SLWebServices.DOWebServiceSoapClient
                        'oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSSaveToHistoryReqMoveSp", Parms)

                    End If

                Catch ex As Exception
                    NotPassNotifyPanel.Visible = True
                    NotPassText.Text = ex.Message
                    GridReqMove.AllowPaging = True
                    BindGrid()
                    Exit Sub
                End Try

            End If

        Else
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = "You can't leave empty"
            GridReqMove.AllowPaging = True
            BindGrid()
            Exit Sub
        End If

    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As System.EventArgs) Handles btnDelete.Click

        NotPassNotifyPanel.Visible = False
        PassNotifyPanel.Visible = False

        Dim ds As DataSet
        Dim Parms As String

        Dim count As Integer = 0
        SetData()

        GridReqMove.AllowPaging = False
        BindGrid()

        Dim arr As ArrayList = DirectCast(ViewState("SelectedRecordsMoveOut"), ArrayList)
        count = arr.Count

        If arr.Count = 0 Then
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = "Please, Selected Transactions"
            GridReqMove.AllowPaging = True
            BindGrid()
            Exit Sub
        End If

        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient

        Try
            For i As Integer = 0 To GridReqMove.Rows.Count - 1

                If arr.Contains(GridReqMove.DataKeys(i).Value) Then

                    res = New Object
                    Parms = "<Parameters><Parameter>" & GridReqMove.DataKeys(i).Value.ToString() & "</Parameter>" & _
                            "<Parameter>" & "D" & "</Parameter></Parameters>"

                    oWS = New SLWebServices.DOWebServiceSoapClient
                    res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSProcessSaveOrDelSp", Parms)

                    arr.Remove(GridReqMove.DataKeys(i).Value)

                End If

            Next

            If res = "0" Then
                PassNotifyPanel.Visible = True
                PassText.Text = "Delete successful"
            Else
                NotPassNotifyPanel.Visible = True
                NotPassText.Text = "Delete not successful"
                GridReqMove.AllowPaging = True
                BindGrid()
                Exit Sub
            End If

            ViewState("SelectedRecords") = arr
            hfCount.Value = "0"

            GridReqMove.AllowPaging = True
            BindGrid()

            txtAddItem.Text = String.Empty
            txtAddQtyReq.Text = String.Empty
            lblum.Text = String.Empty
            ddlAddLot.Items.Clear()

            'Try
            '    For Each Row As GridViewRow In GridReqMove.Rows
            '        Dim CheckSelect As CheckBox = DirectCast(Row.FindControl("SelectCheckBox"), CheckBox)

            '        If CheckSelect.Checked Then
            '            ds = New DataSet
            '            oWS = New SLWebServices.DOWebServiceSoapClient

            '            Parms = "<Parameters><Parameter>" & Row.Cells(8).Text & "</Parameter>" & _
            '                    "<Parameter>" & "D" & "</Parameter></Parameters>"

            '            oWS = New SLWebServices.DOWebServiceSoapClient
            '            res = New Object
            '            res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSProcessSaveOrDelSp", Parms)
            '        End If

            '    Next


        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            Exit Sub
        End Try

    End Sub

    Protected Sub GridReqMove_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridReqMove.RowEditing
        GridReqMove.EditIndex = e.NewEditIndex
        BindGrid()
    End Sub

    Protected Sub GridReqMove_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridReqMove.RowCancelingEdit
        GridReqMove.EditIndex = -1
        BindGrid()
    End Sub

    Protected Sub GridReqMove_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridReqMove.RowUpdating

        Dim RowPointer As String = GridReqMove.DataKeys(e.RowIndex).Values("RowPointer").ToString()
        Dim QtyReq As TextBox = CType(GridReqMove.Rows(e.RowIndex).FindControl("txtQtyReq"), TextBox)

        oWS = New SLWebServices.DOWebServiceSoapClient
        Dim Parms As String = ""

        Try
            Parms = "<Parameters><Parameter>" & RowPointer & "</Parameter>" & _
                    "<Parameter>" & CDec(QtyReq.Text) & "</Parameter></Parameters>"

            res = New Object
            res = oWS.CallMethod(Session("Token"), "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSUpdateQtyReqSp", Parms)

            GridReqMove.EditIndex = -1
            BindGrid()
        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            Exit Sub
        End Try

    End Sub

    Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click

            NotPassNotifyPanel.Visible = False
            PassNotifyPanel.Visible = False

            If ddlAddLot.Items.Count > 0 Then

                If txtAddItem.Text = String.Empty Or ddlAddLot.SelectedItem.Value = "" Or txtAddQtyReq.Text = String.Empty Then
                    NotPassNotifyPanel.Visible = True
                    NotPassText.Text = "You can't leave empty"
                    Exit Sub
                Else

                    If (rdoSelectdJob.Checked = True And txtJobSearch.Text <> String.Empty) Or
                        (rdoSelectdJob.Checked = True And txtCoSearch.Text <> String.Empty) Or
                        (rdoSelectdOther.Checked = True) Then

                        Try
                            oWS = New SLWebServices.DOWebServiceSoapClient
                            Dim Parms As String = ""

                            Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
                                    "<Parameter>" & txtAddItem.Text & "</Parameter>" & _
                                    "<Parameter>" & ddlAddLot.SelectedItem.Value & "</Parameter>" & _
                                    "<Parameter>" & CDec(txtAddQtyReq.Text) & "</Parameter>" & _
                                    "<Parameter>" & IIf(rdoSelectdJob.Checked = True, txtJobSearch.Text, IIf(rdoSelectdCo.Checked = True, txtCoSearch.Text, DBNull.Value)) & "</Parameter>" & _
                                    "<Parameter>" & IIf(rdoSelectdJob.Checked = True, "J", IIf(rdoSelectdCo.Checked = True, "C", "O")) & "</Parameter>" & _
                                    "<Parameter>" & Session("UserWhse").ToString & "</Parameter>" & _
                                    "<Parameter>" & Session("UserName").ToString & "</Parameter></Parameters>"

                            res = New Object
                            res = oWS.CallMethod(Session("Token"), "PPCC_WMSTmpReqMoveOuts", "PPCC_WMSAddItemManualSp", Parms)

                            If res = "0" Then
                                PassNotifyPanel.Visible = True
                                PassText.Text = "Add item successful"
                            End If

                            BindGrid()

                            txtAddItem.Text = String.Empty
                            txtAddQtyReq.Text = String.Empty
                            lblum.Text = String.Empty
                            ddlAddLot.Items.Clear()

                        Catch ex As Exception
                            NotPassNotifyPanel.Visible = True
                            NotPassText.Text = ex.Message
                            Exit Sub
                        End Try

                    Else
                        NotPassNotifyPanel.Visible = True
                        NotPassText.Text = "You can't leave empty Job/Customer Order"
                        Exit Sub

                    End If

                End If

            Else

                If txtAddItem.Text = String.Empty Or ddlAddLot.Items.Count = 0 Or txtAddQtyReq.Text = String.Empty Then
                    NotPassNotifyPanel.Visible = True
                    NotPassText.Text = "You can't leave empty"
                    Exit Sub
                End If

            End If
        

    End Sub


    Protected Sub txtAddItem_TextChanged(sender As Object, e As System.EventArgs) Handles txtAddItem.TextChanged

        NotPassNotifyPanel.Visible = False
        PassNotifyPanel.Visible = False

        If txtAddItem.Text <> String.Empty Then

            Dim iCheckItem As Integer
            iCheckItem = CheckItemExist(txtAddItem.Text)

            If iCheckItem <> 1 Then
                NotPassNotifyPanel.Visible = True
                NotPassText.Text = txtAddItem.Text & " is not a valid Item"
                Exit Sub
            End If

            Dim Filter As String
            Dim ds As DataSet
            Dim strLoc As String
            strLoc = ""

            ds = New DataSet

            Filter = "Whse='" & Session("UserWhse").ToString & "'"
            ds = New DataSet
            oWS = New SLWebServices.DOWebServiceSoapClient
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLWhses", "whsUf_Whse_LocWMS", Filter, "", "", 0)
            If ds.Tables(0).Rows.Count > 0 Then
                strLoc = ds.Tables(0).Rows(0)("whsUf_Whse_LocWMS").ToString
            End If

            '-----------------------------------------------------------------'

            Dim iCheckIssueBy As Integer
            iCheckIssueBy = CheckItemIssuedBy(txtAddItem.Text)

            If iCheckIssueBy = 1 Then

                ddlAddLot.Items.Clear()

                LotTable = New DataSet

                Filter = "Whse = '" & Session("UserWhse").ToString & "' And Loc = '" & strLoc & "' And Item = '" & txtAddItem.Text & "'"

                LotTable = oWS.LoadDataSet(Session("Token").ToString, "SLLotLocs", "Lot, QtyOnHand", Filter, "Lot", "", -1)
                For Each dRow As DataRow In LotTable.Tables(0).Rows
                    ddlAddLot.Items.Add(New ListItem(dRow("Lot") & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") & FormatNumber(dRow("QtyOnHand"), 2), dRow("Lot")))
                Next

                ddlAddLot.Items.Insert(0, New ListItem("", ""))

            Else
                ddlAddLot.Items.Clear()

                LotTable = New DataSet

                Filter = "Item = '" & txtAddItem.Text & "' And Whse = '" & Session("UserWhse").ToString & "' And Loc = '" & strLoc & "'"

                LotTable = oWS.LoadDataSet(Session("Token").ToString, "SLItemLocs", "QtyOnHand", Filter, "", "", -1)
                For Each dRow As DataRow In LotTable.Tables(0).Rows
                    ddlAddLot.Items.Add(New ListItem("991299999999999" & HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") & FormatNumber(dRow("QtyOnHand"), 2), "991299999999999"))
                Next

                ddlAddLot.Items.Insert(0, New ListItem("", ""))

            End If

            '-----------------------------------------------------------------'

            ds = New DataSet

            oWS = New SLWebServices.DOWebServiceSoapClient
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLItems", "UM", "Item = '" & txtAddItem.Text & "'", "", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then
                lblum.Text = ds.Tables(0).Rows(0)("UM").ToString
            Else
                lblum.Text = ""
            End If

        End If

    End Sub

    Protected Sub ddlAddLot_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlAddLot.SelectedIndexChanged

        Dim StrLot As String
        StrLot = ddlAddLot.SelectedItem.Value

        Dim Filter As String
        Dim ds As DataSet

        Dim strLoc As String
        strLoc = ""

        ds = New DataSet

        Filter = "Whse='" & Session("UserWhse").ToString & "'"
        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLWhses", "whsUf_Whse_LocWMS", Filter, "", "", 0)
        If ds.Tables(0).Rows.Count > 0 Then
            strLoc = ds.Tables(0).Rows(0)("whsUf_Whse_LocWMS").ToString
        End If


        ds = New DataSet

        Dim iCheckIssueBy As Integer
        iCheckIssueBy = CheckItemIssuedBy(txtAddItem.Text)

        If iCheckIssueBy = 1 Then

            ds = New DataSet

            Filter = "Whse = '" & Session("UserWhse").ToString & "' And Loc = '" & strLoc & "' And Item = '" & txtAddItem.Text & "' And Lot = '" & StrLot & "'"

            oWS = New SLWebServices.DOWebServiceSoapClient
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLLotLocs", "QtyOnHand", Filter, "", "", -1)
            If ds.Tables(0).Rows.Count > 0 Then
                txtAddQtyReq.Text = FormatNumber(ds.Tables(0).Rows(0)("QtyOnHand").ToString, 2)
            Else
                txtAddQtyReq.Text = "0.00"
            End If

        Else

            ds = New DataSet

            Filter = "Item = '" & txtAddItem.Text & "' And Whse = '" & Session("UserWhse").ToString & "' And Loc = '" & strLoc & "'"
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemLocs", "QtyOnHand", Filter, "", "", -1)

            If ds.Tables(0).Rows.Count > 0 Then
                txtAddQtyReq.Text = FormatNumber(ds.Tables(0).Rows(0)("QtyOnHand").ToString, 2)
            Else
                txtAddQtyReq.Text = "0.00"
            End If

        End If


    End Sub

    Sub GetData()

        Dim arr As ArrayList
        If ViewState("SelectedRecordsMoveOut") IsNot Nothing Then
            arr = DirectCast(ViewState("SelectedRecordsMoveOut"), ArrayList)
        Else
            arr = New ArrayList()
        End If

        If GridReqMove.Rows.Count > 0 Then

            Dim chkAll As CheckBox = DirectCast(GridReqMove.HeaderRow.Cells(0).FindControl("HdrCheckBox"), CheckBox)
            For i As Integer = 0 To GridReqMove.Rows.Count - 1
                If chkAll.Checked = True Then
                    If Not arr.Contains(GridReqMove.DataKeys(i).Value) Then
                        arr.Add(GridReqMove.DataKeys(i).Value)
                    End If
                Else
                    Dim chk As CheckBox = DirectCast(GridReqMove.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)
                    If chk.Checked = True Then
                        If Not arr.Contains(GridReqMove.DataKeys(i).Value) Then
                            arr.Add(GridReqMove.DataKeys(i).Value)
                        End If
                    Else
                        If arr.Contains(GridReqMove.DataKeys(i).Value) Then
                            arr.Remove(GridReqMove.DataKeys(i).Value)
                        End If
                    End If
                End If
            Next
        End If

        ViewState("SelectedRecordsMoveOut") = arr

    End Sub

    Sub SetData()
        Dim currentCount As Integer = 0
        Dim chkAll As CheckBox = DirectCast(GridReqMove.HeaderRow.Cells(0).FindControl("HdrCheckBox"), CheckBox)
        chkAll.Checked = True
        Dim arr As ArrayList = DirectCast(ViewState("SelectedRecordsMoveOut"), ArrayList)

        For i As Integer = 0 To GridReqMove.Rows.Count - 1
            Dim chk As CheckBox = DirectCast(GridReqMove.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)
            If chk IsNot Nothing Then
                chk.Checked = arr.Contains(GridReqMove.DataKeys(i).Value)
                If Not chk.Checked Then
                    chkAll.Checked = False
                Else
                    currentCount += 1
                End If
            End If
        Next

        hfCount.Value = (arr.Count - currentCount).ToString()
    End Sub

    Function CheckItemExist(Item As String) As Integer

        Dim Result As Integer = 0
        Dim ds As DataSet

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLItems", "Item", "Item = '" & Item & "'", "Item", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Result = 1
        Else
            Result = 0
        End If

        Return Result

    End Function

    Function CheckItemIssuedBy(Item As String) As Integer

        Dim Result As Integer = 0
        Dim ds As DataSet

        oWS = New SLWebServices.DOWebServiceSoapClient

        ds = New DataSet
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLItems", "LotTracked", "Item = '" & Item & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Result = ds.Tables(0).Rows(0)("LotTracked").ToString
        Else
            Result = 0
        End If

        Return Result

    End Function

    Function CheckPrintPickList(RefType As String, RefNum As String, Suffix As String) As Integer
        Dim iCheckPrint As Integer = 0
        Dim PickDate As String = ""
        Dim ds As DataSet

        If RefType = "J" Then

            oWS = New SLWebServices.DOWebServiceSoapClient

            ds = New DataSet
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobmatls", "PickDate", "Job = '" & RefNum & "' and Suffix = '" & Suffix & "' AND JobStat IN ('R','S')", "", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then
                PickDate = ds.Tables(0).Rows(0)("PickDate").ToString

                If PickDate = "" Or IsDBNull(PickDate) Then
                    iCheckPrint = 0
                Else
                    iCheckPrint = 1
                End If

            End If


        ElseIf RefType = "C" Then

            oWS = New SLWebServices.DOWebServiceSoapClient

            ds = New DataSet
            ds = oWS.LoadDataSet(Session("Token").ToString, "SLCoitems", "PickDate", "CoNum = '" & RefNum & "' AND Stat = 'O' AND CoStat = 'O'", "", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then
                PickDate = ds.Tables(0).Rows(0)("PickDate").ToString

                If PickDate = "" Or IsDBNull(PickDate) Then
                    iCheckPrint = 0
                Else
                    iCheckPrint = 1
                End If

            End If

        End If

        Return iCheckPrint

    End Function

    Sub ClearAfterSubmit()

        If GridReqMove.Rows.Count = 0 Then
            txtOrder.Text = String.Empty
            txtJobSearch.Text = String.Empty
            txtCoSearch.Text = String.Empty
            txtAddItem.Text = String.Empty
            txtAddQtyReq.Text = String.Empty
            lblum.Text = String.Empty
            ddlAddLot.Items.Clear()
            ddlCo.Items.Clear()
            ddlItem.Items.Clear()
            ddlJob.Items.Clear()
            ddlsuffix.Items.Clear()
            rdoSelectdJob.Checked = True
            rdoSelectdCo.Checked = False
            rdoSelectdOther.Checked = False
            btnSubmit.Enabled = True
            GridReqMove.DataSource = Nothing
            GridReqMove.DataBind()
        Else
            ddlAddLot.Items.Clear()
            ddlCo.Items.Clear()
            ddlItem.Items.Clear()
            ddlJob.Items.Clear()
            ddlsuffix.Items.Clear()
            lblum.Text = String.Empty
            txtAddItem.Text = String.Empty
            txtAddQtyReq.Text = String.Empty
            txtOrder.Text = String.Empty
        End If
        
    End Sub

End Class
