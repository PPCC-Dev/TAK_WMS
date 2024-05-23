<%@ Page Title="WMS RePrint Sticker Barcode" Language="VB" MasterPageFile="~/SiteMasterNew.master" AutoEventWireup="false" CodeFile="RePrintStickerBarcode.aspx.vb" Inherits="RePrintStickerBarcode" %>

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
            $("#<%=chkeditqty.ClientID%>").prop("disabled", true);

        });

        $(document).ready(function () {

            var params = new URLSearchParams(window.location.search);

            if (params.get('Print') === "GRN") {

                document.getElementById("divdocnum").style.display = "none";
                document.getElementById("divunposted").style.display = "none";
                document.getElementById("divgrn").style.display = "block";
                document.getElementById("divbarcode").style.display = "block";


            } else if (params.get('Print') === "Misc. Receipt") {

                document.getElementById("divdocnum").style.display = "block";
                document.getElementById("divunposted").style.display = "none";
                document.getElementById("divgrn").style.display = "none";
                document.getElementById("divbarcode").style.display = "block";

            } else if (params.get('Print') === "Qty Move") {

                document.getElementById("divdocnum").style.display = "block";
                document.getElementById("divunposted").style.display = "none";
                document.getElementById("divgrn").style.display = "none";
                document.getElementById("divbarcode").style.display = "block";

            } else if (params.get('Print') === "RMA") {

                document.getElementById("divdocnum").style.display = "block";
                document.getElementById("divunposted").style.display = "none";
                document.getElementById("divgrn").style.display = "none";
                document.getElementById("divbarcode").style.display = "block";

            } else if (params.get('Print') === "Unposted Job") {

                document.getElementById("divdocnum").style.display = "block";
                document.getElementById("divunposted").style.display = "block";
                document.getElementById("divgrn").style.display = "none";
                document.getElementById("divbarcode").style.display = "block";


            }

        });

     </script>


    <script type="text/javascript">

        $(function () {
            $("#<%=chkeditqty.ClientID%>").change(function () {
                var status = this.checked;
                if (status)
                    $("#<%=txtqty.ClientID%>").prop("disabled", false);
                else 
                    $("#<%=txtqty.ClientID%>").prop("disabled", true);

            })
        })

        $(function () {
            $("#<%=ddlstartbarcode.ClientID%>").change(function () {

                if ($("#<%=ddlstartbarcode.ClientID%>").val() === $("#<%=ddlendbarcode.ClientID%>").val() 
                    && $("#<%=ddlstartbarcode.ClientID%>").val() !== null) {
                    $("#<%=chkeditqty.ClientID%>").prop('disabled', false);
                } else {
                    $("#<%=chkeditqty.ClientID%>").prop('disabled', true);
                }
            })

        })

        $(function () {
            $("#<%=ddlendbarcode.ClientID%>").change(function () {

                if ($("#<%=ddlstartbarcode.ClientID%>").val() === $("#<%=ddlendbarcode.ClientID%>").val() 
                    && $("#<%=ddlstartbarcode.ClientID%>").val() !== null) {
                    $("#<%=chkeditqty.ClientID%>").prop('disabled', false);
                } else {
                    $("#<%=chkeditqty.ClientID%>").prop('disabled', true);
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
                                        <asp:CheckBox ID="chkeditqty" runat="server" class="form-check-input mb-1" /><spen>&nbsp;แก้ไข Qty : </spen>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:TextBox ID="txtqty" runat="server" class="form-control form-control-sm mb-1" Text="0.00" AutoComplete="off" disabled></asp:TextBox>
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
                                        <span>Lot : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">                                        
                                        <asp:DropDownList ID="ddlstartlot" runat="server" AutoPostBack="true" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                        
                                    </div>
                                    
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div"> 
                                        <asp:DropDownList ID="ddlendlot" runat="server" AutoPostBack="true" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        
                    </table>

                    <asp:HiddenField ID="vCoreweight" runat="server" Value="0" />
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
                                        <asp:DropDownList ID="ddlstartgrn" runat="server" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true" ></asp:DropDownList>
                                        
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
                                        <asp:TextBox ID="txtstartgrnline" runat="server" class="form-control form-control-sm mb-1" AutoComplete="off"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:TextBox ID="txtendgrnline" runat="server" class="form-control form-control-sm mb-1" AutoComplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="divbarcode" class="row align-items-center pt-2"> 
                    <table width="100%">
                        <tr>
                            <td>
                                <div class="row align-items-center">
                                    <div class="col-sm-3 text-right width-div">
                                        <span>Barcode No. : </span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:DropDownList ID="ddlstartbarcode" runat="server" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>                                        
                                    </div>
                                    <div class="col-sm-2 text-right width-div">
                                        <span></span>
                                    </div>
                                    <div class="col-sm-2 width-div">
                                        <asp:DropDownList ID="ddlendbarcode" runat="server" class="form-control form-control-sm mb-1 selectpicker" data-live-search="true"></asp:DropDownList>
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

