<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="SummarySLA_Report.aspx.cs" Inherits="WorkFlowManager.SummarySLA_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="assets/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
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
        #cssTable td
        {
            text-align: center;
            vertical-align: middle;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Summary SLA Report</h1>
    <br />
    <form id="form1" runat="server">
    <fieldset style="border-style: double; border-color: #0066FF; background-color: #f6f6f6;
        height: 70px;">
        <legend></legend>
        <div class="container">
            <table id="cssTable">
                <tr>
                    <td>
                        <asp:Label ID="lblDate" runat="server" Font-Bold="True" Text="From Date :"></asp:Label>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="datepickerfrom" ClientIDMode="Static" runat="server" CssClass="txtbox"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Day : "></asp:Label>
                        &nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlselectDay" runat="server" ClientIDMode="Static" CssClass="txtbox">
                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="6+" Value="6+"></asp:ListItem>
                        </asp:DropDownList>
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
        </div>
        <br />
        <div class="container">
            <asp:GridView ID="grdSummarySLAReport" runat="server" AutoGenerateColumns="False"
                HeaderStyle-CssClass="header" RowStyle-CssClass="rows" CssClass="mydatagrid"
                CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True" OnPageIndexChanging="OnPaging"
                PageIndex="10">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <%--     <asp:BoundField HeaderText="Date Added" DataField="RECEIVEDATE" DataFormatString="{0:dd-M-yyyy}">
                </asp:BoundField>--%>
                    <asp:BoundField HeaderText="ReceivedDate" DataField="ReceivedDate"></asp:BoundField>
                    <asp:BoundField HeaderText="File" DataField="IND_FI"></asp:BoundField>
                    <asp:BoundField HeaderText="Client" DataField="FI_ClientCode"></asp:BoundField>
                    <asp:BoundField HeaderText="Source" DataField="FI_Source"></asp:BoundField>
                    <asp:BoundField HeaderText="Status" DataField="IND_Status"></asp:BoundField>
                    <asp:BoundField HeaderText="Invoice" DataField="IND_InvoiceNo"></asp:BoundField>
                    <asp:BoundField HeaderText="AssignedBy" DataField="IND_IP_Assigned_By"></asp:BoundField>
                    <asp:BoundField HeaderText="ProcessedBy" DataField="IND_IP_Processed_By"></asp:BoundField>
                    <asp:BoundField HeaderText="ProcessDate" DataField="ProcessDate"></asp:BoundField>
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
    </fieldset>
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
