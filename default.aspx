<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="_default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js" type="text/javascript"></script>
    <script src="js/jquery.maskedinput.js" type="text/javascript"></script>
    <link href="css/examples.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <%--<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">--%>
    <meta name="viewport" content="width=device-width, initial-scale=1">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div class="row" style="padding-top: 10px">
    <div class="col-lg-12">
         <%--<div class="panel panel-default">
                <div class="panel-heading">
                    <label>Please Scan Employee Card</label>
                </div>
                <asp:Panel ID="NotPassNotifyPanel" runat="server" CssClass= "alert alert-danger alert-dismissable" Visible="false">
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        <asp:Literal ID="NotPassText" runat="server"></asp:Literal>
                </asp:Panel>--%>
                <!-- /.panel-heading -->
                <%--<div class="panel-body">--%>
                    
                    <%--<table style="padding-top: 20px">
                        <tr>
                            <td>
                               <asp:TextBox ID="txtBarcode" runat="server" 
                                                    CssClass="form-control typeahead" Width="400px" TabIndex="1" AutoPostBack="true"></asp:TextBox>
                            </td>
                            
                        </tr>
                    </table>
            </div>
        </div>--%>
    </div>
</div>
<!-- jQuery -->
<script src="bower_components/jquery/dist/jquery.min.js" type="text/javascript"></script>
<!-- Bootstrap Core JavaScript -->
<script src="bower_components/bootstrap/dist/js/bootstrap.min.js" type="text/javascript"></script>
<!-- Metis Menu Plugin JavaScript -->
<script src="bower_components/metisMenu/dist/metisMenu.min.js" type="text/javascript"></script>

<!-- Custom Theme JavaScript -->
<script src="dist/js/sb-admin-2.js" type="text/javascript"></script>

</asp:Content>

