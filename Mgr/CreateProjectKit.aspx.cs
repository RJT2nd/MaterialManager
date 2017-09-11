using MaterialManager.Logic;
using MaterialManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgr_CreateProjectKit : System.Web.UI.Page
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

    protected void Submit_Click(object sender, EventArgs e)
    {
        DataActions actions = new DataActions();
        int kitID = actions.AddProjectKit(ProjectKitDescription.Text);
        Response.Redirect("AddPartsToProjectKit.aspx?ProjectKitID=" + kitID);
    }
}