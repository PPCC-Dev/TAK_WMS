
<%@ Page Title="" Language="vb" MasterPageFile="~/SNPRWeb1.Master" AutoEventWireup="false" CodeFile="CycleCountUpdate.aspx.vb" Inherits="CycleCountUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        
        <asp:Table ID="Table2" runat="server" style="font-size: small">
            <asp:TableRow>
                <asp:TableCell><asp:Label ID="labWhse" runat="server" Text="Whse :" CssClass="labels" ></asp:Label></asp:TableCell>
                <asp:TableCell><asp:TextBox ID="txtWhse" runat="server" ReadOnly="True" Width="81px"></asp:TextBox></asp:TableCell>
                <asp:TableCell><asp:Label ID="labEmployee" runat="server" Text="Employee :" CssClass="labels"></asp:Label></asp:TableCell>
                <asp:TableCell><asp:DropDownList ID="ddlEmployee" runat="server" Enabled="false"></asp:DropDownList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell><asp:Label ID="labBarcode" runat="server" Text="Barcode :"></asp:Label></asp:TableCell>
                <asp:TableCell ColumnSpan="3"><asp:TextBox ID="txtBarcode" runat="server" Width="216px"></asp:TextBox><asp:Button ID="btnScan" runat="server" Text="Scan" Visible="true" CssClass="button_green" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell><asp:CheckBox ID="chkCancel" runat="server" text="Cancel" /></asp:TableCell>
                <asp:TableCell><asp:Label ID="labBlank00" runat="server" Text="&nbsp;"></asp:Label></asp:TableCell>
                <asp:TableCell><asp:Button ID="btnClearAll" runat="server" Text="Clear All" CssClass="button_green" /></asp:TableCell>
                <asp:TableCell><asp:Button ID="btnProcess" runat="server" Text="Process" CssClass="button_green" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
             
            </asp:TableRow>
        </asp:Table>
   
         <asp:GridView ID="grdCycleCount" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" ForeColor="#333333" GridLines="None" 
            style="font-size: small">
             <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField headertext="Tag ID">
                    <ItemTemplate>
                        <asp:Label ID="labTagID" runat="server" style="text-align:center" Text='<%# Eval("tag_id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField headertext="Item">
                    <ItemTemplate>
                        <asp:Label ID="labTagItem" runat="server" style="text-align:center" Text='<%# Eval("item") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField headertext="Item Desc">
                    <ItemTemplate>
                        <asp:Label ID="labTagDescription" runat="server" style="text-align:center" Text='<%# Eval("description") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField headertext="U/M">
                    <ItemTemplate>
                        <asp:Label ID="labTagUM" runat="server" style="text-align:center" Text='<%# Eval("u_m") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField headertext="Lot">
                    <ItemTemplate>
                        <asp:Label ID="labTagLot" runat="server" style="text-align:center" Text='<%# Eval("lot") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField headertext="Qty">
                    <ItemTemplate>
                        <asp:Label ID="labTagQty" runat="server" style="text-align:center" Text='<%# Eval("tag_qty") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField headertext="Loc">
                    <ItemTemplate>
                        <asp:Label ID="labTagLoc" runat="server" style="text-align:center" Text='<%# Eval("loc") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
             <EditRowStyle BackColor="#999999" />
             <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
             <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
             <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
             <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
             <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
             <SortedAscendingCellStyle BackColor="#E9E7E2" />
             <SortedAscendingHeaderStyle BackColor="#506C8C" />
             <SortedDescendingCellStyle BackColor="#FFFDF8" />
             <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView> 
   
        <br />
        <asp:Label ID="labError" runat="server" Text="labError"></asp:Label>
       
</asp:Content>
