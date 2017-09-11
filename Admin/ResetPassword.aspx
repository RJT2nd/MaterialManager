<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="Admin_ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/Admin/AdminPage.aspx">Admin</a></li>
        <li><a href="/Admin/UserAdmin.aspx">User Administration</a></li>
        <li class="active">Reset Password</li>
    </ul>

    <div id="ChangeUserPassword" runat="server" class="ContentHead">
        <h1>Reset User Password</h1>
    </div>

    <asp:DetailsView ID="UserDetails" runat="server" DataKeyNames="Id" SelectMethod="UserDetails_GetItem" UpdateMethod="UserDetails_UpdateItem" InsertMethod="UserDetails_InsertItem" AutoGenerateEditButton="true" AutoGenerateInsertButton="true"
        EnableViewState="false" AutoGenerateRows="false" ItemType="MaterialManager.ApplicationUser" CssClass="table table-striped table-bordered table-hover" CellPadding="4">
        <Fields>

            <asp:BoundField DataField="UserName" HeaderText="User Name" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" SortExpression="UserName" HeaderStyle-CssClass="info">
                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField DataField="Title" HeaderText="Title" HeaderStyle-CssClass="info">
                <HeaderStyle CssClass="info"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField DataField="Email" HeaderText="E-Mail" HeaderStyle-CssClass="info">
                <HeaderStyle CssClass="info"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField DataField="JBUser" HeaderText="JobBoss User" HeaderStyle-CssClass="info">
                <HeaderStyle CssClass="info"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField DataField="JBPassword" HeaderText="JobBoss Password" HeaderStyle-CssClass="info">
                <HeaderStyle CssClass="info"></HeaderStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="Role" HeaderStyle-CssClass="info">
                <ItemTemplate>
                    <asp:Label runat="server" ID="UserRole" OnLoad="Role_Load"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>


        </Fields>


    </asp:DetailsView>
</asp:Content>

