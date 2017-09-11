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
//using MaterialManager.Extensions;
using MaterialManager;

public partial class Admin_EditUser : System.Web.UI.Page
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

    private void updateUser()
    {
        var _db = new MaterialManager.ApplicationDbContext();

    }

    // The id parameter should match the DataKeyNames value set on the control
    // or be decorated with a value provider attribute, e.g. [QueryString]int id
    public IQueryable<MaterialManager.ApplicationUser>UserDetails_GetItem([QueryString]string id)
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

    protected void UpdateBtn_Click(object sender, EventArgs e)
    {
        updateUser();
    }

    protected void DeleteBtn_Click(object sender, EventArgs e)
    {
        updateUser();
    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void UserDetails_UpdateItem(string Id)
    {
        MaterialManager.ApplicationUser item = null;

        var _db = new MaterialManager.ApplicationDbContext();
        
        item = _db.Users.Find(Id);
       // item = _db.Users.Where(u => u.Id == id).ToList().FirstOrDefault();
        // Load the item here, e.g. item = MyDataLayer.Find(id);
        if (item == null)
        {
            // The item wasn't found
            ModelState.AddModelError("", String.Format("Item with id {0} was not found", Id));
            return;
        }
        TryUpdateModel(item);
        if (ModelState.IsValid)
        {
            var manager = new UserManager<MaterialManager.ApplicationUser>(new UserStore<MaterialManager.ApplicationUser>(_db));
            var user = manager.FindById(Request.QueryString["id"]);
            string currentRole = manager.GetRoles(user.Id).FirstOrDefault();
            string newRole = ((DropDownList)UserDetails.FindControl("roleEditList")).SelectedItem.Value;

            if (currentRole != null)
            {
                manager.RemoveFromRole(user.Id, currentRole);
            }
            manager.AddToRole(user.Id, newRole);

            _db.SaveChanges();
        }
    }

    public void UserDetails_InsertItem()
    {
        var _db = new MaterialManager.ApplicationDbContext();
        var item = new MaterialManager.ApplicationUser();
        var UserManager = new UserManager<MaterialManager.ApplicationUser>(new UserStore<MaterialManager.ApplicationUser>(_db));
        string role = ((DropDownList)UserDetails.FindControl("roleInsertList")).SelectedItem.Value;

        TryUpdateModel(item);
        if (ModelState.IsValid)
        {
            UserManager.Create(item, "Pa$$word");
            UserManager.AddToRole(item.Id, role);
        }
    }

    // When Reset Password is clicked, the user's password will be reset and the password they are reset to will be the user's JobBoss password
    public void ResetPassword_Clicked(object sender, EventArgs e)
    {
        // Gets the ID of the user
        string Id = UserDetails.Rows[0].Cells[1].Text.ToString();

        // Creates a DbContext, UserManager, UserStore, and finds the user
        var _db = new MaterialManager.ApplicationDbContext();
        var UserManager = new UserManager<MaterialManager.ApplicationUser>(new UserStore<MaterialManager.ApplicationUser>(_db));
        UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
        ApplicationUser user = UserManager.FindById(Id);

        if (user == null)
        {
            ResetStatus.InnerHtml = "Reset Failed";
            ResetStatus.Attributes["class"] = "label label-danger";
            return;
        }

        // Sets the password to the user's JobBoss password if one is set, otherwise it is set to their username
        if (user.JBPassword != "")
        {
            UserManager.RemovePassword(Id);
            UserManager.AddPassword(Id, user.JBPassword);
            // Shows the user that the reset succeeded
            ResetStatus.InnerHtml = "Reset To JBPassword";
            ResetStatus.Attributes["class"] = "label label-success";
        }
        else
        {
            UserManager.RemovePassword(Id);
            UserManager.AddPassword(Id, user.UserName);
            // Shows the user that the reset succeeded
            ResetStatus.InnerHtml = "Reset To Username";
            ResetStatus.Attributes["class"] = "label label-success";
        }
    }
}