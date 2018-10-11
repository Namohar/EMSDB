<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QC_SummaryReport.aspx.cs" Inherits="WorkFlowManager.QC_CR_Reports.QC_SummaryReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <script src="../assets/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <link href="../assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
       <script src="../Scripts/jquery.timepicker.js" type="text/javascript"></script>
           <link rel="stylesheet" href="//code.jquery.com/ui/1.8.24/themes/base/jquery-ui.css">
    <script src="//code.jquery.com/jquery-1.8.2.js" type="text/javascript"></script>
    <script src="//code.jquery.com/ui/1.8.24/jquery-ui.js" type="text/javascript"></script>
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
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
 <h1>Summary Report</h1>
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
       
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"   OnDataBinding="CrystalReportViewer1_DataBinding"  />
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
</body>
</html>
