<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="WorkFlowManager.Test1" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
    $('form').live("submit", function () {
        ShowProgress();
    });
</script>
</head>
<body>
      <form id="form1" runat="server">



      <div>

          <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0" EnableHistory="True">

          </asp:ScriptManager>

          <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">

              <ContentTemplate>

                  <asp:Label ID="Label1" runat="server" Text="Page navigation:   "></asp:Label>

                  <asp:Button runat="server" ID="btnFirst"

                      Text="|<--" ToolTip="Go to first page"/>

                  <asp:Button runat="server" ID="btnPrev"

                      Text="<<" ToolTip="Go to previous page"/>

                  <asp:Button runat="server" ID="btnNext"

                      Text=">>" ToolTip="Go to next page"/>

                  <asp:Button runat="server" ID="btnLast"

                      Text="-->|" ToolTip="Go to last page"/>
               
                  <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"                    

                  HasCrystalLogo="False" GroupTreeStyle-ShowLines="True"

                      Width="100%" Height="100%"

                  EnableDatabaseLogonPrompt="False" ReuseParameterValuesOnRefresh="True"

                      HasPageNavigationButtons="False" ShowAllPageIds="True"

                      EnableParameterPrompt="False" />                   

              </ContentTemplate>

          </asp:UpdatePanel>
  <%--        dj vjvdjvjd--%>
          <div>
          Country: <asp:DropDownList ID="ddlCountries" runat="server">
    <asp:ListItem Text="All" Value="" />
    <asp:ListItem Text="USA" Value="USA" />
    <asp:ListItem Text="Brazil" Value="Brazil" />
    <asp:ListItem Text="France" Value="France" />
    <asp:ListItem Text="Germany" Value="Germany" />
</asp:DropDownList>
<asp:Button ID="btnSubmit" runat="server" Text="Load Customers"
    OnClick="btnSubmit_Click"  />
<hr />
<asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField DataField="CustomerId" HeaderText="Customer Id" />
        <asp:BoundField DataField="ContactName" HeaderText="Contact Name" />
        <asp:BoundField DataField="City" HeaderText="City" />
    </Columns>
</asp:GridView>
<div class="loading" align="center">
    Loading. Please wait.<br />
    <br />
    <img src="loader.gif" alt="" />
</div>
          </div>

      </div>

      </form>

</body>
</html>
