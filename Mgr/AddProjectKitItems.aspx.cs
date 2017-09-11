using MaterialManager.Logic;
using MaterialManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgr_AddProjectKitItems : System.Web.UI.Page
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

    // Gets data to be stored in the JobBOSS Parts List
    public IQueryable<JBXML.Respond.JBXMLJBXMLRespondMaterialListQueryRsMaterial> JobBossPartsList_GetData()
    {
        DataActions actions = new DataActions();
        return actions.getJBMaterialList().AsQueryable();
    }

    // Changes webpages over to ViewJobBossMaterialData and sends the item ID. It is then called for 
    protected void JobBossPartsList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddToProjectKit")
        {
            DataActions actions = new DataActions();
            LinkButton linkView = (LinkButton)e.CommandSource;
            String[] commandArguments = linkView.CommandArgument.Split(new char[] { ',' }); // Creates an array with both command arguments: jbMaterialID and index in order
            string jbMaterialID = commandArguments[0];
            int index = Convert.ToInt32(commandArguments[1]);

            // Finds the row containing the button that was pressed
            GridViewRow row = JobBossPartsList.Rows[index];

            // finds the quantity textbox control, casts it as a textbox in order to access the .Text property then converts the text into an int denoting the quantity 
            int quantity = Convert.ToInt32(((TextBox)row.FindControl("quantity")).Text);

            int currentProjectKitID = Convert.ToInt32(Request.QueryString["ProjectKitID"]);
            actions.AddPartToProjectKit(currentProjectKitID, jbMaterialID, quantity);
            Response.Redirect("AddPartsToProjectKit?ProjectKitID=" + currentProjectKitID);
        }
    }

    public void AddPart_ServerClick(object sender, EventArgs e)
    {
        int currentProjectKitID = Convert.ToInt32(Request.QueryString["ProjectKitID"]);
        Response.Redirect("AddProjectKitItems.aspx?ProjectKitID=" + currentProjectKitID);
    }

    // Filters the list of JobBoss Parts by any field
    public void FilterJobBossPartsList(object sender, EventArgs e)
    {
        int cellNumber = 2; // The first cell contains the view button so we start at Cell[1] to avoid the view button at [0]
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