using MaterialManager.Logic;
using MaterialManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgr_AddPOLineItems : System.Web.UI.Page
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
            price = actions.GetPOTotal(Convert.ToInt32(Request.QueryString["POID"]));
            POTotal.Text = String.Format("{0:C}", actions.GetPOTotal(Convert.ToInt32(Request.QueryString["POID"])));

        }
    }

    // Gets data to be stored in the JobBOSS Parts List
    public IQueryable<JBXML.Respond.JBXMLJBXMLRespondMaterialListQueryRsMaterial> JobBossPartsList_GetData()
    {
        DataActions actions = new DataActions();
        int POID = Convert.ToInt32(Request.QueryString["POID"]);
        return actions.getJBMaterialListFilteredByVendor(POID).AsQueryable();
    }

    protected void JobBossPartsList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddToPO")
        {
            DataActions actions = new DataActions();
            LinkButton linkView = (LinkButton)e.CommandSource;
            String[] commandArguments = linkView.CommandArgument.Split(new char[] { ',' }); // Creates an array with both command arguments: jbMaterialID and index in order
            string jbMaterialID = commandArguments[0];
            int index = Convert.ToInt32(commandArguments[1]);

            // Finds the row containing the button that was pressed
            GridViewRow row = JobBossPartsList.Rows[index];

            // Finds the quantity textbox control, casts it as a textbox in order to access the .Text property then converts the text into an int denoting the quantity 
            int quantity = Convert.ToInt32(((TextBox)row.FindControl("quantity")).Text);

            // Does the same for price 
            decimal price = Convert.ToDecimal(((TextBox)row.FindControl("price")).Text);

            int currentPOID = Convert.ToInt32(Request.QueryString["POID"]);
            actions.AddPartToPO(currentPOID, jbMaterialID, quantity, price);
            Response.Redirect("AddPartsToPO?POID=" + currentPOID);
        }
    }

    // Filters the list of JobBoss Parts by any field
    public void FilterJobBossPartsList(object sender, EventArgs e)
    {
        int cellNumber = 3; // The first cell contains the view button so we start at Cell[1] to avoid the view button at [0]
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