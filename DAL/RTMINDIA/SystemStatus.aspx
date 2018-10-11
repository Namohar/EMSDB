<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SystemStatus.aspx.cs" Inherits="SystemStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <form runat="server">
 <div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-desktop"></i>System Status</h3>
	    	<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-desktop"></i>System Status</li>
			</ol>
	</div>

    <div class="row">
       <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
               <div class="panel-body">
                 <table style="width: 100%;">
         <tr>
             <td>
                 &nbsp;
             </td>
             <td>
                 &nbsp;</td>
             <td>
                 &nbsp;
                 &nbsp;
             </td>
         </tr>
         <tr>
             <td class="style1" colspan="3" style="text-align: center">
                 <asp:Label ID="lblTeam" runat="server" Font-Bold="True" Text="Select Team:"></asp:Label>
                 <asp:DropDownList ID="ddlTeam" runat="server" Width="200px" AutoPostBack="True" 
                     onselectedindexchanged="ddlTeam_SelectedIndexChanged">
                 </asp:DropDownList>
                 <asp:Image ID="imgloading" runat="server" ImageUrl="~/images/715.GIF" Visible="false" />
             </td>
         </tr>
         <tr>
             <td colspan="2">
                 &nbsp;
             </td>
             <td>
                 &nbsp;
                 &nbsp;
             </td>
         </tr>
         <tr>
             <td colspan="2" style="text-align: right">
                 <asp:Button ID="btnExport" CssClass="btn btn-primary" runat="server" 
                     Text="Export" onclick="btnExport_Click" />
             </td>
             <td>
                 &nbsp;</td>
         </tr>
         <tr>
             <td colspan="2" style="text-align: right">
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
         </tr>
         <tr>
             <td colspan="3">
                 <asp:Panel ID="Panel1" runat="server" Height="330px" ScrollBars="Vertical">
                     <asp:GridView ID="gvSystemStatus" runat="server" AutoGenerateColumns="False" 
                         Width="100%" class="table table-striped table-bordered table-hover">
                         <Columns>
                             <asp:BoundField HeaderText="User Name" DataField="User Name" >
                             
                             </asp:BoundField>
                             <asp:BoundField HeaderText="Client" DataField="Client" >
                             
                             </asp:BoundField>
                             <asp:BoundField HeaderText="Task"  DataField="Task" >
                             
                             </asp:BoundField>
                             <asp:BoundField HeaderText="SubTask" DataField="SubTask">
                           
                             </asp:BoundField>
                             <asp:BoundField HeaderText="Log" DataField="Log" >
                             
                             </asp:BoundField>
                             <asp:BoundField HeaderText="Start Time" DataField="Start Time">
                            
                             </asp:BoundField>
                             <asp:BoundField HeaderText="Duration" DataField="Duration">
                             
                             </asp:BoundField>
                             <asp:BoundField HeaderText="Status" DataField="Status">
                             
                             </asp:BoundField>
                             <asp:BoundField HeaderText="Comments" DataField="Comments">
                            
                             </asp:BoundField>
                         </Columns>
                     </asp:GridView>
                 </asp:Panel>
             </td>
         </tr>
         <tr>
             <td colspan="2">
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
         </tr>
     </table>
               </div>
            </div>
        </div>
    </div>
</form>
</asp:Content>

