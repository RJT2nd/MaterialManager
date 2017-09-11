using MaterialManager.Logic;
using MaterialManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgr_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataActions actions = new DataActions();
        int currentProject = actions.GetCurrentProject();

        // If no project is currently selected, the user is brought to the project selection screen
        if (currentProject == 0)
        {
            Response.Redirect("SelectProject.aspx");
        }
        else
        {
            // Fills in the Project Data at the top of the page
            Project projectData = actions.getProject(actions.GetCurrentProject());
            ProjectName.Text = projectData.ProjectName;
            ContractNumber.Text = projectData.ContractNumber;
        }
    }

    // The id parameter should match the DataKeyNames value set on the control
    // or be decorated with a value provider attribute, e.g. [QueryString]int id
    public MaterialManager.Models.RequestForQuote RFQDetails_GetItem([QueryString]int RFQID)
    {
        DataActions actions = new DataActions();
        return actions.GetRFQByID(RFQID);
    }

    // The return type can be changed to IEnumerable, however to support
    // paging and sorting, the following parameters must be added:
    //     int maximumRows
    //     int startRowIndex
    //     out int totalRowCount
    //     string sortByExpression
    public IQueryable<MaterialManager.Models.RequestForQuote> RFQList_GetData()
    {
        DataActions actions = new DataActions();
        return actions.GetRFQList().AsQueryable();
    }

    // Filters RFQ List and is Case Sensitive****
    public void FilterRFQList(object sender, EventArgs e)
    {
        // Selects the correct category to filter/search by
        int cellNumber = 1; // The first cell [0] is filled with the button to view the RFQ. So we begin the list at the second index [1].
        if (FilterDDL.SelectedValue == "RFQ ID")
        {
            cellNumber += 0;
        }
        else if (FilterDDL.SelectedValue == "JobBoss RFQ Reference")
        {
            cellNumber += 1;
        }
        else if (FilterDDL.SelectedValue == "RFQ Sent to Vendor")
        {
            cellNumber += 2;
        }
        else if (FilterDDL.SelectedValue == "Date of RFQ")
        {
            cellNumber += 3;
        }
        else if (FilterDDL.SelectedValue == "Review Status")
        {
            cellNumber += 4;
        }
        else if (FilterDDL.SelectedValue == "Approval Status")
        {
            cellNumber += 5;
        }

        // Completes the logic of the filter/search
        foreach (GridViewRow Row in RFQList.Rows)
        {
            if (!Row.Cells[cellNumber].Text.Contains(FilterTB.Text))
            {
                Row.Visible = false;
            }
            else
            {
                Row.Visible = true;
            }

            // This part of the search checks if the user typed a certain status, that way the status itself can be filtered
            if (cellNumber == 6) // cellIndex 6 currently holds the review status property
            {
                if (FilterTB.Text == "Reviewed")
                {
                    Row.Visible = Row.Cells[cellNumber].Text == "Reviewed";
                }
                else if (FilterTB.Text == "Not Reviewed")
                {
                    Row.Visible = Row.Cells[cellNumber].Text == "Not Reviewed";
                }
            }
            else if (cellNumber == 7) // cellIndex 7 currently holds the approval status property
            {
                if (FilterTB.Text == "Approved")
                {
                    Row.Visible = Row.Cells[cellNumber].Text == "Approved";
                }
                else if (FilterTB.Text == "Not Approved")
                {
                    Row.Visible = Row.Cells[cellNumber].Text == "Not Approved";
                }
            }
        }
    }

    protected void RFQList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // When the user clicks on the AddFromRFQ Button, parts from the rfq selected are added to the currently selected rfq
        if(e.CommandName == "AddFromRFQ_Click")
        {
            DataActions actions = new DataActions();
            Button AddFromRFQButton = (Button)e.CommandSource;
            RequestForQuote rfqToAdd = actions.GetRFQByID(Convert.ToInt32(AddFromRFQButton.CommandArgument));
            RequestForQuote currentRFQ = actions.GetRFQByID(Convert.ToInt32(Request.QueryString["RFQID"]));

            // Gets list of RFQLineItems to add to the rfq selected
            List<RFQLineItem> RFQLineItemsToAdd = actions.GetRFQLineItemsList(rfqToAdd.RFQID);

            // Adds the line items one by one
            foreach (RFQLineItem ItemToAdd in RFQLineItemsToAdd)
            {
                Part PartToAdd = actions.getPart(ItemToAdd.PartID);
                actions.AddPartToRFQ(currentRFQ.RFQID, PartToAdd.JBMaterialID, (int)ItemToAdd.Quantity);
            }

            Response.Redirect("AddPartsToRFQ.aspx?RFQID=" + currentRFQ.RFQID);
        }
    }

    protected void RFQList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}