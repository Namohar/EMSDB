<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ReportHome.aspx.cs" Inherits="ReportHome" %>
<%@ Register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <form runat="server">
   <div class="row">
	 <div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-table"></i> Real Time Reports</h3>
	    	<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-table"></i>Real Time Reports</li>
			</ol>
	</div>
  </div>

  <div class="row">
       <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
              <div class="panel-heading" style="text-align:center">
               <asp:RadioButton ID="rbRealTime" runat="server" Checked="True" Font-Bold="True" 
                    Font-Size="Medium" GroupName="RTM" Text="Real Time" AutoPostBack="True" 
                    oncheckedchanged="rbRealTime_CheckedChanged" />
                <asp:RadioButton ID="rbSummary" runat="server" Font-Bold="True" 
                    Font-Size="Medium" GroupName="RTM" Text="Summary Report" 
                    AutoPostBack="True" oncheckedchanged="rbSummary_CheckedChanged" />
                <asp:RadioButton ID="rbClient" runat="server" Font-Bold="True" 
                    Font-Size="Medium" GroupName="RTM" Text="By Client" AutoPostBack="True" 
                    oncheckedchanged="rbClient_CheckedChanged" Visible="False" />
                <asp:RadioButton ID="rbTask" runat="server" Font-Bold="True" Font-Size="Medium" 
                    GroupName="RTM" Text="By Task" AutoPostBack="True" 
                    oncheckedchanged="rbTask_CheckedChanged" Visible="False" />
                <asp:RadioButton ID="rbCompleteLog" runat="server" Font-Bold="True" 
                    Font-Size="Medium" GroupName="RTM" Text="Complete Log" AutoPostBack="True" 
                    oncheckedchanged="rbCompleteLog_CheckedChanged" Visible="False" />&nbsp;<asp:RadioButton 
                    ID="rbTsheet" runat="server" AutoPostBack="True" Font-Bold="True" 
                    Font-Size="Medium" GroupName="RTM" oncheckedchanged="rbTsheet_CheckedChanged" 
                    Text="TSheet Report" />
                &nbsp;<asp:RadioButton ID="rbIncorrect" runat="server" AutoPostBack="True" 
                    Font-Bold="True" Font-Size="Medium" GroupName="RTM" 
                    oncheckedchanged="rbIncorrect_CheckedChanged" Text="Incomplete Records" />
                &nbsp;<asp:RadioButton ID="rbEarlyLogoff" runat="server" AutoPostBack="True" GroupName="RTM" 
                    Font-Bold="True" Font-Size="Medium" 
                    oncheckedchanged="rbEarlyLogoff_CheckedChanged" Text="Early Logoff" />
                <asp:RadioButton ID="rbLeave" runat="server" Font-Bold="True" 
                    Font-Size="Medium" GroupName="RTM" oncheckedchanged="rbLeave_CheckedChanged" 
                    Text="Leave Records" AutoPostBack="True" Visible="False" />
              </div>
              <div class="panel-body">
                    <div style="text-align:center; width: 100%;"> 
                       <asp:RadioButton ID="rbUser" runat="server" AutoPostBack="True" Checked="True" 
                            Font-Bold="True" GroupName="user" oncheckedchanged="rbUser_CheckedChanged" 
                            Text="User" />
                        <asp:RadioButton ID="rbTeam" runat="server" AutoPostBack="True" 
                            Font-Bold="True" GroupName="user" oncheckedchanged="rbTeam_CheckedChanged" 
                            Text="Team" />
                    </div>  
                    <br />
                  <table style="width: 100%;">
                      <tr>
                          <td colspan="2">
                              <asp:Label ID="lblError" runat="server" Font-Bold="True"></asp:Label>
                          </td>
                      </tr>
                      <tr>
                          <td style="text-align: right" width="40%">
                              <asp:Label ID="lblEmp" runat="server" Text="Employee:" Font-Bold="True"></asp:Label>
                          </td>
                          <td width="60%">
                              <asp:DropDownList ID="ddlEmp" runat="server" 
                                    onselectedindexchanged="ddlEmp_SelectedIndexChanged" AutoPostBack="True" class="form-control" >
                                </asp:DropDownList>
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
                              <input id="datepicker" runat="server" clientidmode="Static" type="text" class="form-control" /></td>
                      </tr>
                      <tr>
                          <td style="text-align: right">
                              &nbsp;</td>
                          <td>
                              &nbsp;</td>
                      </tr>
                      <tr>
                          <td style="text-align: right">
                              <asp:Label ID="lblTo" runat="server" Font-Bold="True" Text="To:" 
                                  Visible="False"></asp:Label>
                          </td>
                          <td>
                              <input id="datepickerTo" runat="server" clientidmode="Static" type="text" 
                                  visible="False" class="form-control" /></td>
                      </tr>
                      <tr>
                          <td style="text-align: right">
                          </td>
                          <td>
                              &nbsp;</td>
                      </tr>
                      <tr>
                          <td style="text-align: right">
                              &nbsp;</td>
                          <td>
                              <asp:Button ID="btnPrint" runat="server" class="btn btn-primary" 
                                  onclick="btnPrint_Click" Text="Print" />
                              &nbsp;<asp:Button ID="btnReset" runat="server" class="btn btn-primary" 
                                  onclick="btnReset_Click" Text="Reset" />
                          </td>
                      </tr>
                  </table>
                  <br />
                  <div style="text-align: center">
                      <asp:Label ID="lblNotice" runat="server" Font-Bold="True" Font-Size="Medium" 
                    ForeColor="Red" 
                    Text="End Users are advised to edit these records under &quot;Incomplete Records&quot; feature." 
                    Visible="False"></asp:Label>
                  </div>
                  <br />
                  <div>
                       <asp:GridView ID="gvTSheet" runat="server" Visible="False" Width="100%" 
                           AutoGenerateColumns="False" HorizontalAlign="Center" class="table table-striped table-bordered table-hover">
                    <Columns>
                        <asp:BoundField DataField="username" HeaderText="User Name" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="date" HeaderText="Date" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="jobcode" HeaderText="Job Code" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="hours" HeaderText="Hours" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="notes" HeaderText="Notes" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="service code value" HeaderText="Service Code" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle 
                               HorizontalAlign="Center" />
                    <RowStyle HorizontalAlign="Center" />
                </asp:GridView>

                <asp:GridView ID="gvIncorrectData" runat="server" AutoGenerateColumns="False" Visible="False" 
                    Width="100%" onrowcancelingedit="gvIncorrectData_RowCancelingEdit" 
                    onrowdatabound="gvIncorrectData_RowDataBound" 
                    onrowediting="gvIncorrectData_RowEditing" 
                    onrowupdating="gvIncorrectData_RowUpdating" DataKeyNames="R_ID" class="table table-striped table-bordered table-hover">
                    <Columns>
                        <asp:TemplateField HeaderText="ID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%# Eval("R_ID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Name">
                            <ItemTemplate>
                                <asp:Label ID="lblUser" runat="server" Text='<%# Eval("R_User_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("R_Start_Date_Time") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Client">
                            <ItemTemplate>
                                <asp:Label ID="lblClient" runat="server" Text='<%# Eval("CL_ClientName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <cc1:dropdowncheckboxes ID="ddlMultiple" runat="server" 
                                    style="top: 0px; left: 0px; height: 16px; width: 202px" >
                                    <Style DropDownBoxBoxHeight="130" DropDownBoxBoxWidth="250" 
                                        SelectBoxWidth="200" />
                                </cc1:dropdowncheckboxes>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Task">
                            <ItemTemplate>
                                <asp:Label ID="lblTask" runat="server" Text='<%# Eval("TL_Task") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SubTask">
                            <ItemTemplate>
                                <asp:Label ID="lblSubTask" runat="server" Text='<%# Eval("STL_SubTask") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Duration">
                            <ItemTemplate>
                                <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("R_Duration") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comments">
                            <ItemTemplate>
                                <asp:Label ID="lblComments" runat="server" Text='<%# Eval("R_Comments") %>'></asp:Label>
                            </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" />
                    </Columns>
                </asp:GridView>

                 <asp:GridView ID="gvEarlyLogoff" runat="server" AutoGenerateColumns="False" Visible="False" 
                    Width="100%" DataKeyNames="EL_ID" 
                    onrowcancelingedit="gvEarlyLogoff_RowCancelingEdit" 
                    onrowediting="gvEarlyLogoff_RowEditing" 
                    onrowupdating="gvEarlyLogoff_RowUpdating" class="table table-striped table-bordered table-hover">
                    <Columns>
                        <asp:TemplateField HeaderText="ID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%# Eval("EL_ID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Name">
                            <ItemTemplate>
                                <asp:Label ID="lblUser" runat="server" Text='<%# Eval("EL_User_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("EL_Date") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scheduled">
                            <ItemTemplate>
                                <asp:Label ID="lblScheduled" runat="server" Text='<%# Eval("EL_Scheduled") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actual">
                            <ItemTemplate>
                                <asp:Label ID="lblActual" runat="server" Text='<%# Eval("EL_Actual") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Office Hours">
                            <ItemTemplate>
                                <asp:Label ID="lblTotal" runat="server" 
                                    Text='<%# Eval("EL_Total_Office_Hours") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Cab Delay">
                            <ItemTemplate>
                                <asp:Label ID="lblDelay" runat="server" Text='<%# Eval("Cab Delay") %>'></asp:Label>
                            </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Reason">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlReason" runat="server" Width="200px">
                                    <asp:ListItem>--Select--</asp:ListItem>
                                    <asp:ListItem>Half Day Leave</asp:ListItem>
                                    <asp:ListItem>No Task</asp:ListItem>
                                    <asp:ListItem>Unknown</asp:ListItem>
                                    <asp:ListItem>Personal</asp:ListItem>
                                    <asp:ListItem>Vacation</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comments">
                            <ItemTemplate>
                                <asp:Label ID="lblReason" runat="server" Text='<%# Eval("EL_Reason") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblComments" runat="server" Text='<%# Eval("EL_Comments") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </EditItemTemplate>
                           <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" />
                    </Columns>
                </asp:GridView>

                <asp:GridView ID="GridView1" runat="server" Width="60%" 
                  AutoGenerateColumns="False" onrowdatabound="GridView1_RowDataBound" 
                  Height="162px" CellPadding="4" ForeColor="#333333" GridLines="None" 
                  HorizontalAlign="Center" Visible="False">
                  <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                  <Columns>
                      <asp:TemplateField HeaderText="User Name">
                          <ItemTemplate>
                              <asp:Label ID="lblName" runat="server" Text='<%# Eval("User") %>'></asp:Label>
                          </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="Date">
                          <ItemTemplate>
                              <asp:Label ID="lblDate" runat="server" Text='<%# Eval("MissedDate") %>'></asp:Label>
                          </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="Reason">
                          <ItemTemplate>
                              <asp:DropDownList ID="ddlReason" runat="server" Width="150px">
                              </asp:DropDownList>
                          </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                      </asp:TemplateField>
                      <asp:TemplateField>
                          <ItemTemplate>
                              <asp:LinkButton ID="lbUpdate" runat="server" Font-Bold="True" Font-Size="Small" 
                                  onclick="lbUpdate_Click">Update</asp:LinkButton>
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
                  </div>
              </div>
           </div>
       </div>
  </div>
</form>
<script type="text/javascript">
    $(function () {
        $("#datepicker").datepicker({ maxDate: new Date() });
    });

    $(function () {
        //            $("#datepickerTo").datepicker({ maxDate: "+1M +10D" });
        $("#datepickerTo").datepicker({ maxDate: new Date() });
    });
  </script>
 </asp:Content>