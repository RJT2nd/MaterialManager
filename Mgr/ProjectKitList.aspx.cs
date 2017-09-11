using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Models;
using MaterialManager.Logic;

public partial class Mgr_ProjectKitList : System.Web.UI.Page
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

    public IQueryable<ProjectKit> KitList_GetData()
    {
        DataActions action = new DataActions();
        return action.GetKitList().AsQueryable();
    }

    public void KitList_DeleteItem(int ProjectKitID)
    {
        DataActions actions = new DataActions();
        if (actions.GetProjectKitByID(ProjectKitID) != null)
        {
            actions.DeleteProjectKit(ProjectKitID);
        }
    }

    protected void KitList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void KitList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    // Filters the list of Kits by any field
    public void FilterKitList(object sender, EventArgs e)
    {
        int cellNumber = 2; // The first cell contains the view button so we start at Cell[1] to avoid the view button at [0]
        if (FilterDDL.SelectedValue == "Kit ID")
        {
            cellNumber += 0;
        }
        else if (FilterDDL.SelectedValue == "Description")
        {
            cellNumber += 1;
        }

        foreach (GridViewRow Row in KitList.Rows)
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

    protected void KitList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!User.IsInRole("Creation"))
        {
            e.Row.Cells[0].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
            {
                ((LinkButton)e.Row.Cells[0].Controls[0]).Attributes["onclick"] = "if(!confirm('Are you sure to delete this project kit?'))return   false;";
            }
        }
    }
}