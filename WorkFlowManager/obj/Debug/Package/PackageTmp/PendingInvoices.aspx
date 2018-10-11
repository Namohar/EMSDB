<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PendingInvoices.aspx.cs"
    Inherits="WorkFlowManager.PendingInvoices" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

    <div style="margin-left:10px">
        <h3>Pending Invoices</h3>
        <div class="row">
            <div class="col-lg-12 ">
                <div class="table-responsive">
                    <asp:GridView ID="grdPendingReport" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="header"
                        RowStyle-CssClass="rows" CssClass="mydatagrid" CellPadding="4" ForeColor="#333333"
                        GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Sl.No.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Client" HeaderText="Client" ItemStyle-Width="100">
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Date Added" HeaderText="Date Added" ItemStyle-Width="100">
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Incomplete" HeaderText="Incomplete" ItemStyle-Width="100">
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle CssClass="header" BackColor="#507CD1" Font-Bold="True" ForeColor="White">
                        </HeaderStyle>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle CssClass="rows" BackColor="#EFF3FB"></RowStyle>
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
