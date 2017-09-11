using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Logic;
using MaterialManager.Models;
using System.Data;

public partial class Mgr_RFQList : System.Web.UI.Page
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
        int cellNumber = 0;
        if(FilterDDL.SelectedValue == "RFQ ID")
        {
            cellNumber = 2;
        }else if(FilterDDL.SelectedValue == "JobBoss RFQ Reference")
        {
            cellNumber = 3;
        }else if(FilterDDL.SelectedValue == "RFQ Sent to Vendor")
        {
            cellNumber = 4;
        }else if(FilterDDL.SelectedValue == "Date of RFQ")
        {
            cellNumber = 5;
        }else if(FilterDDL.SelectedValue == "Review Status")
        {
            cellNumber = 6;
        }else if(FilterDDL.SelectedValue == "Approval Status")
        {
            cellNumber = 7;
        }

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

            if (cellNumber == 6)
            {
                if(FilterTB.Text == "Reviewed")
                {
                    Row.Visible = Row.Cells[cellNumber].Text == "Reviewed";
                }else if(FilterTB.Text == "Not Reviewed")
                {
                    Row.Visible = Row.Cells[cellNumber].Text == "Not Reviewed";
                }
            } else if(cellNumber == 7)
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

    // The id parameter name should match the DataKeyNames value set on the control
    public void RFQList_DeleteItem(int RFQID)
    {
        DataActions actions = new DataActions();
        if(actions.GetRFQByID(RFQID) != null)
        {
            actions.DeleteRFQ(RFQID);
        }
    }

    protected void RFQList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    //// The return type can be changed to IEnumerable, however to support
    //// paging and sorting, the following parameters must be added:
    ////     int maximumRows
    ////     int startRowIndex
    ////     out int totalRowCount
    ////     string sortByExpression
    //public List<MaterialManager.Models.RFQDocument> RFQDocumentList_GetData()
    //{
    //    if (RFQList.SelectedValue != null)
    //    {
    //        DataActions actions = new DataActions();
    //        return actions.GetRFQDocumentList((int)RFQList.SelectedValue);
    //    }
    //    else
    //    {
    //        return new List<RFQDocument>();
    //    }
    //}

    //// The id parameter name should match the DataKeyNames value set on the control
    //public void RFQDocumentList_DeleteItem(int id)
    //{

    //}

    //protected void RFQDocumentList_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DataBind();
    //}

    protected void RFQList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //RFQDocumentList.DataBind();
    }

    protected void RFQList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!User.IsInRole("Creation"))
        {
            e.Row.Cells[0].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
            {
                ((LinkButton)e.Row.Cells[0].Controls[0]).Attributes["onclick"] = "if(!confirm('Are you sure to delete this RFQ?'))return   false;";
            }
        }
    }
}