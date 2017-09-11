//using System.Security.Claims;
//using System.Security.Principal;

//namespace MaterialManager.Extensions
//{
//    public static class IdentityExtensions
//    {
//        public static string GetRole(this IIdentity identity)
//        {
//            var claim = ((ClaimsIdentity)identity).FindFirst("Role");
//            // Test for null to avoid issues during local testing
//            return (claim != null) ? claim.Value : string.Empty;
//        }

//        //public static void SetRole(ref string role, string newRole)
//        //{
//        //    role = newRole;
//        //}
//    }
//}