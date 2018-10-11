<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="WeeklyReport.aspx.cs" Inherits="WorkFlowManager.WeeklyReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="assets/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <%--    <link href="assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
     <script src="Scripts/jquery.timepicker.js" type="text/javascript"></script>
 <link rel="stylesheet" href="//code.jquery.com/ui/1.8.24/themes/base/jquery-ui.css">
    <script src="//code.jquery.com/jquery-1.8.2.js" type="text/javascript"></script>
    <script src="//code.jquery.com/ui/1.8.24/jquery-ui.js" type="text/javascript"></script>--%>
    <link href="css/WeeklyReport.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .txtbox
        {
            background-repeat: repeat-x;
            border: 1px solid #d1c7ac;
            color: #333333;
            padding: 3px;
            margin-right: 4px;
            margin-bottom: 8px;
            font-family: tahoma, arial, sans-serif;
        }
        .style1
        {
            width: 136px;
        }
        .gridrowstyle
        {
            display: table-cell;
            vertical-align: middle;
            height: 50px;
            border: 1px solid red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Weekly Report</h1>
    <br />
    <form id="form1" runat="server">
    <fieldset style="border-style: double; border-color: #0066FF; background-color: #f6f6f6;
        height: 70px;">
        <legend></legend>
        <div class="container">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblDate" runat="server" Font-Bold="True" Text="From Date:"></asp:Label>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="datepickerfrom" ClientIDMode="Static" runat="server" CssClass="txtbox"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblTo" runat="server" Font-Bold="True" Text="To Date:"></asp:Label>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="datepickerTo" ClientIDMode="Static" runat="server" CssClass="txtbox"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <div style="position: relative; top: -62px; margin-left: 530px">
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClick="btnGenerate_Click"
                    CssClass="btn btn-primary btn-sm" />
                &nbsp; &nbsp; &nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" CssClass="btn btn-primary btn-sm" />
                &nbsp; &nbsp; &nbsp;
                <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click"
                    CssClass="btn btn-primary btn-sm" />
            </div>
            <%--        <a href="PendingInvoices.aspx">Hyperlink </a>--%>
        </div>
    </fieldset>
    <div class="container">
        <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="header"
            RowStyle-CssClass="rows" CssClass="mydatagrid" CellPadding="4" ForeColor="#333333"
            GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField HeaderText="Date Added" DataField="RECEIVEDATE" DataFormatString="{0:dd-M-yyyy}">
                </asp:BoundField>
                <asp:BoundField HeaderText="1Day" DataField="Day1Count"></asp:BoundField>
                <asp:BoundField HeaderText="2Day" DataField="Day2Count"></asp:BoundField>
                <asp:BoundField HeaderText="3Day" DataField="Day3Count"></asp:BoundField>
                <asp:BoundField HeaderText="4Day" DataField="Day4Count"></asp:BoundField>
                <asp:BoundField HeaderText="5Day" DataField="Day5Count"></asp:BoundField>
                <asp:BoundField HeaderText="6DayPlus" DataField="Day6Count"></asp:BoundField>
                <%--  <asp:BoundField HeaderText="Remaining in Queue" DataField="Pending"></asp:BoundField>--%>
                <asp:BoundField HeaderText="Total" DataField="TotalInvoice"></asp:BoundField>
                <asp:BoundField HeaderText="1Day%" DataField="Day1Percentage"></asp:BoundField>
                <asp:BoundField HeaderText="2Day%" DataField="Day2Percentage"></asp:BoundField>
                <asp:BoundField HeaderText="3Day%" DataField="Day3Percentage"></asp:BoundField>
                <asp:BoundField HeaderText="4Day%" DataField="Day4Percentage"></asp:BoundField>
                <asp:BoundField HeaderText="5Day%" DataField="Day5Percentage"></asp:BoundField>
                <asp:BoundField HeaderText="6DayPlus%" DataField="Day6Percentage"></asp:BoundField>
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
    <br />
    <div id="subgrid" runat="server" visible="false">
        <div class="container">
            <div class="row">
                <div class="col-md-4">
                    <div class="jumbotron">
                        <asp:Label ID="LblPending" runat="server" Text="Pending/In Queue Volumes:" Visible="false"></asp:Label>
                        <div style="width: 300px">
                            <asp:GridView ID="grdPending" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="header"
                                RowStyle-CssClass="rows" CssClass="mydatagrid" CellPadding="4" ForeColor="#333333"
                                GridLines="None">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField HeaderText="Date Added" DataField="RECEIVEDATE" DataFormatString="{0:dd-M-yyyy}">
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Remaining in Queue" DataField="TotalPending"></asp:BoundField>
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
                <div class="col-md-6">
                    <div class="jumbotron">
                        <asp:Label ID="lblSLA" runat="server" Text="Met GS SLA" Visible="false"></asp:Label>
                        <br />
                        <div style="width: 400px">
                            <asp:GridView ID="grdSLA" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="header"
                                RowStyle-CssClass="rows" CssClass="mydatagrid" CellPadding="4" ForeColor="#333333"
                                GridLines="None">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField HeaderText="Date Added" DataField="RECEIVEDATE" DataFormatString="{0:dd-M-yyyy}">
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="#Inv" DataField="TotalInvoice"></asp:BoundField>
                                    <asp:BoundField HeaderText="Met GS SLA(#)" DataField="ProcessedInvoices"></asp:BoundField>
                                    <asp:BoundField HeaderText="GS SLA(%)" DataField="Percentage"></asp:BoundField>
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
        </div>
    </div>
    </form>
    <%--    scripts--%>
    <script type="text/javascript">
        $(function () {
            $("#datepickerfrom").datepicker({
                numberOfMonths: 1,
                onSelect: function (selected) {
                    var dt = new Date(selected);
                    dt.setDate(dt.getDate());
                    $("#datepickerTo").datepicker("option", "minDate", dt);
                }
            });
            $("#datepickerTo").datepicker({
                numberOfMonths: 1,
                onSelect: function (selected) {
                    var dt = new Date(selected);
                    dt.setDate(dt.getDate());
                    $("#datepickerfrom").datepicker("option", "maxDate", dt);
                }
            });
        });
    </script>
</asp:Content>
