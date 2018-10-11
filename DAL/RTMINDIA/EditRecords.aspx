<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EditRecords.aspx.cs" Inherits="EditRecords" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        #timepicker
        {
            width: 99px;
        }
    </style>
    <style type="text/css">
        .style1
        {
            height: 217px;
        }
        .modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=60);
        opacity: 0.6;
    }
    .styled-button-2 {
	-webkit-box-shadow:rgba(0,0,0,0.2) 0 1px 0 0;
	-moz-box-shadow:rgba(0,0,0,0.2) 0 1px 0 0;
	box-shadow:rgba(0,0,0,0.2) 0 1px 0 0;
	border-bottom-color:#333;
	border:1px solid #61c4ea;
	background-color:#7cceee;
	border-radius:5px;
	-moz-border-radius:5px;
	-webkit-border-radius:5px;
	color:#333;
	font-family:'Verdana',Arial,sans-serif;
	font-size:14px;
	text-shadow:#b2e2f5 0 1px 0;
	padding:5px
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
  <div class="row">
	 <div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-edit"></i>Edit Records</h3>
	    	<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-pencil-square-o"></i>Edit Records</li>
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
               <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
  <ContentTemplate>
                <table style="width: 100%;" >
         <tr>
             <td colspan="2">
                 &nbsp;
                 &nbsp;
                 &nbsp;
                 <asp:Label ID="lblError" runat="server" Font-Bold="True"></asp:Label>
             </td>
         </tr>
         <tr>
             <td style="text-align: right" width="40%">
                 <asp:Label ID="lblEmployee" runat="server" Font-Bold="True" Text="Employee:"></asp:Label>
             </td>
             <td>
                 <asp:DropDownList ID="ddlEmp" runat="server" Width="150px" class="form-control">
                 </asp:DropDownList>
                 <asp:Label ID="lblid" runat="server" Visible="False"></asp:Label>
             </td>
         </tr>
                    <tr>
                        <td style="text-align: right" width="40%">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
         <tr>
             <td style="text-align: right">
                 <asp:Label ID="lblDate" runat="server" Font-Bold="True" Text="Date:"></asp:Label>
             </td>
             <td>
                <div> <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" Width="150px" class="txtDate form-control"></asp:TextBox></div>
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
                 <asp:Button ID="btnDisplay" runat="server" onclick="btnDisplay_Click" 
                     Text="Display" class="btn btn-primary" />
             </td>
         </tr>
         <tr>
             <td style="text-align: right">
                 &nbsp;</td>
             <td style="text-align: right">
                
             </td>
         </tr>
     </table>
     <div class="table-responsive">
         <asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="False" 
            onrowcancelingedit="gvRecords_RowCancelingEdit" 
            onrowdatabound="gvRecords_RowDataBound" 
            onrowediting="gvRecords_RowEditing" 
            onrowupdating="gvRecords_RowUpdating" Width="100%" 
            DataKeyNames="R_ID" class="table table-striped table-bordered table-hover">
            <Columns>
                <asp:TemplateField HeaderText="Employee">
                    <ItemTemplate>
                        <asp:Label ID="lblUser" runat="server" Text='<%# Eval("R_User_Name") %>'></asp:Label>
                    </ItemTemplate>
                   
                    <ItemStyle HorizontalAlign="Left" />
                   
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Client">
                    <ItemTemplate>
                        <asp:Label ID="lblClient" runat="server" Text='<%# Eval("CL_ClientName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="lblEditClient" runat="server" Text='<%# Eval("CL_ClientName") %>' Visible ="false"></asp:Label>
                        <asp:DropDownList ID="ddlGvClient" runat="server" Width="200px">
                        </asp:DropDownList>
                    </EditItemTemplate>
                   
                    <ItemStyle HorizontalAlign="Left" />
                   
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Task">
                    <ItemTemplate>
                    <asp:Label ID="lblTask" runat="server" Text='<%# Eval("TL_Task") %>'></asp:Label>  
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="lblEditTask" runat="server" Text='<%# Eval("TL_Task") %>' Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlGvTask" runat="server" Width="200px" 
                            AutoPostBack="True" onselectedindexchanged="ddlGvTask_SelectedIndexChanged">
                        </asp:DropDownList>
                    </EditItemTemplate>
                   
                    <ItemStyle HorizontalAlign="Left" />
                   
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Duration">
                    <ItemTemplate>
                        <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("R_Duration") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Comments">
                    <ItemTemplate>
                        <asp:Label ID="lblComments" runat="server" Text='<%# Eval("R_Comments") %>'></asp:Label>
                    </ItemTemplate>
                   
                    <EditItemTemplate>
                        <asp:TextBox ID="txtComments" runat="server" Text='<%# Eval("R_Comments") %>' 
                            Width="150px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                   
                </asp:TemplateField>
               
                <asp:CommandField HeaderText="Edit" ShowEditButton="True" >
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Left" />
                </asp:CommandField>
                <asp:TemplateField HeaderText="Insert">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkInsert" runat="server" onclick="lnkInsert_Click">Split</asp:LinkButton>
                    </ItemTemplate>
                  
                    <ItemStyle HorizontalAlign="Left" />
                  
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
     </div>

     <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
    <asp:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="divPopUp">
    </asp:ModalPopupExtender>
    <div id="divPopUp" class="modalPopup" style="display: none">
    <div class="header">
       Split the record
    </div>
    <div class="body">
        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
        <table style="width: 100%;">
            <tr>
                <td style="text-align: left" colspan="3">
                    <asp:Label ID="lblPopError" runat="server" Text=""></asp:Label>
                </td>
               
            </tr>
            <tr>
                <td style="text-align: right" width="40%">
                     Client:
                </td>
                <td style="text-align: left" colspan="2">
                    <asp:DropDownList ID="ddlNewClient" runat="server" Width="200px">
                    </asp:DropDownList>
                </td>
               
            </tr>
            <tr>
                <td style="text-align: right">
                    Task:
                </td>
                <td style="text-align: left" colspan="2">
                    <asp:DropDownList ID="ddlNewTask" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlNewTask_SelectedIndexChanged" Width="200px">
                    </asp:DropDownList>
                </td>
                
            </tr>
            <tr>
                <td style="text-align: right">
                   Sub Task:
                </td>
                <td style="text-align: left" colspan="2">
                    <asp:DropDownList ID="ddlNewSubTask" runat="server" Width="200px">
                    </asp:DropDownList>
                </td>
            </tr>
            
             <tr>
                <td style="text-align: right">
                   Duration:
                </td>
                <td style="text-align: left" colspan="2">
                    <asp:TextBox ID="txtDuration" runat="server" placeholder="HH:MM:SS" ></asp:TextBox> 
                </td>
            </tr>
            <tr>
               <td style="text-align: right"></td>
               <td style="text-align: left" colspan="2">
                 <asp:Label ID="lblMainDuration" runat="server" Text=""></asp:Label>
               </td>
            </tr>

             <tr>
                <td style="text-align: right">
                   Comments:
                </td>
                <td style="text-align: left" colspan="2">
                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" MaxLength="200" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left" colspan="3">
                    <asp:Label ID="lblTeamId" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblEmpID" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblUserName" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblStartTime" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblEndTime" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblOldRId" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblOldDuration" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblProcess" runat="server" Text="" Visible="false"></asp:Label>
                </td>
                
            </tr>
        </table>       
        </div>
        <div class="footer" align="right">
            <asp:Button ID="btnYes" runat="server" Text="Insert" CssClass="yes" 
                onclick="btnYes_Click" />
            <asp:Button ID="btnNo" runat="server" Text="Cancel" CssClass="no" 
                onclick="btnNo_Click" />
        </div>
    </div>

    </ContentTemplate>
   </asp:UpdatePanel>
              </div>
            </div>
        </div>
    </div>
</form>
<script type="text/javascript">
    function pageLoad(sender, args) {
        if (args.get_isPartialLoad()) {
            $(".txtDate").datepicker({ maxDate: new Date(), minDate: '-10' });
        }
    }
    $(function () {
        $(".txtDate").datepicker({ maxDate: new Date(), minDate: '-10' });
    });
  </script>
</asp:Content>

