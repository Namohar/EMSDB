<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="AdminInvoiceProcessing.aspx.cs" Inherits="WorkFlowManager.AdminInvoiceProcessing" %>

<%--<%@ Register src="UserControl/AutoRedirect.ascx" tagname="AutoRedirect" tagprefix="asp"%>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/quicksearch.js"></script>
    <script type="text/javascript">
        var startingIndex = 0, gridViewID = '<%= gvInvoices.ClientID %>';
        function selectCheckbox(e, selectedIndex) {
            if (e.shiftKey) {

                if (startingIndex < selectedIndex) {

                    $(':checkbox', '#' + gridViewID).slice(startingIndex, selectedIndex).prop("checked", true);
                }
                else
                    $(':checkbox', '#' + gridViewID).slice(selectedIndex, startingIndex).prop("checked", true);
            }
            startingIndex = selectedIndex;
        }

        $(function () {
            $('.search_textbox').each(function (i) {
                $(this).quicksearch("[id*=gvInvoices] tr:not(:has(th))", {
                    'testQuery': function (query, txt, row) {
                        return $(row).children(":eq(" + i + ")").text().toLowerCase().indexOf(query[0].toLowerCase()) != -1;
                    }
                });
            });
        });
    </script>
    <%-- Grd header fixed--%>
    <script type="text/javascript">
        function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {
            var tbl = document.getElementById(gridId);
            if (tbl) {
                var DivHR = document.getElementById('DivHeaderRow');
                var DivMC = document.getElementById('DivMainContent');
                var DivFR = document.getElementById('DivFooterRow');

                //*** Set divheaderRow Properties ****
                DivHR.style.height = headerHeight + 'px';
                //                    DivHR.style.height = "50px";
                DivHR.style.width = (parseInt(width) - 16) + 'px';
                DivHR.style.position = 'relative';
                DivHR.style.top = '0px';
                DivHR.style.zIndex = '10';
                DivHR.style.verticalAlign = 'top';

                //*** Set divMainContent Properties ****
                DivMC.style.width = width + 'px';
                DivMC.style.height = height + 'px';
                DivMC.style.position = 'relative';
                DivMC.style.top = -headerHeight + 'px';
                DivMC.style.zIndex = '1';

                //*** Set divFooterRow Properties ****
                DivFR.style.width = (parseInt(width) - 16) + 'px';
                DivFR.style.position = 'relative';
                DivFR.style.top = -headerHeight + 'px';
                DivFR.style.verticalAlign = 'top';
                DivFR.style.paddingtop = '2px';

                if (isFooter) {
                    //                    var tblfr = tbl.cloneNode(true);
                    //                    tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
                    //                    var tblBody = document.createElement('tbody');
                    //                    tblfr.style.width = '100%';
                    //                    tblfr.cellSpacing = "0";
                    //                    tblfr.border = "0px";
                    //                    tblfr.rules = "none";
                    //                    //*****In the case of Footer Row *******
                    //                    tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
                    //                    tblfr.appendChild(tblBody);
                    //                    DivFR.appendChild(tblfr);
                }
                //****Copy Header in divHeaderRow****
                DivHR.appendChild(tbl.cloneNode(true));
            }
        }



        function OnScrollDiv(Scrollablediv) {
            document.getElementById('DivHeaderRow').scrollLeft = Scrollablediv.scrollLeft;
            //  document.getElementById('DivFooterRow').scrollLeft = Scrollablediv.scrollLeft;
        }


    </script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[id*=chkHeader]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                }
            });
        });
        $("[id*=chkSelect]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkHeader]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkSelect]", grid).length == $("[id*=chkSelect]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });
    </script>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .selected
        {
            background-color: #A1DCF2;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="Form1" runat="server">
    <div class="row">
        <div class="col-lg-12">
            <h3 class="page-header">
                <i class="fa fa-file-o"></i>EMSDB Admin</h3>
            <ol class="breadcrumb">
                <li><i class="fa fa-home"></i><a href="DashBoard.aspx">Home</a></li>
                <li><i class="fa fa-file-o"></i>EMSDB Admin</li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div>
                        <asp:Label ID="lblClient" runat="server" Text="Select Client:" Font-Bold="True" Font-Size="Medium"></asp:Label>
                        &nbsp;
                        <asp:DropDownList ID="ddlClient" runat="server" AutoPostBack="True" Width="200px"
                            OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTotal" runat="server" Text=" Today Total Invoices:"></asp:Label>
                        <asp:Label ID="txtTotal" runat="server" Text="0" Font-Bold="True"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblAssigned" runat="server" Text="Invoices Assigned:"></asp:Label>
                        <asp:Label ID="txtAssigned" runat="server" Text="0" Font-Bold="True"></asp:Label>
                        <br />
                        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="table-responsive">
                        <div style="text-align: right; width: 100%">
                            <%-- <asp:DropDownList ID="ddlList" runat="server" Height="27px">
                  <asp:ListItem Enabled="true" Text="Select field" Value="-1"></asp:ListItem>
    <asp:ListItem Text="PDF" Value="1"></asp:ListItem>
    <asp:ListItem Text="Batch" Value="2"></asp:ListItem>
    <asp:ListItem Text="Source" Value="3"></asp:ListItem>
        <asp:ListItem Text="Staus" Value="4"></asp:ListItem>
                     </asp:DropDownList>
                 <asp:TextBox ID="search_textbox" runat="server"  placeholder="Search" class="search_textbox"
                    ></asp:TextBox>
    
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                            <asp:Label ID="lblUserError" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp
                            <asp:Label ID="lblUser" runat="server" Text="Select User:"></asp:Label>
                            <asp:DropDownList ID="ddlUser" runat="server">
                            </asp:DropDownList>
                            <asp:Button ID="btnSubmit" runat="server" Text="Assign" class="btn btn-primary" OnClick="btnSubmit_Click" />
                        </div>
                        <%--      <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Height="500px">--%>
                        <div id="DivRoot" align="left">
                            <div style="overflow: hidden;" id="DivHeaderRow">
                            </div>
                            <div style="overflow: scroll;" onscroll="OnScrollDiv(this)" id="DivMainContent">
                                <asp:GridView ID="gvInvoices" runat="server" AutoGenerateColumns="False" class="table table-striped table-bordered table-hover"
                                    DataKeyNames="FI_ID">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFIID" runat="server" Text='<%# Eval("FI_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label3" runat="server" Text="Select"></asp:Label>
                                                <br />
                                                <br />
                                                <asp:CheckBox ID="chkHeader" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" onclick='<%# string.Format("javascript:selectCheckbox(event,{0});", Container.DataItemIndex) %>'
                                                    AutoPostBack="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PDF">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label1" runat="server" Text="PDF"></asp:Label><br />
                                                <asp:TextBox ID="txtSearch1" runat="server" placeholder="PDF" class="search_textbox"
                                                    Visible="false"></asp:TextBox>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkViewFile" runat="server" Text='<%# Eval("FI_FileName") %>'
                                                    OnClick="lnkViewFile_Click" CommandArgument='<%# Eval("FI_ID") %>' Target="_blank"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Batch">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label2" runat="server" Text="Batch"></asp:Label><br />
                                                <asp:TextBox ID="txtSearch2" runat="server" placeholder="Batch" class="search_textbox"
                                                    Visible="false"></asp:TextBox>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBatch" runat="server" Text='<%# Eval("FI_CreatedOn") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Source">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label4" runat="server" Text="Source"></asp:Label><br />
                                                <asp:TextBox ID="txtSearch3" runat="server" placeholder="Source" class="search_textbox"
                                                    Visible="false"></asp:TextBox>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSource" runat="server" Text='<%# Eval("FI_Source") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("IND_Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice#" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("IND_InvoiceNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IP Assigned By" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIPAssigned" runat="server" Text='<%# Eval("IND_IP_Assigned_By") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IP Assigned To" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIPProcessed" runat="server" Text='<%# Eval("IND_IP_Processed_By") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="QC Assigned By" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQCAssigned" runat="server" Text='<%# Eval("IND_QC_Assigned_By") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="QC Assigned To" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQCProcessed" runat="server" Text='<%# Eval("IND_QC_Processed_By") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle Height="2px" />
                                </asp:GridView>
                            </div>
                            <div id="DivFooterRow" style="overflow: hidden; margin-left: 550px">
                                <asp:Button ID="btndelete" runat="server" class="btn btn-primary" Text="Delete" OnClick="btndelete_Click" /></div>
                        </div>
                        <%--     </asp:Panel>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
