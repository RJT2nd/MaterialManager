using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MaterialManager.Logic;

/// <summary>
/// Summary description for ProductDatabaseInitializer
/// </summary>

namespace MaterialManager
{
    public class UserDatabaseInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        private void InitializeIdentityForEF(ApplicationDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityResult IdRoleResult;

            string name = "Administrator";
            string password = "Pa$$word";
            string JBUser = "JBTest";
            string JBPass = "JBTest";
            string phone = "703-900-5450";
            string email = "adam.grimm@kihomac.com";
            string title = "KIHOMAC Administrator";

            //RoleActions roleActions = new RoleActions();
            //roleActions.createRoles();

            foreach (string role in RoleActions.roles)
            {
                bool roleExists = RoleManager.RoleExists(role);
                if (!roleExists)
                {
                    IdRoleResult = RoleManager.Create(new IdentityRole(role));
                    if (!IdRoleResult.Succeeded)
                    {
                        throw new Exception("Role not created");
                    }
                }
            }

                //Create default administrator
                var user = new ApplicationUser();
            user.UserName = name;
            user.JBUser = JBUser;
            user.JBPassword = JBPass;
            user.Email = email;
            user.PhoneNumber = phone;
            user.Title = title;
            var adminresult = UserManager.Create(user, password);

            

            //Add User Admin to all rolls
            if (adminresult.Succeeded)
            {
                foreach (string role in RoleActions.roles)
                {
                    var result = UserManager.AddToRole(user.Id, role);
                }
            }
        }

     }
}