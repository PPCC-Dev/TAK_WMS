<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPageHH.master" AutoEventWireup="false" CodeFile="qt_move_hh.aspx.vb" Inherits="qt_move_hh" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            font-size: small;
        }
        .button_green {
            background-color: #4CAF50; /* Green */
            border: none;
            color: white;
            padding: 5px 5px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
        }
        .button_blue {
            background-color: #008CBA; /* Blue */
            border: none;
            color: white;
            padding: 5px 5px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
        }
        .button_red {
            background-color: #f44336; /* Red */
            border: none;
            color: white;
            padding: 5px 5px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
        }
    </style>

    <script type="text/javascript">
        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
        }

        document.onkeypress = stopRKey;
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
     <tr>
        <td style="text-align: left" class="style1" colspan="2">
           <asp:Label ID="Literal1" runat="server" ForeColor="Red" style="font-size: small"></asp:Label>
         </td>
    </tr>
                    
     <tr>
        <td style="text-align: left" class="style1">
            &nbsp;</td>
        <td style="padding-left:5px; padding-bottom: 5px; text-align: left;">
            <asp:Label ID="UserNameLabel" runat="server" style="font-size: small"></asp:Label>
         </td>
    </tr>
                    
     <tr>
        <td style="text-align: left" class="style1">
            &nbsp;</td>
        <td style="padding-left:5px; padding-bottom: 5px; text-align: left;">
                          
            <asp:Button ID="MainMenuButton" runat="server" Text="หน้าหลัก" 
                CssClass="button_green" Width="100px" />&nbsp;<asp:Button 
                ID="SignoutButton" runat="server" Text="ออก" CssClass="button_green" 
                Width="115px" /></td>
    </tr>
                    
    <tr>
        <td style="text-align: right" class="style1">ผู้ทำรายการ</td>
        <td style="padding-left:5px; padding-bottom: 5px">
                          
                <asp:DropDownList ID="UserDropDown" runat="server" 
                style="text-align: left" CssClass="form-control" Width="200px" 
                    BackColor="#FFFF99" Enabled="False"> 
                </asp:DropDownList>
                          
        </td>
    </tr>
                    
    <tr>
        <td style="text-align: right; vertical-align:top" class="style1">วันที่ทำรายการ</td>
        <td style="padding-left:5px; padding-bottom: 5px">
                          
            <asp:TextBox ID="TransDateTextBox" runat="server" Width="100px" style="text-align:center"
                CssClass="form-control"></asp:TextBox>
                          
            <asp:Button ID="PickDateButton" runat="server" Text="..." CssClass="button_green" />
            <asp:Calendar ID="TransactionDateCalendar" runat="server" BackColor="White" 
                BorderColor="#999999" Font-Names="Verdana" Font-Size="8pt" 
                ForeColor="Black" Height="180px" Width="200px" CellPadding="4" 
                DayNameFormat="Shortest" Visible="False">
                <DayHeaderStyle Font-Bold="True" Font-Size="7pt" BackColor="#CCCCCC" />
                <NextPrevStyle 
                    VerticalAlign="Bottom" />
                <OtherMonthDayStyle ForeColor="#808080" />
                <SelectedDayStyle BackColor="#666666" ForeColor="White" Font-Bold="True" />
                <SelectorStyle BackColor="#CCCCCC" />
                <TitleStyle BackColor="#999999" BorderColor="Black" 
                    Font-Bold="True" />
                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                <WeekendDayStyle BackColor="#FFFFCC" />
            </asp:Calendar>
                          
        </td>
    </tr>
                    
    <tr style="display: none">
        <td style="text-align: right" class="style1">
            
            จากที่ตั้ง</td>
        <td style="padding-left:5px; padding-bottom: 5px">
                          
            <asp:TextBox ID="FromSiteTextBox" runat="server" ReadOnly="True" 
                    CssClass="form-control" Width="100px"></asp:TextBox></td>
    </tr>
                    
    <tr style="display:none">
        <td style="text-align: right" class="style1">
            
            ไปที่ตั้ง</td>
        <td style="padding-left:5px; padding-bottom: 5px">
                          
            <asp:TextBox ID="ToSiteTextBox" runat="server" ReadOnly="True" 
                    CssClass="form-control" Width="100px"></asp:TextBox></td>
    </tr>
    
  
    <tr style="display:none">
        <td style="text-align: right" class="style1">จากคลัง</td>
        <td style="padding-left:5px; padding-bottom: 5px">
                          
            <asp:TextBox ID="FromWhseTextBox" runat="server" ReadOnly="True" 
                    CssClass="form-control" Width="100px"></asp:TextBox></td>
    </tr>
                    
    <tr style="display:none">
        <td style="text-align: right" class="style1">ไปคลัง</td>
        <td style="padding-left:5px; padding-bottom: 5px">
                          
            <asp:TextBox ID="ToWhseTextBox" runat="server" ReadOnly="True" 
                    CssClass="form-control" Width="100px"></asp:TextBox></td>
    </tr>
                    
    <tr>
        <td style="text-align: right" class="style1">จาก ที่จัดเก็บ</td>
        <td style="padding-left:5px; padding-bottom: 5px">
            <asp:TextBox ID="FromLocationTextBox" runat="server" Width="200px" AutoPostBack="true"></asp:TextBox>
                <asp:DropDownList ID="FromLocationDropDown" runat="server" 
                CssClass="form-control" Width="200px" BackColor="#FFFF99" style="display:none">
            </asp:DropDownList></td>
    </tr>
                    
    <tr>
        <td style="text-align: right" class="style1">ไป ที่จัดเก็บ</td>
        <td style="padding-left:5px; padding-bottom: 5px">
            <asp:TextBox ID="ToLocationTextBox" runat="server" Width="200px" AutoPostBack="true"></asp:TextBox>
                <asp:DropDownList ID="ToLocationDropDown" runat="server" 
                CssClass="form-control" Width="200px" BackColor="#FFFF99" style="display:none">
            </asp:DropDownList></td>
    </tr>
                    
    <tr>
        <td style="text-align: right" class="style1">&nbsp;</td>
        <td style="padding-left:5px; padding-bottom: 5px">
                          
                <asp:Button ID="SwitchButton" runat="server" Text="สลับ" Width="100px" CssClass="button_green" /></td>
    </tr>
                    
    <tr>
        <td style="text-align: right" class="style1">รหัสบาร์โค้ด</td>
        <td style="padding-left:5px; padding-bottom: 5px"><asp:TextBox ID="BarCodeTextBox" runat="server" Width="200px" CssClass="form-control" AutoPostBack="true"></asp:TextBox></td>
    </tr>
    <tr>
        <td style="text-align: right" class="style1">&nbsp;</td>
        <td style="padding-left:5px; padding-bottom: 5px">
                <asp:Button ID="ProcessButton" runat="server" Text="Process" Width="100px" 
                            CssClass="button_green" 
                    OnClientClick="javascript:return confirm('ยืนยัน?');" />&nbsp;<asp:Button ID="CancelAllButton"
                    runat="server" Text="Reset" CssClass="button_green" Width="115px" /></td>
    </tr>
    <tr>
        <td style="text-align: right" class="style1">&nbsp;</td>
        <td style="padding-left:5px; padding-bottom: 5px">
                <asp:CheckBox ID="DeleteCheckBox" runat="server" style="font-size: small" 
                    Text="ยกเลิกบางรายการ" />
        </td>
    </tr>
                    
    </table>
            <div class="dataTable_wrapper">
                <asp:GridView ID="QtyMoveGridView" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-striped table-bordered table-hover" CellPadding="4" 
                    ForeColor="#333333" GridLines="None">
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle Font-Size="Small" BackColor="#5D7B9D" Font-Bold="True" 
                        ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle Font-Size="Small" BackColor="#F7F6F3" ForeColor="#333333" />
                    <AlternatingRowStyle Font-Size="Small" BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="DeleteButton" runat="server" 
                                    CommandArgument='<%# Eval("RowPointer") %>' CommandName="DeleteMove" UseSubmitBehavior="false"
                                    CssClass="button_red" Text="ลบ" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="รหัสบาร์โค้ด">
                            <ItemTemplate>
                                <asp:Label ID="TagLabel" runat="server" style="vertical-align:middle" 
                                    Text='<%# Eval("TagID") %>'></asp:Label>
                                <asp:HiddenField ID="RowPointerHidden" runat="server" Value='<%# Eval("RowPointer") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="รหัสสินค้า" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="ItemLabel" runat="server" style="vertical-align:middle" 
                                    Text='<%# Eval("Item") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ชื่อสินค้า" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="ItemDescriptionLabel" runat="server" 
                                    style="text-align:center; vertical-align:middle" 
                                    Text='<%# Eval("Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--itmUf_Item_ModelName--%>
                        <asp:TemplateField HeaderText="Model">
                            <ItemTemplate>
                                <asp:Label ID="ItemModelLabel" runat="server" style="text-align:center; vertical-align:middle" Text='<%# Eval("ModelName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="ล็อต" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LotLabel" runat="server" 
                                    style="text-align:center; vertical-align:middle" 
                                    Text='<%# Eval("Lot") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="จำนวน">
                            <ItemTemplate>
                                <asp:Label ID="QtyLabel" runat="server" 
                                    style="text-align:center; vertical-align:middle" 
                                    Text='<%# Eval("Qty", "{0:N0}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="หน่วยนับ" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="UMLabel" runat="server" style="text-align:center" 
                                    Text='<%# Eval("UM") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Error !!!">
                            <ItemTemplate>
                                <%--<asp:CheckBox ID="ErrorCheckBox" runat="server" Checked='<%# Eval("ErrFlag") %>' Enabled="false"/>--%>
                                <asp:Label ID="ErrorLabel" runat="server" 
                                    style="text-align:center; vertical-align:middle" 
                                    Text='<%# Eval("ErrMsg") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
            </div>

</asp:Content>

