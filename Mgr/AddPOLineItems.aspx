﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddPOLineItems.aspx.cs" Inherits="Mgr_AddPOLineItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <ul class="breadcrumb">
        <li><a href="SelectProject.aspx">Home</a></li>
        <li><a href="ProjectMenu.aspx">Project</a></li>
        <li><a href="POList.aspx">PO List</a></li>
        <li class="active">Create PO</li>
    </ul>

    <div id="AddPOLineItemsTitle" runat="server" class="ContentHead">
        <h1>Add Line Items</h1>

        <p><b>Project: </b>
            <asp:Label runat="server" ID="ProjectName" />
        </p>
        <p><b>Contract Number: </b>
            <asp:Label runat="server" ID="ContractNumber" /></p>
        <p><b>Purchase Order Total: </b>
            <asp:Label runat="server" ID="POTotal" /></p>
    </div>

    <asp:Panel ID="filterJobBossPartsForm" CssClass="" runat="server" DefaultButton="FilterJobBossPartsButton">
        <asp:TextBox CssClass="form-control col-lg-4" runat="server" ID="FilterJobBossPartsTB"></asp:TextBox>
        <asp:DropDownList CssClass="form-control col-lg-4" runat="server" ID="FilterJobBossPartsDDL">
            <asp:ListItem>Material</asp:ListItem>
            <asp:ListItem>Description</asp:ListItem>
            <asp:ListItem>Vendor Reference</asp:ListItem>
            <asp:ListItem>Notes</asp:ListItem>
            <asp:ListItem>Extended Description</asp:ListItem>
            <asp:ListItem>Last Updated</asp:ListItem>
            <asp:ListItem>Primary Vendor</asp:ListItem>
            <asp:ListItem>Shape</asp:ListItem>
        </asp:DropDownList>
        <asp:Button CssClass="btn btn-default" runat="server" ID="FilterJobBossPartsButton" OnClick="FilterJobBossPartsList" Text="Filter" />
    </asp:Panel>
    <asp:GridView ID="JobBossPartsList" runat="server" AllowSorting="true" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="JBXML.Respond.JBXMLJBXMLRespondMaterialListQueryRsMaterial"
        SelectMethod="JobBossPartsList_GetData" CssClass="table table-striped table-bordered table-hover" OnRowCommand="JobBossPartsList_RowCommand"
        AutoGenerateDeleteButton="false" DataKeyNames="Material">
        <Columns>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton Text="Add To PO" runat="server" CausesValidation="false" CommandName="AddToPO" CommandArgument='<%# Eval("Material") + "," + Container.DataItemIndex %>' ControlStyle-CssClass="btn btn-primary">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <asp:TextBox ID="quantity" TextMode="Number" Text="1" Width="8EM" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Price">
                <ItemTemplate>
                    <asp:TextBox ID="price" TextMode="Number" Text="0.00" Width="8EM" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Material" HeaderText="Material" SortExpression="Material" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:BoundField DataField="Vendor_Reference" HeaderText="Vendor Reference" SortExpression="Vendor_Reference" />
            <asp:BoundField DataField="Note_Text" HeaderText="Notes" SortExpression="Note_Text" />
            <asp:BoundField DataField="Ext_Description" HeaderText="Extended Description" SortExpression="Ext_Description" />
            <asp:BoundField DataField="Last_Updated" HeaderText="Last Updated" SortExpression="Last_Updated" />
            <asp:BoundField DataField="Primary_Vendor" HeaderText="Primary Vendor" SortExpression="Primary_Vendor" />
            <asp:BoundField DataField="Shape" HeaderText="Shape" SortExpression="Shape" />


        </Columns>
    </asp:GridView>
</asp:Content>
