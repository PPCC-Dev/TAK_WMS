Imports System.Web
Imports System.Web.Services
Imports System.Xml
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Web.Script.Serialization

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WebService
    Inherits System.Web.Services.WebService

    Dim oWS As SLWebServices.DOWebServiceSoapClient

    <WebMethod()> _
    Public Function GetMFGScrapReasonCode() As Data.DataTable
        Dim ds As New Data.DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLReasons", "ReasonCode, Description", "ReasonClass='MFG SCRAP'", "ReasonCode", "", 0)
        Return ds.Tables(0)
    End Function

    Public Function GetDownTimeReasonCode() As Data.DataTable
        Dim ds As New Data.DataSet
        Dim Filter As String = "ReasonClass='MFG DOWN TIME'"
        Dim PropList As String = ""
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLReasons", "ReasonCode, Description", Filter, "ReasonCode", "", 0)
        Return ds.Tables(0)
    End Function

    <WebMethod()> _
    Public Function GetUM() As Data.DataTable
        Dim ds As New Data.DataSet
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLUMs", "UM, Description", "", "UM", "", 0)
        Return ds.Tables(0)
    End Function

    <WebMethod()> _
    Public Function InsertJobTrans(ByVal Job As String, _
                                   ByVal Suffix As String, _
                                   ByVal TransType As String, _
                                   ByVal TransDate As String, _
                                   ByVal QtyComplete As String, _
                                   ByVal QtyScrapped As String, _
                                   ByVal OperNum As String, _
                                   ByVal AHrs As String, _
                                   ByVal NextOper As String, _
                                   ByVal EmpNum As String, _
                                   ByVal StartTime As String, _
                                   ByVal EndTime As String, _
                                   ByVal IndCode As String, _
                                   ByVal PayRate As String, _
                                   ByVal QtyMoved As String, _
                                   ByVal Whse As String, _
                                   ByVal Loc As String, _
                                   ByVal UserCode As String, _
                                   ByVal CloseJob As String, _
                                   ByVal IssueParents As String, _
                                   ByVal Lot As String, _
                                   ByVal CompleteOp As String, _
                                   ByVal PrRate As String, _
                                   ByVal JobRate As String, _
                                   ByVal Shift As String, _
                                   ByVal LowLevel As String, _
                                   ByVal Backflush As String, _
                                   ByVal ReasonCode As String, _
                                   ByVal TransClass As String, _
                                   ByVal PsNum As String, _
                                   ByVal Wc As String, _
                                   ByVal AwaitingEOP As String, _
                                   ByVal Fixovhd As String, _
                                   ByVal Varovhd As String, _
                                   ByVal CostCode As String, _
                                   ByVal CoProductMix As String, _
                                   ByVal ImportDocID As String, _
                                   ByVal ContainerNum As String, _
                                   ByVal EmpCount As String, _
                                   ByVal LeaveHours As String, _
                                   ByVal ManHours As String, _
                                   ByVal Remark As String, _
                                   ByVal Speed As String, ByRef TransNum As String) As Integer

        Dim Parms As String = "<Parameters><Parameter>" & Job & "</Parameter>" & _
                                "<Parameter>" & Suffix & "</Parameter>" & _
                                "<Parameter>" & TransType & "</Parameter>" & _
                                "<Parameter>" & TransDate & "</Parameter>" & _
                                "<Parameter>" & QtyComplete & "</Parameter>" & _
                                "<Parameter>" & QtyScrapped & "</Parameter>" & _
                                "<Parameter>" & OperNum & "</Parameter>" & _
                                "<Parameter>" & AHrs & "</Parameter>" & _
                                "<Parameter>" & NextOper & "</Parameter>" & _
                                "<Parameter>" & EmpNum & "</Parameter>" & _
                                "<Parameter>" & StartTime & "</Parameter>" & _
                                "<Parameter>" & EndTime & "</Parameter>" & _
                                "<Parameter>" & IndCode & "</Parameter>" & _
                                "<Parameter>" & PayRate & "</Parameter>" & _
                                "<Parameter>" & QtyMoved & "</Parameter>" & _
                                "<Parameter>" & Whse & "</Parameter>" & _
                                "<Parameter>" & Loc & "</Parameter>" & _
                                "<Parameter>" & UserCode & "</Parameter>" & _
                                "<Parameter>" & CloseJob & "</Parameter>" & _
                                "<Parameter>" & IssueParents & "</Parameter>" & _
                                "<Parameter>" & Lot & "</Parameter>" & _
                                "<Parameter>" & CompleteOp & "</Parameter>" & _
                                "<Parameter>" & PrRate & "</Parameter>" & _
                                "<Parameter>" & JobRate & "</Parameter>" & _
                                "<Parameter>" & Shift & "</Parameter>" & _
                                "<Parameter>" & LowLevel & "</Parameter>" & _
                                "<Parameter>" & Backflush & "</Parameter>" & _
                                "<Parameter>" & ReasonCode & "</Parameter>" & _
                                "<Parameter>" & TransClass & "</Parameter>" & _
                                "<Parameter>" & PsNum & "</Parameter>" & _
                                "<Parameter>" & Wc & "</Parameter>" & _
                                "<Parameter>" & AwaitingEOP & "</Parameter>" & _
                                "<Parameter>" & Fixovhd & "</Parameter>" & _
                                "<Parameter>" & Varovhd & "</Parameter>" & _
                                "<Parameter>" & CostCode & "</Parameter>" & _
                                "<Parameter>" & CoProductMix & "</Parameter>" & _
                                "<Parameter>" & ImportDocID & "</Parameter>" & _
                                "<Parameter>" & ContainerNum & "</Parameter>" & _
                                "<Parameter>" & EmpCount & "</Parameter>" & _
                                "<Parameter>" & LeaveHours & "</Parameter>" & _
                                "<Parameter>" & ManHours & "</Parameter>" & _
                                "<Parameter>" & Remark & "</Parameter>" & _
                                "<Parameter>" & Speed & "</Parameter>" & _
                                "<Parameter>" & TransNum & "</Parameter></Parameters>"
        Dim res As Object
        oWS = New SLWebServices.DOWebServiceSoapClient
        res = oWS.CallMethod(Session("Token").ToString, "PPCC_JobTrans", "PPCC_CreateJobTransactionSp", Parms)

        Return Convert.ToInt32(res)
    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Function GetJobList(ByVal prefixText As String) As String

        Dim ds As New Data.DataSet
        Dim Filter As String = "Stat = 'R' And Type = 'J' And Job LIKE '" & prefixText & "%'"
        Dim PropList As String = "Job, Item, Description"
        oWS = New SLWebServices.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", PropList, Filter, "Job", "", 0)

        Dim j As New List(Of Job)

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            j.Add(New Job With {.Job = ds.Tables(0).Rows(i)("Job").ToString, .Item = ds.Tables(0).Rows(i)("Item").ToString, .Description = ds.Tables(0).Rows(i)("Description").ToString})
        Next

        Dim serializer As New JavaScriptSerializer()
        Dim serializedResult = serializer.Serialize(j)

        Return serializedResult
    End Function

    Public Class Job
        Private _Job As String
        Private _Item As String
        Private _Description As String

        Public Property Job As String
            Get
                Return _Job
            End Get
            Set(value As String)
                _Job = value
            End Set
        End Property

        Public Property Item As String
            Get
                Return _Item
            End Get
            Set(value As String)
                _Item = value
            End Set
        End Property

        Public Property Description As String
            Get
                Return _Description
            End Get
            Set(value As String)
                _Description = value
            End Set
        End Property
    End Class
End Class