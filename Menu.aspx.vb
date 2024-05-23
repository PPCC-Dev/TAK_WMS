Imports System.Data
Imports System.Web.Services

Partial Class Menu
    Inherits System.Web.UI.Page

    Dim oWS As SLWebServices.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim Parms As String

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Session("Token") Is Nothing Then
            Response.Redirect("signin.aspx")
        Else
            If Session("Token").ToString = "" Then
                Response.Redirect("signin.aspx")
            End If

        End If

        If Not Page.IsPostBack Then

            Session("CanAccessBCPallet") = "0"
            Session("CanAccessReqMoveIn") = "0"
            Session("CanAccessReqMoveOut") = "0"
            Session("CanAccessReqMoveInhouse") = "0"
            Session("CanAccessMassQtyMove") = "0"
            Session("CanAccessPrintStickerBC") = "0"
            Session("CanAccessRePrintStickerBC") = "0"

            Dim ds As New Data.DataSet
            oWS = New SLWebServices.DOWebServiceSoapClient

            Dim PropertyList As String = "UserNamesUf_Users_Access_BCPallet, UserNamesUf_Users_Access_ReqMoveIn, UserNamesUf_Users_Access_ReqMoveOut, UserNamesUf_Users_Access_ReqMoveInhouse"
            PropertyList = PropertyList & ", UserNamesUf_Users_Access_MassQtyMove, UserNamesUf_Users_Access_PrintStickerBC, UserNamesUf_Users_Access_RePrintStickerBC"

            Dim Filter As String = "Username='" & Session("UserName").ToString & "'"
            ds = oWS.LoadDataSet(Session("Token").ToString, "UserNames", PropertyList, Filter, "", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then

                If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_BCPallet") IsNot DBNull.Value Then
                    If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_BCPallet").ToString = "1" Then
                        Session("CanAccessBCPallet") = "1"
                    End If
                End If

                If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveIn") IsNot DBNull.Value Then
                    If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveIn").ToString = "1" Then
                        Session("CanAccessReqMoveIn") = "1"
                    End If
                End If

                If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveOut") IsNot DBNull.Value Then
                    If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveOut").ToString = "1" Then
                        Session("CanAccessReqMoveOut") = "1"
                    End If
                End If

                If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveInhouse") IsNot DBNull.Value Then
                    If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_ReqMoveInhouse").ToString = "1" Then
                        Session("CanAccessReqMoveInhouse") = "1"
                    End If
                End If

                If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_MassQtyMove") IsNot DBNull.Value Then
                    If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_MassQtyMove").ToString = "1" Then
                        Session("CanAccessMassQtyMove") = "1"
                    End If
                End If

                If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_PrintStickerBC") IsNot DBNull.Value Then
                    If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_PrintStickerBC").ToString = "1" Then
                        Session("CanAccessPrintStickerBC") = "1"
                    End If
                End If

                If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_RePrintStickerBC") IsNot DBNull.Value Then
                    If ds.Tables(0).Rows(0)("UserNamesUf_Users_Access_RePrintStickerBC").ToString = "1" Then
                        Session("CanAccessRePrintStickerBC") = "1"
                    End If
                End If

            End If

            LinkBCPallet.Visible = Session("CanAccessBCPallet")
            LinkBRequestMoveIn.Visible = Session("CanAccessReqMoveIn")
            LinkRequestMoveOut.Visible = Session("CanAccessReqMoveOut")
            LinkBRequestMoveIn_InhouseWhse.Visible = Session("CanAccessReqMoveInhouse")
            LinkMassQtyMove.Visible = Session("CanAccessMassQtyMove")
            LinkPrintSticker.Visible = Session("CanAccessPrintStickerBC")
            LinkRePrintSticker.Visible = Session("CanAccessRePrintStickerBC")

            If Session("CanAccessBCPallet") <> "1" Then
                Me.divBCPallet.Attributes("class") = ""
            End If

            If Session("CanAccessReqMoveIn") <> "1" Then
                Me.divRequestMoveIn.Attributes("class") = ""
            End If

            If Session("CanAccessReqMoveOut") <> "1" Then
                Me.divRequestMoveOut.Attributes("class") = ""
            End If

            If Session("CanAccessReqMoveInhouse") <> "1" Then
                Me.divRequestMoveIn_InhouseWhse.Attributes("class") = ""
            End If

            If Session("CanAccessMassQtyMove") <> "1" Then
                Me.divMassQtyMove.Attributes("class") = ""
            End If

            If Session("CanAccessPrintStickerBC") <> "1" Then
                Me.divPrintSticker.Attributes("class") = ""
            End If

            If Session("CanAccessRePrintStickerBC") <> "1" Then
                Me.divRePrintSticker.Attributes("class") = ""
            End If
        End If

    End Sub

End Class
