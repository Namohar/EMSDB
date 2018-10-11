<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PSL_ScoreCard.aspx.cs" Inherits="PSL_ScoreCard" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<form id="Form1" runat="server">

  <div class="row">
				<div class="col-lg-12">
					<h3 class="page-header"><i class="fa fa-bar-chart-o"></i> PSL Score Card</h3>
					<ol class="breadcrumb">
						<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
						<li><i class="fa fa-bar-chart-o"></i>PSL</li>
					</ol>
				</div>
			</div>
    <div class="row">
       <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <Triggers>
             <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
           </Triggers>
       <ContentTemplate>
          <asp:Timer ID="Timer1" runat="server" Interval="60000" ontick="Timer1_Tick">
          </asp:Timer>
          <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
              <div class="panel-heading">
                 <asp:Label ID="lblLine1Header" runat="server" Text="PSL Strike Rate (Invoices/Hour)"></asp:Label>
              </div>
              <div class="panel-body">
                   <asp:LineChart ID="LineChart1" runat="server" ChartHeight="400" 
                           ChartWidth="900" ChartTitleColor="#0E426C" Visible="false" CategoryAxisLineColor="#D08AD9"
                      ValueAxisLineColor="#D08AD9" BaseLineColor="#A156AB" BorderColor="White" 
                           Font-Bold="True" BorderStyle="None">

                    </asp:LineChart>         
              </div>
           </div>
          </div>

           <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
              <div class="panel-heading">
                 <asp:Label ID="lblLine2Header" runat="server" Text="PSL Strike Rate (Cumulative)"></asp:Label>
              </div>
              <div class="panel-body">
                  <asp:LineChart ID="LineChart2" runat="server" ChartHeight="400" 
                       ChartWidth="900" ChartTitleColor="#0E426C" Visible="false" CategoryAxisLineColor="#D08AD9"
                  ValueAxisLineColor="#D08AD9" BaseLineColor="#A156AB" BorderColor="White" 
                       Font-Bold="True">

                  </asp:LineChart>          
              </div>
           </div>
          </div>

          <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
              <div class="panel-heading">
                 <asp:Label ID="lblTotalInvoiceHeader" runat="server" Text=""></asp:Label>
              </div>
              <div class="panel-body">
                   <asp:Chart ID="BarChart2" runat="server" Height="400px" Width="900px">
                        <Series>
                            <asp:Series Name="Series1">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                                       
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart> 
              </div>
           </div>
         </div>

         <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
              <div class="panel-heading">
                 <asp:Label ID="lblEfectiveRateHeader" runat="server" Text=""></asp:Label>
              </div>
              <div class="panel-body">
                 <asp:Chart ID="ChartTodayEffRate" runat="server" Height="400px" Width="900px">
                    <Series>
                        <asp:Series Name="Series1">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                                       
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>   
              </div>
           </div>
         </div>

         <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
              <div class="panel-heading">
                 <asp:Label ID="lblPrevTotal" runat="server" Text=""></asp:Label>
              </div>
              <div class="panel-body">
                  <asp:Chart ID="BarChart1" runat="server" Height="400px" Width="900px">
                    <Series>
                        <asp:Series Name="Series1">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                                       
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>    
              </div>
           </div>
         </div>

         <div class="col-md-12 col-sm-12 col-xs-12">
           <div class="panel panel-default">
              <div class="panel-heading">
                 <asp:Label ID="lblPrevEffectiveRate" runat="server" Text=""></asp:Label>
              </div>
              <div class="panel-body">
                   <asp:Chart ID="ChartPrevDayEffRate" runat="server" Height="400px" Width="900px">
                        <Series>
                            <asp:Series Name="Series1">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                                       
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>   
              </div>
           </div>
         </div>
       </ContentTemplate>
      </asp:UpdatePanel>
    </div>
</form>
</asp:Content>

