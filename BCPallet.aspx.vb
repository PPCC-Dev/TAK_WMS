Imports System.Data
Imports System.Drawing
Imports System.Web.Services
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms

Partial Class BCPallet
    Inherits System.Web.UI.Page

    Dim oWS As SLWebServices.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim dt As DataTable
    Dim res As Object
    Dim Rpt As String = "PPCC_BarcodePalletItem"
    Private Shared iSave As Integer = 0
    Private Shared iPrint As Integer = 0
    Private Shared iProcess As Integer = 0
    'Private Shared iPalletNum As Integer = 0
    
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

 
        If Session("CanAccessBCPallet") <> "1" Then Response.Redirect("default.aspx")

        If Not Page.IsPostBack Then
            SessionID = Guid.NewGuid

            txtBarcode.Focus()
            'chkUpdateQty.Attributes.Add("Onclick", "setFocus()")

            oWS = New SLWebServices.DOWebServiceSoapClient
            Dim obj As Object
            Dim Parms As String = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter></Parameters>"
            obj = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSDeleteTempSp", Parms)

        End If

        If IsPostBack Then
            GetData()
        End If

        Dim arr As ArrayList = DirectCast(ViewState("SelectedRecords"), ArrayList)
        If Not IsNothing(arr) Then
            Label1.Text = arr.Count
        Else
            Label1.Text = 0
        End If

        'BindGrid()

    End Sub


    Protected Sub txtBarcode_TextChanged(sender As Object, e As System.EventArgs) Handles txtBarcode.TextChanged

        Try
            NotPassNotifyPanel.Visible = False
            PassNotifyPanel.Visible = False
            txtBarcode.AutoCompleteType = AutoCompleteType.Disabled

            Dim barcodeType As String
            Dim result As Integer = 0
            Dim Barcode As String
            Dim PalletStat As String = ""
            Dim PrintBarcode As Integer = 0
            'Dim BarcodePallet As String = ""
            Dim PalletRevision As Integer = 0
            Dim Revision As Integer = 0
            Dim CheckSave As Integer = 0
            Dim Parms As String = ""
            Dim iCheckPalletScan As Integer = 0

            Barcode = txtBarcode.Text

            barcodeType = type_barcode(txtBarcode.Text)

            If txtPalletID.Text = String.Empty Then

                If barcodeType = "P" And Len(Barcode) = 10 Then

                    ''---------------------Check Submited ---------------------'
                    ds = New DataSet
                    oWS = New SLWebServices.DOWebServiceSoapClient

                    Parms = "<Parameters><Parameter>" & txtBarcode.Text & "</Parameter>" & _
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
                                    txtBarcode.Text = String.Empty
                                    txtBarcode.Focus()
                                    Exit Sub
                                End If

                            End If
                            i += 1
                        Next

                    End If
                    ''---------------------End Check Submited ---------------------'

                    PalletStat = CheckStatusPallet(Left(txtBarcode.Text, 8))

                    If PalletStat = "A" Then

                        PrintBarcode = CheckPrintPallet(txtBarcode.Text)
                        PalletRevision = RevisionPallet(Left(txtBarcode.Text, 8))
                        Revision = RevisionPalletBarcode(txtBarcode.Text)
                        CheckSave = CheckSavePallet(txtBarcode.Text)

                        If PalletRevision <> Revision Then
                            NotPassNotifyPanel.Visible = True
                            NotPassText.Text = "Revision [" & Barcode & "] is not a valid"
                            txtBarcode.Text = String.Empty
                            txtBarcode.Focus()
                            Exit Sub
                        End If

                        ds = New DataSet
                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                                "<Parameter>" & txtBarcode.Text & "</Parameter>" & _
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

                        res = New Object
                        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSLockedByPalletSp", Parms)

                        If res = "0" Then

                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(Parms)
                            Dim i As Integer = 1
                            For Each node As XmlNode In doc.DocumentElement
                                If i = 3 Then

                                    If node.InnerText <> String.Empty Then
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

                        If CheckSave > 0 Then
                            iSave = 1
                        End If

                        If PrintBarcode > 0 Then
                            txtBCPallet.Text = "Print"
                            iPrint = 1
                        End If

                        ds = New DataSet
                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Dim Parameter As String = ""

                        Parameter = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
                                    "<Parameter>" & txtBarcode.Text & "</Parameter>" & _
                                    "<Parameter>" & Session("UserName").ToString & "</Parameter></Parameters>"
                        oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSSetSessionPalletSp", Parameter)

                        txtPalletID.Text = Barcode

                        'txtBarcode.Text = String.Empty
                        'txtBarcode.Focus()

                        'Dim Parms As String = ""

                        ds = New DataSet
                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Parms = "<Parameters><Parameter>" & txtPalletID.Text & "</Parameter>" & _
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & _
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

                        res = New Object
                        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSTotalQtyAndLotByPalletSp", Parms)

                        If res = "0" Then

                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(Parms)
                            Dim i As Integer = 1
                            For Each node As XmlNode In doc.DocumentElement
                                If i = 2 Then
                                    lblTotalLot.Text = node.InnerText
                                ElseIf i = 3 Then
                                    lblTotalQty.Text = node.InnerText

                                End If
                                i += 1
                            Next

                        End If


                    ElseIf PalletStat = "I" Then

                        NotPassNotifyPanel.Visible = True
                        NotPassText.Text = "Pallet [" & Barcode & "] status inactive"
                        txtBarcode.Text = String.Empty
                        txtBarcode.Focus()
                        Exit Sub

                    ElseIf PalletStat = "N" Then

                        NotPassNotifyPanel.Visible = True
                        NotPassText.Text = Barcode & " is not a valid pallet"
                        txtBarcode.Text = String.Empty
                        txtBarcode.Focus()
                        Exit Sub

                    End If

                Else

                    NotPassNotifyPanel.Visible = True
                    NotPassText.Text = "Please, scan barcode pallet"
                    txtBarcode.Text = String.Empty
                    txtBarcode.Focus()
                    Exit Sub

                End If

            Else

                If barcodeType = "P" And Len(Barcode) = 10 Then

                    '---------------------Check Save ---------------------'
                    ds = New DataSet
                    oWS = New SLWebServices.DOWebServiceSoapClient

                    Parms = "<Parameters><Parameter>" & txtPalletID.Text & "</Parameter>" & _
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

                    res = New Object
                    res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSCheckSaveSp", Parms)

                    If res = "0" Then

                        Dim doc As XmlDocument = New XmlDocument()
                        doc.LoadXml(Parms)
                        Dim i As Integer = 1
                        For Each node As XmlNode In doc.DocumentElement
                            If i = 2 Then

                                If node.InnerText <> String.Empty Or node.InnerText <> "" Then
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
                    '---------------------End Check Save ---------------------'

                    '---------------------Check Submited ---------------------'
                    ds = New DataSet
                    oWS = New SLWebServices.DOWebServiceSoapClient

                    Parms = "<Parameters><Parameter>" & txtBarcode.Text & "</Parameter>" & _
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
                                    txtBarcode.Text = String.Empty
                                    txtBarcode.Focus()
                                    Exit Sub
                                End If

                            End If
                            i += 1
                        Next

                    End If
                    '---------------------End Check Submited ---------------------'

                    PalletStat = CheckStatusPallet(Left(txtBarcode.Text, 8))

                    If PalletStat = "A" Then

                        PrintBarcode = CheckPrintPallet(txtBarcode.Text)
                        PalletRevision = RevisionPallet(Left(txtBarcode.Text, 8))
                        Revision = RevisionPalletBarcode(txtBarcode.Text)
                        CheckSave = CheckSavePallet(txtBarcode.Text)

                        If PalletRevision <> Revision Then
                            NotPassNotifyPanel.Visible = True
                            NotPassText.Text = "Revision [" & Barcode & "] is not a valid"
                            txtBarcode.Text = String.Empty
                            txtBarcode.Focus()
                            Exit Sub
                        End If

                        ds = New DataSet
                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                                "<Parameter>" & txtBarcode.Text & "</Parameter>" & _
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

                        res = New Object
                        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSLockedByPalletSp", Parms)

                        If res = "0" Then

                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(Parms)
                            Dim i As Integer = 1
                            For Each node As XmlNode In doc.DocumentElement
                                If i = 3 Then

                                    If node.InnerText <> String.Empty Then
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

                        If iSave <> 1 Or iPrint <> 1 Then
                            iSave = 0
                            iPrint = 0
                        End If
                        If CheckSave > 0 Then
                            iSave = 1
                        End If

                        If PrintBarcode > 0 Then
                            txtBCPallet.Text = "Print"
                            iPrint = 1
                        End If

                        ds = New DataSet
                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Dim Parameter As String = ""

                        Parameter = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
                                     "<Parameter>" & txtBarcode.Text & "</Parameter>" & _
                                     "<Parameter>" & Session("UserName").ToString & "</Parameter></Parameters>"
                        oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSSetSessionPalletSp", Parameter)


                        'If iPrint = 0 Then
                        'Dim Parms As String = ""
                        ds = New DataSet
                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
                                "<Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                                "<Parameter>" & txtBarcode.Text & "</Parameter>" & _
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

                        res = New Object
                        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSCountPalletSp", Parms)

                        If res = "0" Then

                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(Parms)
                            Dim i As Integer = 1
                            For Each node As XmlNode In doc.DocumentElement
                                If i = 4 Then

                                    If node.InnerText <> String.Empty Then
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


                        'End If

                        txtPalletID.Text = Barcode

                        'txtBarcode.Text = String.Empty
                        'txtBarcode.Focus()

                        'Dim Parms As String = ""

                        ds = New DataSet
                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Parms = "<Parameters><Parameter>" & txtPalletID.Text & "</Parameter>" & _
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & _
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

                        res = New Object
                        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSTotalQtyAndLotByPalletSp", Parms)

                        If res = "0" Then

                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(Parms)
                            Dim i As Integer = 1
                            For Each node As XmlNode In doc.DocumentElement
                                If i = 2 Then
                                    lblTotalLot.Text = node.InnerText
                                ElseIf i = 3 Then
                                    lblTotalQty.Text = node.InnerText

                                End If
                                i += 1
                            Next

                        End If

                    ElseIf PalletStat = "I" Then

                        NotPassNotifyPanel.Visible = True
                        NotPassText.Text = "Pallet [" & Barcode & "] status inactive"
                        txtBarcode.Text = String.Empty
                        txtBarcode.Focus()
                        Exit Sub

                    ElseIf PalletStat = "N" Then

                        NotPassNotifyPanel.Visible = True
                        NotPassText.Text = Barcode & " is not a valid pallet"
                        txtBarcode.Text = String.Empty
                        txtBarcode.Focus()
                        Exit Sub

                    End If

                Else

                    Dim CheckGoods As String
                    CheckGoods = CheckBarcodeGoods(Barcode)

                    If CheckGoods = "E" Then

                        'Dim Parms As String = ""

                        ds = New DataSet
                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Parms = "<Parameters><Parameter>" & txtPalletID.Text & "</Parameter>" & _
                                "<Parameter>" & Barcode & "</Parameter>" & _
                                "<Parameter>" & SessionID.ToString & "</Parameter>" & _
                                "<Parameter>" & Session("UserName").ToString & "</Parameter>" & _
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

                        res = New Object
                        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSScanBarcodePalletSp", Parms)

                        If res = "0" Then

                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(Parms)
                            Dim i As Integer = 1
                            For Each node As XmlNode In doc.DocumentElement
                                If i = 5 Then

                                    If node.InnerText <> String.Empty Then
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

                        'Dim Parms As String = ""

                        ds = New DataSet
                        oWS = New SLWebServices.DOWebServiceSoapClient

                        Parms = "<Parameters><Parameter>" & txtPalletID.Text & "</Parameter>" & _
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & _
                                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

                        res = New Object
                        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSTotalQtyAndLotByPalletSp", Parms)

                        If res = "0" Then

                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(Parms)
                            Dim i As Integer = 1
                            For Each node As XmlNode In doc.DocumentElement
                                If i = 2 Then
                                    lblTotalLot.Text = node.InnerText
                                ElseIf i = 3 Then
                                    lblTotalQty.Text = node.InnerText

                                End If
                                i += 1
                            Next

                        End If

                    Else
                        NotPassNotifyPanel.Visible = True
                        NotPassText.Text = Barcode & " is not a valid barcode"
                        txtBarcode.Text = String.Empty
                        txtBarcode.Focus()
                        Exit Sub

                    End If

                End If

            End If

            GridData.AllowPaging = False
            BindGrid()

            GetData()
            SetData()

            GridData.AllowPaging = True
            BindGrid()

            'MsgBox(ViewState("SelectedRecords").ToString)

            txtBarcode.Text = String.Empty
            txtBarcode.Focus()
            'MsgBox(iPrint)
        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            txtBarcode.Text = String.Empty
            txtBarcode.Focus()
            Exit Sub
        End Try
        

    End Sub

    Function type_barcode(type As String) As String
        Dim strtype As String = Left(type, 1)
        Return strtype
    End Function

    Function RevisionPalletBarcode(PalletID As String) As Integer
        Dim strRevision As String = Right(PalletID, 2)
        Dim Revision As Integer = 0

        Select Case strRevision
            Case "01"
                Revision = 1
            Case "02"
                Revision = 2
            Case "03"
                Revision = 3
            Case "04"
                Revision = 4
            Case "05"
                Revision = 5
            Case "06"
                Revision = 6
            Case "07"
                Revision = 7
            Case "08"
                Revision = 8
            Case "09"
                Revision = 9
            Case Else
                Revision = CInt(strRevision)
        End Select

        Return Revision
    End Function

    Function CheckStatusPallet(PalletID As String) As String

        Dim Stat As String = "" 'A = Active, I = InActive, N = Not Found

        Dim ds As DataSet
        Dim Pallet As String

        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSPalletMasters", "PalletID", "PalletID ='" & PalletID & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Pallet = ds.Tables(0).Rows(0)("PalletID").ToString

            ds = New DataSet
            ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSPalletMasters", "PalletStat", "PalletID ='" & PalletID & "'", "", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then
                Stat = ds.Tables(0).Rows(0)("PalletStat").ToString
            End If

        Else
            Stat = "N"

        End If

        Return Stat

    End Function

    Function RevisionPallet(PalletID As String) As Integer
        Dim Revision As Integer = 0
        Dim BarcodeRevision As Integer = 0

        Dim ds As DataSet

        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSPalletMasters", "PalletRev", "PalletID ='" & PalletID & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Revision = ds.Tables(0).Rows(0)("PalletRev").ToString

        End If

        'If Revision <> RevisionPalletBarcode(PalletID) Then

        'End If

        Return Revision

    End Function

    Function CheckPrintPallet(PalletID As String) As Integer

        Dim BarcodePrint As Integer = 0

        Dim ds As DataSet

        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSDetailPallets", "PrintBarcodeConPal", "PalletID ='" & PalletID & "' And Stat = 'I' And PrintBarcodeConPal > 0", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            BarcodePrint = 1
        End If

        Return BarcodePrint

    End Function

    Function CheckSavePallet(PalletID As String) As Integer

        Dim SavePallet As Integer = 0

        Dim ds As DataSet

        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSDetailPallets", "CheckSave", "PalletID ='" & PalletID & "' And Stat = 'I' And CheckSave > 0", "CreateDate DESC", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            SavePallet = CInt(ds.Tables(0).Rows(0)("CheckSave").ToString)
        End If

        Return SavePallet

    End Function

    Function CheckBarcodeGoods(Barcode As String) As String

        Dim Stat As String = "" 'E = Exist, N = Not Found

        Dim ds As DataSet

        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSBarcodes", "Barcode", "Barcode ='" & Barcode & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Stat = "E"

        Else
            Stat = "N"

        End If

        Return Stat

    End Function

    Function CheckPalletLockedBy(PalletID As String) As String

        Dim LockedBy As String = ""
        Dim ds As DataSet

        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_WMSPalletMasters", "LockedBy", "PalletID ='" & PalletID & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            LockedBy = ds.Tables(0).Rows(0)("LockedBy").ToString

            If LockedBy = Session("UserName").ToString Then
                LockedBy = ""
            End If

        End If

        Return LockedBy

    End Function

    Sub CheckSelected()

        If GridData.Rows.Count > 0 Then

            GridData.AllowPaging = False
            BindGrid()

            For i As Integer = 0 To GridData.Rows.Count - 1
                Dim SelectCheckBox As CheckBox = DirectCast(GridData.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)

                If GridData.Rows(i).Cells(15).Text = "1" Then
                    SelectCheckBox.Checked = True

                End If

            Next

            GridData.AllowPaging = True
            BindGrid()

        End If

    End Sub

    Sub BindGrid()
        Dim ds As DataSet
        ds = New DataSet

        Dim Filter As String = ""
        Dim Propertie As String = ""

        'If (iPrint <> 0 Or iSave <> 0) Then
        'Filter = "SessionID = '" & SessionID.ToString & "' And PalletID = '" & txtPalletID.Text & "' And Stat = 'I' AND CheckSave in (0,1)"
        Filter = "SessionID = '" & SessionID.ToString & "' AND CheckSave in (0,1) And UserName = '" & Session("UserName").ToString & "'"
        'Else
        'Filter = "SessionID = '" & SessionID.ToString & "' And BarcodePallet is null"
        'End If

        Propertie = "PalletID, Barcode, Item, Lot, Seq, VendLot, Qty, UM, CreateDate, Username, SessionID, RowPointer, Stat, Description, PalletNo, Selected"

        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token"), "PPCC_WMSDetailPallets", Propertie, Filter, "PalletID, Barcode ASC, CreateDate DESC", "", 0)

        GridData.DataSource = ds.Tables(0)
        GridData.DataBind()

        For i As Integer = 0 To GridData.Rows.Count - 1
            Dim SelectCheckBox As CheckBox = DirectCast(GridData.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)

            If GridData.Rows(i).Cells(15).Text = "1" Then
                SelectCheckBox.Checked = True

            Else
                SelectCheckBox.Checked = False
            End If

        Next

        Dim arr As ArrayList = DirectCast(ViewState("SelectedRecords"), ArrayList)
        If Not IsNothing(arr) Then
            Label1.Text = arr.Count
        Else
            Label1.Text = 0
        End If



        'EnableTextboxQty()

        'End If

        'Dim oTotalQty As Object
        'Dim oTotalLot As Object


        'If ds.Tables(0).Rows.Count = 0 Then
        '    lblTotalQty.Text = "0.00"
        '    lblTotalLot.Text = "0.00"
        'Else


        '    'If Len(txtBarcode.Text) > 10 Then
        '    '    oTotalQty = ds.Tables(0).Compute("SUM(Qty)", "PalletID = '" & txtPalletID.Text & "' And Stat = 'I'")

        '    '    Dim View As New DataView(ds.Tables(0))
        '    '    oTotalLot = View.ToTable(True, "Lot", "PalletID").Rows.Count

        '    '    MsgBox(oTotalQty)
        '    '    MsgBox(oTotalLot)

        '    '    lblTotalQty.Text = Convert.ToDouble(oTotalQty).ToString("##,##0.00")
        '    '    lblTotalLot.Text = Convert.ToDouble(oTotalLot).ToString("##,##0.00")


        '    'Else

        '    '    If (iSave <> 0 Or iPrint <> 0) Then
        '    '        oTotalQty = ds.Tables(0).Compute("SUM(Qty)", "PalletID = '" & txtPalletID.Text & "' And Stat = 'I'")

        '    '        Dim View As New DataView(ds.Tables(0))
        '    '        oTotalLot = View.ToTable(True, "Lot", "PalletID").Rows.Count

        '    '        If ds.Tables(0).Rows.Count > 0 Then
        '    '            lblTotalQty.Text = Convert.ToDouble(oTotalQty).ToString("##,##0.00")
        '    '            lblTotalLot.Text = Convert.ToDouble(oTotalLot).ToString("##,##0.00")
        '    '        Else
        '    '            lblTotalQty.Text = "0.00"
        '    '            lblTotalLot.Text = "0.00"
        '    '        End If              

        '    '    Else
        '    '        lblTotalQty.Text = "0.00"
        '    '        lblTotalLot.Text = "0.00"
        '    '    End If

        '    'End If

        'End If

    End Sub

    Protected Sub btnClear_Click(sender As Object, e As System.EventArgs) Handles btnClear.Click

        Try
            NotPassNotifyPanel.Visible = False
            PassNotifyPanel.Visible = False

            oWS = New SLWebServices.DOWebServiceSoapClient

            Dim Parms As String
            Dim res As Object
            'Dim count As Integer = 0
            'SetData()

            GridData.AllowPaging = False
            BindGrid()

            Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter></Parameters>"

            res = New Object
            res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSClearBCPalletSp", Parms)

            If res = "0" Then
                ClearForm()
            Else
                NotPassNotifyPanel.Visible = True
                NotPassText.Text = "Clear not successful"
                txtBarcode.Text = String.Empty
                txtBarcode.Focus()
                GridData.AllowPaging = True
                BindGrid()
                Exit Sub
            End If

            GridData.AllowPaging = True
            BindGrid()
            GetTotal()
            Label1.Text = 0
            txtBarcode.Focus()


            'If iPrint = 0 Then
            '    Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
            '            "<Parameter>" & "C" & "</Parameter>" & _
            '            "<Parameter>" & txtPalletID.Text & "</Parameter></Parameters>"

            '    res = New Object
            '    res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSProcessBarcodePalletSp", Parms)

            'Else

            '    Parms = "<Parameters><Parameter>" & txtPalletID.Text & "</Parameter>" & _
            '            "<Parameter>" & "C" & "</Parameter>" & _
            '            "<Parameter>" & SessionID.ToString & "</Parameter></Parameters>"


            '    res = New Object
            '    res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSProcessBarcodePalletAfterPtintSp", Parms)
            'End If


            'If res = "0" Then
            '    ClearForm()

            'Else
            '    NotPassNotifyPanel.Visible = True
            '    NotPassText.Text = "Clear not successful"
            '    txtBarcode.Text = String.Empty
            '    txtBarcode.Focus()
            '    Exit Sub
            'End If

        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            txtBarcode.Text = String.Empty
            txtBarcode.Focus()
            Exit Sub
        End Try

        

    End Sub


    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Try
            NotPassNotifyPanel.Visible = False
            PassNotifyPanel.Visible = False

            'ds = New DataSet
            oWS = New SLWebServices.DOWebServiceSoapClient

            Dim Parms As String
            Dim res As Object
            Dim count As Integer = 0
            SetData()

            GridData.AllowPaging = False
            BindGrid()

            Dim arr As ArrayList = DirectCast(ViewState("SelectedRecords"), ArrayList)
            count = arr.Count

            'If count = 0 Then
            '    NotPassNotifyPanel.Visible = True
            '    NotPassText.Text = "Please, Selected Transactions"
            '    GridData.AllowPaging = True
            '    BindGrid()
            '    Exit Sub
            'End If
            If arr.Count > 0 Then
                For i = 0 To arr.Count - 1
                    res = New Object
                    Parms = "<Parameters><Parameter>" & arr(i).ToString() & "</Parameter>" & _
                            "<Parameter>" & SessionID.ToString & "</Parameter>" & _
                            "<Parameter>" & txtPalletID.Text & "</Parameter></Parameters>"
                    res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSSavedBCPalletSp", Parms)

                    arr.Remove(arr(i).ToString())
                Next
            Else
                NotPassNotifyPanel.Visible = True
                NotPassText.Text = "Please, Selected Transactions"
                GridData.AllowPaging = True
                BindGrid()
                Exit Sub

            End If

            Label1.Text = arr.Count

            'For i As Integer = 0 To GridData.Rows.Count - 1

            '    If arr.Contains(GridData.DataKeys(i).Value) Then

            '        res = New Object
            '        Parms = "<Parameters><Parameter>" & GridData.DataKeys(i).Value.ToString() & "</Parameter>" & _
            '                "<Parameter>" & SessionID.ToString & "</Parameter>" & _
            '                "<Parameter>" & txtPalletID.Text & "</Parameter></Parameters>"
            '        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSSavedBCPalletSp", Parms)

            '        arr.Remove(GridData.DataKeys(i).Value)

            '    End If

            'Next

            If res = "0" Then
                PassNotifyPanel.Visible = True
                PassText.Text = "Save successful"
                txtBarcode.Text = String.Empty
                txtBarcode.Focus()
                iSave = 1
            Else
                NotPassNotifyPanel.Visible = True
                NotPassText.Text = "Save not successful"
                txtBarcode.Text = String.Empty
                txtBarcode.Focus()
                GridData.AllowPaging = True
                BindGrid()
                Exit Sub
            End If

            ViewState("SelectedRecords") = arr
            'ViewState.Remove("SelectedRecords")
            hfCount.Value = "0"

            'If GridData.Rows.Count > 0 Then

            '    For Each Row As GridViewRow In GridData.Rows
            '        Dim CheckSelect As CheckBox = DirectCast(Row.FindControl("SelectCheckBox"), CheckBox)
            '        CheckSelect.Checked = True
            '    Next

            'End If

            GridData.AllowPaging = True
            BindGrid()
            txtBarcode.Focus()

            'For Each Row As GridViewRow In GridData.Rows

            '    Dim CheckSelect As CheckBox = DirectCast(Row.FindControl("SelectCheckBox"), CheckBox)
            '    Dim txtqty As TextBox = DirectCast(Row.FindControl("txtQty"), TextBox)

            '    If CheckSelect.Checked = True Then

            '        iSelected += 1

            '        Parms = "<Parameters><Parameter>" & Row.Cells(12).Text & "</Parameter>" & _
            '                "<Parameter>" & txtqty.Text & "</Parameter>" & _
            '                "<Parameter>" & "S" & "</Parameter></Parameters>"

            '        res = New Object
            '        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSSelectedBarcodeSaveSp", Parms)

            '    End If

            'Next

            ''MsgBox(iSelected)

            'If iSelected = 0 Then
            '    NotPassNotifyPanel.Visible = True
            '    NotPassText.Text = "Please, Selected Transactions"
            '    Exit Sub
            'End If


            'If iPrint = 0 Then
            '    Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
            '            "<Parameter>" & "S" & "</Parameter>" & _
            '            "<Parameter>" & txtPalletID.Text & "</Parameter></Parameters>"

            '    res = New Object
            '    res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSProcessBarcodePalletSp", Parms)

            'Else
            '    Parms = "<Parameters><Parameter>" & txtPalletID.Text & "</Parameter>" & _
            '            "<Parameter>" & "S" & "</Parameter>" & _
            '            "<Parameter>" & SessionID.ToString & "</Parameter></Parameters>"

            '    res = New Object
            '    res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSProcessBarcodePalletAfterPtintSp", Parms)

            'End If


            'If res = "0" Then
            '    PassNotifyPanel.Visible = True
            '    PassText.Text = "Save successful"
            '    txtBarcode.Text = String.Empty
            '    txtBarcode.Focus()
            '    iSave = 1
            'Else
            '    NotPassNotifyPanel.Visible = True
            '    NotPassText.Text = "Save not successful"
            '    txtBarcode.Text = String.Empty
            '    txtBarcode.Focus()
            '    Exit Sub
            'End If
        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            txtBarcode.Text = String.Empty
            txtBarcode.Focus()
            Exit Sub
        End Try
        

    End Sub


    Protected Sub btnDelete_Click(sender As Object, e As System.EventArgs) Handles btnDelete.Click
        Try
            
            NotPassNotifyPanel.Visible = False
            PassNotifyPanel.Visible = False

            'ds = New DataSet
            oWS = New SLWebServices.DOWebServiceSoapClient

            Dim Parms As String
            Dim res As Object
            Dim count As Integer = 0

            SetData()

            GridData.AllowPaging = False
            BindGrid()

            Dim arr As ArrayList = DirectCast(ViewState("SelectedRecords"), ArrayList)
            count = arr.Count


            'If count = 0 Then
            '    NotPassNotifyPanel.Visible = True
            '    NotPassText.Text = "Please, Selected Transactions"
            '    GridData.AllowPaging = True
            '    BindGrid()
            '    Exit Sub
            'End If
            If arr.Count > 0 Then

                For i = 0 To arr.Count - 1
                    res = New Object
                    Parms = "<Parameters><Parameter>" & arr(i).ToString() & "</Parameter></Parameters>"
                    res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSDeleteBCPalletSp", Parms)

                    arr.Remove(arr(i).ToString())
                Next
            Else
                NotPassNotifyPanel.Visible = True
                NotPassText.Text = "Please, Selected Transactions"
                GridData.AllowPaging = True
                BindGrid()
                Exit Sub
            End If

            'For i As Integer = 0 To GridData.Rows.Count - 1

            '    If arr.Contains(GridData.DataKeys(i).Value) Then

            '        res = New Object
            '        Parms = "<Parameters><Parameter>" & GridData.DataKeys(i).Value.ToString() & "</Parameter></Parameters>"
            '        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSDeleteBCPalletSp", Parms)

            '        arr.Remove(GridData.DataKeys(i).Value)

            '    End If

            'Next

            If res = "0" Then
                PassNotifyPanel.Visible = True
                PassText.Text = "Delete successful"
                txtBarcode.Text = String.Empty
                txtBarcode.Focus()
            Else
                NotPassNotifyPanel.Visible = True
                NotPassText.Text = "Delete not successful"
                txtBarcode.Text = String.Empty
                txtBarcode.Focus()
                GridData.AllowPaging = True
                BindGrid()
                Exit Sub
            End If


            'ViewState("SelectedRecords") = arr
            ViewState.Remove("SelectedRecords")
            hfCount.Value = "0"

            GridData.AllowPaging = True
            BindGrid()
            GetTotal()
            txtBarcode.Focus()

            Label1.Text = 0

            'GridData.AllowPaging = False
            'BindGrid()

            'For Each Row As GridViewRow In GridData.Rows

            '    Dim CheckSelect As CheckBox = DirectCast(Row.FindControl("SelectCheckBox"), CheckBox)
            '    Dim txtqty As TextBox = DirectCast(Row.FindControl("txtQty"), TextBox)

            '    If CheckSelect.Checked = True Then

            '        iSelectedDel += 1

            '        Parms = "<Parameters><Parameter>" & Row.Cells(12).Text & "</Parameter>" & _
            '                "<Parameter>" & txtqty.Text & "</Parameter>" & _
            '                "<Parameter>" & "D" & "</Parameter></Parameters>"

            '        res = New Object
            '        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSSelectedBarcodeSaveSp", Parms)

            '    End If

            'Next

            'If iSelectedDel = 0 Then
            '    NotPassNotifyPanel.Visible = True
            '    NotPassText.Text = "Please, Selected Transactions"
            '    Exit Sub
            'End If

            'If res = "0" Then

            '    If iPrint = 0 Then
            '        Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
            '                "<Parameter>" & "D" & "</Parameter>" & _
            '                "<Parameter>" & txtPalletID.Text & "</Parameter></Parameters>"

            '        res = New Object
            '        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSProcessBarcodePalletSp", Parms)

            '    Else
            '        Parms = "<Parameters><Parameter>" & txtPalletID.Text & "</Parameter>" & _
            '                "<Parameter>" & "D" & "</Parameter>" & _
            '                "<Parameter>" & SessionID.ToString & "</Parameter></Parameters>"

            '        res = New Object
            '        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSProcessBarcodePalletAfterPtintSp", Parms)

            '    End If

            '    If res = "0" Then
            '        PassNotifyPanel.Visible = True
            '        PassText.Text = "Delete successful"
            '        txtBarcode.Text = String.Empty
            '        txtBarcode.Focus()
            '    Else
            '        NotPassNotifyPanel.Visible = True
            '        NotPassText.Text = "Delete not successful"
            '        txtBarcode.Text = String.Empty
            '        txtBarcode.Focus()
            '        Exit Sub
            '    End If

            'End If

            'GridData.AllowPaging = True
            'BindGrid()
            'txtBarcode.Focus()

        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            txtBarcode.Text = String.Empty
            txtBarcode.Focus()
            Exit Sub
        End Try
        

    End Sub

    Sub ShowPageCommand(sender As Object, e As GridViewPageEventArgs) Handles GridData.PageIndexChanging
        GridData.PageIndex = e.NewPageIndex
        BindGrid()
        SetData()

        'GridData.AllowPaging = False
        'BindGrid()

        'GetData()
        'SetData()

        'GridData.AllowPaging = True
        'BindGrid()

    End Sub

    Protected Sub GridData_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridData.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim PalletNo As Integer = Integer.Parse(IIf(e.Row.Cells(14).Text <> "", IIf(IsDBNull(e.Row.Cells(14).Text), 0, e.Row.Cells(14).Text), 0))

            For Each cell As TableCell In e.Row.Cells

                If PalletNo = 1 Then
                    cell.Attributes.Add("style", "background-color:#FFE4E4")
                    'cell.Attributes.Add("style", "background-color:#D0ECFF")
                End If

                If PalletNo = 2 Then
                    cell.Attributes.Add("style", "background-color:#FFE1A5")
                    'cell.Attributes.Add("style", "background-color:#FFEFAE")
                End If

                If PalletNo = 3 Then
                    cell.Attributes.Add("style", "background-color:#D0ECFF")
                    'cell.Attributes.Add("style", "background-color:#F7E0F9")
                End If
            Next

        End If

    End Sub

    'Protected Sub GridData_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridData.RowEditing
    '    GridData.EditIndex = e.NewEditIndex
    '    BindGrid()
    'End Sub

    'Protected Sub GridData_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridData.RowCancelingEdit
    '    GridData.EditIndex = -1
    '    BindGrid()
    'End Sub

    'Protected Sub GridData_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridData.RowUpdating

    '    Dim RowPointer As String = GridData.DataKeys(e.RowIndex).Values("RowPointer").ToString()
    '    Dim txtqty As TextBox = CType(GridData.Rows(e.RowIndex).FindControl("txtQty"), TextBox)

    '    oWS = New SLWebServices.DOWebServiceSoapClient
    '    Dim Parms As String = ""

    '    Try
    '        Parms = "<Parameters><Parameter>" & RowPointer & "</Parameter>" & _
    '                "<Parameter>" & CDec(txtqty.Text) & "</Parameter></Parameters>"

    '        res = New Object
    '        res = oWS.CallMethod(Session("Token"), "PPCC_WMSDetailPallets", "PPCC_WMSUpdateQtyBCPalletSp", Parms)

    '        GridData.EditIndex = -1
    '        BindGrid()
    '        GetTotal()

    '    Catch ex As Exception
    '        NotPassNotifyPanel.Visible = True
    '        NotPassText.Text = ex.Message
    '        Exit Sub
    '    End Try

    'End Sub

    Protected Sub btnPreview_Click(sender As Object, e As System.EventArgs) Handles btnPreview.Click
        Try
            NotPassNotifyPanel.Visible = False
            PassNotifyPanel.Visible = False

            Dim Parms As String = ""

            '---------------------Check Save ---------------------'
            ds = New DataSet
            oWS = New SLWebServices.DOWebServiceSoapClient

            Parms = "<Parameters><Parameter>" & txtPalletID.Text & "</Parameter>" & _
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

            res = New Object
            res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSCheckSaveSp", Parms)

            If res = "0" Then

                Dim doc As XmlDocument = New XmlDocument()
                doc.LoadXml(Parms)
                Dim i As Integer = 1
                For Each node As XmlNode In doc.DocumentElement
                    If i = 2 Then

                        If node.InnerText <> String.Empty Or node.InnerText <> "" Then
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
            '---------------------End Check Save ---------------------'

            If iSave <> 0 Then

                Dim TaskParms As String = ""

                oWS = New SLWebServices.DOWebServiceSoapClient
                Parms = "<Parameters>"
                Parms &= "<Parameter>" & SessionID.ToString & "</Parameter>"
                Parms &= "</Parameters>"

                oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSUpdateLockedByPreviewSp", Parms)

                'If txtBCPallet.Text = String.Empty Then
                TaskParms = SessionID.ToString & "," & Session("UserName").ToString & "," & "V,"
                'Else
                '    TaskParms = SessionID.ToString & "," & Session("UserName").ToString & "," & "R,"
                'End If

                Dim CallTaskCount As Integer = 0
                Dim CountNoTask As Integer = 0
                Dim TaskName As String = ""
                'Dim Parms As String = ""

                oWS = New SLWebServices.DOWebServiceSoapClient
                ds = New Data.DataSet
                ds = oWS.LoadDataSet(Session("Token"), "BGTaskDefinitions", "TaskName", "TaskName='" & Rpt.ToString & "'", "", "", 0)
                If ds.Tables(0).Rows.Count = 0 Then
                    CountNoTask += 1
                Else
                    TaskName = ds.Tables(0).Rows(0)("TaskName").ToString
                End If


                If CountNoTask = 0 Then
                    oWS = New SLWebServices.DOWebServiceSoapClient
                    Parms = "<Parameters>"
                    Parms &= "<Parameter>" & TaskName.ToString & "</Parameter>"
                    Parms &= "<Parameter>" & TaskParms & "</Parameter>"
                    Parms &= "<Parameter>" & Session("UserName").ToString & "</Parameter>"
                    Parms &= "<Parameter>" & "Preview" & "</Parameter>"
                    Parms &= "<Parameter ByRef='Y'>" & String.Empty & "</Parameter>"
                    Parms &= "</Parameters>"

                    oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSInsertActiveBGTaskForPrintBCSp", Parms)

                    Dim doc As XmlDocument = New XmlDocument()
                    doc.LoadXml(Parms)
                    Dim TaskNumber As String = ""
                    Dim i As Integer = 1

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

                    Dim script As String = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", script, True)

                    'If Not IsNothing(Session("UrlReport")) Then
                    '    PassNotifyPanel.Visible = True
                    '    PassText.Text = "Printed successful"
                    '    txtBarcode.Text = String.Empty
                    '    txtBarcode.Focus()
                    'Else
                    '    NotPassNotifyPanel.Visible = True
                    '    NotPassText.Text = "Printed not successful, Please check background task history"
                    '    txtBarcode.Text = String.Empty
                    '    txtBarcode.Focus()
                    '    Exit Sub
                    'End If

                    'ClearForm()
                    'BindGrid()

                    btnPrint.Enabled = True

                End If

                If CallTaskCount = 0 Or CountNoTask > 0 Then
                    NotPassNotifyPanel.Visible = True
                    NotPassText.Text = "Undefined background task definition"
                    txtBarcode.Text = String.Empty
                    txtBarcode.Focus()
                    Exit Sub
                End If

            Else
                NotPassNotifyPanel.Visible = True
                NotPassText.Text = "Please, save transactions"
                txtBarcode.Text = String.Empty
                txtBarcode.Focus()
                Exit Sub

            End If
        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            txtBarcode.Text = String.Empty
            txtBarcode.Focus()
            Exit Sub
        End Try


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

    Protected Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        Try
            NotPassNotifyPanel.Visible = False
            PassNotifyPanel.Visible = False

            ds = New DataSet
            oWS = New SLWebServices.DOWebServiceSoapClient

            Dim TaskParms As String = ""

            'If txtBCPallet.Text = String.Empty Then
            TaskParms = SessionID.ToString & "," & Session("UserName").ToString & "," & "P,"
            'Else
            'TaskParms = SessionID.ToString & "," & Session("UserName").ToString & "," & "R," & txtPalletID.Text
            'End If

            Dim CallTaskCount As Integer = 0
            Dim CountNoTask As Integer = 0
            Dim TaskName As String = ""
            Dim Parms As String = ""

            oWS = New SLWebServices.DOWebServiceSoapClient
            ds = New Data.DataSet
            ds = oWS.LoadDataSet(Session("Token"), "BGTaskDefinitions", "TaskName", "TaskName='" & Rpt.ToString & "'", "", "", 0)
            If ds.Tables(0).Rows.Count = 0 Then
                CountNoTask += 1
            Else
                TaskName = ds.Tables(0).Rows(0)("TaskName").ToString
            End If


            If CountNoTask = 0 Then
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
                Dim TaskNumber As String = ""
                Dim i As Integer = 1

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

                'Dim script As String = "window.open('" & Session("UrlReport").ToString & "', '_blank')"
                'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "response", script, True)

                If Not IsNothing(Session("UrlReport")) Then
                    PassNotifyPanel.Visible = True
                    PassText.Text = "Printed successful"
                    txtBarcode.Text = String.Empty
                    txtBarcode.Focus()
                Else
                    NotPassNotifyPanel.Visible = True
                    NotPassText.Text = "Printed not successful, Please check background task history"
                    txtBarcode.Text = String.Empty
                    txtBarcode.Focus()
                    Exit Sub
                End If

                SessionID = Guid.NewGuid
                ClearForm()
                BindGrid()

            End If

            'Dim Parms As String

            'Parms = "<Parameters><Parameter>" & SessionID.ToString & "</Parameter>" & _
            '        "<Parameter>" & Session("UserName").ToString & "</Parameter>" & _
            '        "<Parameter>" & IIf(iPrint = 0, "N", "R") & "</Parameter>" & _
            '        "<Parameter>" & txtPalletID.Text & "</Parameter></Parameters>"

            'oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSPrintBarcodePalletSp", Parms)
        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
            txtBarcode.Text = String.Empty
            txtBarcode.Focus()
            Exit Sub
        End Try
        

    End Sub

    'Sub SetPalletNum(ByRef dt As Data.DataTable)

    '    Dim dtGrid As DataTable
    '    dtGrid = New DataTable

    '    dt = GridData.DataSource

    '    dt.Columns.Add("PalletNum", Type.GetType("System.Int32"))

    '    For Each dr As Data.DataRow In dt.Rows

    '        If hfCount.Value = 1 Then
    '            dr("PalletNum") = 1

    '        ElseIf hfCount.Value = 2 Then
    '            dr("PalletNum") = 2

    '        Else
    '            dr("PalletNum") = 3
    '        End If

    '    Next

    'End Sub

    Sub GetData()

        Dim arr As ArrayList
        If ViewState("SelectedRecords") IsNot Nothing Then
            arr = DirectCast(ViewState("SelectedRecords"), ArrayList)
        Else
            arr = New ArrayList()
        End If

        If GridData.Rows.Count > 0 Then

            Dim chkAll As CheckBox = DirectCast(GridData.HeaderRow.Cells(0).FindControl("HdrCheckBox"), CheckBox)
            For i As Integer = 0 To GridData.Rows.Count - 1
                If chkAll.Checked = True Then
                    If Not arr.Contains(GridData.DataKeys(i).Value) Then
                        arr.Add(GridData.DataKeys(i).Value)
                    End If
                Else
                    Dim chk As CheckBox = DirectCast(GridData.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)
                    If chk.Checked = True Then
                        If Not arr.Contains(GridData.DataKeys(i).Value) Then
                            arr.Add(GridData.DataKeys(i).Value)
                        End If
                    Else
                        If arr.Contains(GridData.DataKeys(i).Value) Then
                            arr.Remove(GridData.DataKeys(i).Value)
                        End If
                    End If
                End If
            Next
        End If

        ViewState("SelectedRecords") = arr

    End Sub

    Sub SetData()
        Dim currentCount As Integer = 0
        Dim chkAll As CheckBox = DirectCast(GridData.HeaderRow.Cells(0).FindControl("HdrCheckBox"), CheckBox)
        chkAll.Checked = True
        Dim arr As ArrayList = DirectCast(ViewState("SelectedRecords"), ArrayList)

        For i As Integer = 0 To GridData.Rows.Count - 1
            Dim chk As CheckBox = DirectCast(GridData.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)
            If chk IsNot Nothing Then
                chk.Checked = arr.Contains(GridData.DataKeys(i).Value)
                If Not chk.Checked Then
                    chkAll.Checked = False
                Else
                    currentCount += 1
                End If
            End If
        Next

        hfCount.Value = (arr.Count - currentCount).ToString()
    End Sub

    Sub GetTotal()
        ds = New DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient

        Dim Parms As String = ""

        Parms = "<Parameters><Parameter>" & txtPalletID.Text & "</Parameter>" & _
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & _
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter></Parameters>"

        res = New Object
        res = oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSTotalQtyAndLotByPalletSp", Parms)

        If res = "0" Then

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)
            Dim i As Integer = 1
            For Each node As XmlNode In doc.DocumentElement
                If i = 2 Then
                    lblTotalLot.Text = node.InnerText
                ElseIf i = 3 Then
                    lblTotalQty.Text = node.InnerText

                End If
                i += 1
            Next

        End If

    End Sub

    Sub HdrCheckBox_OnCheckedChanged(sender As Object, e As EventArgs)

        If GridData.Rows.Count > 0 Then

            Dim chkAll As CheckBox = DirectCast(GridData.HeaderRow.Cells(0).FindControl("HdrCheckBox"), CheckBox)
            For i As Integer = 0 To GridData.Rows.Count - 1

                Dim chk As CheckBox = DirectCast(GridData.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)
                oWS = New SLWebServices.DOWebServiceSoapClient

                Dim Parms As String = ""

                Parms = "<Parameters><Parameter>" & GridData.Rows(i).Cells(13).Text & "</Parameter>" & _
                        "<Parameter ByRef='Y'>" & IIf(chk.Checked = True, 1, 0) & "</Parameter></Parameters>"

                oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSUpdateSelectedBCPalletSp", Parms)
            Next

        End If

    End Sub

    Sub SelectCheckBox_OnCheckedChanged(sender As Object, e As EventArgs)

        If GridData.Rows.Count > 0 Then

            For i As Integer = 0 To GridData.Rows.Count - 1
                Dim chk As CheckBox = DirectCast(GridData.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)
                oWS = New SLWebServices.DOWebServiceSoapClient

                Dim Parms As String = ""

                Parms = "<Parameters><Parameter>" & GridData.Rows(i).Cells(13).Text & "</Parameter>" & _
                        "<Parameter ByRef='Y'>" & IIf(chk.Checked = True, 1, 0) & "</Parameter></Parameters>"

                oWS.CallMethod(Session("Token").ToString, "PPCC_WMSDetailPallets", "PPCC_WMSUpdateSelectedBCPalletSp", Parms)
            Next

        End If

    End Sub


    Function CheckSelectedTransaction() As Integer
        Dim iSelected As Integer = 0

        GridData.AllowPaging = False
        BindGrid()

        If GridData.Rows.Count > 0 Then

            For i As Integer = 0 To GridData.Rows.Count
                Dim chk As CheckBox = DirectCast(GridData.Rows(i).Cells(0).FindControl("SelectCheckBox"), CheckBox)

                If chk.Checked = True Then
                    iSelected += 1
                    Exit For
                End If

            Next

        End If

        GridData.AllowPaging = True
        BindGrid()

        Return iSelected

    End Function

    Sub ClearForm()

        txtBarcode.Text = String.Empty
        txtBarcode.Focus()
        txtPalletID.Text = String.Empty
        lblTotalQty.Text = "0.00"
        lblTotalLot.Text = "0.00"
        'chkUpdateQty.Checked = False
        txtBCPallet.Text = String.Empty
        Session.Remove("UrlReport")
        'GridData.DataSource = Nothing
        'GridData.DataBind()
        iSave = 0
        iPrint = 0
        iProcess = 0
        btnPrint.Enabled = False
        Label1.Text = 0

    End Sub

End Class
