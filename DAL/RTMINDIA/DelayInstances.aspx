<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DelayInstances.aspx.cs" Inherits="DelayInstances" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <form runat="server">
  <div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-table"></i>Delay Instances</h3>
	    	<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-table"></i>Delay Instances</li>
			</ol>
	</div>

    <div class="row">
       <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
               <div class="panel-body">
                 <table style="width: 100%;">
         <tr>
             <td width="40%">
                 &nbsp;
             </td>
             <td>
                 &nbsp;
                 &nbsp;
             </td>
         </tr>
         <tr>
             <td style="text-align: right">
                 <asp:Label ID="lblteam" runat="server" Text="Select Team" Font-Bold="True"></asp:Label>
             </td>
             <td>
                 <asp:DropDownList ID="ddlTeam" runat="server" class="form-control">
                 </asp:DropDownList>
             </td>
         </tr>
         <tr>
             <td style="text-align: right">
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
         </tr>
         <tr>
             <td style="text-align: right">
                 <asp:Label ID="lblFrom" runat="server" Text="From Date" Font-Bold="True"></asp:Label>
             </td>
             <td>
                 <asp:TextBox ID="txtFrom" runat="server" ClientIDMode="Static"  class="form-control"></asp:TextBox>
             </td>
         </tr>
         <tr>
             <td style="text-align: right">
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
         </tr>
         <tr>
             <td style="text-align: right">
                 <asp:Label ID="lblTo" runat="server" Text="To Date" Font-Bold="True"></asp:Label>
             </td>
             <td>
                 <asp:TextBox ID="txtTo" ClientIDMode="Static" runat="server" class="form-control"></asp:TextBox>
             </td>
         </tr>
         <tr>
             <td style="text-align: right">
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
         </tr>
         <tr>
             <td style="text-align: right">
                 &nbsp;</td>
             <td>
                 <asp:Button ID="btnPrint" CssClass="btn btn-primary" runat="server" 
                     Text="Print" onclick="btnPrint_Click" />
             </td>
         </tr>
         <tr>
             <td style="text-align: right">
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
         </tr>
         <tr>
             <td colspan="2">
                 <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Vertical">
                     <asp:GridView ID="grdInstances" runat="server" AutoGenerateColumns="False" 
                         Width="100%" class="table table-striped table-bordered table-hover">
                         <Columns>
                             <asp:BoundField DataField="User Name" HeaderText="User Name">
                             
                             </asp:BoundField>
                             <asp:BoundField DataField="# of Delayed Login" HeaderText="# of Delayed Login">
                             
                             </asp:BoundField>
                             <asp:BoundField DataField="# of Delayed Logoff" 
                                 HeaderText="# of Delayed Logoff">
                             
                             </asp:BoundField>
                             <asp:BoundField DataField="Total Hours (Delayed Login)" 
                                 HeaderText="Total Hours (Delayed Login)">
                             
                             </asp:BoundField>
                             <asp:BoundField DataField="Total Hours (Delayed Logoff)" 
                                 HeaderText="Total Hours (Delayed Logoff)">
                            
                             </asp:BoundField>
                         </Columns>
                     </asp:GridView>
                 </asp:Panel>
             </td>
         </tr>
     </table>
               </div>
           </div>
        </div>
    </div>
</form>
<script type="text/javascript">
    $(function () {
        $("#txtFrom").datepicker({ maxDate: new Date() });
    });

    $(function () {
        //            $("#datepickerTo").datepicker({ maxDate: "+1M +10D" });
        $("#txtTo").datepicker({ maxDate: new Date() });
    });
  </script>
</asp:Content>

