using MaterialManager.Logic;
using MaterialManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgr_AddPartsToProjectKit : System.Web.UI.Page
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
            // Project Info
            Project projectData = actions.getProject(actions.GetCurrentProject());
            ProjectName.Text = projectData.ProjectName;
            ContractNumber.Text = projectData.ContractNumber;
        }
    }

    public IQueryable<ExtendedProjectKitItem> ProjectKitItemList_GetData()
    {
        int ProjectKitID = Convert.ToInt32(Request.QueryString["ProjectKitID"]);
        DataActions action = new DataActions();
        return action.GetExtendedProjectKitItemsList(ProjectKitID).AsQueryable();
    }

    protected void ProjectKitItemList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewJobBossData")
        {
            LinkButton linkView = (LinkButton)e.CommandSource;
            String partID = linkView.CommandArgument;
            Response.Redirect("ViewJobBossMaterialData.aspx?partID=" + partID);
        }
    }

    public void ProjectKitItemList_DeleteItem(int ProjectKitItemID)
    {
        DataActions action = new DataActions();
        if (action.GetProjectKitItemByID(ProjectKitItemID) != null)
        {
            action.DeleteProjectKitItem(ProjectKitItemID);
        }
    }

    public void AddPart_ServerClick(object sender, EventArgs e)
    {
        int currentProjectKitID = Convert.ToInt32(Request.QueryString["ProjectKitID"]);
        Response.Redirect("AddProjectKitItems.aspx?ProjectKitID=" + currentProjectKitID);
    }

    // Filters the list of Kit Line items by any field
    public void FilterKitItemList(object sender, EventArgs e)
    {
        int cellNumber = 2; // The first cell contains the view button so we start at Cell[1] to avoid the view button at [0]
        if (FilterDDL.SelectedValue == "JBMaterialID")
        {
            cellNumber += 0;
        }
        else if (FilterDDL.SelectedValue == "NSN")
        {
            cellNumber += 2;
        }
        else if (FilterDDL.SelectedValue == "Description")
        {
            cellNumber += 3;
        }
        else if (FilterDDL.SelectedValue == "Quantity")
        {
            cellNumber += 4;
        }

        foreach (GridViewRow Row in ProjectKitItemList.Rows)
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

    // The id parameter name should match the DataKeyNames value set on the control
    public void ProjectKitItemList_UpdateItem(int ProjectKitItemID)
    {

    }

    protected void ProjectKitItemList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int RowIndex = e.RowIndex;

        GridViewRow gvr = ProjectKitItemList.Rows[RowIndex];

        MaterialManager.Models.Part item = null;
        DataActions actions = new DataActions();
        ProjectKitItem projectKitItem = null;

        int RFQLineItemID1 = Convert.ToInt32(((Label)(gvr.FindControl("ProjectKitItemIDLabel"))).Text);
        projectKitItem = actions.GetProjectKitItemByID(RFQLineItemID1);
        item = actions.getPart(projectKitItem.PartID);
        // Load the item here, e.g. item = MyDataLayer.Find(id);
        if (item == null)
        {
            // The item wasn't found
            ModelState.AddModelError("", String.Format("Item's Part ID was not found"));
            return;
        }
        if (projectKitItem == null)
        {
            // The item wasn't found
            ModelState.AddModelError("", String.Format("Item with id {0} was not found", RFQLineItemID1));
            return;
        }

        //try
        //{
        // Save changes here, e.g. MyDataLayer.SaveChanges();
        item.NSN = ((TextBox)gvr.FindControl("NSNTB")).Text;
        item.PartNumber = ((TextBox)gvr.FindControl("PartNumberTB")).Text;
        item.Description = ((TextBox)gvr.FindControl("DescriptionTB")).Text;
        projectKitItem.Quantity = Convert.ToInt32(((TextBox)gvr.FindControl("QuantityTB")).Text);

        actions.SaveChanges();
    }

    protected void ViewKitLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProjectKitView.aspx?ProjectKitID=" + Convert.ToInt32(Request.QueryString["ProjectKitID"]));
    }
}