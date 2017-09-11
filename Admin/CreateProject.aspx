<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CreateProject.aspx.cs" Inherits="Admin_CreateProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="AdminPage.aspx">Admin</a></li>
        <li><a href="ProjectAdmin.aspx">Projects</a></li>
        <li class="active">Create</li>
    </ul>
    <h1>Create New Project</h1>
    <asp:Panel runat="server" DefaultButton="submitButton" class="jumbotron">
        <h2>Project Details</h2>
        <hr />
        <div class="form form-group row">
            <label for="ProjectNameTB" class="col-sm-2 col-form-label">Project Name</label>
            <div class="col-sm-10">
                <asp:TextBox  runat="server" ID="ProjectNameTB" ControlStyle-CssClass="form-control input-group-sm"></asp:TextBox>
            </div>
        </div>
        <div class="form form-group row">
            <label for="ContractNumberTB" class="col-sm-2 col-form-label">Contract Number</label>
            <div class="col-sm-10">
                <asp:TextBox  runat="server" ID="ContractNumberTB" ControlStyle-CssClass="form-control input-group-sm"></asp:TextBox>
            </div>
        </div>
        <div class="form form-group row">
            <label for="FundsAlloctedTB" class="col-sm-2 col-form-label">Funds Allocated</label>
            <div class="col-sm-10">
                <asp:TextBox  runat="server" ID="FundsAlloctedTB" ControlStyle-CssClass="form-control input-group-sm"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="FundsAlloctedTB" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^-?\d+(\.\d+)?$"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-offset-2 col-xs-3 col-sm-2 col-md-1 col-lg-1">
                <button class="btn btn-default" runat="server" onserverclick="Cancel_Click">Cancel</button>
            </div>
            <div class="col-sm-2 col-md-1 col-xs-3 col-lg-1">
                <asp:Button Text="Create" ID="submitButton" runat="server" onclick="Submit_Click" class="btn btn-primary" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>

