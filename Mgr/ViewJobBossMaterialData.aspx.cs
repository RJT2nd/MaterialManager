using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Logic;
using MaterialManager.Models;
using System.Web.ModelBinding;


public partial class Mgr_ViewJobBossData : System.Web.UI.Page
{
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
            updateTitles();

        }

    }

    private void updateTitles()
    {
        int id = Convert.ToInt32(Request.QueryString["PartID"]);
        if (id > 0)
        {
            DataActions actions = new DataActions();
            Part partData = actions.getPart(id);
            PartNumber.Text = partData.PartNumber;
            NSN.Text = partData.NSN;
        }
    }

    // The id parameter should match the DataKeyNames value set on the control
    // or be decorated with a value provider attribute, e.g. [QueryString]int id
    // It is called to find the item by its id
    public JBXML.Respond.JBXMLJBXMLRespondMaterialQueryRs JobBossDetails_GetItem([QueryString("PartID")]int id)
    {
        DataActions actions = new DataActions();
        Part thisPart = actions.getPart(id);

        return actions.getJBMaterialDetails(thisPart.JBMaterialID);




    }

}
