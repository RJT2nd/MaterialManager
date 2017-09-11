using MaterialManager.Logic;
using MaterialManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgr_CreateRFQ : System.Web.UI.Page
{
    public int currentRFQID;
    // Calls the information for the header regarding the project
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

    // Once Submit is clicked, a new RFQ is created and the user is brought to the RFQ's Screen
    protected void Submit_Click(object sender, EventArgs e)
    {
        // Grabs information from the two date inputs and also sets the minimum time allowed by the sql database
        DateTime minDateTime = Convert.ToDateTime("1753-01-01 00:00:00 AM");
        DateTime rfqDT;
        DateTime vendorDT;
        bool inputErrorExists = false;

        // Not sure if this whole error handling is necessary, TryParse might completely handle it as seen with the pop up when an invalid date is entered
        if (!DateTime.TryParse(RFQDateTime.Text, out rfqDT))
        {
            inputErrorExists = true;
            rfqDtValidate.InnerHtml = "<p class='text-danger'>Invalid input.</p>";
        }
        if (!DateTime.TryParse(VendorDateTime.Text, out vendorDT))
        {
            inputErrorExists = true;
            rfqDtValidate.InnerHtml = "<p class='text-danger'>Invalid input.</p>";
        }

        // Checks to see if there is an error. If there is, then the submission doesn't go through.
        if (inputErrorExists)
        {
            return;
        }

        // Checks whether the input dates are after the minimum sql DateTime.
        // SQL has 2 data types for DateTime: DateTime and DateTime2. For our purposes, DateTime is being used, meaning an entry before the minimum DateTime would throw an error.
        if (rfqDT.CompareTo(minDateTime) < 0)
        {
            rfqDtValidate.InnerHtml = "<p class='text-danger'>Failed to submit. Please enter a valid date after 01/01/1753</p>";
            inputErrorExists = true;
        } else
        {
            rfqDtValidate.InnerHtml = "";
        }

        if (vendorDT.CompareTo(minDateTime) < 0)
        {
            vendorDtValidate.InnerHtml = "<p class='text-danger'>Failed to submit. Please enter a valid date after 01/01/1753</p>";
            inputErrorExists = true;
        }
        else
        {
            vendorDtValidate.InnerHtml = "";
        }

        // Checks to see if there is an error. If there is, then the submission doesn't go through.
        if (inputErrorExists)
        {
            return;
        }

        // Adds new RFQ Object
        DataActions actions = new DataActions();
        currentRFQID = actions.AddRFQ(rfqDT, vendorDT);
        Response.Redirect("AddPartsToRFQ.aspx?RFQID=" + currentRFQID);
    }

    public void Cancel_Clicked(object sender, EventArgs e)
    {
        Response.Redirect("ProjectMenu.aspx");
    }
}