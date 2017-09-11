<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="POView.aspx.cs" Inherits="Mgr_POView" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <ul class="breadcrumb">
        <li><a href="SelectProject.aspx">Home</a></li>
        <li><a href="ProjectMenu.aspx">Project</a></li>
        <li><a href="POList.aspx">PO List</a></li>
        <li class="active">PO Details</li>
    </ul>

    <div id="POViewTitle" runat="server" class="ContentHead">
        <h1>Purchase Order Details</h1>

        <p>
            <b>Project: </b>
            <asp:Label runat="server" ID="ProjectName" />
        </p>
        <p>
            <b>Contract Number: </b>
            <asp:Label runat="server" ID="ContractNumber" />
        </p>

    </div>
    <asp:DetailsView ID="PODetails" runat="server" DataKeyNames="PurchaseOrderID" SelectMethod="PODetails_GetItem" InsertMethod="PODetails_InsertItem" UpdateMethod="PODetails_UpdateItem"
        AutoGenerateEditButton="false"
        EnableViewState="false" AutoGenerateRows="false" ItemType="MaterialManager.Models.PurchaseOrder" CssClass="table table-striped table-bordered table-hover" CellPadding="4">
        <Fields>
            <asp:BoundField DataField="PurchaseOrderID" HeaderText="Purchase Order ID" HeaderStyle-CssClass="info" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" />
            <asp:BoundField DataField="POVendor" HeaderText="Vendor" HeaderStyle-CssClass="info" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" />
            <asp:BoundField DataField="POtoVendorDate" HeaderText="Purchase Order Sent to Vendor" HeaderStyle-CssClass="info" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" />
            <asp:BoundField DataField="RequiredDeliveryDate" HeaderText="Required Delivery Date" HeaderStyle-CssClass="info" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" />
            <asp:BoundField DataField="ExpectedDeliveryDate" HeaderText="Expected Delivery Date" HeaderStyle-CssClass="info" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" />
            <asp:BoundField DataField="ActualDeliveryDate" HeaderText="Actual Delivery Date" HeaderStyle-CssClass="info" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" />
            <asp:BoundField DataField="DeliveryAddress" HeaderText="Delivery Address" HeaderStyle-CssClass="info" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" />
            <asp:BoundField DataField="MarkFor" HeaderText="Mark For" HeaderStyle-CssClass="info" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" />
            <asp:BoundField DataField="Justification" HeaderText="Justification" HeaderStyle-CssClass="info" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" />
            <asp:BoundField DataField="ReviewStatus" HeaderText="Review Status" HeaderStyle-CssClass="info" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" />
            <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status" HeaderStyle-CssClass="info" HeaderStyle-Font-Bold="true" HeaderStyle-Width="25%" />
            <asp:TemplateField HeaderText="Purchase Order Total">
                <ItemTemplate>
                    <asp:Label runat="server" OnLoad="POTotal_Load" ID="POTotal" />
                </ItemTemplate>
                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>
        </Fields>
    </asp:DetailsView>

    <asp:LinkButton ID="EditLinkButton" CssClass="btn btn-default" runat="server" OnLoad="EditLinkButton_Load" OnClick="EditLinkButton_Click" Text="Edit" />
    <asp:Button ID="RevertStatus" runat="server" OnClick="RevertStatus_Clicked" OnLoad="RevertStatus_OnLoad" ControlStyle-CssClass="btn btn-default" />
    <asp:Button ID="ChangeStatus" runat="server" OnClick="ChangeStatus_Clicked" OnLoad="ChangeStatus_OnLoad" ControlStyle-CssClass="btn btn-primary" />

    <!-- Export To Excel Section -->
    <button runat="server" class="btn btn-info pull-right" data-toggle="modal" data-target="#ExportModal">Export PO</button>

    <div class="jumbotron">
        <h3>PO Line Items</h3>
        <div id="filter" class="form-group">
            <asp:TextBox runat="server" CssClass="form-control col-lg-4" ID="FilterTB"></asp:TextBox>
            <asp:DropDownList runat="server" CssClass="form-control col-lg-4" ID="FilterDDL">
                <asp:ListItem>JobBoss ID</asp:ListItem>
                <asp:ListItem>NSN</asp:ListItem>
                <asp:ListItem>Part Number</asp:ListItem>
                <asp:ListItem>Description</asp:ListItem>
                <asp:ListItem>Quantity</asp:ListItem>
                <asp:ListItem>Price</asp:ListItem>
            </asp:DropDownList>
            <button type="submit" runat="server" class="btn btn-default" id="FilterSubmit" onserverclick="FilterPOLineItemList">Filter</button>
        </div>
        <asp:GridView ID="POLineItemList" runat="server" AllowSorting="true" AutoGenerateColumns="False" GridLines="Vertical" CellPadding="4" ItemType="MaterialManager.Models.ExtendedPOLineItem"
            SelectMethod="POLineItemList_GetData" CssClass="table table-striped table-bordered table-hover" OnRowCommand="POLineItemList_RowCommand" OnRowDataBound="POLineItemList_RowDataBound"
            AutoGenerateDeleteButton="True" DeleteMethod="POLineItemList_DeleteItem" DataKeyNames="POLineItemID">
            <Columns>
                <asp:BoundField DataField="PartID" HeaderText="Part ID" SortExpression="PartID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="JBMaterialID" HeaderText="JobBoss ID" SortExpression="JBMaterialID" />
                <asp:BoundField DataField="NSN" HeaderText="NSN" SortExpression="NSN" />
                <asp:BoundField DataField="PartNumber" HeaderText="Part Number" SortExpression="PartNumber" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                <asp:BoundField DataField="Price" HeaderText="Unit Price" SortExpression="Price" DataFormatString="{0:C}" />
                <asp:BoundField DataField="TotalPrice" HeaderText="Total Price" SortExpression="TotalPrice" DataFormatString="{0:C}" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton Text="View Job Boss Data" runat="server" CausesValidation="false" CommandName="ViewJobBossData" CommandArgument='<%# Eval("PartID") %>' ControlStyle-CssClass="btn btn-primary">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
    <div class="jumbotron">
        <h3>PO Documents</h3>
        <asp:Panel runat="server" ID="filterDocs" CssClass="form-group" DefaultButton="FilterDocsSubmit">
            <asp:TextBox runat="server" CssClass="form-control col-lg-4" ID="FilterDocsTB"></asp:TextBox>
            <asp:DropDownList runat="server" CssClass="form-control col-lg-4" ID="FilterDocsDDL">
                <asp:ListItem>File Name</asp:ListItem>
                <asp:ListItem>Description</asp:ListItem>
            </asp:DropDownList>
            <asp:Button runat="server" CssClass="btn btn-default" OnClick="FilterPODocumentList" ID="FilterDocsSubmit" Text="Filter" />
        </asp:Panel>
        <asp:GridView ID="PODocumentList" runat="server" AllowSorting="true" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="MaterialManager.Models.PODocument"
            SelectMethod="PODocumentList_GetData" CssClass="table table-striped table-bordered table-hover" OnRowCommand="PODocumentList_RowCommand" OnRowDataBound="PODocumentList_RowDataBound"
            AutoGenerateDeleteButton="true" DeleteMethod="PODocumentList_DeleteItem" DataKeyNames="PODocID" AutoGenerateSelectButton="false">
            <Columns>

                <asp:BoundField DataField="POID" HeaderText="Purchase Order ID" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" SortExpression="POID" />
                <asp:BoundField DataField="PODocID" HeaderText="PO Document ID" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" SortExpression="PODocID" />
                <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName" />
                <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" Text="Download" CommandName="DownloadFile" CommandArgument='<%# Eval("FileName") + "," + Eval("PODocID") %>' CssClass="btn btn-primary" />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
        <asp:Label runat="server" ID="LabelAddStatus"></asp:Label>
        <!-- Trigger the modal with a button -->
        <button type="button" class="btn btn-info" id="AddDocumentButton" runat="server" onload="AddDocumentButton_Load" data-toggle="modal" data-target="#addDocModal">Add Document</button>
    </div>
    <!-- Modal -->
    <div id="addDocModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Add PO Document</h4>
                </div>
                <div class="modal-body">
                    <fieldset>


                        <div class="form-group">
                            <label for="fileDescription" class="col-lg-2 control-label">File Description:</label>
                            <div class="col-lg-10">
                                <asp:TextBox runat="server" CssClass="form-control" Rows="3" ID="fileDescription" placeholder="Description of file..."></asp:TextBox>

                            </div>

                        </div>
                        <div class="form-group">
                            <div class="col-lg-10">
                                <br />
                                <asp:FileUpload ID="POFile" runat="server" />
                            </div>
                        </div>

                    </fieldset>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <asp:Button type="button" ID="AddDocButton" CssClass="btn btn-primary" runat="server" OnClick="AddDocButton_Click" Text="Save"></asp:Button>
                </div>
            </div>

        </div>
    </div>

    <!-- Modal -->
    <div id="ExportModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Export PO</h4>
                </div>
                <div class="modal-body">
                    <fieldset>
                        <div class="form-group">
                            <label for="ExportOption" class="col-lg-2 control-label">Excel Format:</label>
                            <div class="col-lg-10">
                                <asp:DropDownList ID="ExportOption" CssClass="form-control" runat="server">
                                    <asp:ListItem Text="Vertical" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Horizontal" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:GridView ID="ExportPODocumentList" runat="server" AllowSorting="true" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4"
                                ItemType="MaterialManager.Models.PODocument" SelectMethod="PODocumentList_GetData" CssClass="table table-striped table-bordered table-hover"
                                OnRowCommand="PODocumentList_RowCommand" AutoGenerateDeleteButton="false" DeleteMethod="PODocumentList_DeleteItem" DataKeyNames="PODocID"
                                AutoGenerateSelectButton="false">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="DownloadCB" Checked="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="POID" HeaderText="Purchase Order ID" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" SortExpression="POID" />
                                    <asp:BoundField DataField="PODocID" HeaderText="PO Document ID" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" SortExpression="PODocID" />
                                    <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName" />
                                    <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="ExcelExport" runat="server" OnClick="ExcelExport_Click" ControlStyle-CssClass="btn btn-info" Text="Export PO" />
                </div>
            </div>

        </div>
    </div>
</asp:Content>
