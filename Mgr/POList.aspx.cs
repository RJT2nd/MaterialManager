using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Logic;
using MaterialManager.Models;

public partial class Mgr_POList : System.Web.UI.Page
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

    // Filters the list of Purchase Orders by any field
    public void FilterPOList(object sender, EventArgs e)
    {
        int cellNumber = 2; // The first few cells contains the view button so we start at Cell[2] to avoid the view button at [0]
        if (FilterDDL.SelectedValue == "PO ID")
        {
            cellNumber += 0;
        }
        else if (FilterDDL.SelectedValue == "Vendor")
        {
            cellNumber += 1;
        }
        else if (FilterDDL.SelectedValue == "PO Sent to Vendor")
        {
            cellNumber += 2;
        }
        else if (FilterDDL.SelectedValue == "Required Delivery Date")
        {
            cellNumber += 3;
        }
        else if (FilterDDL.SelectedValue == "Justification")
        {
            cellNumber += 4;
        }
        else if (FilterDDL.SelectedValue == "Review Status")
        {
            cellNumber += 5;
        }
        else if (FilterDDL.SelectedValue == "Approval Status")
        {
            cellNumber += 6;
        }

        foreach (GridViewRow Row in POList.Rows)
        {
            if (!Row.Cells[cellNumber].Text.Contains(FilterTB.Text))
            {
                Row.Visible = false;
            }
            else
            {
                Row.Visible = true;
            }

            if (cellNumber == 6)
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
            else if (cellNumber == 7)
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

    // The return type can be changed to IEnumerable, however to support
    // paging and sorting, the following parameters must be added:
    //     int maximumRows
    //     int startRowIndex
    //     out int totalRowCount
    //     string sortByExpression
    public IQueryable<MaterialManager.Models.PurchaseOrder> POList_GetData()
    {
        DataActions actions = new DataActions();
        return actions.GetPOList().AsQueryable();
    }

    protected void POList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void POList_DeleteItem(int PurchaseOrderID)
    {

    }

    protected void POList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void POList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!User.IsInRole("Creation"))
        {
            e.Row.Cells[0].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
            {
                ((LinkButton)e.Row.Cells[0].Controls[0]).Attributes["onclick"] = "if(!confirm('Are you sure to delete this purchase order?'))return   false;";
            }
        }
    }
}