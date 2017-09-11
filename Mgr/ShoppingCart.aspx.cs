using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Logic;
using MaterialManager.Models;
using System.Collections.Specialized;
using System.Collections;
using System.Web.ModelBinding;

public partial class ShoppingCart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
        {
            //decimal cartTotal = 0;
            //cartTotal = usersShoppingCart.GetTotal();
            if (usersShoppingCart.GetCartItems().Count() > 0)
            {
                //lblTotal.Text = String.Format("{0:c}", cartTotal);
            }
            else
            {
                //LabelTotaltext.Text = "";
                //lblTotal.Text = "";
                ShoppingCartTitle.InnerText = "Shopping Cart is Empty";
                UpdateBtn.Visible = false;
            }
        }
    }

    public IQueryable<ExtendedCartItem> GetShoppingCartItems()
    {
        ShoppingCartActions actions = new ShoppingCartActions();
        return actions.GetExtendedCartItems();
    }

    public IQueryable<ExtendedCartItem> updateCartItems()
    {
        using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
        {
            String cartId = usersShoppingCart.GetCartId();

            ShoppingCartActions.ShoppingCartUpdates[] cartUpdates = new ShoppingCartActions.ShoppingCartUpdates[CartList.Rows.Count];

            for (int i = 0; i < CartList.Rows.Count; i++)
            {
                IOrderedDictionary rowValues = new OrderedDictionary();
                rowValues = GetValues(CartList.Rows[i]);
                cartUpdates[i].ProductId = Convert.ToInt32(rowValues["PartID"]);

                CheckBox cbRemove = new CheckBox();
                cbRemove = (CheckBox)CartList.Rows[i].FindControl("Remove");
                cartUpdates[i].RemoveItem = cbRemove.Checked;

                TextBox quantityTextBox = new TextBox();
                quantityTextBox = (TextBox)CartList.Rows[i].FindControl("PurchaseQuantity");

                cartUpdates[i].PurchaseQuantity = Convert.ToInt16(quantityTextBox.Text.ToString());
            }
            usersShoppingCart.UpdateShoppingCartDatabase(cartId, cartUpdates);
            CartList.DataBind();
            //lblTotal.Text = String.Format("{0:c}", usersShoppingCart.GetTotal());
            return usersShoppingCart.GetExtendedCartItems();

        }
    }

    public static IOrderedDictionary GetValues(GridViewRow row)
    {
        IOrderedDictionary values = new OrderedDictionary();
        foreach (DataControlFieldCell cell in row.Cells)
        {
            if(cell.Visible)
            {
                cell.ContainingField.ExtractValuesFromCell(values, cell, row.RowState, true);
            }
        }
        return values;
    }

    protected void ClearCart_Click(object sender, EventArgs e)
    {
        ShoppingCartActions CartActions = new ShoppingCartActions();
        foreach (ExtendedCartItem item in GetShoppingCartItems())
        {
            CartActions.RemoveItem(CartActions.GetCartId(), item.PartID);
        }
        CartList.DataBind();
    }

    protected void ClearCart_Load(object sender, EventArgs e)
    {
        ShoppingCartActions CartActions = new ShoppingCartActions();
        if (CartActions.GetExtendedCartItems().Count() <= 0)
        {
            ClearCart.Visible = false;
        }
    }

    protected void UpdateBtn_Click(object sender, EventArgs e)
    {
        updateCartItems();
    }

    protected void UpdateBtn_Load(object sender, EventArgs e)
    {
        if (!User.IsInRole("Creation"))
        {
            UpdateBtn.Visible = false;
        }
    }

    protected void CreateRFQButton_Load(object sender, EventArgs e)
    {
        ShoppingCartActions CartActions = new ShoppingCartActions();
        if (!User.IsInRole("Creation") || CartActions.GetExtendedCartItems().Count() <= 0)
        {
            CreateRFQButton.Visible = false;
        }
    }

    protected void CreateRFQSubmitButton_Click(object sender, EventArgs e)
    {
        DataActions actions = new DataActions();

        // Grabs information from the two date inputs and also sets the minimum time allowed by the sql database
        DateTime minDateTime = Convert.ToDateTime("1753-01-01 00:00:00 AM");
        DateTime rfqDate = Convert.ToDateTime(((TextBox)RFQDateTime).Text);
        DateTime vendorDate = Convert.ToDateTime(((TextBox)VendorDateTime).Text);

        bool inputErrorExists = false;

        // Checks whether the input dates are after the minimum sql DateTime.
        // SQL has 2 data types for DateTime: DateTime and DateTime2. For our purposes, DateTime is being used, meaning an entry before the minimum DateTime would throw an error.
        if (rfqDate.CompareTo(minDateTime) < 0)
        {
            rfqDtValidate.InnerHtml = "<p class='text-danger'>Failed to submit. Please enter a valid date after 01/01/1753</p>";
            inputErrorExists = true;
        }
        else
        {
            rfqDtValidate.InnerHtml = "";
        }

        if (vendorDate.CompareTo(minDateTime) < 0)
        {
            vendorDtValidate.InnerHtml = "<p class='text-danger'>Failed to submit. Please enter a valid date after 01/01/1753</p>";
            inputErrorExists = true;
        }
        else
        {
            vendorDtValidate.InnerHtml = "";
        }

        if (!inputErrorExists)
        {
            int currentRFQID = actions.AddRFQ(rfqDate, vendorDate);

            foreach (ExtendedCartItem CartItem in GetShoppingCartItems())
            {
                Part p = new Part(CartItem.ItemPart);
                actions.AddPartToRFQ(p, currentRFQID, CartItem.Quantity);
            }

            // Clears the cart
            ShoppingCartActions CartActions = new ShoppingCartActions();
            foreach (ExtendedCartItem item in GetShoppingCartItems())
            {
                CartActions.RemoveItem(CartActions.GetCartId(), item.PartID);
            }
            CartList.DataBind();

            // Redirects to the RFQ Editing page
            Response.Redirect("AddPartsToRFQ.aspx?RFQID=" + currentRFQID);
        }
    }

    // Filtering/Searching of Shopping Cart Items
    public void FilterCartList(object sender, EventArgs e)
    {
        int cellNumber = 3; // The first cell containing content
        if (FilterLineItemsDDL.SelectedValue == "JobBoss ID")
        {
            cellNumber += 0;
        }
        else if (FilterLineItemsDDL.SelectedValue == "NSN")
        {
            cellNumber += 1;
        }
        else if (FilterLineItemsDDL.SelectedValue == "Part Number")
        {
            cellNumber += 2;
        }
        else if (FilterLineItemsDDL.SelectedValue == "Description")
        {
            cellNumber += 3;
        }

        foreach (GridViewRow Row in CartList.Rows)
        {
            if (!Row.Cells[cellNumber].Text.Contains(FilterLineItemsTB.Text))
            {
                Row.Visible = false;
            }
            else
            {
                Row.Visible = true;
            }
        }
    }

    protected void filterCartListForm_Load(object sender, EventArgs e)
    {
        ShoppingCartActions CartActions = new ShoppingCartActions();
        if (CartActions.GetExtendedCartItems().Count() <= 0)
        {
            filterCartListForm.Visible = false;
        }
    }
}