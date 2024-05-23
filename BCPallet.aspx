<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" EnableEventValidation="true" AutoEventWireup="false" CodeFile="BCPallet.aspx.vb" Inherits="BCPallet" %>

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

<script src="jquery-1.2.6.min.js" type="text/javascript"></script>

<script language="javascript" type="text/javascript">
    function confirmDelete() {
        var CountDelete = document.getElementById("<%=Label1.ClientID %>").innerText;
        if (confirm("คุณต้องการลบ " + CountDelete + " รายการใช่หรือไม่?"))
            return true;
        return false;
    }
</script>

<script type = "text/javascript">
    function Check_Click(objRef) {
        //Get the Row based on checkbox
        var row = objRef.parentNode.parentNode;

        //Get the reference of GridView
        var GridView = row.parentNode;

        //Get all input elements in Gridview
        var inputList = GridView.getElementsByTagName("input");

        for (var i = 0; i < inputList.length; i++) {
            //The First element is the Header Checkbox
            var headerCheckBox = inputList[0];

            //Based on all or none checkboxes
            //are checked check/uncheck Header Checkbox
            var checked = true;
            if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                if (!inputList[i].checked) {
                    checked = false;
                    break;
                }
            }
        }
        headerCheckBox.checked = checked;

    }
    function checkAll(objRef) {
        var GridView = objRef.parentNode.parentNode.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            var row = inputList[i].parentNode.parentNode;
            if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                if (objRef.checked) {
                    inputList[i].checked = true;
                }
                else {
                    if (row.rowIndex % 2 == 0) {
//                        row.style.backgroundColor = "#C2D69B";
                    }
                    else {
//                        row.style.backgroundColor = "white";
                    }
                    inputList[i].checked = false;
                }
            }
        }
    }
    //-->
</script>

 <style type="text/css">
    .hide { display: none; }
    .centerHeaderText th { text-align: center; }
    .table-condensed{ font-size: 13px; }
    .Itemgrid-fontsize{ font-size: 12px; }
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
                    <label>BC Pallet</label>
                </div>
                <!-- /.panel-heading -->

        <div class="panel-body">
            <asp:ScriptManager ID="scriptmanager2" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel runat="server" ID="updatepanel2" UpdateMode="Conditional">
                <ContentTemplate>
                        
                    <asp:Panel ID="NotPassNotifyPanel" runat="server" CssClass= "alert alert-danger alert-dismissable" Visible="false">
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        <asp:Literal ID="NotPassText" runat="server"></asp:Literal>
                    </asp:Panel>
                    <asp:Panel ID="PassNotifyPanel" runat="server" CssClass= "alert alert-success alert-dismissable" Visible="false">
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        <asp:Literal ID="PassText" runat="server"></asp:Literal>
                    </asp:Panel>

                <%--<asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">--%>
                        <table style="padding-top: 20px; width:100%;">
                        <%--<tr>
                            <td style="text-align:right;" >
                                
                            </td>

                            <td style="padding-left: 10px; text-align:left;" colspan = "5">
                                 <asp:RadioButton ID="rdoNew" runat="server" 
                                              GroupName="RadioType" Checked="True" />&nbsp;&nbsp;New Barcode
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rdoReprint" runat="server" 
                                              GroupName="RadioType" />&nbsp;&nbsp;Reprint Barcode
                            </td>

                            <td style="padding-right: 10px; text-align:center;">
                                
                            </td>
                        </tr>--%>
                        <tr style="height:50px;">
                            <td style="text-align:right;" >
                                <label>Barcode:&nbsp;&nbsp;</label>
                            </td>
                                <td style="padding-left: 10px; text-align:left;" colspan = "5">
                                    <asp:TextBox ID="txtBarcode" runat="server" Width="500px" CssClass="form-control typeahead" AutoPostBack="True" AutoComplete="off"></asp:TextBox>
                                </td>
                            

                            <td style="padding-right: 10px; text-align:center;">
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="btn btn-primary" Width="120px" OnClientClick="showProgress();" />
                            </td>
                        </tr>
                        <tr style="height:50px;">
                            <td style="text-align:right;">
                                <%--<div class="radio">
                                   <asp:CheckBox ID="chkUpdateQty" runat="server" Checked="false" Enabled="True" AutoPostBack="true" onchange="setFocus();" /><B>&nbsp;&nbsp;Update Qty</B>
                                </div>--%>
                            </td>

                            <td style="padding-left: 20px; text-align:left;">
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-warning" Width="80px" />
                            </td>

                            <td style="padding-left: 20px; text-align:left;">
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger" Width="80px" OnClientClick="return confirmDelete();" />
                            </td>

                            <td style="padding-left: 20px; text-align:left;">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success" Width="80px" />
                            </td>

                            <td style="text-align:right;" >
                                <label>Pallet ID:&nbsp;&nbsp;</label>
                            </td>
                            <td style="padding-left: 10px; text-align:left;">
                                <asp:TextBox ID="txtPalletID" runat="server" CssClass="form-control typeahead" Width="230px" Enabled="false"></asp:TextBox>
                            </td>

                            <td style="padding-right: 10px; text-align:center;">
                                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Width="120px" Enabled="false" Visible="true" OnClientClick="showProgress();" />                                                
                            </td>
                        </tr>
                    </table>

                    <%--</asp:View>--%>
                                       
                    <br />
                    <asp:TextBox ID="txtBCPallet" runat="server" Enabled="false" Visible="false"></asp:TextBox>
                    <asp:HiddenField ID="hfCount" runat="server" Value = "0" />
                    <asp:Label ID="Label1" runat="server" style="display:none"></asp:Label>

                    <%--<asp:View ID="View2" runat="server">--%>
                    <table style="padding-top: 30px; width:50%;">
                        <tr>
                             <td  style="text-align:left; width:100px;">
                                <B>Total Qty:&nbsp;&nbsp;</B>                           
                            </td>
                            <td style="text-align:right; width:150px;">
                                <asp:Label ID="lblTotalQty" runat="server" Text="0.00" Font-Bold="true"></asp:Label>
                            </td>
                             <td  style="width:70px;" />
                             <td  style="text-align:left; width:100px;">
                                <B>Total Lot:&nbsp;&nbsp;</B>                          
                            </td>
                            <td style="text-align:right; width:150px;">
                                <asp:Label ID="lblTotalLot" runat="server" Text="0.00" Font-Bold="true" style="text-align:right;"></asp:Label>
                            </td>
                        </tr>
                       </table>

                        <table style="padding-top: 20px; width:100%;">                       
                        <tr>
                            <td rowspan="2">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                            <%--<div style ="height:350px; width:100%; overflow:auto;">--%>
                                            <asp:GridView ID="GridData" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" CssClass="table table-bordered table-striped table-hover table-condensed" EmptyDataRowStyle-HorizontalAlign="Center"
                                            DataKeyNames="RowPointer" HeaderStyle-CssClass= "centerHeaderText" style="width:100%;"
                                            EmptyDataText="No Record Data" EnableModelValidation="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-Font-Bold="true"
                                            AllowPaging="true" PageSize="10" OnPageIndexChanging="ShowPageCommand" OnRowDataBound="GridData_RowDataBound">
                                            <PagerStyle CssClass="pagination-ys" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="HdrCheckBox" runat="server" onclick="checkAll(this);" OnCheckedChanged="HdrCheckBox_OnCheckedChanged" AutoPostBack="true"  />                                                   
                                                    </HeaderTemplate>
                                                   <ItemTemplate>
                                                        <asp:CheckBox ID="SelectCheckBox" runat="server" onclick = "Check_Click(this)" OnCheckedChanged="SelectCheckBox_OnCheckedChanged" AutoPostBack="true"  />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            
                                                <asp:TemplateField HeaderText="Line" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Pallet" DataField="PalletID" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" ItemStyle-CssClass="Itemgrid-fontsize" ReadOnly="true" />
                                                <asp:BoundField HeaderText="Barcode" DataField="Barcode" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" ItemStyle-CssClass="Itemgrid-fontsize" HeaderStyle-Width="150px" ItemStyle-Width="150px" ReadOnly="true" />
                                                <asp:BoundField HeaderText="Item" DataField="Item" ItemStyle-HorizontalAlign="Left" ReadOnly="true" >
                                                    <HeaderStyle CssClass="hide" />
                                                    <ItemStyle CssClass="hide" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" ItemStyle-CssClass="Itemgrid-fontsize" ReadOnly="true"  />
                                                <asp:BoundField HeaderText="Lot" DataField="Lot" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Wrap="true" ItemStyle-CssClass="Itemgrid-fontsize" ReadOnly="true" />
                                                <asp:BoundField HeaderText="Seq" DataField="Seq" ItemStyle-HorizontalAlign="Center"  ItemStyle-Wrap="true" ItemStyle-CssClass="Itemgrid-fontsize" ReadOnly="true" />
                                                <asp:BoundField HeaderText="VendorLot" DataField="VendLot" ItemStyle-HorizontalAlign="Left"  ItemStyle-Wrap="true" ItemStyle-CssClass="Itemgrid-fontsize" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Qty" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="true" ItemStyle-CssClass="Itemgrid-fontsize">
                                                    <ItemTemplate>
                                                       <asp:Label ID="lblQty" Text='<%#Eval("Qty", "{0:N2}")%>' runat="server" style="text-align: right; font-size=12px;" />
                                                    </ItemTemplate>
                                                     <EditItemTemplate>
                                                        <asp:TextBox ID="txtQty" Text='<%#Eval("Qty", "{0:N2}")%>' runat="server" Width="90px" CssClass="form-control typeahead" Autocomplete="off" style="text-align:right; font-size:12px;" ></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="myRegValid" runat="server" ErrorMessage="Only numeric and decimal 2 pleaces" ControlToValidate="txtQty" ValidationExpression="(\d{1,3}(,\d{3})*\.\d{2})|(\d+(\.\d{2})?)" ForeColor="Red" Font-Size="10px" Display="Dynamic" ></asp:RegularExpressionValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>  
                                                <asp:BoundField HeaderText="UM" DataField="UM" ItemStyle-HorizontalAlign="Left"  ItemStyle-Wrap="true" ItemStyle-CssClass="Itemgrid-fontsize" ReadOnly="true" />
                                                <asp:BoundField HeaderText="CreateDate" DataField="CreateDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center"  ItemStyle-Wrap="true" ItemStyle-CssClass="Itemgrid-fontsize" ReadOnly="true" />
                                                <asp:BoundField HeaderText="CreateBy" DataField="Username" ItemStyle-HorizontalAlign="Left"  ItemStyle-Wrap="true" ItemStyle-CssClass="Itemgrid-fontsize" ReadOnly="true" />
                                                <asp:BoundField HeaderText="RowPointer" DataField="RowPointer" ItemStyle-HorizontalAlign="Left"  ItemStyle-Wrap="true" ReadOnly="true" >
                                                    <HeaderStyle CssClass="hide" />
                                                    <ItemStyle CssClass="hide" />                                                
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="PalletNo" DataField="PalletNo" ItemStyle-HorizontalAlign="Left"  ItemStyle-Wrap="true" ReadOnly="true" >
                                                    <HeaderStyle CssClass="hide" />
                                                    <ItemStyle CssClass="hide" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Selected" DataField="Selected" ItemStyle-HorizontalAlign="Left"  ItemStyle-Wrap="true" ReadOnly="true" >
                                                    <HeaderStyle CssClass="hide" />
                                                    <ItemStyle CssClass="hide" />
                                                </asp:BoundField>

                                                <%--<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Update Qty">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnEdit" runat="server" CommandName="Edit" style="font-size:xx-small;" Text="Edit" CssClass="btn btn-default btn-xm" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Update" style="font-size:xx-small;"  CssClass="btn btn-default btn-xm" />
                                                    <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" style="font-size:xx-small;"  CssClass="btn btn-default btn-xm" />
                                                </EditItemTemplate>
                                                </asp:TemplateField>--%>
                                               
                                            </Columns>
                                        </asp:GridView>
                                        <%--</div>--%>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="GridData" />
                                    </Triggers>
                                </asp:UpdatePanel>
                       
                            
                            </td>
                        </tr>               

                    </table>

                    <%--</asp:View>
                    <asp:View ID="View2" runat="server">
                        
                    </asp:View>
                </asp:MultiView>--%>
                           
            </div>

            </ContentTemplate>
            <Triggers>
               <asp:PostBackTrigger ControlID="btnPreview" />
               <asp:PostBackTrigger ControlID="txtBarcode" />              
               <asp:PostBackTrigger ControlID="btnClear" />
               <asp:PostBackTrigger ControlID="btnDelete" />
               <asp:PostBackTrigger ControlID="btnSave" />
               <asp:PostBackTrigger ControlID="btnPrint" />
               <asp:PostBackTrigger ControlID="GridData" />
            </Triggers>
            </asp:UpdatePanel>

            <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="updatepanel2">
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

<!-- jQuery -->
<script src="bower_components/jquery/dist/jquery.min.js" type="text/javascript"></script>
<!-- Bootstrap Core JavaScript -->
<script src="bower_components/bootstrap/dist/js/bootstrap.min.js" type="text/javascript"></script>
<!-- Metis Menu Plugin JavaScript -->
<script src="bower_components/metisMenu/dist/metisMenu.min.js" type="text/javascript"></script>

<!-- DataTables JavaScript -->
<script src="bower_components/DataTables/media/js/jquery.dataTables.min.js" type="text/javascript"></script>
<script src="bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js" type="text/javascript"></script>

<!-- Custom Theme JavaScript -->
<script src="dist/js/sb-admin-2.js" type="text/javascript"></script>

<script src="js/typeahead.bundle.js" type="text/javascript"></script>

<script type="text/javascript">
    function SelectAllCheckboxes1(chk) {
        $('#<%=GridData.ClientID%>').find("input:checkbox").each(function () {
            if (this != chk) { this.checked = chk.checked; }
        });
    }
</script>



<%--<script type="text/javascript">

    function setFocus() {
        var checkbox = document.getElementById('<%=chkUpdateQty.ClientID %>');
        var Textbox = document.getElementById('<%= txtBarcode.ClientID%>');
        if (checkbox.checked) {
            Textbox.disabled = false;
            Textbox.focus();
        }
        else {
            Textbox.disabled = false;
            Textbox.focus();
        }
    }
    </script>--%>

   <%-- <script type="text/javascript">
        function setFocusRedio() {
            var rdoNew = document.getElementById('<%=rdoNew.ClientID %>');
            var rdoReprint = document.getElementById('<%=rdoReprint.ClientID %>');
            var Textbox = document.getElementById('<%= txtBarcode.ClientID%>');
            if (rdoNew.checked) {
                Textbox.disabled = false;
                Textbox.focus();
            }
            else if (rdoReprint.checked) {
                Textbox.disabled = false;
                Textbox.focus();
            }
        }
    </script>--%>

    <script type="text/javascript">
        function CheckOrUnCheckAll(obj) {
            var valid = false;
            //Varibale to hold the checked checkbox count
            var chkselectcount = 0;
            //Get the gridview object
            var gridview = document.getElementById('<%= GridData.ClientID %>');
            //Loop thorugh items
            for (var i = 0; i < gridview.getElementsByTagName("input").length; i++) {
                //Get the object of input type
                var node = gridview.getElementsByTagName("input")[i];
                //check if object is of type checkbox and checked or not
                if (node != null && node.type == "checkbox") {
                    //check or uncheck the checkboxes based on passsed value
                    node.checked = obj;
                }
            }
            return false;
        }

        
    </script>

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
            var updateProgress = $get("<%= UpdateProgress.ClientID %>");
            updateProgress.style.display = "block";
        }
    </script>

</asp:Content>
