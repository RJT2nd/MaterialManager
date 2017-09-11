<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddFromRFQ.aspx.cs" Inherits="Mgr_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <ul class="breadcrumb">
        <li><a href="SelectProject.aspx">Home</a></li>
        <li><a href="ProjectMenu.aspx">Project</a></li>
        <li><a href="RFQList.aspx">RFQ List</a></li>
        <li class="active">Items From RFQ</li>
    </ul>
    <div id="SelectProjectTitle" runat="server" class="ContentHead">
        <h1>Add Line Items</h1>

        <p><b>Project: </b>
            <asp:Label runat="server" ID="ProjectName" />
        </p>
        <p><b>Contract Number: </b>
            <asp:Label runat="server" ID="ContractNumber" /></p>
    </div>

    <asp:DetailsView runat="server" ID="RFQDetails" DataKeyNames="RFQID" SelectMethod="RFQDetails_GetItem" InsertMethod="RFQDetails_InsertItem" UpdateMethod="RFQDetails_UpdateItem"
        EnableViewState="false" AutoGenerateRows="false" ItemType="MaterialManager.Models.RequestForQuote" CssClass="table table-striped table-bordered table-hover" CellPadding="4" AutoGenerateEditButton="true">
        <Fields>
            <asp:BoundField DataField="RFQID" HeaderText="RFQ ID" ReadOnly="true" ><HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle></asp:BoundField>
            <asp:BoundField DataField="JBReferenceCode" HeaderText="RFQ Reference Number" ReadOnly="true" ><HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle></asp:BoundField>
            <asp:BoundField DataField="RFQDate" HeaderText="RFQ Date" ><HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle></asp:BoundField>
            <asp:BoundField DataField="RFQtoVendorDate" HeaderText="RFQ Sent to Vendor"><HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle></asp:BoundField>
            <asp:BoundField DataField="ReviewStatus" HeaderText="Review Status" ReadOnly="true" ><HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle></asp:BoundField>
            <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status" ReadOnly="true" ><HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle></asp:BoundField>
        </Fields>    
    </asp:DetailsView>

    <h3>RFQ List</h3>
    <div id="filter" class="form-group">
        <asp:TextBox runat="server" CssClass="form-control col-lg-4" ID="FilterTB"></asp:TextBox>
        <asp:DropDownList runat="server" CssClass="form-control col-lg-4" ID="FilterDDL">
            <asp:ListItem>RFQ ID</asp:ListItem>
            <asp:ListItem>JobBoss RFQ Reference</asp:ListItem>
            <asp:ListItem>RFQ Sent to Vendor</asp:ListItem>
            <asp:ListItem>Date of RFQ</asp:ListItem>
            <asp:ListItem>Review Status</asp:ListItem>
            <asp:ListItem>Approval Status</asp:ListItem>
        </asp:DropDownList>
        <button runat="server" id="FilterSubmit" class="btn btn-default" onserverclick="FilterRFQList">Filter</button>
    </div>
    <asp:GridView ID="RFQList" runat="server" AllowSorting="true" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="MaterialManager.Models.RequestForQuote"
        SelectMethod="RFQList_GetData" CssClass="table table-striped table-bordered table-hover" OnRowCommand="RFQList_RowCommand"
        DataKeyNames="RFQID" AutoGenerateSelectButton="false" SelectedRowStyle-CssClass="warning" OnSelectedIndexChanged="RFQList_SelectedIndexChanged">
        <Columns>

            <asp:HyperLinkField Text="View" DataNavigateUrlFields="RFQID" DataNavigateUrlFormatString="RFQView.aspx?RFQID={0}" />
            <asp:BoundField DataField="RFQID" HeaderText="part ID" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" SortExpression="RFQID" />
            <asp:BoundField DataField="JBReferenceCode" HeaderText="JobBoss RFQ Reference" SortExpression="JBReferenceCode" />
            <asp:BoundField DataField="RFQToVendorDate" HeaderText="RFQ Sent to Vendor" SortExpression="RFQToVendorDate" />
            <asp:BoundField DataField="RFQDate" HeaderText="Date of RFQ" SortExpression="RFQDate" />
            <asp:BoundField DataField="ReviewStatus" HeaderText="Review Status" SortExpression="ReviewStatus" />
            <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status" SortExpression="ApprovalStatus" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button CssClass="btn btn-primary" Text="Add" runat="server" CommandName="AddFromRFQ_Click" CommandArgument='<%# Eval("RFQID") %>' />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

</asp:Content>

