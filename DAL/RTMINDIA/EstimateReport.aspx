<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EstimateReport.aspx.cs" Inherits="EstimateReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <form runat="server">
  <div class="row">
	 <div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-clock-o"></i> Resource Estimation Report</h3>
	    	<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-clock-o"></i>Resource Estimation Report</li>
			</ol>
	 </div>
   </div>

   <div class="row">
     <div class="col-md-12 col-sm-12 col-xs-12">
       <div class="panel panel-default">
             <%-- <div class="panel-heading" style="text-align:center">
                  <asp:Label ID="lblPageHeading" runat="server" Text="Resource Estimation"></asp:Label>
              </div>--%>
         <div class="panel-body">
             <table style="width: 100%;">
                 <tr>
                     <td style="text-align: right">
                         <asp:Label ID="lblid" runat="server" Visible="False"></asp:Label></td>
                     <td>
                         <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                     </td>
                 </tr>
                 <tr>
                     <td style="text-align: right">
                        <asp:Label ID="lblEmp" runat="server" Text="Employee" Font-Bold="True"></asp:Label>
                     </td>
                     <td>
                         <asp:DropDownList ID="ddlEmp" runat="server" Width="200px" class="form-control">     
                         </asp:DropDownList>
                     </td>
                 </tr>
               
                 <tr>
                     <td style="text-align: right">
                     <br />
                       <asp:Label ID="lblDate" runat="server" Text="Select Date" Font-Bold="True"></asp:Label>
                     </td>
                     <td>
                     <br />
                        <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" Width="200px" class="form-control"></asp:TextBox>
                     </td>
                 </tr>
                
                 <tr>
                     <td style="text-align: right">
                         &nbsp;</td>
                     <td>
                     <br />
                         <asp:Button ID="btnDisplay" runat="server" class="btn btn-primary" 
     Font-Bold="True" Text="Display" onclick="btnDisplay_Click" /></td>
                 </tr>
             </table>
             <br />
             <div style="width:100%; text-align:right">
             <asp:Label ID="lblTotalEstimate" runat="server" Font-Bold="True" 
                   Text="Total Duration Estimated:"></asp:Label>&nbsp;
                <asp:Label ID="lblTotal" runat="server" Font-Bold="True"></asp:Label>
             </div>
             <div class="table-responsive">
               <asp:GridView ID="gvEstimate" runat="server" AutoGenerateColumns="False" 
                     Width="100%" class="table table-striped table-bordered table-hover">
                   <Columns>
                       <asp:BoundField DataField="EST_UserName" HeaderText="Employee Name">
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                       <asp:BoundField DataField="EST_Date" HeaderText="Date">
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                       <asp:BoundField DataField="CL_ClientName" HeaderText="Client">
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                       <asp:BoundField DataField="TL_Task" HeaderText="Task">
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                       <asp:BoundField DataField="STL_SubTask" HeaderText="SubTask">
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                       <asp:BoundField DataField="EST_Duration" HeaderText="User Estimate">
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                   </Columns>
               </asp:GridView>
             </div>
         </div>
      </div>
    </div>
    </div>
</form>
<script type="text/javascript">
    $(function () {
        $("#txtDate").datepicker();
    });
  </script>
</asp:Content>

