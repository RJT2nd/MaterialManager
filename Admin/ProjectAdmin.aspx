﻿<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProjectAdmin.aspx.cs" Inherits="Admin_ProjectAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="AdminPage.aspx">Admin</a></li>
        <li class="active">Projects</li>
    </ul>
    <h1>Project List</h1>
    <asp:Panel id="filter" CssClass="form-group" runat="server" DefaultButton="FilterProjectsButton">
        <asp:TextBox runat="server" CssClass="form-control col-lg-4" ID="FilterTB"></asp:TextBox>
        <asp:DropDownList runat="server" CssClass="form-control col-lg-4" ID="FilterDDL">
            <asp:ListItem>Project ID</asp:ListItem>
            <asp:ListItem>Project Name</asp:ListItem>
            <asp:ListItem>Contract Number</asp:ListItem>
        </asp:DropDownList>
        <asp:Button runat="server" CssClass="btn btn-default" ID="FilterProjectsButton" OnClick="FilterProjectList" Text="Filter" />
    </asp:Panel>
    <asp:GridView runat="server" OnRowDataBound="ProjectListGridView_RowDataBound" AllowSorting="true" DataKeyNames="ProjectID" ID="ProjectListGridView" ItemType="MaterialManager.Models.Project" SelectMethod="ProjectList_GetData"
        CssClass="table table-striped table-bordered table-hover" OnRowCommand="ProjectListGridView_RowCommand" AutoGenerateDeleteButton="True" AutoGenerateEditButton="True"
        DeleteMethod="ProjectListGridView_DeleteItem" UpdateMethod="ProjectListGridView_UpdateItem" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ProjectID" HeaderText="Project ID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ReadOnly="true"/>
            
            <asp:TemplateField HeaderText="Project Name" SortExpression="ProjectName">
                <EditItemTemplate>
                    <asp:TextBox ID="ProjectNameTB" runat="server" Text='<%# Bind("ProjectName") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="ProjectNameLabel" runat="server" Text='<%# Bind("ProjectName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Contract Number" SortExpression="ContractNumber">
                <EditItemTemplate>
                    <asp:TextBox ID="ContractNumberTB" runat="server" Text='<%# Bind("ContractNumber") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="ContractNumberLabel" runat="server" Text='<%# Bind("ContractNumber") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Funds Allocated" SortExpression="FundsAllocated">
                <EditItemTemplate>
                    <asp:TextBox ID="FundsAllocatedTB" runat="server" Text='<%# Bind("FundsAllocated") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="FundsAllocatedLabel" runat="server" Text='<%# Bind("FundsAllocated", "{0:C}") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <Button runat="server" onserverclick="AddProject" class="btn btn-primary">New Project</Button>
</asp:Content>

