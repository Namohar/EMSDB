<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RTMComments.aspx.cs" Inherits="RTMComments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<form runat="server">
<div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-desktop"></i>Customize</h3>
	    	<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-desktop"></i>Customize</li>
			</ol>
	</div>

     <div  class="row" >
       <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
       <div class="col-md-4 col-sm-12 col-xs-12">
          <div class="panel panel-default">
             <div class="panel-heading">
               RTM Comments
             </div>
             <div class="panel-body">
               <div id ="divComments" runat="server" style="float:left">
          
                  <asp:Label ID="lblTeam" runat="server" Text="Select Team:"></asp:Label>
                  <asp:DropDownList ID="ddlTeam" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="ddlTeam_SelectedIndexChanged" Width="200px">
                            </asp:DropDownList><br /><br />
                  <asp:Label ID="Label1" runat="server" Text="Is Comments Mandatory:"></asp:Label><br />
                  <asp:RadioButton ID="rbYes" runat="server" GroupName="comm" Text="Yes" />
            &nbsp;<asp:RadioButton ID="rbNo" runat="server" Checked="True" GroupName="comm" Text="No" /><br /><br />
                  <asp:Button ID="btnSubmit" class="btn btn-primary" runat="server" Text="Submit" 
                                onclick="btnSubmit_Click" />
                </div>
             </div>
          </div>
       </div>

       <div class="col-md-4 col-sm-12 col-xs-12">
          <div class="panel panel-default">
             <div class="panel-heading">
               RTM Task Duration reminder
             </div>
             <div class="panel-body">
                <div id="divReminder" runat="server" style="float:left">
                    <asp:Label ID="lblEmp" runat="server" Text="Select Employee:"></asp:Label>
                    <asp:DropDownList ID="ddlEmp" runat="server" AutoPostBack="True" onselectedindexchanged="ddlEmp_SelectedIndexChanged" 
                       >
                    </asp:DropDownList><br /><br />
                    <asp:Label ID="lblReminder" runat="server" Text="Is Hourly Reminder (Task Duration) Mandatory:"></asp:Label>
                     <asp:RadioButton ID="rbRemYes" runat="server" GroupName="rem" Text="Yes" />
            &nbsp;<asp:RadioButton ID="rbRemNo" runat="server" Checked="True" GroupName="rem" Text="No" /><br /><br />
      
                    <asp:Button ID="btnReminder" class="btn btn-primary" runat="server" 
                        Text="Submit" onclick="btnReminder_Click" />
                </div>
             </div>
          </div>
       </div>

       <div class="col-md-4 col-sm-12 col-xs-12">
          <div class="panel panel-default">
             <div class="panel-heading">
               Customize Task Duration reminder
             </div>
             <div class="panel-body">
                 <div id ="divCustomize" runat="server" style="float:left">
                        <asp:Label ID="lblCustEmp" runat="server" Text="Select Employee:"></asp:Label>
                        <asp:DropDownList ID="ddlCustEmp" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlEmp_SelectedIndexChanged" Width="200px">
                        </asp:DropDownList><br /><br />
                        <asp:Label ID="lblDuration" runat="server" Text="Duration:"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlDuration" runat="server" Width="200px">
                            <asp:ListItem>--Select Duration--</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>45</asp:ListItem>
                            <asp:ListItem>60</asp:ListItem>
                        </asp:DropDownList>
                        <strong><span class="style4">Mins<br />
                        <br />
                        <asp:Button ID="btnCustomize" class="btn btn-primary" runat="server" 
                            Text="Submit" onclick="btnCustomize_Click" />
                        </span></strong></div><br /><br />
                    </div>
             </div>
          </div>
       </div>
    
</form>
</asp:Content>

