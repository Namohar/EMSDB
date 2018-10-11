<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Estimate.aspx.cs" Inherits="Estimate" %>
<%@ Register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 125px;
        }
        .style2
        {
            width: 206px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <form runat="server">
   <div class="row">
	 <div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-clock-o"></i> Resource Estimation</h3>
	    	<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-clock-o"></i>Resource Estimation</li>
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
                <div class="row">
                  <div class="col-lg-12">
                     <form role="form">
                       <div class="form-group">
                           <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#003399" Text="Please input your estimate here"></asp:Label>
                           <table style="width: 100%;">
                               <tr>
                                   <td class="style1">
                                       <asp:Label ID="lblid" runat="server" Text="" Visible="false"></asp:Label>  </td>
                                   <td class="style2">
                                      <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label></td>
                                   <td>
                                       &nbsp;</td>
                               </tr>
                               <tr>
                                   <td style="text-align: right" class="style1">
                                        <asp:Label ID="lblEmp" runat="server" Text="Employee:" Font-Bold="True"></asp:Label>
                                   </td>
                                   <td class="style2">
                                       <asp:DropDownList ID="ddlEmp" runat="server" Width="200px" AutoPostBack="True" onselectedindexchanged="ddlEmp_SelectedIndexChanged" class="form-control">     
                                       </asp:DropDownList>
                                   </td>
                                   <td rowspan="9" style="vertical-align: top">
                                       <asp:GridView ID="gvPreviousEstimates" runat="server" 
                                    AutoGenerateColumns="False" Width="100%" CellPadding="4" 
                                    ForeColor="#333333" GridLines="None" HorizontalAlign="Center">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClientName" runat="server" 
                                                    Text='<%# Eval("CL_ClientName") %>'></asp:Label>
                                                <asp:Label ID="lblClientId" runat="server" Text='<%# Eval("EST_ClientId") %>' 
                                                    Visible="False"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Task">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskName" runat="server" Text='<%# Eval("TL_Task") %>'></asp:Label>
                                                <asp:Label ID="lblTaskId" runat="server" Text='<%# Eval("EST_TaskId") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Task">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubTaskName" runat="server" Text='<%# Eval("STL_SubTask") %>'></asp:Label>
                                                <asp:Label ID="lblSubTaskId" runat="server" Text='<%# Eval("EST_SubTaskId") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Duration">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNewDuration" runat="server" placeholder="HH:MM:SS" Enabled="false" MaxLength="8"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comments">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNewComments" runat="server" Width="200px" Enabled="false" MaxLength="350"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>
                                <br />
                                <br />
                                <asp:Button ID="btnAdd" runat="server" Text="Estimate" class="btn btn-primary" onclick="btnAdd_Click" />
                                   </td>
                               </tr>
                               <tr>
                                   <td style="text-align: right" class="style1">
                                      <asp:Label ID="lblClient" runat="server" Text="Client:" Font-Bold="True"></asp:Label>
                                   </td>
                                   <td class="style2">
                                       <asp:DropDownList ID="ddlClient" runat="server" Width="200px" class="form-control">
                                       </asp:DropDownList>
                                   </td>
                               </tr>
                               <tr>
                                   <td style="text-align: right" class="style1">
                                       <asp:Label ID="lblTask" runat="server" Text="Task:" Font-Bold="True"></asp:Label></td>
                                   <td class="style2">
                                       <asp:DropDownList ID="ddlTask" runat="server" Width="200px" AutoPostBack="True" onselectedindexchanged="ddlTask_SelectedIndexChanged" class="form-control">
                                       </asp:DropDownList></td>
                               </tr>
                               <tr>
                                   <td style="text-align: right" class="style1">
                                       <asp:Label ID="lblSubTask" runat="server" Text="Sub Task:" Font-Bold="True"></asp:Label></td>
                                   <td class="style2">
                                       <asp:DropDownList ID="ddlSubTask" runat="server" Width="200px" AutoPostBack="True" onselectedindexchanged="ddlSubTask_SelectedIndexChanged" class="form-control">
                                       </asp:DropDownList></td>
                               </tr>
                               <tr>
                                   <td style="text-align: right" class="style1">
                                       <asp:Label ID="lblMultiple" runat="server" Font-Bold="True" Text="Select Clients:" Visible="False"></asp:Label></td>
                                   <td class="style2">
                                       <cc1:dropdowncheckboxes ID="ddlMultiple" runat="server" class="form-control"
                                            style="top: 0px; left: 0px; height: 18px; width: 202px" Visible="False" Font-Size="Small" 
                                           >
                                            <Style SelectBoxWidth="200" DropDownBoxBoxWidth="350" DropDownBoxBoxHeight="130" />
                                         </cc1:dropdowncheckboxes></td>
                               </tr>
                               <tr>
                                   <td style="text-align: right" class="style1">
                                       <asp:Label ID="lblDate" runat="server" Text="Select Date:" Font-Bold="True"></asp:Label></td>
                                   <td class="style2">
                                      <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" Width="200px" class="form-control"></asp:TextBox></td>
                               </tr>
                               <tr>
                                   <td style="text-align: right" class="style1">
                                       <asp:Label ID="lblTime" runat="server" Text="Duration:" Font-Bold="True"></asp:Label></td>
                                   <td class="style2">
                                       <asp:TextBox ID="txtTime" runat="server" Width="200px" placeholder="HH:MM:SS" class="form-control" CausesValidation="True" ValidationGroup="val"></asp:TextBox></td>
                               </tr>
                               <tr>
                                   <td style="text-align: right" class="style1">
                                       <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Comments:"></asp:Label></td>
                                   <td class="style2">
                                       <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Width="200px" MaxLength="250" class="form-control"></asp:TextBox></td>
                               </tr>
                               <tr>
                                 <td>
                                 
                                 </td>
                                 <td class="style2">
                                    <asp:Button ID="Button1" runat="server" class="btn btn-primary" Font-Bold="True" Text="Submit" onclick="Button1_Click" />
                                 </td>
                               </tr>
                           </table>
                           
                         </div>
                      </form>
                     </div>


                     <%--<div class="col-lg-8">
                       <form role="form">
                         <div class="form-group">
                         <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#003399" 
                         Text="Or, you can also choose from your past choices!"></asp:Label>
                         <br />
                             <asp:Label ID="lblId2" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:GridView ID="gvPreviousEstimates" runat="server" 
                                    AutoGenerateColumns="False" Width="100%" CellPadding="4" 
                                    ForeColor="#333333" GridLines="None" HorizontalAlign="Center">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClientName" runat="server" 
                                                    Text='<%# Eval("CL_ClientName") %>'></asp:Label>
                                                <asp:Label ID="lblClientId" runat="server" Text='<%# Eval("EST_ClientId") %>' 
                                                    Visible="False"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Task">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskName" runat="server" Text='<%# Eval("TL_Task") %>'></asp:Label>
                                                <asp:Label ID="lblTaskId" runat="server" Text='<%# Eval("EST_TaskId") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Task">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubTaskName" runat="server" Text='<%# Eval("STL_SubTask") %>'></asp:Label>
                                                <asp:Label ID="lblSubTaskId" runat="server" Text='<%# Eval("EST_SubTaskId") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Duration">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNewDuration" runat="server" placeholder="HH:MM:SS" Enabled="false" MaxLength="8"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comments">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNewComments" runat="server" Width="200px" Enabled="false" MaxLength="350"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>
                                <br />
                                <br />
                                <asp:Button ID="btnAdd" runat="server" Text="Estimate" class="btn btn-primary" onclick="btnAdd_Click" />
                         </div>
                       </form>
                     </div>--%>
                </div>
                <br />
                <br />
                <div class="table-responsive">
                 <asp:GridView ID="gvEstimateNew" runat="server" AutoGenerateColumns="False" 
            onrowcancelingedit="gvEstimateNew_RowCancelingEdit" 
            onrowdatabound="gvEstimateNew_RowDataBound" 
            onrowediting="gvEstimateNew_RowEditing" 
            onrowupdating="gvEstimateNew_RowUpdating" Width="100%" 
            DataKeyNames="EST_ID" class="table table-striped table-bordered table-hover">
            <Columns>
                <asp:TemplateField HeaderText="Employee">
                    <ItemTemplate>
                        <asp:Label ID="lblUser" runat="server" Text='<%# Eval("EST_UserName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Task">
                    <ItemTemplate>
                        <asp:Label ID="lblTask" runat="server" Text='<%# Eval("TL_Task") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlGvTask" runat="server" Width="150px" 
                            AutoPostBack="True" onselectedindexchanged="ddlGvTask_SelectedIndexChanged">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SubTask">
                    <ItemTemplate>
                        <asp:Label ID="lblSubTask" runat="server" Text='<%# Eval("STL_SubTask") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlGvSubTask" runat="server" Width="150px" AutoPostBack="True" onselectedindexchanged="ddlGvSubTask_SelectedIndexChanged">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Client">
                    <ItemTemplate>
                        <asp:Label ID="lblClient" runat="server" Text='<%# Eval("CL_ClientName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlGvClient" runat="server" Width="150px">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Duration">
                    <ItemTemplate>
                        <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("EST_Duration") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDuration" runat="server" Text='<%# Eval("EST_Duration") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Comments">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditComm" runat="server" Text='<%# Eval("EST_Comments") %>' 
                            TextMode="MultiLine" MaxLength="350"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblComments" runat="server" Text='<%# Eval("EST_Comments") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("EST_Date") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField HeaderText="Edit" ShowEditButton="True" />
                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkRemove" runat="server"
                            CommandArgument = '<%# Eval("EST_ID")%>'
                         OnClientClick = "return confirm('Do you want to delete?')"
                        Text = "Delete" OnClick = "DeleteRecord"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
              </div>
           </div>
      </div>
    </div>
    
</form>
<script src="http://code.jquery.com/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.11.2/jquery-ui.js" type="text/javascript"></script>
    
<script type="text/javascript">
    $(function () {
        $("#txtDate").datepicker({ minDate: new Date() });
    });

    $(function () {
        $("[id*=chkRow]").bind("click", function () {

            //Find and reference the GridView.
            var grid = $(this).closest("table");

            //Find and reference the Header CheckBox.
            //var chkHeader = $("[id*=chkHeader]", grid);

            //If the CheckBox is Checked then enable the TextBoxes in thr Row.
            if (!$(this).is(":checked")) {
                var td = $("td", $(this).closest("tr"));
                td.css({ "background-color": "#FFF" });
                $("input[type=text]", td).attr("disabled", "disabled");
            } else {
                var td = $("td", $(this).closest("tr"));
                td.css({ "background-color": "#D8EBF2" });
                $("input[type=text]", td).removeAttr("disabled");
            }

            //Enable Header Row CheckBox if all the Row CheckBoxes are checked and vice versa.
            if ($("[id*=chkRow]", grid).length == $("[id*=chkRow]:checked", grid).length) {
                chkHeader.attr("checked", "checked");
            } else {
                chkHeader.removeAttr("checked");
            }
        });
    });
  </script>
  </asp:Content>