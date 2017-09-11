using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Logic;
using MaterialManager.Models;

public partial class Mgr_ProjectMenu : System.Web.UI.Page
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
            decimal funded = projectData.FundsAllocated;
            decimal expended = actions.GetExpendedFunds();
            Funding.Text = String.Format("{0:c}", funded);
            Expended.Text = String.Format("{0:c}", expended);
            Remaining.Text = String.Format("{0:c}", (funded-expended));
            //Remaining.CssClass = "text-success";

        }
    }

   
}