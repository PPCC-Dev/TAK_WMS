<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RequestMoveOut.aspx.vb" Inherits="RequestMoveOut" %>

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
        .hide { display: none; }
        .centerHeaderText th { text-align: center; }
        .table-condensed{ font-size: 13.5px; }
 </style>

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
                    <label>Request Move Pallet Out WMS</label>
                </div>

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

                    <!-- /.panel-heading -->
                    <div class="panel-body">
                        <%--<asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="View1" runat="server">--%>
                                     <table style="padding-top: 20px; width:100%;">
                                    <tr style="height:30px;">
                                        <td style="text-align:right;" >
                                            <label>Order Number:&nbsp;&nbsp;</label>
                                        </td>

                                        <td style="padding-left: 10px; text-align:left;">
                                            <asp:TextBox ID="txtOrder" runat="server" CssClass="form-control typeahead" Width="150px" Enabled="false"></asp:TextBox>
                                        </td>

                                        <td style="padding-left: 10px; text-align:left;">
                                            <div class="radio">                                  
                                                 <asp:RadioButton ID="rdoSelectdJob" runat="server" 
                                                  GroupName="RadioType" Checked="True" AutoPostBack="true" /><B>Job Number</B>
                                            </div>
                                         </td>

                                        <td style="padding-left: 10px; text-align:left;">

                                             <asp:DropDownList ID="ddlJob" runat="server" CssClass="form-control" Width="150px" style="display:none">
                                             </asp:DropDownList>

                                                    <div id="div-job">
                                                        <table>
                                                            <tr>
                                                                <td><asp:TextBox ID="txtJobSearch" runat="server" 
                                                                        CssClass="form-control typeahead" Width="180px" 
                                                                        Autocomplete="off" AutoPostBack="true" Enabled="true"></asp:TextBox>

                                                                    <%--<asp:HiddenField ID="hfJobNum" runat="server" />--%>
                                                                
                                                             </td>
                                                             <td style="width:5px;"></td>
                                                             <td><asp:DropDownList ID="ddlsuffix" runat="server" CssClass="form-control" Width="60px" Enabled="false" AutoPostBack="true"></asp:DropDownList>

                                                                    <%--<asp:HiddenField ID="hfJobNum" runat="server" />--%>
                                                                
                                                             </td>
                                                        
                                                            </tr>
                                                        </table>
                                                
                                                    </div>
                                        </td>

                                        <td style="padding-left: 5px; text-align:Left;">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" Width="120px" OnClientClick="showProgress();" />
                                        </td>
                              
                                    </tr>
                                    <tr style="height:30px;">
                                        <td style="text-align:right;" rowspan="2"></td>

                                        <td style="padding-left: 10px; text-align:left;" rowspan="2"></td>

                                        <td style="padding-left: 10px; text-align:left;">
                                            <div class="radio">                                  
                                                 <asp:RadioButton ID="rdoSelectdCo" runat="server" 
                                                  GroupName="RadioType" AutoPostBack="true" /><B>CO Number</B>
                                            </div>
                                         </td>

                                         <td style="padding-left: 10px; text-align:left;">
                                           <asp:DropDownList ID="ddlCo" runat="server" CssClass="form-control" Width="200px" style="display:none">
                                           </asp:DropDownList>

                                           <div id="div-co">
                                           <table>
                                               <tr>
                                                    <td><asp:TextBox ID="txtCoSearch" runat="server" 
                                                                CssClass="form-control typeahead" Width="250px" 
                                                                AutoPostBack="True" Autocomplete="off" Enabled="false"></asp:TextBox></td>
                                                    </tr>
                                                </table>
                                                
                                            </div>
                                        </td>
                                                           
                                    </tr>

                                    <tr style="height:30px;">

                                        <td style="padding-left: 10px; text-align:left;">
                                            <div class="radio">                                  
                                                 <asp:RadioButton ID="rdoSelectdOther" runat="server" 
                                                  GroupName="RadioType" AutoPostBack="true" /><B>เบิกอื่นๆ</B>
                                            </div>
                                         </td>

                                         <td style="padding-left: 10px; text-align:left;">

                                        </td>
                                                           
                                    </tr>
                                </table> 
                                <asp:HiddenField ID="hfCount" runat="server" Value = "0" />                                                    
                            <table style="width:100%;">
                                <tr>
                                    <td>                           
                                    
                                        <asp:GridView ID="GridReqMove" runat="server" AutoGenerateColumns="False" DataKeyNames="RowPointer"
                                                    EnableModelValidation="True" CssClass="table table-striped table-bordered table-hover table-condensed" AllowPaging="true" PageSize="10"
                                                    OnPageIndexChanging="ShowPageCommand" style="width:100%" HeaderStyle-CssClass= "centerHeaderText"  >
                                                    <PagerStyle CssClass="pagination-ys" />
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" ItemStyle-Width="70px" >
                                                            <HeaderTemplate>
                                                               <%-- <asp:CheckBox ID="HdrCheckBox" runat="server" onclick="javascript:SelectAllCheckboxes1(this);"  />--%>
                                                               <asp:CheckBox ID="HdrCheckBox" runat="server" onclick="checkAll(this);" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%--<asp:CheckBox ID="SelectCheckBox" runat="server" CommandName="CheckBoxCommand" />--%>
                                                                <asp:CheckBox ID="SelectCheckBox" runat="server" onclick = "Check_Click(this)" AutoPostBack="true" OnCheckedChanged="SelectCheckBox_OnCheckedChanged" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Line" DataField="RefLine" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-Width="50px" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="Item" DataField="Item" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px" ItemStyle-Width="300px" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="Lot" DataField="Lot" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="130px" ItemStyle-Width="130px" ReadOnly="true" />
                                                        <asp:TemplateField HeaderText="Qty To Shipped" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="130px" ItemStyle-Width="130px" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyReq" Text='<%#Eval("QtyIssued", "{0:N2}")%>' runat="server" style="text-align: right" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtQtyReq" Text='<%#Eval("QtyIssued", "{0:N2}")%>' runat="server" Width="100px"  CssClass="form-control typeahead" Autocomplete="off" style="text-align:right;" ></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="myRegValid" runat="server" ErrorMessage="Only numeric and decimal 2 pleaces" ControlToValidate="txtQtyReq" ValidationExpression="(\d{1,3}(,\d{3})*\.\d{2})|(\d+(\.\d{2})?)" ForeColor="Red" Font-Size="10px" Display="Dynamic"></asp:RegularExpressionValidator>
                                                        </EditItemTemplate>
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Request Qty" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="130px" ItemStyle-Width="130px" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyReq2" Text='<%#Eval("QtyReq", "{0:N2}")%>' runat="server" style="text-align: right" />
                                                        </ItemTemplate>                                               
                                                        </asp:TemplateField>                                                    
                                                        <asp:BoundField HeaderText="UM" DataField="UM" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" ItemStyle-Width="70px" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="Warning Message" DataField="msg" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" ItemStyle-Width="200px" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="RowPointer" DataField="RowPointer" ItemStyle-HorizontalAlign="Left" ReadOnly="true" >
                                                            <HeaderStyle CssClass="hide" />
                                                            <ItemStyle CssClass="hide" />
                                                         </asp:BoundField>
                                                         <asp:BoundField HeaderText="" DataField="Selected" ItemStyle-HorizontalAlign="Left" ReadOnly="true" >
                                                            <HeaderStyle CssClass="hide" />
                                                            <ItemStyle CssClass="hide" />
                                                         </asp:BoundField>

                                                         <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                         <ItemTemplate>
                                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-default btn-sm" />
                                                         </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Update" CssClass="btn btn-default btn-sm" />
                                                            <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-default btn-sm" />
                                                        </EditItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                </td>
                                                </tr>
                                                </table>

                                    <table style="width:100%;">
                                    <tr>
                                       <td> 
                                        <table class="table table-striped table-bordered table-hover" style="border-collapse:collapse; width:100%">
                                        <tr> 
                                            <td style="white-space: nowrap">
                                                <table>
                                                    <tr>
                                                        <td style="width:60px"><label>Item: </label></td>
                                                        <td style="width:300px"><asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" Width="250px" style="display:none;"></asp:DropDownList>
                                                                                <div id="div-item"><asp:TextBox ID="txtAddItem" AutoPostBack="true" runat="server" CssClass="form-control typeahead" Width="250px" style="text-align: left;"></asp:TextBox></div></td>    
                                                        <td style="width:100px"><asp:Label ID="lblum" runat="server" Font-Bold="true"></asp:Label></td>                                                        
                                                        <td style="width:60px"><label>Lot: </label></td>
                                                        <td style="width:230px"><asp:DropDownList ID="ddlAddLot" runat="server" CssClass="form-control" Width="180px" AutoPostBack="true"></asp:DropDownList></td>
                                                        <td style="width:100px"><label>Request Qty: </label></td>
                                                        <td style="width:180px"><asp:TextBox ID="txtAddQtyReq" runat="server" CssClass="form-control" Width="130px"  style="text-align: right;" Autocomplete="off"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:60px" />
                                                        <td style="width:300px" />
                                                        <td style="width:100px" />
                                                        <td style="width:60px" />
                                                        <td style="width:230px" />
                                                        <td style="width:100px" />
                                                        <td style="width:180px"><asp:RegularExpressionValidator ID="myRegValidAddQtyReq" runat="server" ErrorMessage="Only numeric and decimal 2 pleaces" ControlToValidate="txtAddQtyReq" ValidationExpression="(\d{1,3}(,\d{3})*\.\d{2})|(\d+(\.\d{2})?)" ForeColor="Red" Font-Size="10px" Display="Dynamic"></asp:RegularExpressionValidator></td>             
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
	                                </table>      
                                                         
                                </td>
                               </tr>               
                            </table>

                                <center>
                                    <table style="width:50%;">
                                        <tr style="height:50px; text-align:center;">
                                            <td style="text-align:right;">
                                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-info" Width="80px" />
                                            </td>

                                            <td style="text-align:center;">
                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger" Width="80px"  />
                                            </td>

                                            <td style="text-align:left;">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success" Width="80px" />
                                            </td>                          
                                        </tr>
                                    </table>
                                </center>

                           <%-- </asp:View>
                        </asp:MultiView>--%>
                </div>

                </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSubmit" />
                        <asp:PostBackTrigger ControlID="rdoSelectdJob" />
                        <asp:PostBackTrigger ControlID="txtJobSearch" />
                        <asp:PostBackTrigger ControlID="ddlJob" />
                        <asp:PostBackTrigger ControlID="rdoSelectdCo" />
                        <asp:PostBackTrigger ControlID="txtCoSearch" />
                        <asp:PostBackTrigger ControlID="ddlCo" />
                        <asp:PostBackTrigger ControlID="rdoSelectdOther" />
                        <asp:PostBackTrigger ControlID="btnAdd" />
                        <asp:PostBackTrigger ControlID="btnSave" />
                        <asp:PostBackTrigger ControlID="btnDelete" />
                        <asp:PostBackTrigger ControlID="txtAddItem" />
                        <asp:PostBackTrigger ControlID="ddlsuffix" />
                        <asp:PostBackTrigger ControlID="ddlAddLot" />
                        <asp:PostBackTrigger ControlID="GridReqMove" />
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

<!-- Custom Theme JavaScript -->
<script src="dist/js/sb-admin-2.js" type="text/javascript"></script>

<!-- DataTables JavaScript -->
<script src="bower_components/DataTables/media/js/jquery.dataTables.min.js" type="text/javascript"></script>
<script src="bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js" type="text/javascript"></script>

<script src="js/typeahead.bundle.js" type="text/javascript"></script>

<script type="text/javascript">
    function SelectAllCheckboxes1(chk) {
        $('#<%=GridReqMove.ClientID%>').find("input:checkbox").each(function () {
            if (this != chk) { 
            this.checked = chk.checked; }
        });
    }
</script>

<%--<script type="text/javascript">
    function Submit() {
        javascript: return confirm('Do you want to submit?'); 
        showProgress();
    }
</script>--%>

<script type="text/javascript">

    var substringMatcher = function (strs) {
        return function findMatches(q, cb) {
            var matches, substringRegex;
            // an array that will be populated with substring matches
            matches = [];
            // regex used to determine if a string contains the substring `q`
            substrRegex = new RegExp(q, 'i');
            // iterate through the pool of strings and for any string that
            // contains the substring `q`, add it to the `matches` array
            $.each(strs, function (i, str) {
                if (substrRegex.test(str)) {
                    matches.push(str);
                }
            });
            cb(matches);
        };
    };

    var arr_job = [];
    var arr_co = [];
    var arr_item = [];
    
    $('select#<%=ddlJob.ClientID%>').find('option').each(function () {
        arr_job.push($(this).val());
    });

    $('#div-job .typeahead').typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    },
        {
            name: 'arr_job',
            source: substringMatcher(arr_job)
        }).bind("typeahead:selected", function (obj, datum, name) {
            __doPostBack('#<%=txtJobSearch.ClientID%>', null);
        });


    $('select#<%=ddlco.ClientID%>').find('option').each(function () {
        arr_co.push($(this).val());
    });

    $('#div-co .typeahead').typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    },
    {
        name: 'arr_co',
        source: substringMatcher(arr_co)
    }).bind("typeahead:selected", function (obj, datum, name) {
        __doPostBack('#<%=txtCoSearch.ClientID%>', null);
    });


    $('select#<%=ddlItem.ClientID%>').find('option').each(function () {
        arr_item.push($(this).val());
    });

    $('#div-item .typeahead').typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    },
    {
        name: 'arr_item',
        source: substringMatcher(arr_item)
    }).bind("typeahead:selected", function (obj, datum, name) {
        __doPostBack('#<%=txtAddItem.ClientID%>', null);
    });

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

