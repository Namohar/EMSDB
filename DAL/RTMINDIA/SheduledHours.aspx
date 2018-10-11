<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SheduledHours.aspx.cs" Inherits="SheduledHours" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript" src="Scripts/jquery.timepicker.js"></script>
    <link rel="stylesheet" type="text/css" href="Styles/jquery.timepicker.css" />
    <style type="text/css">
        #timepicker
        {
            width: 99px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<form runat="server">
   <div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-eye"></i>Roster</h3>
	    	<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-eye"></i>Roster</li>
			</ol>
	</div>

    <div class="row">
       <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
               <div class="panel-body">
                    <div style="Width:100%; text-align: center;">
                         <asp:Label ID="lblTeam" runat="server" Text="Select Team:" Font-Bold="True"></asp:Label>
                        &nbsp;
                        <asp:DropDownList ID="ddlTeam" runat="server" AutoPostBack="True" 
                             onselectedindexchanged="ddlTeam_SelectedIndexChanged" Width="200px">
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                        <br />
                        <br />
                        <asp:GridView ID="gvEmployee" runat="server" AutoGenerateColumns="False" 
                            Width="100%" onrowcancelingedit="gvEmployee_RowCancelingEdit" 
                            onrowediting="gvEmployee_RowEditing" onrowupdating="gvEmployee_RowUpdating" class="table table-striped table-bordered table-hover">
                            <Columns>
                                <asp:TemplateField HeaderText="Employee ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("UL_Employee_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("UL_User_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Scheduled Login">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSCHLogin" runat="server" Text='<%# Eval("UL_SCH_Login") %>' ClientIDMode="Static" class="time"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSCHLogin" runat="server" Text='<%# Eval("UL_SCH_Login") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Scheduled Logout">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSCHLogout" runat="server" 
                                            Text='<%# Eval("UL_SCH_Logout") %>' ClientIDMode="Static" class="time"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSCHLogout" runat="server" Text='<%# Eval("UL_SCH_Logout") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True" />
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
         $('#txtSCHLogin').timepicker({ 'step': 15 });
     });

     $(function () {
         $('#txtSCHLogout').timepicker({ 'step': 15 });
     });
    </script>

</asp:Content>

