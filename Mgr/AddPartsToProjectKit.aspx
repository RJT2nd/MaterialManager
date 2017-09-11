﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddPartsToProjectKit.aspx.cs" Inherits="Mgr_AddPartsToProjectKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="SelectProject.aspx">Home</a></li>
        <li><a href="ProjectMenu.aspx">Project</a></li>
        <li><a href="ProjectKitList.aspx">Kit List</a></li>
        <li class="active">Create Kit</li>
    </ul>

    <h1>Add Kit Items</h1>

    <p><b>Project: </b>
        <asp:Label runat="server" ID="ProjectName" />
    </p>
    <p><b>Contract Number: </b>
        <asp:Label runat="server" ID="ContractNumber" /></p>

    <h2>Project Kit Parts</h2>
    <asp:Panel id="filter" CssClass="form-group" runat="server" DefaultButton="FilterButton">
        <asp:TextBox CssClass="form-control col-lg-4" runat="server" ID="FilterTB"></asp:TextBox>
        <asp:DropDownList CssClass="form-control col-lg-4" runat="server" ID="FilterDDL">
            <asp:ListItem>JBMaterialID</asp:ListItem>
            <asp:ListItem>NSN</asp:ListItem>
            <asp:ListItem>Description</asp:ListItem>
            <asp:ListItem>Quantity</asp:ListItem>
        </asp:DropDownList>
        <asp:Button CssClass="btn btn-default" runat="server" ID="FilterButton" OnClick="FilterKitItemList" Text="Filter" />
    </asp:Panel>
    <asp:GridView ID="ProjectKitItemList" runat="server" AllowSorting="true" AutoGenerateColumns="false" GridLines="Vertical" CellPadding="4" ItemType="MaterialManager.Models.ExtendedProjectKitItem"
        SelectMethod="ProjectKitItemList_GetData" CssClass="table table-striped table-bordered table-hover" OnRowCommand="ProjectKitItemList_RowCommand" UpdateMethod="ProjectKitItemList_UpdateItem" OnRowUpdating="ProjectKitItemList_RowUpdating"
        AutoGenerateDeleteButton="true" DeleteMethod="ProjectKitItemList_DeleteItem" DataKeyNames="ProjectKitItemID" AutoGenerateEditButton="true">
        <Columns>
            <asp:TemplateField HeaderText="Project Kit ID" SortExpression="ProjectKitID">
                <EditItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("ProjectKitID") %>' ID="ProjectKitIDLabel"></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("ProjectKitID") %>' ID="ProjectKitIDLabel"></asp:Label>
                </ItemTemplate>

                <HeaderStyle CssClass="hidden"></HeaderStyle>

                <ItemStyle CssClass="hidden"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Project Kit Item ID" SortExpression="ProjectKitItemID">
                <EditItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("ProjectKitItemID") %>' ID="ProjectKitItemIDLabel"></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("ProjectKitItemID") %>' ID="ProjectKitItemIDLabel"></asp:Label>
                </ItemTemplate>

                <HeaderStyle CssClass="hidden"></HeaderStyle>

                <ItemStyle CssClass="hidden"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Part ID" SortExpression="PartID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden">
                <EditItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("PartID") %>' ID="PartIDLabel"></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("PartID") %>' ID="PartIDLabel"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="JobBoss ID" SortExpression="JBMaterialID">
                <EditItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("JBMaterialID") %>' ID="JBMaterialIDLabel"></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("JBMaterialID") %>' ID="JBMaterialIDLabel"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="NSN" SortExpression="NSN">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("NSN") %>' ID="NSNTB"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("NSN") %>' ID="NSNLabel"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Part Number" SortExpression="PartNumber">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("PartNumber") %>' ID="PartNumberTB"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("PartNumber") %>' ID="PartNumberLabel"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description" SortExpression="Description">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("Description") %>' ID="DescriptionTB"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="DescriptionLabel"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("Quantity") %>' ID="QuantityTB" TextMode="Number"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("Quantity") %>' ID="QuantityLabel"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton text="View Job Boss Data" runat="server" CausesValidation="false" CommandName="ViewJobBossData" CommandArgument='<%# Eval("PartID") %>' ControlStyle-CssClass="btn btn-primary">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
                
        </Columns>
    </asp:GridView>

    <button id="addPart" runat="server" onserverclick="AddPart_ServerClick" class="btn btn-primary">Add Part</button>
    <asp:LinkButton ID="ViewKitLinkButton" runat="server" OnClick="ViewKitLinkButton_Click" CssClass="btn btn-default" Text="Next" />
</asp:Content>
