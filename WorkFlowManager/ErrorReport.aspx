<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ErrorReport.aspx.cs" Inherits="WorkFlowManager.ErrorReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
    <style>
        .Grid
        {
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
            font-family: Calibri;
            color: #474747;
            width: 830px;
        }
        .Grid td
        {
            padding: 2px;
            border: solid 1px #c1c1c1;
        }
        .Grid th
        {
            padding: 4px 2px;
            color: #fff;
            background: #31708f url(Images/grid-header.png) repeat-x top;
            border-left: solid 1px #525252;
            font-size: 0.9em;
            text-align: center;
        }
        .Grid .alt
        {
            background: #fcfcfc url(Images/grid-alt.png) repeat-x top;
        }
        .Grid .pgr
        {
            background: #363670 url(Images/grid-pgr.png) repeat-x top;
        }
        .Grid .pgr table
        {
            margin: 3px 0;
        }
        .Grid .pgr td
        {
            border-width: 0;
            padding: 0 6px;
            border-left: solid 1px #666;
            font-weight: bold;
            color: #fff;
            line-height: 12px;
        }
        .Grid .pgr a
        {
            color: Gray;
            text-decoration: none;
        }
        .Grid .pgr a:hover
        {
            color: #000;
            text-decoration: none;
        }
        .Grid input[type="text"]
        {
            width: 130px;
        }
        .Grid select
        {
            width: 130px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>
        Error Report</h3>
    <br />
    <form id="form1" runat="server">
    <fieldset style="border-style: double; border-color: #0066FF; background-color: #f6f6f6;
        height: 70px; width: 1100px">
        <legend></legend>
        <div class="container">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Select Report:"></asp:Label>
                        &nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlReport" runat="server" ClientIDMode="Static" Width="170px"
                            CssClass="txtbox">
                            <asp:ListItem Text="Manual" Value="1" Selected="true" />
                            <asp:ListItem Text="EDI" Value="2" />
                        </asp:DropDownList>
                    </td>
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
            <div style="position: relative; top: -57px; margin-left: 780px">
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
    <br />
    <div class="container">
        <div class="panel panel-default" style="width: 1100px; background-color: White;">
            <div style="width: 1090px; height: 400px; overflow: scroll;">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="Grid">
                    <Columns>
                        <asp:BoundField HeaderText="FilelName" DataField="IND_FI"></asp:BoundField>
                        <asp:BoundField HeaderText="Source" DataField="FI_Source"></asp:BoundField>
                        <asp:BoundField HeaderText="Client" DataField="FI_ClientCode"></asp:BoundField>
                        <asp:BoundField HeaderText="InvoiceNo" DataField="IND_InvoiceNo"></asp:BoundField>
                        <asp:BoundField HeaderText="Notes" DataField="Notes"></asp:BoundField>
                        <asp:BoundField HeaderText="IP_Assigned_By" DataField="IND_IP_Assigned_By"></asp:BoundField>
                        <asp:BoundField HeaderText="IP_Processed_By" DataField="IND_IP_Processed_By"></asp:BoundField>
                        <asp:BoundField HeaderText="QC_Assigned_By" DataField="IND_QC_Assigned_By"></asp:BoundField>
                        <asp:BoundField HeaderText="QC_Processed_By" DataField="IND_QC_Processed_By"></asp:BoundField>
                        <asp:BoundField HeaderText="Error" DataField="IND_Error"></asp:BoundField>
                        <asp:BoundField HeaderText="RECEIVEDATE" DataField="FI_ReceiptDate" DataFormatString="{0:dd-M-yyyy}">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
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
