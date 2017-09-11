<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SelectProject.aspx.cs" Inherits="Mgr_SelectProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <ul class="breadcrumb">
        <li><a href="SelectProject.aspx">Home</a></li>
        <li><a href="ProjectMenu.aspx">Project</a></li>
        <li class="active">Select Project</li>
    </ul>
    <div id="SelectProjectTitle" runat="server" class="ContentHead">
        <h1>Select Project</h1>
    </div>
    <asp:Panel id="filter" CssClass="form-group" runat="server" DefaultButton="FilterProjectsButton">
        <asp:TextBox runat="server" CssClass="form-control col-lg-4" ID="FilterTB"></asp:TextBox>
        <asp:DropDownList runat="server" CssClass="form-control col-lg-4" ID="FilterDDL">
            <asp:ListItem>Project ID</asp:ListItem>
            <asp:ListItem>Project Name</asp:ListItem>
            <asp:ListItem>Contract Number</asp:ListItem>
        </asp:DropDownList>
        <asp:Button runat="server" CssClass="btn btn-default" ID="FilterProjectsButton" OnClick="FilterProjectList" Text="Filter" />
    </asp:Panel>
    <asp:GridView ID="ProjectList" runat="server" AllowSorting="true" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="MaterialManager.Models.Project"
        SelectMethod="ProjectList_GetData" CssClass="table table-striped table-bordered table-hover" OnRowCommand="ContactsGridView_RowCommand">
        <Columns>
            
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton runat="server" CssClass="btn btn-primary" Text="Select" CommandName="SelectProject" CommandArgument='<%# Eval("ProjectID") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ProjectID" HeaderText="Project ID" SortExpression="ProjectID" />
            <asp:BoundField DataField="ProjectName" HeaderText="Project Name" SortExpression="ProjectName" />
            <asp:BoundField DataField="ContractNumber" HeaderText="Contract Number" SortExpression="ContractNumber" />

        </Columns>
    </asp:GridView>

</asp:Content>

