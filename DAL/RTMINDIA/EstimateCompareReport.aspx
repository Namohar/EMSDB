<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EstimateCompareReport.aspx.cs" Inherits="EstimateCompareReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<form runat="server">
<div class="row">
	 <div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-clock-o"></i>Estimate Comparision</h3>
	    	<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-clock-o"></i>Estimate Comparision</li>
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
           <td>
               
               <asp:Label ID="lblid" runat="server" Visible="False"></asp:Label>
               
           </td>
           <td>
                <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
           </td>
           <td>
               &nbsp;
           </td>
       </tr>
       <tr>
           <td style="text-align: right" width="40%">
              
    <asp:Label ID="lblEmp" runat="server" Text="Employee" 
    Font-Bold="True"></asp:Label>
              
           </td>
           <td>
               
                <asp:DropDownList ID="ddlEmp" runat="server" Width="200px" class="form-control">     
     </asp:DropDownList>
               
           </td>
           <td>
               &nbsp;
           </td>
       </tr>
       <tr>
           <td style="text-align: right" width="40%">
              
               &nbsp;</td>
           <td>
               
                &nbsp;</td>
           <td>
               &nbsp;</td>
       </tr>
       <tr>
           <td style="text-align: right">
              
     <asp:Label ID="lblDate" runat="server" Text="Select Date" Font-Bold="True"></asp:Label>
              
           </td>
           <td>
        <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" Width="200px" class="form-control"></asp:TextBox>
           </td>
           <td>
               &nbsp;</td>
       </tr>
       <tr>
           <td style="text-align: right">
              
               &nbsp;</td>
           <td>
               &nbsp;</td>
           <td>
               &nbsp;</td>
       </tr>
       <tr>
           <td style="text-align: right">
              
               &nbsp;</td>
           <td>
       
     <asp:Button ID="btnDisplay" runat="server" class="btn btn-primary" Font-Bold="True" Text="Display" onclick="btnDisplay_Click" />
               </td>
           <td>
               &nbsp;</td>
       </tr>
   </table>
   <br />
   <div class="table-responsive">
     <asp:GridView ID="gvEstimate" runat="server" AutoGenerateColumns="False" 
                   Width="100%" 
           class="table table-striped table-bordered table-hover">
                   <Columns>
                       <asp:BoundField DataField="Employee Name" HeaderText="Employee Name" >
                       <HeaderStyle HorizontalAlign="Center" />
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                       <asp:BoundField DataField="Date" HeaderText="Date" >
                       <HeaderStyle HorizontalAlign="Center" />
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                       <asp:BoundField DataField="Client" HeaderText="Client" >
                       <HeaderStyle HorizontalAlign="Center" />
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                       <asp:BoundField DataField="Task" HeaderText="Task" >
                       <HeaderStyle HorizontalAlign="Center" />
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                       <asp:BoundField DataField="Subtask" HeaderText="Sub Task" >
                       <HeaderStyle HorizontalAlign="Center" />
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                       <asp:BoundField DataField="User Estimate" HeaderText="User Estimate" >
                       <HeaderStyle HorizontalAlign="Center" />
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:BoundField>
                       <asp:BoundField DataField="RTM Time" HeaderText="RTM Time" >
                       <HeaderStyle HorizontalAlign="Center" />
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
        $("#txtDate").datepicker({ maxDate: -1 });
    });
  </script>
</asp:Content>

