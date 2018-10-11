<%@ Page Title="" Language="C#" MasterPageFile="~/Home.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
 <style>
    #position
    {
        position:fixed;
        top: 52%;
        left: 50%;
        width:30em;
        height:18em;
        margin-top: -9em; /*set to a negative number 1/2 of your height*/
        margin-left: -15em; /*set to a negative number 1/2 of your width*/
    }
   .modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=60);
        opacity: 0.6;
    }
     .modalPopup
    {
        background-color: #FFFFFF;
        width: 300px;
        border: 3px solid #0DA9D0;
        border-radius: 12px;
        padding:0
      
    }
    .modalPopup .header
    {
        background-color: #2FBDF1;
        height: 30px;
        color: White;
        line-height: 30px;
        text-align: center;
        font-weight: bold;
        border-top-left-radius: 6px;
        border-top-right-radius: 6px;
    }
    .modalPopup .body
    {
        min-height: 50px;
        line-height: 30px;
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
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 
<!-- Banner -->
  <form id="Form1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
            <div class="mbr-box__container mbr-section__container container">
            <div class="mbr-box mbr-box--stretched"><div class="mbr-box__magnet mbr-box__magnet--center-left">
                <div class="mbr-hero animated fadeInUp">
                    <h1 class="mbr-hero__text">RTM Reports</h1>
                    <p class="mbr-hero__subtext"><asp:Label ID="lblUserName" runat="server" Text=""></asp:Label></p>
                </div>
                <div class="mbr-buttons btn-inverse mbr-buttons--left">
                   <asp:Button ID="btnLogin" runat="server" class="mbr-buttons__btn btn btn-lg btn-warning animated fadeInUp delay"
                            Text="Login" onclick="btnLogin_Click"></asp:Button>
                            <br />
                    <asp:Label ID="lblError" runat="server" Font-Bold="True"></asp:Label>
                </div>
            </div></div>
        </div>
        <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
    <asp:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="divPopUp">
    </asp:ModalPopupExtender>
    <div id="divPopUp" class="modalPopup" style="display: none">
    <div class="header">
       Location Selection
    </div>
    <div class="body">
        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
        <div style="text-align:center">
            <asp:Label ID="lblLocation" runat="server" Text="Select Location:"></asp:Label>
            <asp:DropDownList ID="ddlLocation" runat="server">
            </asp:DropDownList>
        </div>
        </div>
        <div class="footer" align="right">
            <asp:Button ID="btnYes" runat="server" Text="Select" CssClass="yes" 
                onclick="btnYes_Click" />
            <asp:Button ID="btnNo" runat="server" Text="Cancel" CssClass="no" 
                onclick="btnNo_Click" />
        </div>
    </div>
   </form>
</asp:Content>

