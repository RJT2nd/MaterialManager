<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ShoppingCart.aspx.cs" Inherits="ShoppingCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div id="ShoppingCartTitle" runat="server" class="ContentHead">
        <h1>Shopping Cart</h1>
    </div>

    <asp:Panel runat="server" ID="filterCartListForm" CssClass="form-group" OnLoad="filterCartListForm_Load" DefaultButton="FilterCartListSubmit">
        <asp:TextBox runat="server" CssClass="form-control col-lg-4" ID="FilterLineItemsTB"></asp:TextBox>
        <asp:DropDownList runat="server" CssClass="form-control col-lg-4" ID="FilterLineItemsDDL">
            <asp:ListItem>JobBoss ID</asp:ListItem>
            <asp:ListItem>NSN</asp:ListItem>
            <asp:ListItem>Part Number</asp:ListItem>
            <asp:ListItem>Description</asp:ListItem>
        </asp:DropDownList>
        <asp:Button runat="server" OnClick="FilterCartList" CssClass="btn btn-default" ID="FilterCartListSubmit" Text="Filter" />
    </asp:Panel>

    <asp:GridView ID="CartList" runat="server" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="MaterialManager.Models.ExtendedCartItem"
        SelectMethod="GetShoppingCartItems" CssClass="table table-striped table-bordered" AllowSorting="true">
        <Columns>
            <asp:BoundField DataField="ItemID" HeaderText="Item ID" SortExpression="ItemID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
            <asp:BoundField DataField="CartID" HeaderText="Cart ID" SortExpression="CartID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
            <asp:BoundField DataField="PartID" HeaderText="Part ID" SortExpression="PartID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
            <asp:BoundField DataField="JBMaterialID" HeaderText="JobBoss ID" SortExpression="JBMaterialID" />
            <asp:BoundField DataField="NSN" HeaderText="NSN" SortExpression="NSN" />
            <asp:BoundField DataField="PartNumber" HeaderText="Part Number" SortExpression="PartNumber" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <asp:TextBox ID="PurchaseQuantity" Width="40" runat="server" Text="<%#: Item.Quantity %>"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Remove Item">
                <ItemTemplate>
                    <asp:CheckBox ID="Remove" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <!-- <div>
        <p></p>
        <strong>
            <asp:Label ID="LabelTotaltext" runat="server" Text="Order Total:"></asp:Label>
            <asp:Label ID="lblTotal" runat="server" EnableViewState="false"></asp:Label>
        </strong>
    </div> -->
    <br />
    <table>
        <tr>
            <td>
                <asp:Button ID="ClearCart" runat="server" class="btn btn-default" Text="Clear Cart" OnLoad="ClearCart_Load"
                    OnClientClick="if (!confirm('Are you sure you want to delete all cart items?')) return false;" OnClick="ClearCart_Click" />
            </td>
            <td>
                <asp:Button ID="UpdateBtn" CssClass="btn btn-default" runat="server" OnLoad="UpdateBtn_Load" Text="Update Cart" OnClick="UpdateBtn_Click" />
            </td>
            <td>
                <!-- Modal Trigger Button -->
                <button id="CreateRFQButton" class="btn btn-primary" runat="server" onload="CreateRFQButton_Load" data-toggle="modal" data-target="#CreateRFQModal">Create RFQ</button>
            </td>
        </tr>
    </table>

    <!-- Modal -->
    <div id="CreateRFQModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Create RFQ</h4>
                </div>
                <div class="modal-body">
                    <fieldset>
                        <div class="form-group row">
                            <label for="RFQDateTime" class="col-lg-2 control-label">RFQ Date</label>
                            <div class="col-lg-10">
                                <asp:TextBox runat="server" CssClass="form-control" TextMode="DateTimeLocal" ID="RFQDateTime"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div runat="server" id="rfqDtValidate" class="font-size-14px"></div>
                        </div>
                        <div class="form-group row">
                            <label for="RFQVendorDateTime" class="col-lg-2 control-label">RFQ Vendor Date</label>
                            <div class="col-lg-10">
                                <asp:TextBox runat="server" CssClass="form-control" TextMode="DateTimeLocal" ID="VendorDateTime"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div runat="server" id="vendorDtValidate" class="font-size-14px"></div>
                        </div>
                    </fieldset>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <asp:Button type="button" ID="CreateRFQSubmitButton" CssClass="btn btn-primary" runat="server" OnClick="CreateRFQSubmitButton_Click" Text="Save"></asp:Button>
                </div>
            </div>

        </div>
    </div>
</asp:Content>

