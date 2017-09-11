using MaterialManager.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CreateProject : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProjectAdmin.aspx");
    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        // Take inputs from textboxes
        string ProjectName = ((TextBox)ProjectNameTB).Text;
        string ContractNumber = ((TextBox)ContractNumberTB).Text;
        decimal FundsAllocated = Convert.ToDecimal(((TextBox)FundsAlloctedTB).Text);

        // Save inputs in dictionary to be passed
        Dictionary<string, dynamic> inputs = new Dictionary<string, dynamic>();
        inputs.Add("ProjectName", ProjectName);
        inputs.Add("ContractNumber", ContractNumber);
        inputs.Add("FundsAllocated", FundsAllocated);

        // Sends the dictionary to DataActions to be made into a project
        DataActions actions = new DataActions();
        actions.AddProject(inputs);

        // Redirects the user to the project list
        Response.Redirect("ProjectAdmin.aspx");
    }
}