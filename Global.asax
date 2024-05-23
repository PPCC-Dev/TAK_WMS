<%@ Application Language="VB" %>

<script runat="server">
    
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        'If Session("Employee") IsNot Nothing Then
        '    Dim oWs As New SLWebServices.DOWebServiceSoapClient
        '    Dim obj As Object
        '    Dim Parms As String = "<Parameters>" & "<Parameter>" & String.Empty & "</Parameter>" & _
        '        "<Parameter>" & Session("Employee").ToString & "</Parameter>" & "</Parameters>"
        '    obj = oWs.CallMethod(Session("Token").ToString, "PPCC_JobTrans", "PPCC_WSDeleteJobTranSp", Parms)
        '    'Response.Redirect("signin.aspx")
        'End If
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
        'If Session("Employee") IsNot Nothing Then
        '    Dim oWs As New SLWebServices.DOWebServiceSoapClient
        '    Dim obj As Object
        '    Dim Parms As String = "<Parameters>" & "<Parameter>" & String.Empty & "</Parameter>" & _
        '        "<Parameter>" & Session("Employee").ToString & "</Parameter>" & "</Parameters>"
        '    obj = oWs.CallMethod(Session("Token").ToString, "PPCC_JobTrans", "PPCC_WSDeleteJobTranSp", Parms)
        '    'Response.Redirect("signin.aspx")
        'End If
    End Sub
    
    
</script>