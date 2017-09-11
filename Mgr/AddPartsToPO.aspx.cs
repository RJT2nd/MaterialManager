using MaterialManager.Logic;
using MaterialManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Mgr_AddPartsToPO : System.Web.UI.Page
{
    private decimal price;
    private PurchaseOrder purchaseOrder;

    protected void Page_Load(object sender, EventArgs e)
    {
        DataActions actions = new DataActions();
        int currentProject = actions.GetCurrentProject();
        if (currentProject == 0)
        {
            Response.Redirect("SelectProject.aspx");
        }
        else
        {
            Project projectData = actions.getProject(actions.GetCurrentProject());
            ProjectName.Text = projectData.ProjectName;
            ContractNumber.Text = projectData.ContractNumber;
        }
    }

    protected void POTotal_Load(object sender, EventArgs e)
    {
        DataActions actions = new DataActions();
        purchaseOrder = actions.GetPOByID(Convert.ToInt32(Request.QueryString["POID"]));
        price = actions.GetPOTotal(purchaseOrder.PurchaseOrderID);
        Label POTotal = (Label)PODetails.FindControl("POTotal");
        POTotal.Text = String.Format("{0:C}", price);
    }

    // The id parameter should match the DataKeyNames value set on the control
    // or be decorated with a value provider attribute, e.g. [QueryString]int id
    public PurchaseOrder PODetails_GetItem([QueryString]int POID)
    {
        DataActions actions = new DataActions();
        purchaseOrder = actions.GetPOList().Where(p => p.PurchaseOrderID == POID).FirstOrDefault();
        return purchaseOrder;
    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void PODetails_UpdateItem(object sender)
    {
        if (User.IsInRole("Creation"))
        {
            PurchaseOrder item = purchaseOrder;
            if (item == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", $"Item with id {item.PurchaseOrderID} was not found");
                return;
            }
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                DateTime minDateTime = Convert.ToDateTime("1753-01-01 00:00:00 AM");
                Dictionary<int, dynamic> detailsViewValues = new Dictionary<int, dynamic>();
                DateTime tempDateTime;
                string rowText;
                int rowKey = 0;
                foreach (DetailsViewRow row in PODetails.Rows)
                {
                    switch (rowKey)
                    {
                        case 0:
                            rowText = Convert.ToString(purchaseOrder.PurchaseOrderID);
                            break;
                        case 1:
                            rowText = ((TextBox)PODetails.FindControl("POVendorTB")).Text;
                            break;
                        case 2:
                            rowText = ((TextBox)PODetails.FindControl("POtoVendorDateTB")).Text;
                            break;
                        case 3:
                            rowText = ((TextBox)PODetails.FindControl("RequiredDeliveryDateTB")).Text;
                            break;
                        case 4:
                            rowText = ((TextBox)PODetails.FindControl("ExpectedDeliveryDateTB")).Text;
                            break;
                        case 5:
                            rowText = ((TextBox)PODetails.FindControl("ActualDeliveryDateTB")).Text;
                            break;
                        case 6:
                            rowText = ((TextBox)PODetails.FindControl("DeliveryAddressTB")).Text;
                            break;
                        case 7:
                            rowText = ((TextBox)PODetails.FindControl("MarkForTB")).Text;
                            break;
                        case 8:
                            rowText = ((TextBox)PODetails.FindControl("JustificationTB")).Text;
                            break;
                        default:
                            rowText = "Error";
                            break;
                    }
                    if (rowKey == 0)
                    {
                        detailsViewValues.Add(rowKey, rowText);
                    }
                    else if (rowKey == 2 || rowKey == 3 || rowKey == 4 || rowKey == 5) // Rows 2, 3, 4, and 5 contain DateTime fields
                    {
                        if (DateTime.TryParse(rowText, out tempDateTime))
                        {
                            detailsViewValues.Add(rowKey, tempDateTime);
                        }
                        else if (rowText == "")
                        {
                            detailsViewValues.Add(rowKey, null);
                        }
                        else
                        {
                            string invalidDate = "<p class='text-danger'>Please enter a valid date. Format: MM/DD/YYYY hh:mm:ss AM/PM</p>";
                            switch (rowKey)
                            {
                                case 1:
                                    ((HtmlGenericControl)PODetails.FindControl("POtoVendorDateValidation")).InnerHtml = invalidDate;
                                    break;
                                case 2:
                                    ((HtmlGenericControl)PODetails.FindControl("RequiredDeliveryDateValidation")).InnerHtml = invalidDate;
                                    break;
                                case 3:
                                    ((HtmlGenericControl)PODetails.FindControl("ExpectedDeliveryDateValidation")).InnerHtml = invalidDate;
                                    break;
                                case 4:
                                    ((HtmlGenericControl)PODetails.FindControl("ActualDeliveryDateValidation")).InnerHtml = invalidDate;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        detailsViewValues.Add(rowKey, rowText);
                    }
                    rowKey++;
                }

                DataActions actions = new DataActions();
                actions.UpdatePurchaseOrder(detailsViewValues);
            }
        }

    }

    // PO Line Items Section: ***********************************************************************************************************************************************************************************
    public IQueryable<MaterialManager.Models.ExtendedPOLineItem> POLineItemList_GetData()
    {
        int POID = Convert.ToInt32(Request.QueryString["POID"]);
        DataActions actions = new DataActions();
        return actions.GetExtendedPOLineItemsList(POID).AsQueryable();
    }

    // Filters the POLineItemList GridView by whatever field is picked
    public void FilterPOLineItemList(object sender, EventArgs e)
    {
        int cellNumber = 1; // The first cell contains the view button so we start at Cell[1] to avoid the view button at [0]
        if (FilterDDL.SelectedValue == "JobBoss ID")
        {
            cellNumber += 0;
        }
        else if (FilterDDL.SelectedValue == "NSN")
        {
            cellNumber += 2;
        }
        else if (FilterDDL.SelectedValue == "Part Number")
        {
            cellNumber += 3;
        }
        else if (FilterDDL.SelectedValue == "Description")
        {
            cellNumber += 4;
        }
        else if (FilterDDL.SelectedValue == "Quantity")
        {
            cellNumber += 5;
        }
        else if (FilterDDL.SelectedValue == "Price")
        {
            cellNumber += 6;
        }

        foreach (GridViewRow Row in POLineItemList.Rows)
        {
            if (!Row.Cells[cellNumber].Text.Contains(FilterTB.Text))
            {
                Row.Visible = false;
            }
            else
            {
                Row.Visible = true;
            }
        }
    }

    protected void AddPart_ServerClick(object sender, EventArgs e)
    {
        int poID = Convert.ToInt32(Request.QueryString["POID"]);
        Response.Redirect("AddPOLineItems.aspx?POID=" + poID);
    }

    protected void POLineItemList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName == "ViewJobBossData")
        {
            LinkButton linkView = (LinkButton)e.CommandSource;
            String partID = linkView.CommandArgument;
            Response.Redirect("ViewJobBossMaterialData.aspx?PartID=" + partID);
        }
    }

    public void POLineItemList_DeleteItem(int POLineItemID)
    {
        DataActions actions = new DataActions();
        PurchaseOrder po = actions.GetPOByID(Convert.ToInt32(Request.QueryString["POID"]));
        if (actions.GetPOLineItemByID(POLineItemID) != null)
        {
            actions.DeletePOLineItem(POLineItemID);
            price = actions.GetPOTotal(po.PurchaseOrderID);
            Label POTotal = (Label)PODetails.FindControl("POTotal");
            POTotal.Text = String.Format("{0:c}", price);
        }
    }

    protected void Edit_ServerClick(object sender, EventArgs e)
    {
        int poID = Convert.ToInt32(Request.QueryString["POID"]);
        Response.Redirect("POView.aspx?POID=" + poID);
    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void POLineItemList_UpdateItem(int POLineItemID)
    {

    }

    protected void POLineItemList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int RowIndex = e.RowIndex;

        GridViewRow gvr = POLineItemList.Rows[RowIndex];

        MaterialManager.Models.Part item = null;
        DataActions actions = new DataActions();
        POLineItem poItem = null;

        int POLineItemID1 = Convert.ToInt32(((Label)(gvr.FindControl("POLineItemIDLabel"))).Text);
        poItem = actions.GetPOLineItemByID(POLineItemID1);
        item = actions.getPart(poItem.PartID);
        // Load the item here, e.g. item = MyDataLayer.Find(id);
        if (item == null)
        {
            // The item wasn't found
            ModelState.AddModelError("", String.Format("Item's Part ID was not found"));
            return;
        }
        if (poItem == null)
        {
            // The item wasn't found
            ModelState.AddModelError("", String.Format("Item with id {0} was not found", POLineItemID1));
            return;
        }

        //try
        //{
        // Save changes here, e.g. MyDataLayer.SaveChanges();
        item.NSN = ((TextBox)gvr.FindControl("NSNTB")).Text;
        item.PartNumber = ((TextBox)gvr.FindControl("PartNumberTB")).Text;
        item.Description = ((TextBox)gvr.FindControl("DescriptionTB")).Text;
        poItem.Quantity = Convert.ToInt32(((TextBox)gvr.FindControl("QuantityTB")).Text);
        poItem.Price = Convert.ToDecimal(((TextBox)gvr.FindControl("PriceTB")).Text);

        actions.SaveChanges();
    }

    protected void AddCustomPartButton_Click(object sender, EventArgs e)
    {
        string jbID = AddJobBossIDTB.Text;
        string nsn = AddNSNTB.Text;
        string partNumber = AddPartNumberTB.Text;
        string description = AddDescriptionTB.Text;
        int quantity = Convert.ToInt32(AddQuantityTB.Text != "" ? AddQuantityTB.Text : "0");
        decimal price = Convert.ToDecimal(AddPriceTB.Text != "" ? AddPriceTB.Text : "0");

        int currentPOID = Convert.ToInt32(Request.QueryString["POID"]);

        Part newPart = new Part()
        {
            JBMaterialID = jbID,
            NSN = nsn,
            PartNumber = partNumber,
            Description = description
        };

        DataActions actions = new DataActions();
        actions.AddPartToPO(newPart, currentPOID, quantity, price);

        POLineItemList.DataBind();
    }
}