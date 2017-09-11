using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MaterialManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

/// <summary>
/// Summary description for RoleActions
/// </summary>

namespace MaterialManager.Logic
{
    public class RoleActions
    {
        public static string[] roles = { "Administrator", "Approval", "Review", "Creation", "ReadOnly" };

        public void createRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdRoleResult;
                      

            var roleStore = new RoleStore<IdentityRole>(context);

            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            foreach (string role in roles)
            {
                bool roleExists = roleMgr.RoleExists(role);
                if (!roleExists)
                {
                    IdRoleResult = roleMgr.Create(new IdentityRole(role));
                    if (!IdRoleResult.Succeeded)
                    {
                        throw new Exception("Role not created");
                    }
                }

               /* var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var appUser = new ApplicationUser()
                {
                    UserName = "Admin"
                };
                IdUserResult = userMgr.Create(appUser, "Pa$$word");

                if (IdUserResult.Succeeded)
                {
                    IdUserResult = userMgr.AddToRole(appUser.Id, "Administrator");
                    if (!IdUserResult.Succeeded)
                    {
                        //throw new Exception("Administrator not created");
                    }
                }
                else
                {
                    //throw new Exception("User not created");
                }
                */
            }
        }
    }
}