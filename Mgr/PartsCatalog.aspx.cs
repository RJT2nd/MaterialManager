using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Logic;
using MaterialManager.Models;

public partial class Mgr_PartsCatalog : System.Web.UI.Page
{
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

    // The return type can be changed to IEnumerable, however to support
    // paging and sorting, the following parameters must be added:
    //     int maximumRows
    //     int startRowIndex
    //     out int totalRowCount
    //     string sortByExpression
    public IQueryable<MaterialManager.Models.Part> PartsCatalogList_GetData()
    {
        DataActions actions = new DataActions();
        return actions.GetPartsCatalogList().AsQueryable();
    }

    protected void PartsCatalogList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // If multiple buttons are used in a GridView control, use the
        // CommandName property to determine which button was clicked.
        if (e.CommandName == "ViewJobBossData")
        {
            LinkButton JBButton = (LinkButton)e.CommandSource;
            string partID = JBButton.CommandArgument;

            Response.Redirect("ViewJobBossMaterialData.aspx?PartID="+partID);

        }
        else if (e.CommandName == "AddToCart")
        {
            LinkButton AddToCartButton = (LinkButton)e.CommandSource;
            string partID = AddToCartButton.CommandArgument;
            ShoppingCartActions CartActions = new ShoppingCartActions();
            CartActions.AddToCart(Convert.ToInt32(partID), 1);
        }
    }

    protected void PartsCatalogList_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {

    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void PartsCatalogList_DeleteItem(int PartID)
    {
        DataActions actions = new DataActions();
        actions.DeletePartsCatalogItem(PartID);
    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void PartsCatalogList_UpdateItem(int PartID)
    {
        MaterialManager.Models.Part item = null;
        var _db = new MaterialManager.Models.ProductContext();
        item = _db.Parts.Find(PartID);
        // Load the item here, e.g. item = MyDataLayer.Find(id);
        if (item == null)
        {
            // The item wasn't found
            ModelState.AddModelError("", String.Format("Item with id {0} was not found", PartID));
            return;
        }
        TryUpdateModel(item);
        if (ModelState.IsValid)
        {
            _db.SaveChanges();

        }
    }

    // Filters the list of Parts by any field
    public void FilterPartsCatalogList(object sender, EventArgs e)
    {
        int cellNumber = 1; // The first cell contains the view button so we start at Cell[1] to avoid the view button at [0]
        if (FilterDDL.SelectedValue == "JBMaterialID")
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

        foreach (GridViewRow Row in PartsCatalogList.Rows)
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
}