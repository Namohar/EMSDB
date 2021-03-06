﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="EDI.aspx.cs" Inherits="WorkFlowManager.EDI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/quicksearch.js"></script>
    <style type="text/css">
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 800px;
            border: 3px solid #0DA9D0;
            border-radius: 12px;
            padding: 0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            text-align: center;
            font-weight: bold;
            border-top-left-radius: 6px;
            border-top-right-radius: 6px;
        }
        .modalPopup .body
        {
            min-height: 50px;
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
        
        .loadingmodal
        {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }
        .loading
        {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 400px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }
    </style>
    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("loadingmodal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 100);
        }
        //        $('form').live("submit", function () {
        //            ShowProgress();
        //        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('.search_textbox').each(function (i) {
                $(this).quicksearch("[id*=gvIPInvoices] tr:not(:has(th))", {
                    'testQuery': function (query, txt, row) {
                        return $(row).children(":eq(" + i + ")").text().toLowerCase().indexOf(query[0].toLowerCase()) != -1;
                    }
                });
            });
        });
    </script>
    <%-- Grd header fixed--%>
    <script type="text/javascript">
        //           function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {
        function MakeStaticHeader() {
            var GridId = "<%=gvIPInvoices.ClientID %>";
            var ScrollHeight = 400;
            window.onload = function () {
                var grid = document.getElementById(GridId);
                var gridWidth = grid.offsetWidth;
                var gridHeight = grid.offsetHeight;
                var headerCellWidths = new Array();
                for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                    headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
                }
                grid.parentNode.appendChild(document.createElement("div"));
                var parentDiv = grid.parentNode;

                var table = document.createElement("table");
                for (i = 0; i < grid.attributes.length; i++) {
                    if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                        table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                    }
                }
                table.style.cssText = grid.style.cssText;
                table.style.width = gridWidth + "px";
                table.appendChild(document.createElement("tbody"));
                table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
                var cells = table.getElementsByTagName("TH");

                var gridRow = grid.getElementsByTagName("TR")[0];
                for (var i = 0; i < cells.length; i++) {
                    var width;
                    if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                        width = headerCellWidths[i];
                    }
                    else {
                        width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
                    }
                    cells[i].style.width = parseInt(width - 3) + "px";
                    gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
                }
                parentDiv.removeChild(grid);

                var dummyHeader = document.createElement("div");
                dummyHeader.appendChild(table);
                parentDiv.appendChild(dummyHeader);
                var scrollableDiv = document.createElement("div");
                if (parseInt(gridHeight) > ScrollHeight) {
                    gridWidth = parseInt(gridWidth) + 17;
                }
                scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
                scrollableDiv.appendChild(grid);
                parentDiv.appendChild(scrollableDiv);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="Form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="row">
        <div class="col-lg-12">
            <h3 class="page-header">
                <i class="fa fa-file-o"></i>EDI Invoices</h3>
            <ol class="breadcrumb">
                <li><i class="fa fa-home"></i><a href="DashBoard.aspx">Home</a></li>
                <li><i class="fa fa-file-o"></i>EDI Invoices</li>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="ddlList" runat="server" Height="27px">
                    <asp:ListItem Enabled="true" Text="Select field" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Status" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Invoice" Value="2"></asp:ListItem>
                    <asp:ListItem Text="IP Asg To" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Client" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Source" Value="5"></asp:ListItem>
                    <asp:ListItem Text="ReceiveDate" Value="6"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="search_textbox" runat="server" placeholder="Search" 
                        AutoPostBack="True" ontextchanged="search_textbox_TextChanged"></asp:TextBox>
                <input id="btnSearch" runat="server" onserverclick="btnSearch_Click1" type="button"
                    value="Search" />
                <input id="btnReset" runat="server" onserverclick="btnReset_Click1" type="button"
                    value="Reset" />
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="panel panel-default">
                <div class="panel-body">
                    <%--          <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                    <div style="display: none">
                        <asp:Button ID="btnHidden" runat="server" Text="" OnClick="btnHidden_Click" />
                        <asp:Label ID="lblgvRowId" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblgvInvoiceNo" runat="server" Text=""></asp:Label>
                    </div>
                    <%--        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <div class="table-responsive">
                        <asp:Panel ID="Panel2" runat="server" Width="100%" Height="500px" ScrollBars="Horizontal">
                         <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
                            <contenttemplate>
                                    <asp:GridView ID="gvIPInvoices" runat="server" AutoGenerateColumns="False" class="table table-striped table-bordered table-hover"
                                        OnRowCommand="gvIPInvoices_RowCommand" OnRowDataBound="gvIPInvoices_RowDataBound"
                                        OnRowUpdating="gvIPInvoices_RowUpdating" DataKeyNames="FI_ID" 
                                        Font-Size="Smaller">
                                        <Columns>
                                            <asp:ButtonField Text="SingleClick" CommandName="SingleClick" Visible="False" />
                                            <asp:TemplateField HeaderText="ID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFIID" runat="server" Text='<%# Eval("FI_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text="Select"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch" runat="server" class="search_textbox" ReadOnly="True"
                                                        BorderStyle="None" BorderColor="White" BackColor="#f9f9f9" Width="10px"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text="File"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch4" runat="server" placeholder="File" class="search_textbox"
                                                        Visible="False" Width="100px"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFile" runat="server" Text='<%# Eval("FI_FileName") %>' Font-Underline="True"
                                                        ForeColor="Blue"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="30px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Source">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label6" runat="server" Text="Source"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch5" runat="server" placeholder="Source" class="search_textbox"
                                                        Width="100px" Visible="False"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSource" runat="server" Text='<%# Eval("FI_Source") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label7" runat="server" Text="Status"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch6" runat="server" placeholder="Status" class="search_textbox"
                                                        Width="100px" Visible="False"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("IND_Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice Count">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label8" runat="server" Text="Invoice Count"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch7" runat="server" placeholder="Invoice#" class="" Width="100px"
                                                        Visible="False"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("IND_InvoiceNo") %>'></asp:Label>
                                                    <asp:TextBox ID="txtInvoiceNo" runat="server" Visible="False" MaxLength="3"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="6px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Notes">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text="Notes"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch1" runat="server" class="search_textbox" ReadOnly="True"
                                                        BorderStyle="None" BorderColor="White" BackColor="#f9f9f9" Width="10px"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Notes" runat="server" Text="View/Add" Font-Underline="True" ForeColor="Blue"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issue">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text="Issue"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch2" runat="server" class="search_textbox" ReadOnly="True"
                                                        BorderStyle="None" BorderColor="White" BackColor="#f9f9f9" Width="10px" Visible="False"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIssue" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Completed">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text="Completed"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch3" runat="server" class="search_textbox" ReadOnly="True"
                                                        BorderStyle="None" BorderColor="White" BackColor="#f9f9f9" Width="10px"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkCompleted" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IP Asg By">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label19" runat="server" Text="IP Asg By"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch18" runat="server" placeholder="IP Asg By" class="search_textbox"
                                                        Visible="False" Width="100px"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAssignedBy" runat="server" Text='<%# Eval("IND_IP_Assigned_By") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IP Asg To">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label9" runat="server" Text="IP Asg To"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch8" runat="server" placeholder="IP Asg To" class="search_textbox"
                                                        Visible="False" Width="100px"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCompletedBy" runat="server" Text='<%# Eval("IND_IP_Processed_By") %>'></asp:Label>
                                                    <asp:DropDownList ID="ddlGvUser" runat="server" AutoPostBack="True" Visible="False">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Error#" Visible="false">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label10" runat="server" Text="Error#"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch9" runat="server" placeholder="Error#" class="search_textbox"
                                                        Visible="False" Width="100px"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblError" runat="server" Text='<%# Eval("IND_Error") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="QC Asg By" Visible="false">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label11" runat="server" Text="QC AsgBy"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch10" runat="server" placeholder="QC Asg By" class="search_textbox"
                                                        Visible="False" Width="100px"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQCAssiginedBy" runat="server" Text='<%# Eval("IND_QC_Assigned_By") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="QC Asg To" Visible="false">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label12" runat="server" Text="QC AsgTo"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch11" runat="server" placeholder="QC Asg To" class="search_textbox"
                                                        Visible="False" Width="100px"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQCAssignedTo" runat="server" Text='<%# Eval("IND_QC_Processed_By") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Process" Visible="false">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblduplicates" runat="server" Text="Process"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearch12" runat="server" class="search_textbox" ReadOnly="True"
                                                        BorderStyle="None" BorderColor="White" BackColor="#f9f9f9" Width="10px"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblduplicates1" runat="server" Text=""></asp:Label>
                                                    <asp:DropDownList ID="ddlprocess" runat="server" AutoPostBack="true" Visible="false">
                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Duplicate" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="DNP" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="EDI" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="Statement" Value="4"></asp:ListItem>
                                                        
                                                                    <asp:ListItem Text="Expedite" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ReceivedDate">
                                                <HeaderTemplate>
                                                    <asp:Label ID="LabelReceivedDate" runat="server" Text="ReceivedDate"></asp:Label><br />
                                                    <asp:TextBox ID="txtSearchReceivedDate" runat="server" placeholder="ReceivedDate"
                                                        class="search_textbox" Width="100px" Visible="False"></asp:TextBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceiptDate" runat="server" Text='<%# Eval("ReceiptDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                 </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="File Upload">
                                                <HeaderTemplate>
                                                       <asp:Label ID="lblRead" runat="server" Text="File Upload"></asp:Label><br />
                                                </HeaderTemplate>
                                                <ItemTemplate>                                    
                                                                               <asp:Label ID="btnRead" runat="server" Text="Upload" Font-Underline="True" ForeColor="Blue"></asp:Label>
                                                </ItemTemplate>
                                                     <ItemStyle Width="10px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </contenttemplate>
                                       </asp:UpdatePanel>
                        </asp:Panel>
                    </div>
                    <div class="loading" align="center">
                        <b>Please wait... </b>
                        <br />
                        <br />
                        <img src="images/loader.gif" alt="" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--    <asp:UpdatePanel ID="CommentsUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
    <asp:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden"
        PopupControlID="divPopUp">
    </asp:ModalPopupExtender>
    <div id="divPopUp" class="modalPopup" style="display: none">
        <div class="header">
            Comments
        </div>
        <div class="body">
            <asp:Label ID="lblPopTeam" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblPopFileId" runat="server" Text="" Visible="false"></asp:Label>
            <br />
            <table style="width: 100%;">
                <tr>
                    <td style="text-align: right" width="40%">
                        <asp:Label ID="lblComments" runat="server" Text="Enter Comments:"></asp:Label>
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Width="228px" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        &nbsp;
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="200px">
                            <asp:GridView ID="gvComments" runat="server" class="table table-striped table-bordered table-hover">
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <div class="footer" align="right">
            <asp:Button ID="btnYes" runat="server" Text="Update" CssClass="yes" OnClick="btnYes_Click" />
            <asp:Button ID="btnNo" runat="server" Text="Cancel" CssClass="no" OnClick="btnNo_Click" />
        </div>
    </div>
    <%--     </ContentTemplate>
    </asp:UpdatePanel>--%>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    //SetAutoComplete();
                    //                $('form').live("submit", function () {
                    //                    ShowProgress();
                    //                });
                    //                Scrollable();
                    //                MakeScrollable();
                    MakeStaticHeader();
                }
            });
        };
    </script>
    </form>
</asp:Content>
