<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserAdmin.aspx.cs" Inherits="Admin_UserAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <ul class="breadcrumb">
        <li><a href="~/">Home</a></li>
        <li><a href="~/Admin/AdminPage.aspx">Admin</a></li>
        <li class="active">User Administration</li>
    </ul>
    <div id="UserAdminTitle" runat="server" class="ContentHead">
        <h1>User Administration</h1>
    </div>
    <asp:Panel ID="filter" CssClass="form-group" runat="server" DefaultButton="FilterUsersButton">
        <asp:TextBox runat="server" CssClass="form-control col-lg-4" ID="FilterTB"></asp:TextBox>
        <asp:DropDownList runat="server" CssClass="form-control col-lg-4" ID="FilterDDL">
            <asp:ListItem>User ID</asp:ListItem>
            <asp:ListItem>User Name</asp:ListItem>
            <asp:ListItem>Title</asp:ListItem>
            <asp:ListItem>E-Mail</asp:ListItem>
            <asp:ListItem>JobBoss User</asp:ListItem>
        </asp:DropDownList>
        <asp:Button runat="server" CssClass="btn btn-default" ID="FilterUsersButton" OnClick="FilterUserList" Text="Filter" />
    </asp:Panel>
    <asp:GridView ID="UserList" runat="server" AllowSorting="true" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="MaterialManager.ApplicationUser"
        SelectMethod="GetUsers" CssClass="table table-striped table-bordered table-hover">
        <Columns>
            <asp:HyperLinkField Text="Edit" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="EditUser.aspx?id={0}" />
            <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="true" SortExpression="Id" />
            <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
            <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" />
            <asp:BoundField DataField="JBUser" HeaderText="JobBoss User" SortExpression="JBUser" />
        </Columns>
    </asp:GridView>

</asp:Content>

