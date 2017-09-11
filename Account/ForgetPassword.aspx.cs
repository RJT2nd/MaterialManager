using MaterialManager;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_ForgetPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void RequestPassword_Click(object sender, EventArgs e)
    {
        string userName = ((TextBox)UserName).Text;
        string email = ((TextBox)Email).Text;
        string newPassword = RandomString(12);

        var _db = new MaterialManager.ApplicationDbContext();
        if(_db.Users.Where(_user => _user.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase) && _user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
        {
            UserManager Manager = new UserManager();
            var User = _db.Users.Where(_user => _user.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)
                && _user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)).First();
            ConnectionStringSettings settingsEmailAddress = ConfigurationManager.ConnectionStrings["EmailAddress"];
            ConnectionStringSettings settingsEmailPassword = ConfigurationManager.ConnectionStrings["EmailPassword"];

            var fromAddress = new MailAddress(settingsEmailAddress.ConnectionString, "No Reply");
            var toAddress = new MailAddress(email, userName);
            string fromPassword = settingsEmailPassword.ConnectionString;
            string subject = "MaterialManager Password Reset";
            string body = String.Format("Hello {0}, your new password is {1}", User.UserName, newPassword);

            IdentityResult result = Manager.RemovePassword(User.Id);
            if(result.Succeeded)
                result = Manager.AddPassword(User.Id, newPassword);

            if (result.Succeeded)
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }

    static string RandomString(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(valid[(int)(num % (uint)valid.Length)]);
            }
        }

        return res.ToString();
    }
}