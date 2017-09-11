using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MaterialManager.Logic;



public partial class Admin_UserAdmin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public IQueryable<MaterialManager.ApplicationUser> GetUsers()
    {
        UserActions actions = new UserActions();
        return actions.GetUserList().AsQueryable();
    }

    public void FilterUserList(object sender, EventArgs e)
    {
        int cellNumber = 1; // The first cell contains the select button so we start at Cell[1] to avoid the select button at [0]
        if (FilterDDL.SelectedValue == "User ID")
        {
            cellNumber += 0;
        }
        else if (FilterDDL.SelectedValue == "User Name")
        {
            cellNumber += 1;
        }
        else if (FilterDDL.SelectedValue == "Title")
        {
            cellNumber += 2;
        }
        else if (FilterDDL.SelectedValue == "E-Mail")
        {
            cellNumber += 3;
        }
        else if (FilterDDL.SelectedValue == "JobBoss User")
        {
            cellNumber += 4;
        }

        foreach (GridViewRow Row in UserList.Rows)
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
}
