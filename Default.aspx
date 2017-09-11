<%@ Page Title="Material Manager" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<h1><%: Title %></h1>
    <h3>Welcome to the Material Manager</h3>
    <div class="row">
        <div class="col-lg-4">
            <div class="bs-component">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title">Login</h3>
                    </div>
                    <div class="panel-body">
                        <ul>
                            <li><a href="Account/Register.aspx">Register</a></li>
                            <li><a href="Account/Login.aspx">Login</a></li>
                        </ul>
                    </div>
                </div>

            </div>
        </div>
        <div class="col-lg-4">
            <div class="bs-component">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title">Project</h3>
                    </div>
                    <div class="panel-body">
                        <ul>
                            <li><a href="mgr/SelectProject.aspx">Select Project</a></li>
                            <li><a href="mgr/ProjectMenu.aspx">Project Menu</a></li>
                        </ul>
                    </div>
                </div>

            </div>
        </div>
        <asp:Panel runat="server" OnLoad="AdminPanel_Load" ID="AdminPanel" CssClass="col-lg-4">
            <div class="bs-component">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title">Admin</h3>
                    </div>
                    <div class="panel-body">
                        <ul>
                            <li><a href="Admin/AdminPage.aspx">Admin Menu</a></li>
                            <li><a href="Admin/UserAdmin.aspx">User Administration</a></li>
                            <li><a href="Admin/ProjectAdmin.aspx">Project Administration</a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    
</asp:Content>
