<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="InternalMetrics.aspx.cs" Inherits="InternalMetrics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
  <script type="text/javascript">
      $(function () {
          $("#txtFrom").datepicker({ maxDate: new Date(), minDate: '-60' });
      });

      $(function () {
          $("#txtTo").datepicker({ maxDate: new Date() });
      });
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<form runat="server">
   <div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-table"></i>Internal Metrics</h3>
	    	<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-table"></i>Internal Metrics</li>
			</ol>
	</div>

    <div class="row">
       <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
              <div class="panel-heading" style="text-align:center">
                  <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                  <br />
                  <asp:Label ID="lblFrom" runat="server" Font-Bold="True" Text="From Date:"></asp:Label>
                  <asp:TextBox ID="txtFrom" runat="server" Width="200px" ClientIDMode="Static" ></asp:TextBox>
                  &nbsp;&nbsp;&nbsp;&nbsp;
                  <asp:Label ID="lblTo" runat="server" Font-Bold="True" Text="To Date:"></asp:Label>
                  <asp:TextBox ID="txtTo" runat="server" Width="200px" ClientIDMode="Static" ></asp:TextBox>
                  &nbsp;&nbsp;&nbsp;&nbsp;
                  <asp:Button ID="btnGet" runat="server" Text="Get Details" Class="btn btn-primary" 
                        onclick="btnGet_Click"/>
              </div>
              <div class="panel-body">
                  <div style="text-align: center" id="dropDiv" runat="server" visible="false">
    <asp:Label ID="lblSelect" runat="server" Text="0" Visible="False"></asp:Label>
    <asp:Label ID="lblTeam" runat="server" Text="Team" Font-Bold="True"></asp:Label>
    <asp:DropDownList ID="ddlTeam" runat="server" Width="150px" AutoPostBack="True" 
        onselectedindexchanged="ddlTeam_SelectedIndexChanged">
    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblClient" runat="server" Text="Client" Font-Bold="True"></asp:Label>
    <asp:DropDownList ID="ddlClient" runat="server" Width="150px" 
        AutoPostBack="True" onselectedindexchanged="ddlClient_SelectedIndexChanged">
    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblUser" runat="server" Text="Users" Font-Bold="True"></asp:Label>
    <asp:DropDownList ID="ddlUsers" runat="server" Width="150px" 
        AutoPostBack="True" onselectedindexchanged="ddlUsers_SelectedIndexChanged">
    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblTask" runat="server" Text="Task" Font-Bold="True"></asp:Label>
    <asp:DropDownList ID="ddlTask" runat="server" Width="150px" AutoPostBack="True" 
        onselectedindexchanged="ddlTask_SelectedIndexChanged">
    </asp:DropDownList>
    <br />
    <br />
    <asp:Button ID="btnDisplay" runat="server" Text="Display" 
                 Class="btn btn-primary" onclick="btnDisplay_Click" />
 &nbsp;&nbsp;
    <asp:Button ID="btnExport" runat="server" Text="Export" 
                 Class="btn btn-primary" onclick="btnExport_Click"  />
                 &nbsp;&nbsp;
    <asp:Button ID="btnReset" runat="server" Text="Reset" 
                 Class="btn btn-primary" onclick="btnReset_Click"   />
</div>
<br />
<br />
<div style="text-align: right; width: 100%;">
            <asp:Label ID="lblTotalDur" runat="server" Font-Bold="True" Font-Size="Medium" 
                Text="Total Duration:" Visible="False"></asp:Label>
        <asp:Label ID="lblTotal" runat="server" Font-Bold="True" Font-Size="Medium" 
                Visible="False" ForeColor="Black"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
    <asp:Panel ID="Panel1" runat="server" Height="500px" ScrollBars="Both">
    
        <asp:GridView ID="gvDisplay" runat="server" AutoGenerateColumns="False" 
            Width="100%" class="table table-striped table-bordered table-hover">
            <Columns>
                <asp:BoundField DataField="T_TeamName" HeaderText="Team" >
                
                </asp:BoundField>
                <asp:BoundField DataField="CL_ClientName" HeaderText="Client" >
                
                </asp:BoundField>
                <asp:BoundField DataField="R_User_Name" HeaderText="User" >
                
                </asp:BoundField>
                <asp:BoundField DataField="STL_ServiceCode" HeaderText="Service Code" >
                
                </asp:BoundField>
                <asp:BoundField DataField="duration" HeaderText="Duration" >
                
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
              </div>
           </div>
       </div>
    </div>
</form>
</asp:Content>

