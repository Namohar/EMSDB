<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserLogMonitor.aspx.cs" Inherits="UserLogMonitor" %>
<%@ Register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
	<style>
  .modalBackground
	{
		background-color: Black;
		filter: alpha(opacity=60);
		opacity: 0.6;
	}
	.modalPopup
	{
		background-color: #FFFFFF;
		width: 600px;
		border: 3px solid #0DA9D0;
		border-radius: 12px;
		padding:0
	  
	}
	.modalPopup .header
	{
		background-color: #2FBDF1;
		height: 30px;
		color: White;
		line-height: 30px;
		text-align: center;
		font-weight: bold;
		border-top-left-radius: 6px;
		border-top-right-radius: 6px;
	}
	.modalPopup .body
	{
		min-height: 50px;
		line-height: 30px;
		text-align: center;
		font-weight: bold;
	}
	.modalPopup .footer
	{
		padding: 6px;
	}
	.modalPopup .yes, .modalPopup .no
	{
		height: 23px;
		color: White;
		line-height: 23px;
		text-align: center;
		font-weight: bold;
		cursor: pointer;
		border-radius: 4px;
	}
	.modalPopup .yes
	{
		background-color: #2FBDF1;
		border: 1px solid #0DA9D0;
	}
	.modalPopup .no
	{
		background-color: #9F9F9F;
		border: 1px solid #5C5C5C;
	}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<form runat="server">
  <div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-table"></i>User Log</h3>
			<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-table"></i>User Log</li>
			</ol>
	</div>

	<div class="row">
	   <div class="col-md-12 col-sm-12 col-xs-12">
		   <div class="panel panel-default">
			  <%--<div class="panel-heading" style="text-align:center">

			  </div>--%>

			  <div class="panel-body">
	<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
	</asp:ToolkitScriptManager>
	<div class="table-responsive">
		<table style="width:100%;">
		<tr>
			<td>
				&nbsp;</td>
			<td>
				&nbsp;</td>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
			<td>
				&nbsp;</td>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td style="text-align: right" width="40%">
				<asp:Label ID="lblDate" runat="server" Font-Bold="True" Font-Size="Small" 
					Text="Date"></asp:Label>
			</td>
			<td colspan="2">
				<asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" Width="150px" 
					AutoPostBack="True" ontextchanged="txtDate_TextChanged" class="form-control"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td style="text-align: right" width="40%">
				&nbsp;</td>
			<td colspan="2">
				&nbsp;</td>
		</tr>
		<tr>
			<td style="text-align: right">
				<asp:Label ID="lblTeam" runat="server" Font-Bold="True" Font-Size="Small" 
					Text="Team"></asp:Label>
			</td>
			<td colspan="2">
				<asp:DropDownList ID="ddlTeam" runat="server" AutoPostBack="True" 
					onselectedindexchanged="ddlTeam_SelectedIndexChanged" Width="150px" class="form-control">
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td style="text-align: right">
				&nbsp;</td>
			<td colspan="2">
				&nbsp;</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
			<td colspan="2" style="text-align: center">
				<asp:Button ID="btnExport" class="btn btn-info" runat="server" 
					Text="Export" onclick="btnExport_Click" />
				<asp:Button ID="btnSend" class="btn btn-info" runat="server" 
					Text="Send Mail" onclick="btnSend_Click" />
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
			<td colspan="2" style="text-align: center">
				&nbsp;</td>
		</tr>
		<tr>
			<td colspan="3">
			 
			   <asp:Panel ID="pnlResult" runat="server" HorizontalAlign="Center" 
					 Height="600px" ScrollBars="Vertical"></asp:Panel>
			 
			 </td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
			<td colspan="2">
				&nbsp;</td>
		</tr>
	</table>
	</div>
	<asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
	<asp:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="divPopUp">
	</asp:ModalPopupExtender>
	<div id="divPopUp" class="modalPopup" style="display: none">
	<div class="header">
	   Send Mail
	</div>
	<div class="body">
		<asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
		<table style="width: 100%;">
			<tr>
				<td style="text-align: right" width="40%">
					 Scheduled Time: 
				</td>
				<td style="text-align: left" colspan="2">
				   <cc1:dropdowncheckboxes ID="ddlMultipleTime" runat="server" 
									style="top: 0px; left: 0px; height: 16px; width: 202px"  >
									<Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="200" />
									<Texts SelectBoxCaption="" />
								</cc1:dropdowncheckboxes>
				</td>
			   
			</tr>
			<tr>
				<td style="text-align: right">
					Users: 
				</td>
				<td style="text-align: left" colspan="2">
					<cc1:dropdowncheckboxes ID="ddlEmails" runat="server" 
									style="top: 0px; left: 0px; height: 16px; width: 202px" >
								  <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="200" />
								  <Texts SelectBoxCaption="" />
								</cc1:dropdowncheckboxes>      
				</td>
				
			</tr>
			<tr>
				<td style="text-align: right">
					Notes: 
				</td>
				<td style="text-align: left" colspan="2">
					<textarea id="txtDetails" cols="25" rows="6" style="width: 350px" 
					 maxlength="512" runat="server" name="S1"></textarea>
				</td>
				
			</tr>
		</table>       
	</div>
	<div class="footer" align="right">
		<asp:Button ID="btnYes" runat="server" Text="Send" CssClass="yes" 
			onclick="btnYes_Click" />
		<asp:Button ID="btnNo" runat="server" Text="Cancel" CssClass="no" 
			onclick="btnNo_Click" />
	</div>
</div>
</div>
</div>
 </div>
	</div>
   
</form>

 <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/jquery-1.10.2.js" type="text/javascript"></script>
	<script src="http://code.jquery.com/ui/1.11.2/jquery-ui.js" type="text/javascript"></script>
	 <script type="text/javascript">
		 $(function () {
			 $("#txtDate").datepicker();
		 });

  </script>
  </asp:Content>