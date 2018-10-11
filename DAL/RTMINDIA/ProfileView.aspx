<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProfileView.aspx.cs" Inherits="ProfileView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<form runat="server">
   <div class="row">
	<div class="col-lg-12">
		<h3 class="page-header"><i class="fa fa-user"></i>Profile</h3>
		<ol class="breadcrumb">
			<li><i class="fa fa-home"></i><a href="DashBoard_Admin.aspx">Home</a></li>
			<li><i class="fa fa-user"></i>Profile</li>
		</ol>
	 </div>
   </div>

   <div class="row">
                 <div class="col-lg-12">
                    <section class="panel">
                          <header class="panel-heading tab-bg-info">
                              <ul class="nav nav-tabs">
                                 
                                  <li class="active">
                                      <a data-toggle="tab" href="#profile">
                                          <i class="icon-user"></i>
                                          Profile
                                      </a>
                                  </li>
                                  <li class="">
                                      <a data-toggle="tab" href="#edit-profile">
                                          <i class="icon-envelope"></i>
                                          Edit Profile
                                      </a>
                                  </li>
                              </ul>
                          </header>
                          <div class="panel-body">
                              <div class="tab-content">
                                  <!-- profile -->
                                  <div id="profile" class="tab-pane active">
                                    <section class="panel">
                                     
                                          <div class="row">
                                          <div class="col-md-12 col-sm-12 col-xs-12">
                                             <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    <asp:Label ID="lblViewHeader" runat="server" Text="User  Details" ></asp:Label>
                                                </div>
                                           <div class="panel-body">
                                             
                                              <div class="bio-row">
                                                  <p><span>Full Name </span>: <asp:Label ID="lblName" runat="server" Text=""></asp:Label> </p>
                                             
                                                 <p><span>Team Name </span>: <asp:Label ID="lblTeam" runat="server" Text=""></asp:Label></p>
                                             
                                                  <p><span>Employee Id</span>: <asp:Label ID="lblEmpId" runat="server" Text=""></asp:Label></p>
                                              
                                                  <p><span>User Name</span>: <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label></p>
                                             
                                                  <p><span>Gender</span>: <asp:Label ID="lblGender" runat="server" Text=""></asp:Label></p>
                                             
                                                  <p><span>Email Id </span>: <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></p>
                                              </div>
                                          </div>
                                          </div>

                                        </div>
                                      </div>
                                    </section>
                                      <section>
                                          <div class="row">                                              
                                          </div>
                                      </section>
                                  </div>
                                  <!-- edit-profile -->
                                  <div id="edit-profile" class="tab-pane">
                                    <section class="panel">                                          
                                        <div class="panel-body">
                                          <div class="row">
                                            <div class="col-lg-12">
                                              <div class="panel panel-default">
                                                 <div class="panel-heading">
                                                      Edit Details <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                                 </div>
                                                 <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-lg-6">
                                                            <form role="form">
                                                               <div class="form-group">
                                                                    <asp:Label ID="lblEditFullName" runat="server" Text="Full Name:"></asp:Label>
                                                                    <asp:TextBox ID="txtName" runat="server" ReadOnly="True" class="form-control"></asp:TextBox>
                                                                    <br />
                                                                    <asp:Label ID="lblEditTeam" runat="server" Text="Team Name:"></asp:Label>
                                                                    <asp:TextBox ID="txtTeam" runat="server" ReadOnly="True" class="form-control"></asp:TextBox>
                                                                    <br />
                                                                    <asp:Label ID="lblEditUserName" runat="server" Text="User Name:"></asp:Label>
                                                                    <asp:TextBox ID="txtUserName"   runat="server" ReadOnly="True" class="form-control"></asp:TextBox>
                                                                </div>
                                                                <asp:Button ID="btnSubmit" class="btn btn-default" runat="server" Text="Save" onclick="btnSubmit_Click" />
                                                            </form>
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <form role="form">
                                                               <div class="form-group">
                                                                   <asp:Label ID="lblEditEmpId" runat="server" Text="Employee Id:"></asp:Label>
                                                                   <asp:TextBox ID="txtEmployeeId" runat="server" ReadOnly="True" class="form-control"></asp:TextBox>
                                                                   <br />
                                                                   <br />
                                                                  
                                                                   <asp:Label ID="lblEditGender" runat="server" Text="Gender:"></asp:Label>
                                                                   <label class="radio-inline">
                                                                        <asp:RadioButton ID="rbMale" runat="server" GroupName="gender" Text="M" />
                                                                   </label>
                                                                    <label class="radio-inline">
                                                                        <asp:RadioButton ID="rbFemale" runat="server" GroupName="gender" Text="F" />
                                                                    </label>
                                                                   <br />
                                                                   <br />
                                                                   
                                                                   <asp:Label ID="lblEditEmail" runat="server" Text="Email Id:"></asp:Label>
                                                                   <asp:TextBox ID="txtEmail" runat="server" class="form-control"></asp:TextBox>
               
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                                        ControlToValidate="txtEmail" ErrorMessage="Enter valid Email id" 
                                                                        Font-Bold="True" ForeColor="Red" SetFocusOnError="True" 
                                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                                        ControlToValidate="txtEmail" ErrorMessage="Please enter Valid email id" 
                                                                        Font-Bold="True" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                               </div>
                                                            </form>
                                                        </div>
                                                     </div>
                                                  </div>
                                                </div>
                                              </div>
                                           </div>                    
                                          </div>
                                     </section>
                                  </div>
                              </div>
                          </div>
                      </section>
                 </div>
              </div>
</form>
</asp:Content>

