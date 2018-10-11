<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="EMSDBQC.aspx.cs" Inherits="WorkFlowManager.EMSDBQC" %>

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
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
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
        $('form').live("submit", function () {
            ShowProgress();
        });
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
    </script>


 
    
    <style type="text/css">
        .modal
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
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }
    </style>
      <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        //    $('btnSearch').live("submit", function () {
        $("#btnSearch").click(function () {
            ShowProgress();
        });
    </script>
    <script type="text/javascript">
        function HidePreloader() {
            if (document.getElementById('preloader') != null) {
                document.getElementById('preloader').style.visibility = 'hidden';
            }
        }
    </script>
        <style type="text/css">
        .overlay
        {
            position: fixed;
            z-index: 999;
            height: 20%;
            width: 20%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        img.animated-gif
        {
            width: 30px;
            height: auto;
        }
    </style>
  <%--  <script type="text/javascript">
        function showProgress() {
            var updateProgress = $get("<%= UpdateProgress.ClientID %>");
            updateProgress.style.display = "block";
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="Form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="row">
        <div class="col-lg-12">
            <h3 class="page-header">
                <i class="fa fa-file-o"></i>QC Invoices</h3>
            <ol class="breadcrumb">
                <li><i class="fa fa-home"></i><a href="DashBoard.aspx">Home</a></li>
                <li><i class="fa fa-file-o"></i>QC Invoices</li>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="ddlList" runat="server" Height="27px">
                    <asp:ListItem Enabled="true" Text="Select field" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Status" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Invoice" Value="2"></asp:ListItem>
                    <asp:ListItem Text="QC Asg To" Value="4"></asp:ListItem>

                     <asp:ListItem Text="Client" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Source" Value="6"></asp:ListItem>
                    <asp:ListItem Text="ReceiveDate" Value="7"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="search_textbox" runat="server" placeholder="Search"></asp:TextBox>
                <%--    <asp:Button ID="btnSearch" runat="server" Text="Search" value="submit" OnClick="btnSearch_Click" />--%>
                <input id="btnSearch" runat="server" onserverclick="btnSearch_Click" type="button"
                    value="Search"  OnClientClick="showProgress()" />
                <input id="btnReset" runat="server" onserverclick="btnReset_Click1" type="button"
                    value="Reset" />
                     <asp:UpdatePanel runat="server" ID="updatepanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                      
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSearch" />
                    </Triggers>
                </asp:UpdatePanel>
                            <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="updatepanel1">
                    <ProgressTemplate>
                        <div class="">
                            <div style="position: absolute; margin-left: 480px; top: 67px;">
                                <img alt="" src="images/loader.gif" class="animated-gif" />
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div style="display: none">
                        <asp:Button ID="btnHidden" runat="server" Text="" OnClick="btnHidden_Click" />
                        <asp:Label ID="lblgvRowId" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblgvInvoiceNo" runat="server" Text=""></asp:Label>
                    </div>
                    <asp:Panel ID="Panel1" runat="server" Width="100%" Height="500px" ScrollBars="Auto">
                         <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                        <asp:GridView ID="gvIPInvoices" runat="server" FixHeaders="True" AutoGenerateColumns="False"
                            class="table table-striped table-bordered table-hover" OnRowCommand="gvIPInvoices_RowCommand"
                            OnRowDataBound="gvIPInvoices_RowDataBound" OnRowUpdating="gvIPInvoices_RowUpdating"
                            DataKeyNames="FI_ID" HeaderStyle-CssClass="" ScrollBars="Auto">
                            <Columns>
                                <asp:ButtonField Text="SingleClick" CommandName="SingleClick" Visible="False" />
                                <asp:TemplateField HeaderText="ID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFIID" runat="server" Text='<%# Eval("FI_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-CssClass="Headerpad">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="Select" CssClass=""></asp:Label><br />
                                        <asp:TextBox ID="txtSearch" runat="server" class="search_textbox" ReadOnly="True"
                                            BorderStyle="None" BorderColor="White" BackColor="#f9f9f9" Width="10px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label6" runat="server" Text="File"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch5" runat="server" placeholder="File" class="search_textbox"
                                            Visible="false" Width="100px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFile" runat="server" Text='<%# Eval("FI_FileName") %>' Font-Underline="True"
                                            ForeColor="Blue"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Source">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label7" runat="server" Text="Source"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch6" runat="server" placeholder="Source" class="search_textbox"
                                            Visible="false" Width="100px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSource" runat="server" Text='<%# Eval("FI_Source") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label8" runat="server" Text="Status"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch7" runat="server" placeholder="Status" class="search_textbox"
                                            Visible="false" Width="100px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("IND_Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice#">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label9" runat="server" Text="Invoice#"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch8" runat="server" placeholder="Invoice#" class="search_textbox"
                                            Visible="false" Width="100px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("IND_InvoiceNo") %>'></asp:Label>
                                        <asp:TextBox ID="txtInvoiceNo" runat="server" AutoPostBack="True" Visible="False"
                                            MaxLength="30"></asp:TextBox>
                                    </ItemTemplate>
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
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issue">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label3" runat="server" Text="Issue"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch2" runat="server" class="search_textbox" ReadOnly="True"
                                            BorderStyle="None" BorderColor="White" BackColor="#f9f9f9" Width="10px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIssue" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="QC Done">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label4" runat="server" Text="QC Done"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch3" runat="server" class="search_textbox" ReadOnly="True"
                                            BorderStyle="None" BorderColor="White" BackColor="#f9f9f9" Width="10px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCompleted" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IP Asg By">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label10" runat="server" Text="IP Asg By"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch9" runat="server" placeholder="IP Asg By" class="search_textbox"
                                            Visible="false" Width="100px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssignedBy" runat="server" Text='<%# Eval("IND_IP_Assigned_By") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IP Asg To">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label11" runat="server" Text="IP Asg To"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch10" runat="server" placeholder="IP Asg To" class="search_textbox"
                                            Visible="false" Width="100px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompletedBy" runat="server" Text='<%# Eval("IND_IP_Processed_By") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Error#">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label12" runat="server" Text="Error#"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch11" runat="server" placeholder="Error#" class="search_textbox"
                                            Visible="False" Width="100px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblError" runat="server" Text='<%# Eval("IND_Error") %>'></asp:Label>
                                        <asp:TextBox ID="txtErrorNo" runat="server" onkeypress="return isNumberKey(event)"  MaxLength="3"
                                            Visible="False"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="QC Asg By">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label13" runat="server" Text="QC Asg By"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch12" runat="server" placeholder="QC Asg By" class="search_textbox"
                                            Visible="false" Width="100px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblQCAssiginedBy" runat="server" Text='<%# Eval("IND_QC_Assigned_By") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="QC Asg To">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label14" runat="server" Text="QC Asg To"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch13" runat="server" placeholder="QC Asg To" Visible="false"
                                            Width="100px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblQCAssignedTo" runat="server" Text='<%# Eval("IND_QC_Processed_By") %>'></asp:Label>
                                        <asp:DropDownList ID="ddlGvUser" runat="server" AutoPostBack="True" Visible="False">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Back to IP">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label5" runat="server" Text="Back to IP"></asp:Label><br />
                                        <asp:TextBox ID="txtSearch4" runat="server" class="search_textbox" ReadOnly="True"
                                            BorderStyle="None" BorderColor="White" BackColor="#f9f9f9" Width="10px"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkReturn" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                   
                     </ContentTemplate>
                            </asp:UpdatePanel>
                    </asp:Panel>
                </div>
              <%--  <div class="loading" align="center">
                    <b>Please wait... </b>
                    <br />
                    <br />
                    <img src="images/loader.gif" alt="" />
                </div>--%>
            </div>
        </div>
    </div>

      <asp:UpdatePanel ID="CommentsUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
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
                        <asp:Panel ID="Panel2" runat="server" ScrollBars="Vertical" Height="200px">
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
    
    <asp:Label ID="lblHidden1" runat="server" Text=""></asp:Label>
    <asp:ModalPopupExtender ID="mpeStatus" runat="server" TargetControlID="lblHidden1"
        PopupControlID="dvStatusPopup">
    </asp:ModalPopupExtender>
    <div id="dvStatusPopup" class="modalPopup" style="display: none">
        <div class="header">
            Invoice Status
        </div>
        <div class="body">
            <asp:Label ID="lblPop2FileId" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblInvoiceStatus" runat="server"></asp:Label>
        </div>
        <div class="footer" align="right">
            <asp:Button ID="btnContinue" runat="server" Text="Yes" CssClass="yes" OnClick="btnContinue_Click" />
            <asp:Button ID="btnBack" runat="server" Text="No" CssClass="no" OnClick="btnBack_Click" />
        </div>
    </div>
    
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</asp:Content>
