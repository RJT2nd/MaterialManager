using MaterialManager.Logic;
using MaterialManager.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.CSharp;
using System.Collections;
using System.Collections.Generic;
using System.Web.ModelBinding;

public partial class Mgr_AddPartsToRFQ : System.Web.UI.Page
{
    public int RFQID, RowIndex;
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
            Project projectData = actions.getProject(actions.GetCurrentProject());
            ProjectName.Text = projectData.ProjectName;
            ContractNumber.Text = projectData.ContractNumber;
        }
    }

    // The id parameter should match the DataKeyNames value set on the control
    // or be decorated with a value provider attribute, e.g. [QueryString]int id
    public MaterialManager.Models.RequestForQuote RFQDetails_GetItem([QueryString]int RFQID)
    {
        DataActions actions = new DataActions();
        return actions.GetRFQByID(RFQID);
    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void RFQDetails_UpdateItem(int RFQID)
    {
        MaterialManager.Models.RequestForQuote item = null;
        // Load the item here, e.g. item = MyDataLayer.Find(id);
        DataActions actions = new DataActions();
        item = actions.GetRFQByID(RFQID);
        if (item == null)
        {
            // The item wasn't found
            ModelState.AddModelError("", String.Format("Item with id {0} was not found", RFQID));
            return;
        }
        TryUpdateModel(item);
        if (ModelState.IsValid)
        {
            // Save changes here, e.g. MyDataLayer.SaveChanges();
            actions.SaveChanges();
        }
    }

    // Filters the RFQLineItemList GridView by whatever field is picked
    public void FilterRFQLineItemList(object sender, EventArgs e)
    {
        int cellNumber = 2; // The first cell containing content
        if (FilterLineItemsDDL.SelectedValue == "JobBoss ID")
        {
            cellNumber += 0;
        }
        else if (FilterLineItemsDDL.SelectedValue == "NSN")
        {
            cellNumber += 2;
        }
        else if (FilterLineItemsDDL.SelectedValue == "Part Number")
        {
            cellNumber += 3;
        }
        else if (FilterLineItemsDDL.SelectedValue == "Description")
        {
            cellNumber += 4;
        }
        else if (FilterLineItemsDDL.SelectedValue == "Quantity")
        {
            cellNumber += 5;
        }

        foreach (GridViewRow Row in RFQLineItemList.Rows)
        {
            if (!Row.Cells[cellNumber].Text.Contains(FilterLineItemsTB.Text))
            {
                Row.Visible = false;
            }
            else
            {
                Row.Visible = true;
            }
        }
    }

    public IQueryable<MaterialManager.Models.ExtendedLineItem> RFQLineItemList_GetData()
    {
        int RFQID = Convert.ToInt32(Request.QueryString["RFQID"]);
        DataActions actions = new DataActions();
        return actions.GetExtendedLineItemsList(RFQID).AsQueryable();
    }

    // Changes webpages over to ViewJobBossMaterialData and sends the item ID. It is then called for 
    protected void RFQLineItemList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewJobBossData")
        {
            LinkButton linkView = (LinkButton)e.CommandSource;
            String partID = linkView.CommandArgument;
            Response.Redirect("ViewJobBossMaterialData.aspx?PartID=" + partID);
        }

        //if (e.CommandName == "RowUpdated")
        //{
        //    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //    int RowIndex = gvr.RowIndex;

        //    MaterialManager.Models.Part item = null;
        //    DataActions actions = new DataActions();
        //    RFQLineItem rfqItem = null;

        //    int RFQLineItemID = Convert.ToInt32(gvr.FindControl("RFQLineItemID"));
        //    rfqItem = actions.GetRFQLineItem(RFQLineItemID);
        //    item = actions.getPart(rfqItem.PartID);
        //    // Load the item here, e.g. item = MyDataLayer.Find(id);
        //    if (item == null)
        //    {
        //        // The item wasn't found
        //        ModelState.AddModelError("", String.Format("Item's Part ID was not found"));
        //        return;
        //    }
        //    if (rfqItem == null)
        //    {
        //        // The item wasn't found
        //        ModelState.AddModelError("", String.Format("Item with id {0} was not found", RFQLineItemID));
        //        return;
        //    }

        //    //try
        //    //{
        //    // Save changes here, e.g. MyDataLayer.SaveChanges();
        //    item.NSN = ((TextBox)gvr.FindControl("NSNTB")).Text;
        //    item.PartNumber = ((TextBox)gvr.FindControl("PartNumberTB")).Text;
        //    item.Description = ((TextBox)gvr.FindControl("PartNumberTB")).Text;
        //    rfqItem.Quantity = Convert.ToInt32(((TextBox)gvr.FindControl("QuantityTB")).Text);

        //    actions.SaveChanges();
        //    //}
        //    //catch(Exception ex) { throw ex; }
        //}
    }

    public void RFQLineItemList_DeleteItem(int RFQLineItemID)
    {
        DataActions actions = new DataActions();
        if(actions.GetRFQLineItemByID(RFQLineItemID) != null)
        {
            actions.DeleteRFQLineItem(RFQLineItemID);
        }
    }

    protected void AddPart_ServerClick(object sender, EventArgs e)
    {
        RFQID = Convert.ToInt32(Request.QueryString["RFQID"]);
        Response.Redirect("AddRFQLineItems.aspx?RFQID=" + RFQID);
    }

    protected void Finalize_ServerClick(object sender, EventArgs e)
    {
        RFQID = Convert.ToInt32(Request.QueryString["RFQID"]);
        Response.Redirect("RFQView.aspx?RFQID=" + RFQID);
    }

    protected void AddBOMDocButton_Click(object sender, EventArgs e)
    {
        bool isExcel = false;
        String path = Server.MapPath(@"..\Files\");
        string newFileName = Guid.NewGuid().ToString();
        int currentRFQID = Convert.ToInt32(Request.QueryString["RFQID"]);

        if (BOMFile.HasFile)
        {
            String fileExtension = System.IO.Path.GetExtension(BOMFile.FileName).ToLower();
            String[] AllowedExtensions = { ".xls", ".xlsx" };
            foreach (String AllowedExtension in AllowedExtensions)
            {
                if (AllowedExtension.Contains(fileExtension))
                {
                    isExcel = true;
                }
            }

            if (isExcel)
            {
                try
                {
                    BOMFile.PostedFile.SaveAs(path + newFileName);
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }

                DataActions actions = new DataActions();

                RFQDocument newDoc = new RFQDocument()
                {
                    RFQID = Convert.ToInt32(Request.QueryString["RFQID"]),
                    Description = "RFQ BOM",
                    FileName = BOMFile.FileName,
                    FilePath = path + newFileName
                };

                actions.AddRFQDocument(newDoc);

                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(newDoc.FilePath);
                Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                Dictionary<string, int> DataKeys = new Dictionary<string, int>();
                String[] PossibleKeys = { "JobBoss ID", "NSN", "Part Number", "Description", "Quantity" };

                string xlJBMaterialID = "";
                string xlNSN = "";
                string xlPartNumber = "";
                string xlDescription = "";
                int xlQuantity = 0;
                int colIndex;

                for (int i = 1; i <= colCount; i++)
                {
                    string Key = ((Excel.Range)xlWorksheet.Cells[2, i]).Value2.ToString();
                    if (PossibleKeys.Contains(Key))
                    {
                        DataKeys.Add(Key, i);
                    }
                }

                //// Checking to make sure rowIndex of 3 is available; not necessary?
                //if (xlWorksheet.Cells[3, 3] == null)
                //{
                //    NullReferenceException er = new NullReferenceException();
                //    throw er;
                //}

                for (int rowIndex = 3; rowIndex <= rowCount; rowIndex++)
                {
                    xlJBMaterialID = DataKeys.TryGetValue("JobBoss ID", out colIndex) ? (string)((Excel.Range)xlWorksheet.Cells[rowIndex, colIndex]).Value2 : "";
                    xlNSN = DataKeys.TryGetValue("NSN", out colIndex) ? (string)((Excel.Range)xlWorksheet.Cells[rowIndex, colIndex]).Value2 : "";
                    xlPartNumber = DataKeys.TryGetValue("Part Number", out colIndex) ? (string)((Excel.Range)xlWorksheet.Cells[rowIndex, colIndex]).Value2 : "";
                    xlDescription = DataKeys.TryGetValue("Description", out colIndex) ? (string)((Excel.Range)xlWorksheet.Cells[rowIndex, colIndex]).Value2 : "";
                    xlQuantity = DataKeys.TryGetValue("Quantity", out colIndex) ? Convert.ToInt32(((Excel.Range)xlWorksheet.Cells[rowIndex, colIndex]).Value2) : 0;

                    Part p = new Part()
                    {
                        JBMaterialID = xlJBMaterialID,
                        NSN = xlNSN,
                        PartNumber = xlPartNumber,
                        Description = xlDescription
                    };

                    actions.AddPartToRFQ(p, currentRFQID, xlQuantity);
                }

                xlWorkbook.Close(0);
                xlApp.Quit();
            }

            
            RFQLineItemList.DataBind();
        }
    }

    public void RFQLineItemList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        RowIndex = e.RowIndex;

        GridViewRow gvr = RFQLineItemList.Rows[RowIndex];

        MaterialManager.Models.Part item = null;
        DataActions actions = new DataActions();
        RFQLineItem rfqItem = null;

        int RFQLineItemID1 = Convert.ToInt32(((Label)(gvr.FindControl("RFQLineItemIDLabel"))).Text);
        rfqItem = actions.GetRFQLineItem(RFQLineItemID1);
        item = actions.getPart(rfqItem.PartID);
        // Load the item here, e.g. item = MyDataLayer.Find(id);
        if (item == null)
        {
            // The item wasn't found
            ModelState.AddModelError("", String.Format("Item's Part ID was not found"));
            return;
        }
        if (rfqItem == null)
        {
            // The item wasn't found
            ModelState.AddModelError("", String.Format("Item with id {0} was not found", RFQLineItemID1));
            return;
        }

        //try
        //{
        // Save changes here, e.g. MyDataLayer.SaveChanges();
        item.NSN = ((TextBox)gvr.FindControl("NSNTB")).Text;
        item.PartNumber = ((TextBox)gvr.FindControl("PartNumberTB")).Text;
        item.Description = ((TextBox)gvr.FindControl("DescriptionTB")).Text;
        rfqItem.Quantity = Convert.ToInt32(((TextBox)gvr.FindControl("QuantityTB")).Text);

        actions.SaveChanges();
    }

    public void RFQLineitemList_UpdateItem(object sender, EventArgs e)
    {

    }

    protected void AddFromRFQ_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("AddFromRFQ.aspx?RFQID=" + Request.QueryString["RFQID"]);
    }

    protected void AddCustomPartButton_Click(object sender, EventArgs e)
    {
        string jbID = AddJobBossIDTB.Text;
        string nsn = AddNSNTB.Text;
        string partNumber = AddPartNumberTB.Text;
        string description = AddDescriptionTB.Text;
        int quantity = Convert.ToInt32(AddQuantityTB.Text != "" ? AddQuantityTB.Text : "0");

        int currentRFQID = Convert.ToInt32(Request.QueryString["RFQID"]);

        Part newPart = new Part()
        {
            JBMaterialID = jbID,
            NSN = nsn,
            PartNumber = partNumber,
            Description = description
        };

        DataActions actions = new DataActions();
        actions.AddPartToRFQ(newPart, currentRFQID, quantity);

        RFQLineItemList.DataBind();
    }
}