using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Models;
using MaterialManager.Logic;

public partial class Mgr_ProjectKitView : System.Web.UI.Page
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

    public IQueryable<ExtendedProjectKitItem> KitItemList_GetData()
    {
        int ProjectKitID = Convert.ToInt32(Request.QueryString["ProjectKitID"]);
        DataActions action = new DataActions();
        return action.GetExtendedProjectKitItemsList(ProjectKitID).AsQueryable();
    }

    protected void KitItemList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName == "ViewJobBossData")
        {
            LinkButton linkView = (LinkButton)e.CommandSource;
            String partID = linkView.CommandArgument;
            Response.Redirect("ViewJobBossMaterialData.aspx?partID=" + partID);
        }
    }

    protected void KitItemList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public void ProjectKitItemList_DeleteItem(int ProjectKitItemID)
    {
        DataActions action = new DataActions();
        if (action.GetProjectKitItemByID(ProjectKitItemID) != null)
        {
            action.DeleteProjectKitItem(ProjectKitItemID);
        }
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

        foreach (GridViewRow Row in KitItemList.Rows)
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

    protected void CreateRFQButton_Load(object sender, EventArgs e)
    {
        ShoppingCartActions CartActions = new ShoppingCartActions();
        if (!User.IsInRole("Creation") || KitItemList_GetData().Count() <= 0)
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

            foreach (ExtendedProjectKitItem KitItem in KitItemList_GetData())
            {
                Part p = new Part(KitItem.ItemPart);
                actions.AddPartToRFQ(p, currentRFQID, (int)KitItem.Quantity);
            }

            Response.Redirect("AddPartsToRFQ.aspx?RFQID=" + currentRFQID);
        }
    }

    protected void EditLinkButton_Load(object sender, EventArgs e)
    {
        if (!User.IsInRole("Creation"))
        {
            EditLinkButton.Visible = false;
        }
        else
        {
            EditLinkButton.Visible = true;
        }
    }

    protected void EditLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddPartsToProjectKit.aspx?ProjectKitID=" + Convert.ToInt32(Request.QueryString["ProjectKitID"]));
    }

    protected void KitItemList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!User.IsInRole("Creation"))
        {
            e.Row.Cells[0].Visible = false;
        }
    }
}