using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Logic;
using MaterialManager.Models;

public partial class Mgr_AddPartsToCatalog : System.Web.UI.Page
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

     protected void CatalogedPartsList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // If multiple buttons are used in a GridView control, use the
        // CommandName property to determine which button was clicked.
        if (e.CommandName == "ViewJobBossData")
        {
            int partID = Convert.ToInt32(e.CommandArgument);

            Response.Redirect("ViewJobBossMaterialData.aspx?PartID=" + partID);

        }
        else if (e.CommandName == "AddToCatalog")
        {
            DataActions actions = new DataActions();

            actions.AddPartToCatalog(Convert.ToInt32(e.CommandArgument));
            CatalogedPartsList.DataBind();
        }

    }

    // The return type can be changed to IEnumerable, however to support
    // paging and sorting, the following parameters must be added:
    //     int maximumRows
    //     int startRowIndex
    //     out int totalRowCount
    //     string sortByExpression
    public IQueryable<MaterialManager.Models.Part> CatalogedPartsList_GetData()
    {
        DataActions actions = new DataActions();
        return actions.GetPartsNotInCatalogList().AsQueryable();
    }

    // The return type can be changed to IEnumerable, however to support
    // paging and sorting, the following parameters must be added:
    //     int maximumRows
    //     int startRowIndex
    //     out int totalRowCount
    //     string sortByExpression
    public IQueryable<JBXML.Respond.JBXMLJBXMLRespondMaterialListQueryRsMaterial> JobBossPartsList_GetData()
    {
        DataActions actions = new DataActions();
        return actions.getJBMaterialList().AsQueryable();
    }

    protected void JobBossPartsList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddToCatalog")
        {
            DataActions actions = new DataActions();

            // Convert the row index stored in the CommandArgument
            // property to an Integer.

            int index = Convert.ToInt32(e.CommandArgument);

            // Retrieve the row that contains the button clicked 
            // by the user from the Rows collection.
            GridViewRow row = JobBossPartsList.Rows[index];

            string jobBossID = row.Cells[1].Text;
            actions.AddJBPartToCatalog(jobBossID);
           
        }
    }

    // Filters the list of Parts by any field
    public void FilterCatalogedPartsList(object sender, EventArgs e)
    {
        int cellNumber = 1; // The first cell contains the view button so we start at Cell[1] to avoid the view button at [0]
        if (FilterCatalogedPartsDDL.SelectedValue == "JBMaterialID")
        {
            cellNumber += 0;
        }
        else if (FilterCatalogedPartsDDL.SelectedValue == "NSN")
        {
            cellNumber += 2;
        }
        else if (FilterCatalogedPartsDDL.SelectedValue == "Part Number")
        {
            cellNumber += 3;
        }
        else if (FilterCatalogedPartsDDL.SelectedValue == "Description")
        {
            cellNumber += 4;
        }

        foreach (GridViewRow Row in CatalogedPartsList.Rows)
        {
            if (!Row.Cells[cellNumber].Text.Contains(FilterCatalogedPartsTB.Text))
            {
                Row.Visible = false;
            }
            else
            {
                Row.Visible = true;
            }
        }
    }

    // Filters the list of JobBoss Parts by any field
    public void FilterJobBossPartsList(object sender, EventArgs e)
    {
        int cellNumber = 1; // The first cell contains the view button so we start at Cell[1] to avoid the view button at [0]
        if (FilterJobBossPartsDDL.SelectedValue == "Material")
        {
            cellNumber += 0;
        }
        else if (FilterJobBossPartsDDL.SelectedValue == "Description")
        {
            cellNumber += 1;
        }
        else if (FilterJobBossPartsDDL.SelectedValue == "Vendor Reference")
        {
            cellNumber += 2;
        }
        else if (FilterJobBossPartsDDL.SelectedValue == "Notes")
        {
            cellNumber += 3;
        }
        else if (FilterJobBossPartsDDL.SelectedValue == "Extended Description")
        {
            cellNumber += 4;
        }
        else if (FilterJobBossPartsDDL.SelectedValue == "Last Updated")
        {
            cellNumber += 5;
        }
        else if (FilterJobBossPartsDDL.SelectedValue == "Primary Vendor")
        {
            cellNumber += 6;
        }
        else if (FilterJobBossPartsDDL.SelectedValue == "Shape")
        {
            cellNumber += 7;
        }

        foreach (GridViewRow Row in JobBossPartsList.Rows)
        {
            if (!Row.Cells[cellNumber].Text.Contains(FilterJobBossPartsTB.Text))
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