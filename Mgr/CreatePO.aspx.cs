using MaterialManager.Logic;
using MaterialManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgr_CreatePO : System.Web.UI.Page
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

    public IQueryable<MaterialManager.Models.RequestForQuote> RFQList_GetData()
    {
        DataActions actions = new DataActions();
        return actions.GetRFQList().AsQueryable();
    }

    protected void RFQList_RowCommand(object sender, CommandEventArgs e)
    {
        // Once an RFQ is selected, a new Purchase Order is created and added to the database, containing only a Purchase Order ID and an RFQ ID.
        if (e.CommandName=="selectRFQ")
        {
            DataActions actions = new DataActions();
            int rfqID = Convert.ToInt32(e.CommandArgument);
            int poID = actions.AddPO(rfqID);
            foreach (ExtendedLineItem item in actions.GetExtendedLineItemsList(rfqID))
            {
                Part p = new Part(item.ItemPart);
                actions.AddPartToPO(p, poID, Convert.ToInt32(item.Quantity), Convert.ToDecimal(0));
            }
            Response.Redirect("AddPartsToPO.aspx?POID=" + poID);
        }
    }

    protected void RFQList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void noRFQButton_ServerClick(object sender, EventArgs e)
    {
        DataActions actions = new DataActions();
        int poID = actions.AddPO();
        Response.Redirect("AddPartsToPO.aspx?POID=" + poID);
    }
}