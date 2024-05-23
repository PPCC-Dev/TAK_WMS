<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RequestMoveIn_InhouseWhse.aspx.vb" Inherits="Default2" %>

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

<style type="text/css">
        .centerHeaderText th { text-align: center; }
        .table-condensed{ font-size: 13.5px; }
    </style> 

    <style type="text/css">
        .pagination-ys {
        /*display: inline-block;*/
        padding-left: 0;
        margin: 20px 0;
        border-radius: 4px;
        }
 
        .pagination-ys table > tbody > tr > td {
            display: inline;
        }
 
        .pagination-ys table > tbody > tr > td > a,
        .pagination-ys table > tbody > tr > td > span {
            position: relative;
            float: left;
            padding: 6.5px 11px;
            line-height: 1.42857143;
            text-decoration: none;
            color: #000000;
            background-color: #ffffff;
           border: 1px solid #dddddd;
            margin-left: -1px;
        }
 
        .pagination-ys table > tbody > tr > td > span {
            position: relative;
            float: left;
            padding: 6.5px 11px;
            line-height: 1.42857143;
            text-decoration: none;    
            margin-left: -1px;
            z-index: 2;
            color: #000000;
            background-color: #f5f5f5;
            border-color: #dddddd;
            cursor: default;
        }
 
        .pagination-ys table > tbody > tr > td:first-child > a,
        .pagination-ys table > tbody > tr > td:first-child > span {
            margin-left: 0;
            border-bottom-left-radius: 4px;
            border-top-left-radius: 4px;
        }

        .pagination-ys table > tbody > tr > td:last-child > a,
        .pagination-ys table > tbody > tr > td:last-child > span {
            border-bottom-right-radius: 4px;
            border-top-right-radius: 4px;
        }
 
        .pagination-ys table > tbody > tr > td > a:hover,
        .pagination-ys table > tbody > tr > td > span:hover,
        .pagination-ys table > tbody > tr > td > a:focus,
        .pagination-ys table > tbody > tr > td > span:focus {
            color: #97310e;
            background-color: #eeeeee;
            border-color: #dddddd;
        }
 </style>

    <div class="row" style="padding-top: 10px">
        <div class="col-lg-12">
             <div class="panel panel-default">
                    <div class="panel-heading">
                        <label>Request Move Pallet In - Inhouse Whse</label>
                    </div>
                    

                     <!-- /.panel-heading -->
                    <div class="panel-body">
                    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel runat="server" ID="updatepanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="NotPassNotifyPanel" runat="server" CssClass= "alert alert-danger alert-dismissable" Visible="false">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                            <asp:Literal ID="NotPassText" runat="server"></asp:Literal>
                        </asp:Panel>
                         <asp:Panel ID="PassNotifyPanel" runat="server" CssClass= "alert alert-success alert-dismissable" Visible="false">
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                <asp:Literal ID="PassText" runat="server"></asp:Literal>
                        </asp:Panel>

                        <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClientClick="showProgress();" style="display:none; height:0px; width:0px;" />

                        <table style="padding-top: 20px; width:100%;">
                        <tr style="height:50px;">
                        <td style="text-align:right;" >
                            <label>Barcode:&nbsp;&nbsp;</label>
                        </td>

                        <td style="padding-left: 10px; text-align:left;">
                            <asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control typeahead" Width="500px" AutoPostBack="True" AutoComplete="off" onchange="showProgress();"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBarcodetmp" runat="server" AutoComplete="off" Enabled="false" Visible="false"></asp:TextBox>
                        </td>
                              
                    </tr>
                    </table>
                                      
                    <br />
                        
                    <table style="padding-top: 20px; width:100%; ">

                    <tr>
                        <td>
                        <asp:GridView ID="GridDetail" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="table table-striped table-bordered table-hover table-condensed" EmptyDataRowStyle-HorizontalAlign="Center"
                            HeaderStyle-CssClass= "centerHeaderText" style="width:100%"
                            EmptyDataText="No Record Data" EnableModelValidation="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-Font-Bold="true"
                            AllowPaging="True" PageSize="10" OnPageIndexChanging="ShowPageCommand" >
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>                           
                            <asp:BoundField HeaderText="Pallet ID" DataField="OrderNumber" ItemStyle-HorizontalAlign="Left" />                                    
                            <asp:BoundField HeaderText="Item" DataField="Item" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Lot" DataField="Lot" ItemStyle-HorizontalAlign="Left" />  
                            <asp:BoundField HeaderText="Quantity" DataField="Qty" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />                                     
                            <asp:BoundField HeaderText="UM" DataField="UM" ItemStyle-HorizontalAlign="Left" /> 
                            <asp:TemplateField HeaderText="Qty Convert" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblDerQtyConv" runat="server" Text='<%#Eval("QtyConv", "{0:N2}")%>'></asp:Label>                                            
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                                    <asp:BoundField HeaderText="Qty Convert" DataField="DerQtyConv" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> --%>  
                            <asp:BoundField HeaderText="UM Convert" DataField="FromUM" ItemStyle-HorizontalAlign="Left" /> 
                            <asp:BoundField HeaderText="Error Message" DataField="Msg" ItemStyle-HorizontalAlign="Left" /> 
                            </Columns>
                            </asp:GridView>
                            
                        </td>
                    </tr>               

                </table>

                    <table style="padding-top: 20px; width:100%;">
                    <tr>
                        <td>

                        <asp:GridView ID="GridSummary" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass= "centerHeaderText"
                            EnableModelValidation="True" CssClass="table table-striped table-bordered table-hover table-condensed">
                            <Columns>
                                <asp:BoundField HeaderText="PalletID" DataField="OrderNumber" ItemStyle-HorizontalAlign="Left" /> 
                                <asp:TemplateField HeaderText="Request Move Status" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReqMoveStaus" runat="server" Text='<%#Eval("ReqStat")%>'></asp:Label>                                            
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                            
                        </td>
                    </tr>               

                </table>

                    </ContentTemplate>
                    <Triggers>
                       <asp:PostBackTrigger ControlID="txtBarcode" />
                       <asp:PostBackTrigger ControlID="GridDetail" />
                    </Triggers>
                    </asp:UpdatePanel>

                    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="updatepanel1">
                    <ProgressTemplate>
                        <div class="overlay">
                        <div style=" z-index: 1000; margin-left: 350px;margin-top:200px;opacity: 1;-moz-opacity: 1;">
                            <img alt="" src="image/loading_wait.gif" />
                        </div>
                        </div>
                    </ProgressTemplate>
                    </asp:UpdateProgress>
                    
                    
                </div>
            </div>
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

    <style type="text/css">
    .overlay
    {
    position: fixed;
    z-index: 999;
    height: 100%;
    width: 100%;
    top: 0;
    background-color: White;
    filter: alpha(opacity=100);
    opacity: 0.6;
    -moz-opacity: 0.8;
    }
    </style>
    <script type="text/javascript">
        function showProgress() {
            var Barcode = document.getElementById("txtBarcode").value;
            if (Barcode !== "" || Barcode.length !== 0 || Barcode !== null) {
                var updateProgress = $get("<%= UpdateProgress.ClientID %>");
                updateProgress.style.display = "block";
            }
            else {
                document.getElementById("txtBarcode").focus()
            }
        }
    </script>

</asp:Content>

