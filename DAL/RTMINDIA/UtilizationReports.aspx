<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UtilizationReports.aspx.cs" Inherits="UtilizationReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="PivotStyle.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <form runat="server">
  <div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-table"></i>Utilization Reports</h3>
	    	<ol class="breadcrumb">
				<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
				<li><i class="fa fa-table"></i>Utilization Reports</li>
			</ol>
	</div>

     <div class="row">
       <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
              <div class="panel-heading" style="text-align:center">
                <asp:RadioButton ID="rbClient" runat="server" Font-Bold="True" 
                    Font-Size="Medium" GroupName="RTM" Text="By Client"  
                    Checked="True" AutoPostBack="True" 
                    oncheckedchanged="rbClient_CheckedChanged" />
                <asp:RadioButton ID="rbTask" runat="server" Font-Bold="True" Font-Size="Medium" 
                    GroupName="RTM" Text="By Task" AutoPostBack="True" 
                    oncheckedchanged="rbTask_CheckedChanged" />

                <asp:RadioButton ID="rbSubTask" runat="server" Font-Bold="True" Font-Size="Medium" 
                    GroupName="RTM" Text="By SubTask" AutoPostBack="True" oncheckedchanged="rbSubTask_CheckedChanged" 
                     />
               
                <asp:RadioButton ID="rbServiceCode" runat="server" AutoPostBack="True" 
                    Font-Bold="True" Font-Size="Medium" GroupName="RTM" 
                    oncheckedchanged="rbServiceCode_CheckedChanged" Text="Service Code%" />
               
                <asp:RadioButton ID="rbTaskPer" runat="server" AutoPostBack="True" 
                    Font-Bold="True" Font-Size="Medium" GroupName="RTM" 
                    oncheckedchanged="rbTaskPer_CheckedChanged" Text="Task %" />
               
                <asp:RadioButton ID="rbJobCode" runat="server" AutoPostBack="True" 
                    Font-Bold="True" Font-Size="Medium" GroupName="RTM" 
                    oncheckedchanged="rbJobCode_CheckedChanged" Text="Job Code %" />
               
                <asp:RadioButton ID="rbProductCode" runat="server" AutoPostBack="True" 
                    Font-Bold="True" Font-Size="Medium" GroupName="RTM" 
                    oncheckedchanged="rbProductCode_CheckedChanged" Text="Product Code" />
                <asp:RadioButton ID="rbRU" runat="server" AutoPostBack="True" Font-Bold="True" 
                    Font-Size="Medium" GroupName="RTM" oncheckedchanged="rbRU_CheckedChanged" 
                    Text="Resource Utilization" />
               
                &nbsp;<asp:RadioButton ID="RadioButton1" runat="server" AutoPostBack="True" 
                    Font-Bold="True" Font-Size="Medium" GroupName="RTM" 
                    oncheckedchanged="RadioButton1_CheckedChanged" Text="Peer Support" />
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
                          <td style="text-align: right" width="40%">
                              <asp:Label ID="lblTask" runat="server" Font-Bold="True" Text="Task:" 
                                 Visible="False"></asp:Label>
                         </td>
                          <td>
                             <asp:DropDownList ID="ddlTask" runat="server" Visible="False" class="form-control">
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

                  <div style="text-align: center">
                    <asp:Chart ID="Chart1" runat="server" Height="600px" Width="950px" 
                        BackSecondaryColor="White" Visible="False" BorderlineColor="DarkBlue" 
                        BorderlineDashStyle="Dash">
                        <Series>
                            <asp:Series Name="Series1" CustomProperties="PointWidth=0.8">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" ShadowColor="White">
                                <AxisY TitleFont="Microsoft Sans Serif, 10pt, style=Bold">
                                </AxisY>
                                <AxisX TitleFont="Microsoft Sans Serif, 10pt, style=Bold">
                                </AxisX>
                                <%--<Area3DStyle Enable3D="True" LightStyle="Realistic" />--%>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                    <label title="Test"></label>
                  </div>
                  <div>
                        <asp:Button ID="btnExport" runat="server" Font-Bold="True" Text="Export Data" 
                        class="btn btn-primary" onclick="btnExport_Click" Visible="False" />
    &nbsp;
                    <asp:Button ID="btnPDF" runat="server" Font-Bold="True" Text="Print Graph" 
                        class="btn btn-primary" onclick="btnPDF_Click" Visible="False" />
                  </div>
                  <br />
                  <div class="table-responsive">
                     <asp:GridView ID="gvPeerSupport" runat="server" AutoGenerateColumns="False" Visible="False" 
                        Width="100%" class="table table-striped table-bordered table-hover">
                        <Columns>
                            <asp:BoundField DataField="User Name" HeaderText="User Name">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Date" HeaderText="Date">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Duration" HeaderText="Duration">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Clients" HeaderText="Clients">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Comments" HeaderText="Comments">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Supported Members" HeaderText="Supported Members">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                
                      <div id="div2" runat="server" visible= "false" align="center">
                              <asp:Label ID="lblHeading" runat="server" 
                          Text="" Font-Bold="True" 
                          Font-Size="Medium" ForeColor="#003399" Visible ="False"></asp:Label>
                      </div>
                      <br />
                      <asp:Button ID="btnDetail" class="btn btn-primary" runat="server" 
                    Text="Detail View" onclick="btnDetail_Click" Visible="False" />
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

