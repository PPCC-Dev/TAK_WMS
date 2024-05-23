<%@ Page Title="WMS Print Sticker Barcode" Language="VB" MasterPageFile="~/SiteMasterNew.master" AutoEventWireup="false" CodeFile="PrintStickerBarcode.aspx.vb" Inherits="PrintStickerBarcode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .width-div { width: 50%;}
        .hide { display: none; }        
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

     <script type="text/javascript">
         $(document).ready(function () {

             var params = new URLSearchParams(window.location.search);

             if (params.get('Print') === "GRN") {

                 document.getElementById("divdocnum").style.display = "none";
                 document.getElementById("divunposted").style.display = "none";
                 document.getElementById("divgrn").style.display = "block";


             } else if (params.get('Print') === "Misc. Receipt") {

                 document.getElementById("divdocnum").style.display = "block";
                 document.getElementById("divunposted").style.display = "none";
                 document.getElementById("divgrn").style.display = "none";

             } else if (params.get('Print') === "Qty Move") {

                 document.getElementById("divdocnum").style.display = "block";
                 document.getElementById("divunposted").style.display = "none";
                 document.getElementById("divgrn").style.display = "none";

             } else if (params.get('Print') === "RMA") {

                 document.getElementById("divdocnum").style.display = "block";
                 document.getElementById("divunposted").style.display = "none";
                 document.getElementById("divgrn").style.display = "none";

             } else if (params.get('Print') === "Unposted Job") {

                 document.getElementById("divdocnum").style.display = "block";
                 document.getElementById("divunposted").style.display = "block";
                 document.getElementById("divgrn").style.display = "none";

                

             }

         });

     </script>

     <script type="text/javascript">
         $(function () {
             $("#<%=chkeditqty.ClientID%>").change(function () {
                 var status = this.checked;
                 if (status) {
                     $("#<%=txtqty.ClientID%>").prop("disabled", false);

                     var workmanyway = document.getElementById('<%=rdoworkmanyway.ClientID%>').checked;

                     if (workmanyway === true) {

                         document.getElementById("divGridWorkManyWay").style.display = "block";

                     } else {

                         document.getElementById("divGridWorkManyWay").style.display = "none";

                     }

                 }
                 else {
                     $("#<%=txtqty.ClientID%>").prop("disabled", true);

                     document.getElementById("divGridWorkManyWay").style.display = "none";

                 }
             })
         })

      </script>

      <script type="text/javascript">
        $(function () {
            $("#<%=txtstartjobtran.ClientID%>").change(function () {
                var startjobtran = $("#<%=txtstartjobtran.ClientID%>").val();
                var endjobtran = $("#<%=txtendjobtran.ClientID%>").val();

                $("#<%=chklastunposted.ClientID%>").prop('disabled', true);

                if (startjobtran === endjobtran) {
                    $("#<%=chklastunposted.ClientID%>").prop('disabled', false);
                } else {
                    $("#<%=chklastunposted.ClientID%>").prop('disabled', true);
                    //$("#<%=chklastunposted.ClientID%>").prop('checked', false);
                }
            })

        })

        $(function () {
            $("#<%=txtendjobtran.ClientID%>").change(function () {
                var startjobtran = $("#<%=txtstartjobtran.ClientID%>").val();
                var endjobtran = $("#<%=txtendjobtran.ClientID%>").val();

                $("#<%=chklastunposted.ClientID%>").prop('disabled', true);

                if (startjobtran === endjobtran) {
                    $("#<%=chklastunposted.ClientID%>").prop('disabled', false);
                    //$("#<%=chklastunposted.ClientID%>").prop('checked', true);
                } else {
                    $("#<%=chklastunposted.ClientID%>").prop('disabled', true);
                    //$("#<%=chklastunposted.ClientID%>").prop('checked', false);
                }
            })

        })

        $(function () {
            $("#<%=btnpreview.ClientID%>").click(function () {

                var params = new URLSearchParams(window.location.search);

                if (params.get('Print') === "Unposted Job") {

                    $("#<%=ddlstartjob.ClientID%>").prop("required", true);
                    $("#<%=ddlendjob.ClientID%>").prop("required", true);
                    $("#<%=txtstartsuffix.ClientID%>").prop("required", true);
                    $("#<%=txtendsuffix.ClientID%>").prop("required", true);

                }
            });

        })

        $(function () {
            $("#<%=btnprint.ClientID%>").click(function () {

                var params = new URLSearchParams(window.location.search);

                if (params.get('Print') === "Unposted Job") {

                    $("#<%=ddlstartjob.ClientID%>").prop("required", true);
                    $("#<%=ddlendjob.ClientID%>").prop("required", true);
                    $("#<%=txtstartsuffix.ClientID%>").prop("required", true);
                    $("#<%=txtendsuffix.ClientID%>").prop("required", true);

                }
            });

        })

        $(function () {
            $("#<%=rdoworkmanyway.ClientID%>").change(function () {

                var status = this.checked;
                if (status) {

                    var chkeditqty = document.getElementById('<%=chkeditqty.ClientID%>').checked;

                    if (chkeditqty === true) {

                        document.getElementById("divGridWorkManyWay").style.display = "block";

                    } else {

                        document.getElementById("divGridWorkManyWay").style.display = "none";
                        $("#<%=chkeditqty.ClientID%>").prop('checked', false);
                        $("#<%=txtqty.ClientID%>").prop("disabled", true);
                    }

                }

            });
        })

        $(function () {
            $("#<%=rdoworkoneway.ClientID%>").change(function () {

                var status = this.checked;
                if (status) {
                    document.getElementById("divGridWorkManyWay").style.display = "none";
                    $("#<%=chkeditqty.ClientID%>").prop('checked', false);
                    $("#<%=txtqty.ClientID%>").prop("disabled", true);
                }

            });
        })
 
      </script>

      

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

            <div class="card-body">
               
                <div id="divmain" class="row align-items-center pt-2">
                    <table width="100%">
                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Condition : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:DropDownList ID="ddlcondition" runat="server" class="form-control form-control-sm mb-1" disabled></asp:DropDownList>
                                    </div>     
                                </div>
                            </td>                                
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Barcode Date : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:TextBox ID="txtbarcodedate" runat="server" class="form-control form-control-sm mb-1 datepicker" AutoComplete="off"></asp:TextBox>
                                    </div>                                      
                                </div>
                            </td>                                
                        </tr>
                    </table>
                </div>

                <div id="divdocnum" class="row align-items-center pt-2"> 
                    <table width="100%">
                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Document No. : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:DropDownList ID="ddlstartdocnum" runat="server" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                    </div>
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:DropDownList ID="ddlenddocnum" runat="server" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </td>                                
                        </tr>
                    </table>
                </div>

                <div id="divunposted" class="row align-items-center pt-2">
                    <table width="100%">
                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <spen></spen>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chklastunposted" runat="server" class="form-check-input mb-1" /><spen>&nbsp;Last Unposted</spen>

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
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:DropDownList ID="ddlstartjob" runat="server" AutoPostBack="true" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                        
                                    </div>
                                    
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div"> 
                                        <asp:DropDownList ID="ddlendjob" runat="server" AutoPostBack="true" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Suffix : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:TextBox ID="txtstartsuffix" runat="server" class="form-control form-control-sm mb-1" Text="0000" AutoComplete="off" required></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:TextBox ID="txtendsuffix" runat="server" class="form-control form-control-sm mb-1" Text="0000" AutoComplete="off" required></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Job Transaction : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:TextBox ID="txtstartjobtran" runat="server" class="form-control form-control-sm mb-1" AutoComplete="off"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:TextBox ID="txtendjobtran" runat="server" class="form-control form-control-sm mb-1" AutoComplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Core Weight : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:TextBox ID="txtcoreweight" runat="server" class="form-control form-control-sm mb-1" Text="0.000" AutoComplete="off"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2 text-right width-div">
                                        <span>Production Emp. : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:TextBox ID="txtproductionemp" runat="server" class="form-control form-control-sm mb-1" AutoComplete="off"></asp:TextBox>
                                        <%--<asp:DropDownList ID="ddlproductionemp" runat="server" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>--%>
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">                                    
                                    <div class="col-sm-3 text-right width-div">
                                        <asp:CheckBox ID="chkeditqty" runat="server" class="form-check-input mb-1" /><spen>&nbsp;แก้ไข Qty : </spen>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:TextBox ID="txtqty" runat="server" class="form-control form-control-sm mb-1" Text="0.00" AutoComplete="off" disabled></asp:TextBox>
                                    </div>
                                        
                                    <div class="col-sm-2 text-right width-div">
                                        <span>QC Emp. : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:TextBox ID="txtqcemp" runat="server" class="form-control form-control-sm mb-1" AutoComplete="off"></asp:TextBox>
                                        <%--<asp:DropDownList ID="ddlqcemp" runat="server" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>--%>
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <spen>Date : </spen>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:TextBox ID="txtstartdate" runat="server" class="form-control form-control-sm mb-1 datepicker" AutoComplete="off"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:TextBox ID="txtenddate" runat="server" class="form-control form-control-sm mb-1 datepicker" AutoComplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <spen</spen>
                                    </div>
                                    <div class="col-sm-6 width-div">                                        
                                        <div class="form-check form-check-inline">
                                            <asp:RadioButton ID="rdoworkoneway"  class="form-check-input" runat="server" Checked="true" GroupName="Work"  />
                                            <label class="form-check-label" for="rdoworkoneway">งานทางเดียว</label>
                                        </div>
                                        <div class="form-check form-check-inline">
                                            <asp:RadioButton ID="rdoworkmanyway"  class="form-check-input" runat="server" GroupName="Work" />
                                            <label class="form-check-label" for="rdoworkmanyway">งานหลายทาง</label>
                                        </div>

                                    </div>
                                    
                                </div>
                            </td>
                        </tr>
                        
                    </table>

                    <div id="divGridWorkManyWay" class="table-responsive" style="display:none;">
                        <asp:GridView ID="GridView1" CssClass="table table-bordered" 
                         runat="server" AutoGenerateColumns="false"
                         DataKeyNames="RowPointer">
                            <Columns>
                                <asp:TemplateField HeaderText="Job" ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                            <asp:Label ID="lblJob" runat="server" Text='<%# Eval("job") %>'></asp:Label>                
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Lot" ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                            <asp:Label ID="lblLot" runat="server" Text='<%# Eval("lot") %>'></asp:Label>                
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Suffix" ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                            <asp:Label ID="lblSuffix" runat="server" Text='<%# Eval("suffix") %>'></asp:Label>                
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Row" ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                            <asp:Label ID="lblRow" runat="server" Text='<%# Eval("output_row") %>'></asp:Label>                
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quantity" ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                            <%--<asp:Label ID="lblQty" runat="server" Text='<%# Eval("chang_qty") %>'></asp:Label>--%>
                                            <asp:TextBox ID="txtQty" runat="server" class="form-control form-control-sm mb-1" Text='<%# Eval("chang_qty") %>'></asp:TextBox>
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

                    <asp:HiddenField ID="vMultipleWays" runat="server" />
                    <asp:HiddenField ID="vBarcodeType" runat="server" />
                    <asp:HiddenField ID="vBGTaskName" runat="server" />
                </div> 

                <div id="divgrn" class="row align-items-center pt-2">
                    <table width="100%">
                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    
                                    <div class="col-sm-3 text-right width-div">
                                        <span>GRN : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:DropDownList ID="ddlstartgrn" runat="server" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                        
                                    </div>
                                    
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:DropDownList ID="ddlendgrn" runat="server" class="form-control form-control-sm mb-1 selectpicker txt-margin" data-live-search="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>GRN Line : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:TextBox ID="txtstartgrnline" runat="server" class="form-control form-control-sm mb-1" Text="0000" AutoComplete="off"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:TextBox ID="txtendgrnline" runat="server" class="form-control form-control-sm mb-1" Text="9999" AutoComplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    
                                    <div class="col-sm-3 text-right width-div">
                                        <span>PO : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:DropDownList ID="ddlstartpo" runat="server" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                        
                                    </div>
                                    
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:DropDownList ID="ddlendpo" runat="server" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>PO Line : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:TextBox ID="txtstartpoline" runat="server" class="form-control form-control-sm mb-1" Text="0000" AutoComplete="off"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:TextBox ID="txtendpoline" runat="server" class="form-control form-control-sm mb-1" Text="9999" AutoComplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Vendor : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:DropDownList ID="ddlstartvendnum" runat="server" class="form-control form-control-sm mb-1"></asp:DropDownList>
                                        
                                    </div>
                                    
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:DropDownList ID="ddlendvendnum" runat="server" class="form-control form-control-sm mb-1"></asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>

                    </table>
                </div>

                <div id="divbutton" class="d-flex justify-content-center pt-2">
                    <div class="form-row">
                        <div class="form-group col-md-12 justify-content-center">
                            <asp:Button ID="btnback" runat="server" class="btn btn-primary btn-sm mx-2 mb-0" Text="Back"  UseSubmitBehavior="False" />

                            <asp:Button ID="btnpreview" runat="server" class="btn btn-warning btn-sm mx-2 mb-0" Text="Preview" UseSubmitBehavior="true" />

                            <asp:Button ID="btnprint" runat="server" class="btn btn-success btn-sm mx-2 mb-0" Text="Print" UseSubmitBehavior="true" />
                        </div>
                    </div>
               </div>

                
            </div>
        </div>
     </div>

</asp:Content>

