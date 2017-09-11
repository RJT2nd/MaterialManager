using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Models;
using MaterialManager.Logic;

public partial class Admin_ProjectAdmin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public IQueryable<Project> ProjectList_GetData()
    {
        DataActions actions = new DataActions();
        return actions.GetProjectList().AsQueryable();
    }

    protected void ProjectListGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void ProjectListGridView_UpdateItem(int ProjectID)
    {
        DataActions actions = new DataActions();
        MaterialManager.Models.Project item = null;
        // Load the item here, e.g. item = MyDataLayer.Find(id);
        item = actions.GetProjectByID(ProjectID);
        if (item == null)
        {
            // The item wasn't found
            ModelState.AddModelError("", String.Format("Item with id {0} was not found", ProjectID));
            return;
        }
        TryUpdateModel(item);
        if (ModelState.IsValid)
        {
            // Save changes here, e.g. MyDataLayer.SaveChanges();
            actions.SaveChanges();
        }
    }

    protected void ProjectListGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
            {
                ((LinkButton)e.Row.Cells[0].Controls[1]).Attributes["onclick"] = "if(!confirm('Are you sure to delete this project?'))return   false;";
            }
        }
    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void ProjectListGridView_DeleteItem(int ProjectID)
    {
        DataActions actions = new DataActions();
        if (actions.GetProjectByID(ProjectID) != null)
        {
            actions.DeleteProject(ProjectID);
        }
    }

    public void AddProject(object sender, EventArgs e)
    {
        Response.Redirect("CreateProject.aspx");
    }

    public void FilterProjectList(object sender, EventArgs e)
    {
        int cellNumber = 1; // The first cell contains the select button so we start at Cell[1] to avoid the select button at [0]
        if (FilterDDL.SelectedValue == "Project ID")
        {
            cellNumber += 0;
        }
        else if (FilterDDL.SelectedValue == "Project Name")
        {
            cellNumber += 1;
        }
        else if (FilterDDL.SelectedValue == "Contract Number")
        {
            cellNumber += 2;
        }

        foreach (GridViewRow Row in ProjectListGridView.Rows)
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