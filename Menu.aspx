<%@ Page Title="Home" Language="VB" MasterPageFile="~/SiteMasterNew.master" AutoEventWireup="false" CodeFile="Menu.aspx.vb" Inherits="Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="se-pre-con"></div>
    <div class="container pt-5">
       <div class="row pt-4">
            <div id="divBCPallet" class="col-3 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkBCPallet" runat="server" NavigateUrl="~/BCPallet.aspx">
          
                    <img src="asset/image/BCPallet.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>BC Pallet</h6>         
                </asp:HyperLink>
            </div>

            <div id="divRequestMoveIn" class="col-3 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkBRequestMoveIn" runat="server" NavigateUrl="~/RequestMoveIn.aspx">
          
                    <img src="asset/image/ReqMoveIn.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>Request Move Pallet In WMS</h6>         
                </asp:HyperLink>
            </div>

            <div id="divRequestMoveIn_InhouseWhse" class="col-3 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkBRequestMoveIn_InhouseWhse" runat="server" NavigateUrl="~/RequestMoveIn_InhouseWhse.aspx">
          
                    <img src="asset/image/ReqMoveIn.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>Request Move Pallet In - Inhouse</h6>         
                </asp:HyperLink>
            </div>

            <div id="divRequestMoveOut" class="col-3 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkRequestMoveOut" runat="server" NavigateUrl="~/RequestMoveOut.aspx">
          
                    <img src="asset/image/ReqMoveOut2.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>Request Move Pallet Out WMS</h6>         
                </asp:HyperLink>
            </div>
       </div>

       <div class="row pt-4">
            <div id="divMassQtyMove" class="col-3 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkMassQtyMove" runat="server" NavigateUrl="~/MassQtyMove.aspx">
          
                    <img src="asset/image/MassQtyMove.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>WMS Mass Quantity Move</h6>         
                </asp:HyperLink>
            </div>

            <div id="divPrintSticker" class="col-3 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkPrintSticker" runat="server" NavigateUrl="~/MenuPrintSticker.aspx">
          
                    <img src="asset/image/PrintSticker.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>Print Sticker Barcode</h6>         
                </asp:HyperLink>
            </div>

            <div id="divRePrintSticker" class="col-3 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkRePrintSticker" runat="server" NavigateUrl="~/MenuRePrintSticker.aspx">
          
                    <img src="asset/image/RePrintSticker2.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>RePrint Sticker Barcode</h6>         
                </asp:HyperLink>
            </div>
       </div>

       

       <hr class="featurette-divider">
    </div>
</asp:Content>

