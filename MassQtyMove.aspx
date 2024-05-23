<%@ Page Title="WMS Mass Quantity Move" Language="VB" MasterPageFile="~/SiteMasterNew.master" AutoEventWireup="false" CodeFile="MassQtyMove.aspx.vb" Inherits="MassQtyMove" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <style type="text/css">
        .width-div { width: 50%;}
        .hide { display: none; }
        .fixwidth  { 
            min-width: 202px;
            max-width: 202px;
        }
    </style>

    <script>

        function ShowSweetAlert(type, msg, icon) {
            Swal.fire(
                type,
                msg,
                icon
            )

        }

    </script>

    <script language="javascript" type="text/javascript">
//        var oldgridSelectedColor;

//        function setMouseOverColor(element) {
//            oldgridSelectedColor = element.style.backgroundColor;
//            element.style.backgroundColor = '#A1DCF2';
//            element.style.cursor = 'hand';
//            element.style.textDecoration = 'underline';
//        }

//        function setMouseOutColor(element) {
//            element.style.backgroundColor = oldgridSelectedColor;
//            element.style.textDecoration = 'none';
        //        }


        function GetSelectedRow(lnk) {

            var grid = document.getElementById("<%=GridView1.ClientID%>");
            
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex;
            var RowID = grid.rows[rowIndex].cells[9].getElementsByTagName("span")[0].innerHTML;
            document.getElementById('<%=RowPointerSelected.ClientID%>').value = RowID;

            for (i = 1; i < grid.rows.length; i++) {

                if (grid.rows[i].classList.contains('selected_row')) {

                    if (i !== rowIndex) {
                        grid.rows[i].classList.remove('selected_row');
                    } else {

                        if (RowID == grid.rows[i].cells[9].getElementsByTagName("span")[0].innerHTML) {
                            grid.rows[i].classList.remove('selected_row');
                            document.getElementById('<%=RowPointerSelected.ClientID%>').value = "";
                            $("#<%=btncopy.ClientID%>").prop('disabled', true);
                            $("#<%=btnedit.ClientID%>").prop('disabled', true);
                            $("#<%=btndelete.ClientID%>").prop('disabled', true);
                            
                        }
                    }

                } else {

                    if (i == rowIndex) {
                        grid.rows[i].classList.add('selected_row');
                        $("#<%=btncopy.ClientID%>").prop('disabled', false);
                        $("#<%=btnedit.ClientID%>").prop('disabled', false);
                        $("#<%=btndelete.ClientID%>").prop('disabled', false);

                    }
                }
            }

            return false;
        }




        $(function () {
            $("#<%=btnedit.ClientID%>").click(function () {

                document.getElementById('<%=CommandName.ClientID%>').value = "Edit";
            });

        })

        $(function () {
            $("#<%=btnCopy.ClientID%>").click(function () {

                document.getElementById('<%=CommandName.ClientID%>').value = "Copy";
            });

        })

        $(function () {
            $("#<%=btnadd.ClientID%>").click(function () {

                document.getElementById('<%=CommandName.ClientID%>').value = "Add";
            });

        })

//        $(document).ready(function () {
//            $("#<%= GridView1.ClientID %> tr").click(function () {
//                var rowNumber = $(this).attr("rowNumber");
//                var lblRowPointer = $(this).closest('tr').find('span[id*="lblRowPointer"]').text();
//                document.getElementById('<%=RowPointerSelected.ClientID%>').value = lblRowPointer;
//                //alert(document.getElementById('<%=RowPointerSelected.ClientID%>').value);
//                //return false;
//            })
//        });

//        $(function () {
//            $("[id*=GridView1] td").bind("click", function () {
//                var row = $(this).parent();

//                $("[id*=GridView1] tr").each(function () {
//                    if ($(this)[0] != row[0]) {
//                        $("td", this).removeClass("selected_row");
//                    }
//                });

//                $("td", row).each(function () {
//                    if (!$(this).hasClass("selected_row")) {
//                        $(this).addClass("selected_row");
//                    } else {
//                        $(this).removeClass("selected_row");

//                    }
//                });
//            });
//        });


        function openModal(ItemModel, ItemDescModel, LotTrakcedModel, UmModel, QtyOnHandModel, QtyModel, VendLotModel, LotModel, DisplayType) {
            
            document.getElementById("ItemModel").innerHTML = ItemModel;
            document.getElementById("ItemDescModel").innerHTML = ItemDescModel;
            document.getElementById("UmModel").innerHTML = UmModel;
            document.getElementById("QtyOnHandModel").innerHTML = QtyOnHandModel;
            document.getElementById('<%=txtQty.ClientID%>').value = QtyModel;
            document.getElementById("VendLotModel").innerHTML = VendLotModel;

            if (LotTrakcedModel == "0") {
                document.getElementById('<%=ddllot.ClientID%>').disabled = true;

            } else {
                document.getElementById('<%=ddllot.ClientID%>').disabled = false;

                //alert(DisplayType);

                if (DisplayType == "Click") {
                    document.getElementById('<%=txtQty.ClientID%>').value = "";
                    document.getElementById("VendLotModel").innerHTML = "";

                } else if (DisplayType == "Edit") {

                    var RowID = document.getElementById('<%=RowPointerSelected.ClientID%>').value;

                    if (RowID == "") {
                        return false;
                    }

                    document.getElementById('<%=ddllot.ClientID%>').value = LotModel;
                    //$("#<%=ddllot.ClientID%>").val(LotModel);
                    
                }

            }

                //$(".modal-backdrop").remove();
                //$('#AddModel').modal('show');
                $('#AddModel').modal({
                    backdrop: 'static',
                    keyboard: true,
                    show: true
                });
        };

        function hideModel() {
            $('#AddModel').fadeOut();
            $('.modal-backdrop').fadeOut();
            $('.modal-open').css({ 'overflow': 'visible' });
            $("#<%=btnCloseModel.ClientID%>").prop('disabled', false);
        };

        $(function () {
            $("#<%=btnConfirmModel.ClientID%>").click(function () {

                $("#<%=btnCloseModel.ClientID%>").prop('disabled', true);
            });

        })

        

//        function disableclosemodel() {
//            $("#<%=btnCloseModel.ClientID%>").prop('disabled', true);
//        }

//        function enableclosemodel() {
//            document.getElementById('<%=btnCloseModel.ClientID%>').disabled = false;
//        }


    </script>
        

<style type="text/css">
    .divgrid
    {
        max-height: 600px;
        overflow-y: scroll;
        width: 100%;
        font-size: 11pt;
    }
    td
    {
        cursor: pointer;
    }
    .selected_row
    {
        background-color: #A1DCF2;
    }
</style>

    <%--<script src="js/typeahead.bundle.js" type="text/javascript"></script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<br />

        <div class="se-pre-con"></div>

            <div class="col-lg-12 mt-4">
            <div class="card border-primary">
                <div class="card-header py-12">
                    <div class="row">
                        <div class="col-md-9">
                            <h6 class="m-0"><a href="Menu.aspx">Home</a><span class="mx-2 mb-0">/</span> <strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>
                    </div>
                  
                </div>

                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

               
                    <div class="card-body">

                        <div class="row pt-2">
                            <div class="col-md-9"></div>

                            <div class="col-md-3 float-right">
                                <asp:Button ID="btnprintsticker" runat="server" class="btn btn-warning btn-sm float-right mx-2 mb-0"  Text="Print Sticker" UseSubmitBehavior="false" />
                                <asp:Button ID="btnprocess" runat="server" class="btn btn-success btn-sm float-right mx-2 mb-0" Text="Move" UseSubmitBehavior="false" />
                                
                            </div>
                        </div>
                         <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" >
                         <ContentTemplate>

                            <div class="row align-items-center pt-2">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <div class="row align-items-center">
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>From Site : </span>
                                                </div>
                                                <div class="col-sm-2 width-div">
                                                    <asp:TextBox ID="txtformsite" runat="server" class="form-control form-control-sm mb-1" ReadOnly="True" required></asp:TextBox>
                                                </div>
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>To Site : </span>
                                                </div>
                                                <div class="col-sm-2 width-div">
                                                    <asp:TextBox ID="txttosite" runat="server" class="form-control form-control-sm mb-1" ReadOnly="True" required></asp:TextBox>
                                                </div>
                                            </div>
                                        </td>                                
                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="row align-items-center">
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>From Warehouse : </span>
                                                </div>
                                                <div class="col-sm-2 width-div">
                                                    <asp:DropDownList ID="ddlfromwhse" runat="server" AutoPostBack="true" EnableViewState="true" class="form-control form-control-sm mb-1" required></asp:DropDownList>
                                                </div>
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>To Warehouse : </span>
                                                </div>
                                                <div class="col-sm-2 width-div">
                                                    <asp:DropDownList ID="ddltowhse" runat="server" AutoPostBack="true" EnableViewState="true" class="form-control form-control-sm mb-1" required></asp:DropDownList>
                                                </div>
                                            </div>
                                        </td>                                
                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="row align-items-center">
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>From Location : </span>
                                                </div>
                                                <div class="col-sm-2 width-div">
                                                    <asp:DropDownList ID="ddlfromloc" runat="server" AutoPostBack="true" EnableViewState="true" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                                </div>
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>To Location : </span>
                                                </div>
                                                <div class="col-sm-2 width-div">
                                                    <asp:DropDownList ID="ddltoloc" runat="server" AutoPostBack="true" EnableViewState="true" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </td>                                
                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="row align-items-center">
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>Transaction Date : </span>
                                                </div>
                                                <div class="col-sm-2 width-div">
                                                    <asp:TextBox ID="txttransdate" runat="server" class="form-control form-control-sm mb-1 datepicker" AutoComplete="off"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>Ref. Transaction Date : </span>
                                                </div>
                                                <div class="col-sm-2 width-div">
                                                    <asp:TextBox ID="txtreftransdate" runat="server" class="form-control form-control-sm mb-1 datepicker" AutoComplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </td>                                
                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="row align-items-center">
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>Category Code : </span>
                                                </div>
                                                <div class="col-sm-2 width-div">
                                                    <asp:DropDownList ID="ddlcategorycode" runat="server" class="form-control form-control-sm mb-1" required></asp:DropDownList>
                                                </div>
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>Document No. : </span>
                                                </div>
                                                <div class="col-sm-2 width-div">
                                                    <asp:TextBox ID="txtdocumentnum" runat="server" class="form-control form-control-sm mb-1" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>
                                        </td>                                
                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="row align-items-center">
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>Job : </span>
                                                </div>
                                            
                                                <div id="div-job" class="col-sm-2 width-div">                                   
                                                    <asp:DropDownList ID="ddljob" runat="server" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                                     <%--<asp:TextBox ID="txtjob" runat="server" CssClass="typeahead form-control form-control-sm txt-margin" AutoPostBack="True" MaxLength="10"></asp:TextBox>--%>
                                                </div>
                                                                                    
                                            </div>
                                        </td>                                
                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="row align-items-center">
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>Remark : </span>
                                                </div>
                                                <div class="col-sm-3 width-div">
                                                    <asp:TextBox ID="txtremark" runat="server" class="form-control form-control-sm mb-1" TextMode="MultiLine" rows="3"></asp:TextBox>
                                                    <asp:RegularExpressionValidator runat="server" ID="valInput"
                                                        ControlToValidate="txtremark"
                                                        ValidationExpression="^[\s\S]{0,255}$"
                                                        ErrorMessage="Data Over 255"
                                                        ForeColor="Red"
                                                        Display="Dynamic"></asp:RegularExpressionValidator>
                                                </div>

                                                                                    
                                            </div>
                                        </td>                                
                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="row align-items-center">
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>Search Item : </span>
                                                </div>
                                                <div class="col-sm-3 width-div">
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtsearchitem" runat="server" AutoComplete="off" class="form-control form-control-sm" ></asp:TextBox>
                                                
                                                            <div class="input-group-append">
                                                                <asp:LinkButton runat="server" ID="btnSearch" class="btn btn-primary">
                                                                    <span class="fa fa-search"></span>

                                                                </asp:LinkButton>
                                                                
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>                                        
                                            </div>
                                        </td>                                
                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="row align-items-center">
                                                <div class="col-sm-3 text-right width-div">
                                                    <span>Item : </span>
                                                </div>
                                                <div class="col-sm-3 width-div">
                                                    <asp:DropDownList ID="ddlitem" runat="server" AutoPostBack="true" class="form-control form-control-sm mb-1 selectpicker fixwidth" data-live-search="true"></asp:DropDownList>
                                                
                                                </div>
                                                                                
                                            </div>
                                        </td>                                
                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="row align-items-center">
                                                <div class="col-sm-3 text-right width-div">
                                                    <span></span>
                                                </div>
                                                <div class="col-sm-1 width-div">
                                                    <asp:Button ID="btnadd" runat="server" OnClick="Display" class="btn btn-primary btn-sm mb-0" Text="Add" UseSubmitBehavior="false" />
                                                </div>
                                                                                
                                            </div>
                                        </td>                                
                                    </tr>

                               </table>
                            </div>

                            <div class="row mb-4">
                                &nbsp;<asp:Button ID="btncopy" runat="server" class="btn btn-info btn-sm mx-2 mb-0"  Text="Copy" UseSubmitBehavior="false" disabled />
                                <asp:Button ID="btnedit" runat="server" class="btn btn-info btn-sm mx-2 mb-0"  Text="Edit" UseSubmitBehavior="false" disabled />
                                <asp:Button ID="btndelete" runat="server" class="btn btn-info btn-sm mx-2 mb-0"  Text="Delete" UseSubmitBehavior="false" disabled />
                                <asp:HiddenField ID="sItem" runat="server" />
                                <asp:HiddenField ID="sItemLotTracked" runat="server" Value="0" />
                                <asp:HiddenField ID="sItemDesc" runat="server" Value="" />
                                <asp:HiddenField ID="sItemUM" runat="server" Value="" />
                                <asp:HiddenField ID="sLot" runat="server" Value="" />
                                <asp:HiddenField ID="sQty" runat="server" Value="" />
                                <asp:HiddenField ID="sVendLot" runat="server" Value="" />
                                <asp:HiddenField ID="sItemQtyOnHand" runat="server" Value="" />
                                <asp:HiddenField ID="RowPointerSelected" runat="server" />
                                <asp:HiddenField ID="CommandName" runat="server" />
                                
                                
                            </div>

                            <div class="divgrid">

                                <div class="table-responsive">
                                 <asp:GridView ID="GridView1" CssClass="table table-bordered" 
                                            runat="server" AutoGenerateColumns="false"
                                            EnableModelValidation="True"
                                            DataKeyNames="RowPointer">
                                    
                                    <Columns>
                                        
                                        <%--<asp:TemplateField >
                                             <ItemTemplate>
                                                  <asp:LinkButton ID="LinkSelect" runat="server" Text="Select" OnClientClick="return GetSelectedRow(this);" CausesValidation="false"></asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>--%>
                                        <%--<asp:ButtonField Text = "Select" CommandName = "Select" />--%>
                                        <asp:TemplateField HeaderText="" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                             <ItemTemplate>
                                                  <asp:LinkButton ID="LinkSelect" runat="server" Text="Select" OnClientClick="return GetSelectedRow(this);" CausesValidation="false"></asp:LinkButton>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item") %>'></asp:Label>                
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Description" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblItemDesc" runat="server" Text='<%# Eval("ItemDescription") %>'></asp:Label>                
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Lot" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblLot" runat="server" Text='<%# Eval("Lot") %>'></asp:Label>                
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Vendor Lot" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblVendLot" runat="server" Text='<%# Eval("Uf_ppccmvtran_Vend_lot") %>'></asp:Label>                
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quantity" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>                
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="UM" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblUM" runat="server" Text='<%# Eval("UM") %>'></asp:Label>                
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Status" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblStat" runat="server" Text='<%# Eval("ErrMsg") %>'></asp:Label>                
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="RowPointer" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblRowPointer" runat="server" Text='<%# Eval("RowPointer") %>'></asp:Label>                
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="hide" />
                                            <ItemStyle CssClass="hide" />
                                        </asp:TemplateField>
                                    </Columns>
                                 </asp:GridView>
                            </div>
                            
                            </div>
                       
                            

                            <script type="text/javascript">
                                $(function () {

                                    ApplySelectPicker();
                                   
                                });
                                //On UpdatePanel Refresh
                                var prm = Sys.WebForms.PageRequestManager.getInstance();
                                if (prm != null) {
                                    prm.add_endRequest(function (sender, e) {
                                        if (sender._postBackSettings.panelsToUpdate != null) {
                                            ApplySelectPicker();
                                        }
                                    });
                                };
                                function ApplySelectPicker() {
                                    $('[id*=ddlitem]').selectpicker({
                                        size: 'auto',
                                        width: '202px'
                                        
                                    });

                                    $('[id*=ddlfromloc]').selectpicker();
                                    $('[id*=ddltoloc]').selectpicker();
                                    $('[id*=ddljob]').selectpicker();
//                                    $('[id*=ddlitem]').selectpicker();

                                };
                            </script>

                         </ContentTemplate>
                         <Triggers>
                                <asp:PostBackTrigger ControlID="ddlfromwhse" />
                                <asp:PostBackTrigger ControlID="ddltowhse" />
                                <asp:PostBackTrigger ControlID="txtsearchitem" />
                                <asp:AsyncPostBackTrigger ControlID="ddlitem" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlfromloc" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddltoloc" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="PageIndexChanging" />
                                <%--<asp:AsyncPostBackTrigger ControlID="btnedit" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btncopy" EventName="Click" />--%>
                         </Triggers>
                         </asp:UpdatePanel>

                    </div>
               
                    <div class="container">
                    <div class="modal fade" id="AddModel" role="dialog" tabindex="-1" aria-hidden="true">
                        <div class="modal-dialog">
    
                            <!-- Modal content-->
                            <div class="modal-content">
                            <div class="modal-header">
                           
                                <h4>&nbsp;Add Quantity</h4>
                                <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>    
                            </div>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                    <div class="modal-body">
                                    <div class="row">
                                            <p class="col-sm-4 lebel-text">Item :</p>
                                        <div class="col-sm-8">
                                            <p id="ItemModel" class="lebel-text"></p>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <p class="col-sm-4 lebel-text">Description :</p>
                                        <div class="col-sm-8">
                                            <p id="ItemDescModel" class="lebel-text"></p>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <p class="col-sm-4 lebel-text">Select Lot :</p>
                                        <div class="col-sm-8">
                                                <asp:DropDownList ID="ddllot" runat="server" class="form-control form-control-sm" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddllot_SelectedIndexChanged"></asp:DropDownList>
                                    
                                        </div>
                                    </div>

                                    <div class="row">
                                        <p class="col-sm-4 lebel-text">Qty :</p>
                                        <div class="col-sm-8">
                                                <asp:TextBox ID="txtQty" runat="server" class="form-control form-control-sm" AutoComplete="off"></asp:TextBox>
                                    
                                        </div>
                                    </div>

                                    <div class="row">
                                        <p class="col-sm-4 lebel-text">UM :</p>
                                        <div class="col-sm-8">
                                                <p id="UmModel" class="lebel-text"></p>
                                    
                                        </div>
                                    </div>

                                    <div class="row">
                                        <p class="col-sm-4 lebel-text">Vendor Lot :</p>
                                        <div class="col-sm-8">
                                                <p id="VendLotModel" class="lebel-text"></p>
                                    
                                        </div>
                                    </div>

                                    <div class="row">
                                        <p class="col-sm-4 lebel-text">Qty On Hand :</p>
                                        <div class="col-sm-8">
                                                <p id="QtyOnHandModel" class="lebel-text"></p>
                                    
                                        </div>
                                    </div>

                                </div>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="modal-footer">

                                <asp:Button ID="btnCloseModel" runat="server" Text="Back" class="btn btn-outline-danger btn-sm" data-dismiss="modal" UseSubmitBehavior="false" />
                                <asp:Button ID="btnConfirmModel" runat="server" Text="Confirm" class="btn btn-outline-success btn-sm" UseSubmitBehavior="false" />
                            
                                </div>
                            </div>
      
                        </div>
                    </div>
                    </div>
                    
            </div>   

        </div>

<%--<script type="text/javascript">
    $(document).ready(function () {

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
        $('select#<%=ddljob.ClientID%>').find('option').each(function () {
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
    __doPostBack('#<%=txtjob.ClientID%>', null);
});
    });
</script>--%>

        

</asp:Content>


