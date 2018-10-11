<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="DashBoard" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <form id="Form1" runat="server">
<div class="row">
				<div class="col-lg-12">
					<h3 class="page-header"><i class="fa fa-dashboard"></i> Dashboard</h3>
					<ol class="breadcrumb">
						<li><i class="fa fa-home"></i><a href="DashBoard.aspx">Home</a></li>
						<li><i class="fa fa-dashboard"></i>Dashboard</li>
					</ol>
				</div>
			</div>
            <div  class="row" >
            <div style="width:100%; ">
                <table style="width:100%;">
                    <tr>
                        <td style="text-align: right" width="60%">
                            <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Size="Small" 
                    ForeColor="Black" Text="Total Today's Working Hours:" Visible="False"></asp:Label>
                <asp:Label ID="lblDate" runat="server" Font-Bold="True" Font-Size="XX-Large" 
                    ForeColor="Black"></asp:Label>
                        </td>
                        <td style="text-align: right">
                            <div style="width: 124px; height: 73px; float:right; background-image: url('images/mlCBjjw.jpg'); background-repeat: no-repeat;background-size: 100% 100%; text-align: center;">
                                <br />
                                <asp:Label ID="lblHours" runat="server" Font-Bold="True" Font-Size="Medium" 
                    ForeColor="Red"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            </div>
         <div  class="row" >
            <div class="col-md-4 col-sm-12 col-xs-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <asp:Label ID="lblClientHeader" runat="server" Text="" Visible="false"></asp:Label>
                            </div>
                            <div class="panel-body">
                                <asp:Chart ID="Chart1" runat="server" Height="228px" Width="300px" ViewStateContent="All" Palette="Bright" onclick="Chart1_Click">
                                   <series>
                                        <asp:Series ChartType="Doughnut" Name="Series1" 
                                            ToolTip="#VALX #VAL Hours" PostBackValue="item" BorderWidth="4">
                                        </asp:Series>
                                    </series>
                                    <chartareas>
                                        <asp:ChartArea Name="ChartArea1">
                                            <Area3DStyle Enable3D="True" LightStyle="Realistic" IsClustered="True" 
                                                IsRightAngleAxes="False" />
                                        </asp:ChartArea>
                                    </chartareas>
                                </asp:Chart>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <asp:Label ID="lblRUHeader" runat="server" Text="" Visible="false"></asp:Label>
                            </div>
                            <div class="panel-body">
                                <asp:Chart ID="Chart2" runat="server" Height="228px" Width="300px" 
                                    Palette="SemiTransparent" onclick="Chart2_Click" 
                                    >
                                    <Series>
                                        <asp:Series ChartType="Doughnut" Name="Series1" ToolTip="#VALX #VALY %" 
                                            PostBackValue="item">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1">
                                         <Area3DStyle Enable3D="True" LightStyle="Realistic" />
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <asp:Label ID="lblTaskHeader" runat="server" Text="" Visible="false"></asp:Label>
                            </div>
                            <div class="panel-body">
                                <asp:Chart ID="Chart3" runat="server" Height="228px" Width="300px" onclick="Chart3_Click" 
                                    >
                                    <Series>
                                        <asp:Series ChartType="Doughnut" Name="Series1" ToolTip="#VALX #VAL Hours" 
                                            PostBackValue="item">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1">
                                         <Area3DStyle Enable3D="True" LightStyle="Realistic" />
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                        </div>
                    </div>
        </div>
         <div class="row">
           <div class="col-md-12 col-sm-12 col-xs-12">
                 <div class="panel panel-default">
                    <div class="panel-body">
                       <asp:Chart ID="Chart4" runat="server" Width="900px" Height="350px" >
                        <Series>
                            <asp:Series Name="Series1">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
            
                                <AxisY Title="In Hours" TitleFont="Microsoft Sans Serif, 10pt, style=Bold">
                                </AxisY>
            
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                    </div>
                 </div>
            </div>
        </div>

        <div class="row">
           <div class="col-md-12 col-sm-12 col-xs-12">
                 <div class="panel panel-default">
                    <div class="panel-body">
                       <asp:Chart ID="Chart5" runat="server" Palette="Bright" 
                        Width="900px" Height="350px" >
                        <Series>
                            <asp:Series BorderWidth="2" Legend="Task" Name="Tasks">
                            </asp:Series>
                            <asp:Series BorderWidth="2" ChartArea="ChartArea1" 
                                Legend="Task" Name="Meetings">
                            </asp:Series>
                            <asp:Series BorderWidth="2" ChartArea="ChartArea1" 
                                Legend="Task" Name="Conference-Call">
                            </asp:Series>
                            <asp:Series BorderWidth="2" ChartArea="ChartArea1" 
                                Legend="Task" Name="Breaks">
                            </asp:Series>
                            <asp:Series BorderWidth="5" ChartArea="ChartArea1" 
                                Legend="Task" Name="NonTask">
                            </asp:Series>
                             <asp:Series BorderWidth="5" ChartArea="ChartArea1" 
                                Legend="Task" Name="Peer Support">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                                <AxisY Title="In Hours" TitleFont="Microsoft Sans Serif, 10pt, style=Bold">
                                </AxisY>
                            </asp:ChartArea>
                        </ChartAreas>
                        <Legends>
                            <asp:Legend Docking="Bottom" Name="Task">
                            </asp:Legend>
                            <%--<asp:Legend Name="Meetings">
                            </asp:Legend>
                            <asp:Legend Name="Legend3">
                            </asp:Legend>
                            <asp:Legend Name="Legend4">
                            </asp:Legend>
                            <asp:Legend Name="Legend5">
                            </asp:Legend>--%>
                        </Legends>
                    </asp:Chart>
                    </div>
                 </div>
            </div>
        </div>
        </form>
</asp:Content>

