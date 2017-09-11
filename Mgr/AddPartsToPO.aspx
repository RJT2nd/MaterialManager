<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" Title="" CodeFile="AddPartsToPO.aspx.cs" Inherits="Mgr_AddPartsToPO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <ul class="breadcrumb">
        <li><a href="SelectProject.aspx">Home</a></li>
        <li><a href="ProjectMenu.aspx">Project</a></li>
    </ul>
    <div id="AddPOLineItemsTitle" runat="server" class="ContentHead">
        <h1>Add Line Items</h1>

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
        AutoGenerateEditButton="true"
        EnableViewState="false" AutoGenerateRows="false" ItemType="MaterialManager.Models.PurchaseOrder" CssClass="table table-striped table-bordered table-hover" CellPadding="4">
        <Fields>
            <asp:TemplateField HeaderText="Purchase Order ID" SortExpression="UserName">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("PurchaseOrderID") %>' ID="TextBox1" ReadOnly="true"></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("PurchaseOrderID") %>' ID="TextBox1"></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("PurchaseOrderID") %>' ID="PurchaseOrderID"></asp:Label>
                </ItemTemplate>

                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Purchase Order Sent to Vendor">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("POVendor") %>' ID="POVendorTB"></asp:TextBox>
                    <div runat="server" id="POVendorValidation"></div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("POVendor") %>' ID="POVendorTB"></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("POVendor") %>' ID="POVendorLabel"></asp:Label>
                </ItemTemplate>

                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Purchase Order Sent to Vendor">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("POtoVendorDate") %>' ID="POtoVendorDateTB"></asp:TextBox>
                    <div runat="server" id="POtoVendorDateValidation"></div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("POtoVendorDate") %>' ID="TextBox2"></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("POtoVendorDate") %>' ID="Label2"></asp:Label>
                </ItemTemplate>

                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Required Delivery Date">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("RequiredDeliveryDate") %>' ID="RequiredDeliveryDateTB"></asp:TextBox>
                    <div runat="server" id="RequiredDeliveryDateValidation"></div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("RequiredDeliveryDate") %>' ID="TextBox3"></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("RequiredDeliveryDate") %>' ID="Label3"></asp:Label>
                </ItemTemplate>

                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Expected Delivery Date">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("ExpectedDeliveryDate") %>' ID="ExpectedDeliveryDateTB"></asp:TextBox>
                    <div runat="server" id="ExpectedDeliveryDateValidation"></div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("ExpectedDeliveryDate") %>' ID="TextBox4"></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("ExpectedDeliveryDate") %>' ID="Label4"></asp:Label>
                </ItemTemplate>

                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Actual Delivery Date">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("ActualDeliveryDate") %>' ID="ActualDeliveryDateTB"></asp:TextBox>
                    <div runat="server" id="ActualDeliveryDateValidation"></div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("ActualDeliveryDate") %>' ID="TextBox5"></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("ActualDeliveryDate") %>' ID="Label5"></asp:Label>
                </ItemTemplate>

                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Delivery Address">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("DeliveryAddress") %>' ID="DeliveryAddressTB"></asp:TextBox>
                    <div runat="server" id="DeliveryAddressValidation"></div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("DeliveryAddress") %>' ID="TextBox6"></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("DeliveryAddress") %>' ID="Label6"></asp:Label>
                </ItemTemplate>

                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Mark For">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("MarkFor") %>' ID="MarkForTB"></asp:TextBox>
                    <div runat="server" id="MarkForValidation"></div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("MarkFor") %>' ID="TextBox7"></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("MarkFor") %>' ID="Label7"></asp:Label>
                </ItemTemplate>

                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Justification">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("Justification") %>' ID="JustificationTB"></asp:TextBox>
                    <div runat="server" id="JustificationValidation"></div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("Justification") %>' ID="TextBox8"></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("Justification") %>' ID="Label8"></asp:Label>
                </ItemTemplate>

                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Review Status">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("ReviewStatus") %>' ID="ReviewStatusLabel"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("ReviewStatus") %>' ID="ReviewStatusEditTB" ReadOnly="true"></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("ReviewStatus") %>' ID="ReviewStatusInsertTB" ReadOnly="true"></asp:TextBox>
                </InsertItemTemplate>

                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Approval Status">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("ApprovalStatus") %>' ID="ApprovalStatusLabel"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("ApprovalStatus") %>' ID="ApprovalStatusEditTB" ReadOnly="true"></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("ApprovalStatus") %>' ID="ApprovalStatusInsertTB" ReadOnly="true"></asp:TextBox>
                </InsertItemTemplate>

                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Purchase Order Total">
                <ItemTemplate>
                   <asp:Label runat="server" OnLoad="POTotal_Load" ID="POTotal"/>
                </ItemTemplate>
                <EditItemTemplate>
                   <asp:Label runat="server" OnLoad="POTotal_Load" ID="POTotal"/>
                </EditItemTemplate>
                <HeaderStyle CssClass="info" Font-Bold="True" Width="25%"></HeaderStyle>
            </asp:TemplateField>
        </Fields>
    </asp:DetailsView>

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
    <asp:GridView ID="POLineItemList" runat="server" AllowSorting="true" AutoGenerateColumns="false" GridLines="Vertical" CellPadding="4" ItemType="MaterialManager.Models.ExtendedPOLineItem"
        SelectMethod="POLineItemList_GetData" CssClass="table table-striped table-bordered table-hover" OnRowCommand="POLineItemList_RowCommand" UpdateMethod="POLineItemList_UpdateItem" OnRowUpdating="POLineItemList_RowUpdating"
        AutoGenerateDeleteButton="true" DeleteMethod="POLineItemList_DeleteItem" DataKeyNames="POLineItemID" AutoGenerateEditButton="true">
        <Columns>
            <asp:TemplateField HeaderText="PO ID">
                <EditItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("POID") %>' ID="POIDLabel"></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("POID") %>' ID="POIDLabel"></asp:Label>
                </ItemTemplate>

                <ItemStyle CssClass="hidden"></ItemStyle>

                <HeaderStyle CssClass="hidden"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="POLineItemID">
                <EditItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("POLineItemID") %>' ID="POLineItemIDLabel"></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("POLineItemID") %>' ID="POLineItemIDLabel"></asp:Label>
                </ItemTemplate>

                <ItemStyle CssClass="hidden"></ItemStyle>

                <HeaderStyle CssClass="hidden"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="JobBoss ID" SortExpression="JBMaterialID">
                <EditItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("JBMaterialID") %>' ID="JBMaterialIDLabel"></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("JBMaterialID") %>' ID="JBMaterialIDLabel"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Part ID" SortExpression="PartID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden">
                <EditItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("PartID") %>' ID="PartIDLabel"></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("PartID") %>' ID="PartIDLabel"></asp:Label>
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
            <asp:TemplateField HeaderText="Unit Price" SortExpression="Price">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("Price") %>' ID="PriceTB" TextMode="Number"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# String.Format("{0:C}", Eval("Price")) %>' ID="PriceLabel"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Total Price" SortExpression="TotalPrice">
                <EditItemTemplate>
                    <asp:Label runat="server" Text='<%# String.Format("{0:C}", Eval("TotalPrice")) %>' ID="TotalPriceLabel"></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# String.Format("{0:C}", Eval("TotalPrice")) %>' ID="TotalPriceLabel"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton Text="View Job Boss Data" runat="server" CausesValidation="false" CommandName="ViewJobBossData" CommandArgument='<%# Eval("PartID") %>' ControlStyle-CssClass="btn btn-primary">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

    <!-- Modal Trigger -->
    <button id="CustomPartModalTrigger" class="btn btn-default" data-toggle="modal" data-target="#CustomPartModal">Add Custom Part</button>

    <button id="addPart" runat="server" onserverclick="AddPart_ServerClick" class="btn btn-primary">Add JB Part</button>
    <button id="edit" runat="server" onserverclick="Edit_ServerClick" class="btn btn-primary">Next</button>

    <!-- Modal -->
    <div id="CustomPartModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Add Custom Part</h4>
                </div>
                <div class="modal-body">
                    <fieldset>
                        <div class="form-group row">
                            <label for="AddJobBossIDTB" class="col-lg-2 control-label">JobBoss ID</label>
                            <div class="col-lg-10">
                                <asp:TextBox runat="server" CssClass="form-control" placeholder="Leave blank if not from JobBoss" ID="AddJobBossIDTB"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="AddNSNTB" class="col-lg-2 control-label">NSN</label>
                            <div class="col-lg-10">
                                <asp:TextBox runat="server" CssClass="form-control" ID="AddNSNTB"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="AddPartNumberTB" class="col-lg-2 control-label">Part Number</label>
                            <div class="col-lg-10">
                                <asp:TextBox runat="server" CssClass="form-control" ID="AddPartNumberTB"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="AddDescriptionTB" class="col-lg-2 control-label">Description</label>
                            <div class="col-lg-10">
                                <asp:TextBox runat="server" CssClass="form-control" ID="AddDescriptionTB"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="AddQuantityTB" class="col-lg-2 control-label">Quantity</label>
                            <div class="col-lg-10">
                                <asp:TextBox runat="server" CssClass="form-control" ID="AddQuantityTB" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="AddPriceTB" class="col-lg-2 control-label">Price</label>
                            <div class="col-lg-10">
                                <asp:TextBox runat="server" CssClass="form-control" ID="AddPriceTB"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="AddPriceTB" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^-?\d+(\.\d+)?$"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </fieldset>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <asp:Button type="button" ID="AddCustomPartButton" CssClass="btn btn-primary" runat="server" OnClick="AddCustomPartButton_Click" Text="Add"></asp:Button>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
