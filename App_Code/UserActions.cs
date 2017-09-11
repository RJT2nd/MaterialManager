using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserActions
/// </summary>
namespace MaterialManager.Logic
{
    public class UserActions : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public UserActions()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public List<ApplicationUser> GetUserList()
        {
            return db.Users.ToList();
        }
        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();

                db = null;
            }
        }

        public void UpdateUser(ApplicationUser user)
        {
           
        }
    }
}