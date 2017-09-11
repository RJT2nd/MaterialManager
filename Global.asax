<%@ Application Language="C#" %>
<%@ Import Namespace="MaterialManager" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Data.Entity" %>
<%@ Import Namespace="MaterialManager.Models" %>
<%@ Import Namespace="MaterialManager.Logic" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);

        Database.SetInitializer(new ProductDatabaseInitializer());
        Database.SetInitializer(new UserDatabaseInitializer());

        //RoleActions roleActions = new RoleActions();
        //roleActions.createRoles();
    }

</script>
