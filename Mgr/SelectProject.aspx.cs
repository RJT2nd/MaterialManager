using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Logic;
using MaterialManager.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using MaterialManager;
using Microsoft.AspNet.Identity.EntityFramework;

public partial class Mgr_SelectProject : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    // The return type can be changed to IEnumerable, however to support
    // paging and sorting, the following parameters must be added:
    //     int maximumRows
    //     int startRowIndex
    //     out int totalRowCount
    //     string sortByExpression
    public IQueryable<Project> ProjectList_GetData()
    {
        DataActions actions = new DataActions();
        return actions.GetProjectList().AsQueryable();
    }

    public void ContactsGridView_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        // If multiple buttons are used in a GridView control, use the
        // CommandName property to determine which button was clicked.
        if (e.CommandName == "SelectProject")
        {
            DataActions actions = new DataActions();
            LinkButton SelectButton = (LinkButton)e.CommandSource;

            int projectID = Convert.ToInt32(SelectButton.CommandArgument);

            actions.SetCurrentProject(projectID);

            Response.Redirect("ProjectMenu.aspx");

        }
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

        foreach (GridViewRow Row in ProjectList.Rows)
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