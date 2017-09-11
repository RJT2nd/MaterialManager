<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ForgetPassword.aspx.cs" Inherits="Account_ForgetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <h2>Forget Password</h2>

    <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-md-2 control-label">User name</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                CssClass="text-danger" ErrorMessage="The user name field is required." />
        </div>
    </div>

    <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="Email" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                CssClass="text-danger" ErrorMessage="The email field is required." />
        </div>
    </div>

    <asp:Button ID="RequestPassword" runat="server" OnClick="RequestPassword_Click" CssClass="btn btn-default" Text="Request Password" /> 

</asp:Content>

