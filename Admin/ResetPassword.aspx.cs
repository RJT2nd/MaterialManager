using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.ModelBinding;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MaterialManager;

public partial class Admin_ResetPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void Role_Load(object sender, EventArgs e)
    {
        var _db = new MaterialManager.ApplicationDbContext();
        var manager = new UserManager<MaterialManager.ApplicationUser>(new UserStore<MaterialManager.ApplicationUser>(_db));
        var user = manager.FindById(Request.QueryString["id"]);

        // This statement keeps feeding me null, probably due to the order of page loading
        Label roleLabel = (Label)UserDetails.FindControl("UserRole");
        try
        {
            if (roleLabel != null)
            {
                roleLabel.Text = manager.GetRoles(user.Id).FirstOrDefault();
            }
        }
        catch { }
    }

    public IQueryable<MaterialManager.ApplicationUser> UserDetails_GetItem([QueryString]string id)
    {
        var _db = new MaterialManager.ApplicationDbContext();
        IQueryable<MaterialManager.ApplicationUser> query = _db.Users;

        if (!String.IsNullOrEmpty(id))
        {
            query = query.Where(u => u.Id == id);
        }
        else
        {
            query = null;
        }
        return query;
    }

}