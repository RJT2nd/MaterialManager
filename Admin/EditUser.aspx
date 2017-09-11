<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EditUser.aspx.cs" Inherits="Admin_EditUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <ul class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/Admin/AdminPage.aspx">Admin</a></li>
        <li><a href="/Admin/UserAdmin.aspx">User Administration</a></li>
        <li class="active">Edit User</li>
    </ul>

    <div id="EditUserTitle" runat="server" class="ContentHead">
        <h1>Edit User</h1>
    </div>

    <asp:DetailsView ID="UserDetails" runat="server" DataKeyNames="Id" SelectMethod="UserDetails_GetItem" UpdateMethod="UserDetails_UpdateItem" InsertMethod="UserDetails_InsertItem"
        AutoGenerateEditButton="true" AutoGenerateInsertButton="true" EnableViewState="false" AutoGenerateRows="false" ItemType="MaterialManager.ApplicationUser"
        CssClass="table table-striped table-bordered table-hover" CellPadding="4">
        <Fields>

            <asp:BoundField DataField="Id" HeaderText="ID" HeaderStyle-Width="25%" SortExpression="ID" HeaderStyle-CssClass="info" />
            <asp:BoundField DataField="UserName" HeaderText="User Name" HeaderStyle-Width="25%" SortExpression="UserName" HeaderStyle-CssClass="info">
                <HeaderStyle CssClass="info" Width="25%"></HeaderStyle>
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
                <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="roleEditList">
                        <asp:ListItem>Administrator</asp:ListItem>
                        <asp:ListItem>Approval</asp:ListItem>
                        <asp:ListItem>Review</asp:ListItem>
                        <asp:ListItem>Creation</asp:ListItem>
                        <asp:ListItem>ReadOnly</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:DropDownList runat="server" ID="roleInsertList">
                        <asp:ListItem>Administrator</asp:ListItem>
                        <asp:ListItem>Approval</asp:ListItem>
                        <asp:ListItem>Review</asp:ListItem>
                        <asp:ListItem>Creation</asp:ListItem>
                        <asp:ListItem>ReadOnly</asp:ListItem>
                    </asp:DropDownList>
                </InsertItemTemplate>

                <HeaderStyle CssClass="info"></HeaderStyle>
            </asp:TemplateField>


        </Fields>


    </asp:DetailsView>
    <button runat="server" onserverclick="ResetPassword_Clicked" class="btn btn-default">Reset Password</button>
    <div id="ResetStatus" runat="server"></div>

</asp:Content>

