<%@ Page Title="WMS Print Sticker Barcode Conditions" Language="VB" MasterPageFile="~/SiteMasterNew.master" AutoEventWireup="false" CodeFile="MenuPrintSticker.aspx.vb" Inherits="MenuPrintSticker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="se-pre-con"></div>
    <div class="container pt-5">
       <div class="row pt-4">
            <div class="col-md-9">
                <h6 class="m-0"><a href="Menu.aspx">Home</a><span class="mx-2 mb-0">/</span> <strong class="text-black"><%: Page.Title %></strong>  </h6>
            </div>
       </div>
       <div class="row pt-4">
            <div id="divBCGRN" class="col-4 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkBCGRN" runat="server" NavigateUrl="~/PrintStickerBarcode.aspx?Print=GRN">
          
                    <img src="asset/image/GRN5.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>GRN</h6>         
                </asp:HyperLink>
            </div>

            <div id="divBCMiscReceipt" class="col-4 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkBCMiscReceipt" runat="server" NavigateUrl="~/PrintStickerBarcode.aspx?Print=Misc. Receipt">
          
                    <img src="asset/image/MiscReceipt2.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>Miscelleneouse Receipt</h6>         
                </asp:HyperLink>
            </div>

            <div id="divBCQtyMove" class="col-4 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkBCQtyMove" runat="server" NavigateUrl="~/PrintStickerBarcode.aspx?Print=Qty Move">
          
                    <img src="asset/image/QtyMove.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>Quantity Move</h6>         
                </asp:HyperLink>
            </div>

       </div>

       <div class="row pt-4">
            <div id="divBCRMA" class="col-4 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkBCRMA" runat="server" NavigateUrl="~/PrintStickerBarcode.aspx?Print=RMA">
          
                    <img src="asset/image/RMA2.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>RMA</h6>         
                </asp:HyperLink>
            </div>

            <div id="divBCUnpostedJob" class="col-4 text-center chover rounded" runat="server">
                <asp:HyperLink ID="LinkBCUnpostedJob" runat="server" NavigateUrl="~/PrintStickerBarcode.aspx?Print=Unposted Job">
          
                    <img src="asset/image/UnpostedJob.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6>Unposted Job</h6>         
                </asp:HyperLink>
            </div>

       </div>
  

       <hr class="featurette-divider">
    </div>
</asp:Content>

