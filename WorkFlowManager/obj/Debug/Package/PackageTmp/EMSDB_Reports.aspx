<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="EMSDB_Reports.aspx.cs" Inherits="WorkFlowManager.EMSDB_Reports1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="Styles/GridStyle.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">
        function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {
            var tbl = document.getElementById(gridId);
            if (tbl) {
                var DivHR = document.getElementById('DivHeaderRow');
                var DivMC = document.getElementById('DivMainContent');
                var DivFR = document.getElementById('DivFooterRow');

                //*** Set divheaderRow Properties ****
                DivHR.style.height = headerHeight + 'px';
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



                if (isFooter) {
                    var tblfr = tbl.cloneNode(true);
                    tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
                    var tblBody = document.createElement('tbody');
                    tblfr.style.width = '100%';
                    tblfr.cellSpacing = "0";
                    tblfr.border = "0px";
                    tblfr.rules = "none";

                }
                //****Copy Header in divHeaderRow****
                DivHR.appendChild(tbl.cloneNode(true));
            }
        }



        function OnScrollDiv(Scrollablediv) {
            document.getElementById('DivHeaderRow').scrollLeft = Scrollablediv.scrollLeft;

        }


    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            setupResponsiveTables();

            // Register method to fire at the end of an asp:UpdatePanel response
            $(window).on('load', function () {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endUpdatePanelRequest);
            });
        });

        function endUpdatePanelRequest() {
            setupResponsiveTables();
        }

        function setupResponsiveTables() {
            if ($('#SetupResponsiveTableOnReload').is(':checked')) {
                $('.responsiveTable1').responsiveTable();
            }
        }
    </script>
    <style type="text/css">
        .Form-groupControlls
        {
            background-repeat: repeat-x;
            border: 1px solid #d1c7ac;
            width: 230px;
            color: #333333;
            padding: 3px;
            margin-right: 4px;
            margin-bottom: 8px;
            font-family: tahoma, arial, sans-serif;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container">
        <div class="form-group">
            <strong>Select Report:</strong>
            <asp:DropDownList ID="ddlReport" runat="server" class="Form-groupControlls">
            </asp:DropDownList>
            <strong>Employee Name:</strong>
            <asp:DropDownList ID="ddlEmployee" runat="server" class="Form-groupControlls">
            </asp:DropDownList>
            <br />
            <div id="dvDate" runat="server">
                <strong>From Date:</strong>
                <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                <asp:Button ID="btngenerate" runat="server" Text="Generate Report" class="btn btn-primary" />
            </div>
        </div>
    </div>
    <div class="container">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="col-lg-12">
                        <div class="updatePanel">
                            <div id="DivRoot" align="left">
                                <div style="overflow: hidden;" id="DivHeaderRow">
                                </div>
                                <div style="overflow: scroll;" onscroll="OnScrollDiv(this)" id="DivMainContent">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdReports" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover"
                                                AutoGenerateColumns="False" DataKeyNames="IND_InvoiceNo" EmptyDataText="There are no data records to display.">
                                                <Columns>
                                                    <asp:BoundField DataField="FI_CreatedOn" HeaderText="FI_CreatedOn" HeaderStyle-CssClass="visible-lg"
                                                        ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="FI_FileName" HeaderText="FI_FileName" HeaderStyle-CssClass="visible-lg"
                                                        ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="IND_Status" HeaderText="IND_Status" HeaderStyle-CssClass="visible-lg"
                                                        ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="IND_InvoiceNo" HeaderText="IND_InvoiceNo" HeaderStyle-CssClass="visible-lg"
                                                        ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="IND_IP_Assigned_By" HeaderText="IND_IP_Assigned_By" HeaderStyle-CssClass="visible-lg"
                                                        ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="IND_IP_Processed_By" HeaderText="IND_IP_Processed_By"
                                                        HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="IND_Error" HeaderText="IND_Error" HeaderStyle-CssClass="visible-lg"
                                                        ItemStyle-CssClass="visible-lg" />
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="DivFooterRow" style="overflow: hidden">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
