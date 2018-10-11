<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="EDISummaryReport.aspx.cs" Inherits="WorkFlowManager.EDISummaryReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
    <style>
        fieldset1
        {
            font-size: 12px;
            padding: 2px;
            width: 850px;
            line-height: 1.8;
            background-color: Aqua;
            border-color: Black;
        }
        .panel
        {
            margin: 0 !important;
            padding: 10px !important;
            background-color: #DCDCDC;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
 <h2>EDI Summary Report</h2>
 <br />
    <fieldset style="border-style: double; border-color: #0066FF;  background-color:#f6f6f6; height:70px;" >
    <legend></legend>
    <div class="container">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDate" runat="server" Font-Bold="True" Text="From Date:"></asp:Label>   &nbsp; 
                </td>
                <td>
                    <asp:TextBox ID="datepickerfrom" ClientIDMode="Static"  runat="server" CssClass="txtbox"
                      ></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblTo" runat="server" Font-Bold="True"  Text="To Date:"></asp:Label>    &nbsp; 
                </td>
                <td>
                    <asp:TextBox ID="datepickerTo" ClientIDMode="Static" runat="server" CssClass="txtbox"></asp:TextBox>
                </td>
            </tr>
               </table>
               <br />
          <div style="position:relative; top:-62px; margin-left:530px">
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClick="btnGenerate_Click" CssClass="btn btn-primary btn-sm" />
          &nbsp;    &nbsp;    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" CssClass="btn btn-primary btn-sm" />
                       &nbsp;    &nbsp;    &nbsp;
                            <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" CssClass="btn btn-primary btn-sm" />
                    </div>
          <asp:Label ID="lblmsg" runat="server"  Text="No results were found." Visible="False" 
            Font-Size="Large" CssClass="active"></asp:Label>
    </div>

    </fieldset>

           
       
<div class="panel panel-default" style="width: 1100px; background-color: White;">
                                <fieldset class="">
                                    <div style="width: 1090px; height: 400px; overflow: scroll;">
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" CssClass="Grid"
            GridLines="None">
            
        </asp:GridView>
        
    </div>
    </fieldset>
     </div>
     
       
    </form>
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
