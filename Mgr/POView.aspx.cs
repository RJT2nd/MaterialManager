using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.ModelBinding;
using MaterialManager.Models;
using MaterialManager.Logic;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO.Compression;
using System.Collections.Specialized;

public partial class Mgr_POView : System.Web.UI.Page
{
    private decimal price;
    private PurchaseOrder purchaseOrder;

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

    protected void POTotal_Load(object sender, EventArgs e)
    {
        DataActions actions = new DataActions();
        purchaseOrder = actions.GetPOByID(Convert.ToInt32(Request.QueryString["POID"]));
        price = actions.GetPOTotal(purchaseOrder.PurchaseOrderID);
        Label POTotal = (Label)PODetails.FindControl("POTotal");
        POTotal.Text = String.Format("{0:C}", price);
    }

    // The id parameter should match the DataKeyNames value set on the control
    // or be decorated with a value provider attribute, e.g. [QueryString]int id
    public PurchaseOrder PODetails_GetItem([QueryString]int POID)
    {
        DataActions actions = new DataActions();
        purchaseOrder = actions.GetPOList().Where(p => p.PurchaseOrderID == POID).FirstOrDefault();
        return purchaseOrder;
    }

    public IQueryable<MaterialManager.Models.PODocument> PODocumentList_GetData()
    {
        int POID = Convert.ToInt32(Request.QueryString["POID"]);
        DataActions actions = new DataActions();
        return actions.GetPODocumentListByID(POID).AsQueryable();
    }

    public void AddDocumentButton_Load(object sender, EventArgs e)
    {
        AddDocumentButton.Visible = !(User.IsInRole("ReadOnly"));
    }

    // Review and Approval Buttons: *****************************************************************************************************************************************************************
    // OnLoad events which load the the button texts based on the user's permission level
    public void RevertStatus_OnLoad(object sender, EventArgs e)
    {
        string buttonText = "";
        if (User.IsInRole("Review")) // Sets the text of the button based on the role of the user, or makes it invisible
        {
            buttonText = "Not Reviewed";
        }
        else if (User.IsInRole("Approval"))
        {
            buttonText = "Not Approved";
        }
        else
        {
            RevertStatus.Visible = false;
        }
        RevertStatus.Text = buttonText;
    }

    public void ChangeStatus_OnLoad(object sender, EventArgs e)
    {
        string buttonText = "";
        if (User.IsInRole("Review")) // Sets the text of the button based on the role of the user, or makes it invisible
        {
            buttonText = "Reviewed";
        }
        else if (User.IsInRole("Approval"))
        {
            buttonText = "Approved";
        }
        else
        {
            ChangeStatus.Visible = false;
        }
        ChangeStatus.Text = buttonText;
    }

    // Returns the status of the PO to not reviewed or not approved depending on the user's permission level
    public void RevertStatus_Clicked(object sender, EventArgs e)
    {
        int poID = Convert.ToInt32(Request.QueryString["POID"]);
        DataActions actions = new DataActions();
        if (User.IsInRole("Review"))
        {
            actions.UndoPOReview(poID);
        }
        else if (User.IsInRole("Approval"))
        {
            actions.UndoPOApproval(poID);
        }
    }

    // Sets the value of the RFQ to Reviewed or Approved
    public void ChangeStatus_Clicked(object sender, EventArgs e)
    {
        int poID = Convert.ToInt32(Request.QueryString["POID"]);
        DataActions actions = new DataActions();
        if (User.IsInRole("Review"))
        {
            actions.ReviewPO(poID);
        }
        else if (User.IsInRole("Approval"))
        {
            actions.ApprovePO(poID);
        }
    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void PODetails_UpdateItem(object sender)
    {
        if (User.IsInRole("Creation"))
        {
            PurchaseOrder item = purchaseOrder;
            if (item == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", $"Item with id {item.PurchaseOrderID} was not found");
                return;
            }
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                DateTime minDateTime = Convert.ToDateTime("1753-01-01 00:00:00 AM");
                Dictionary<int, dynamic> detailsViewValues = new Dictionary<int, dynamic>();
                DateTime tempDateTime;
                string rowText;
                int rowKey = 0;
                foreach (DetailsViewRow row in PODetails.Rows)
                {
                    switch (rowKey)
                    {
                        case 0:
                            rowText = Convert.ToString(purchaseOrder.PurchaseOrderID);
                            break;
                        case 1:
                            rowText = ((TextBox)PODetails.FindControl("POVendorTB")).Text;
                            break;
                        case 2:
                            rowText = ((TextBox)PODetails.FindControl("POtoVendorDateTB")).Text;
                            break;
                        case 3:
                            rowText = ((TextBox)PODetails.FindControl("RequiredDeliveryDateTB")).Text;
                            break;
                        case 4:
                            rowText = ((TextBox)PODetails.FindControl("ExpectedDeliveryDateTB")).Text;
                            break;
                        case 5:
                            rowText = ((TextBox)PODetails.FindControl("ActualDeliveryDateTB")).Text;
                            break;
                        case 6:
                            rowText = ((TextBox)PODetails.FindControl("DeliveryAddressTB")).Text;
                            break;
                        case 7:
                            rowText = ((TextBox)PODetails.FindControl("MarkForTB")).Text;
                            break;
                        case 8:
                            rowText = ((TextBox)PODetails.FindControl("JustificationTB")).Text;
                            break;
                        default:
                            rowText = "Error";
                            break;
                    }
                    if (rowKey == 0)
                    {
                        detailsViewValues.Add(rowKey, rowText);
                    }
                    else if (rowKey == 2 || rowKey == 3 || rowKey == 4 || rowKey == 5) // Rows 2, 3, 4, and 5 contain DateTime fields
                    {
                        if (DateTime.TryParse(rowText, out tempDateTime))
                        {
                            detailsViewValues.Add(rowKey, tempDateTime);
                        }
                        else if (rowText == "")
                        {
                            detailsViewValues.Add(rowKey, null);
                        }
                        else
                        {
                            string invalidDate = "<p class='text-danger'>Please enter a valid date. Format: MM/DD/YYYY hh:mm:ss AM/PM</p>";
                            switch (rowKey)
                            {
                                case 1:
                                    ((HtmlGenericControl)PODetails.FindControl("POtoVendorDateValidation")).InnerHtml = invalidDate;
                                    break;
                                case 2:
                                    ((HtmlGenericControl)PODetails.FindControl("RequiredDeliveryDateValidation")).InnerHtml = invalidDate;
                                    break;
                                case 3:
                                    ((HtmlGenericControl)PODetails.FindControl("ExpectedDeliveryDateValidation")).InnerHtml = invalidDate;
                                    break;
                                case 4:
                                    ((HtmlGenericControl)PODetails.FindControl("ActualDeliveryDateValidation")).InnerHtml = invalidDate;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        detailsViewValues.Add(rowKey, rowText);
                    }
                    rowKey++;
                }

                DataActions actions = new DataActions();
                actions.UpdatePurchaseOrder(detailsViewValues);
            }
        }

    }

    public void PODetails_InsertItem()
    {
        var item = new PurchaseOrder();
        TryUpdateModel(item);
        if (ModelState.IsValid)
        {
            // Save changes here

        }
    }

    // The return type can be changed to IEnumerable, however to support
    // paging and sorting, the following parameters must be added:
    //     int maximumRows
    //     int startRowIndex
    //     out int totalRowCount
    //     string sortByExpression
    public IQueryable<MaterialManager.Models.ExtendedPOLineItem> POLineItemList_GetData()
    {
        DataActions actions = new DataActions();
        return actions.GetExtendedPOLineItemsList(Convert.ToInt32(Request.QueryString["POID"])).AsQueryable();
    }

    protected void POLineItemList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewJobBossData")
        {
            //// Convert the row index stored in the CommandArgument
            //// property to an Integer.

            //int index = Convert.ToInt32(e.CommandArgument);

            //// Retrieve the row that contains the button clicked 
            //// by the user from the Rows collection.
            //GridViewRow row = POLineItemList.Rows[index];

            //string partID = row;

            //Response.Redirect("ViewJobBossMaterialData.aspx?PartID=" + partID);

            LinkButton linkView = (LinkButton)e.CommandSource;
            String partID = linkView.CommandArgument;
            Response.Redirect("ViewJobBossMaterialData.aspx?PartID=" + partID);
        }
        PODetails.ChangeMode(DetailsViewMode.ReadOnly);
    }

    public void POLineItemList_DeleteItem(int POLineItemID)
    {
        DataActions actions = new DataActions();
        PurchaseOrder po = actions.GetPOByID(Convert.ToInt32(Request.QueryString["POID"]));
        if (actions.GetPOLineItemByID(POLineItemID) != null)
        {
            actions.DeletePOLineItem(POLineItemID);
            price = actions.GetPOTotal(po.PurchaseOrderID);
            Label POTotal = (Label)PODetails.FindControl("POTotal");
            POTotal.Text = String.Format("{0:c}", price);
        }
    }

    // Filters the POLineItemList GridView by whatever field is picked
    public void FilterPOLineItemList(object sender, EventArgs e)
    {
        int cellNumber = 1; // The first cell contains the view button so we start at Cell[1] to avoid the view button at [0]
        if (FilterDDL.SelectedValue == "JobBoss ID")
        {
            cellNumber += 0;
        }
        else if (FilterDDL.SelectedValue == "NSN")
        {
            cellNumber += 2;
        }
        else if (FilterDDL.SelectedValue == "Part Number")
        {
            cellNumber += 3;
        }
        else if (FilterDDL.SelectedValue == "Description")
        {
            cellNumber += 4;
        }
        else if (FilterDDL.SelectedValue == "Quantity")
        {
            cellNumber += 5;
        }
        else if (FilterDDL.SelectedValue == "Price")
        {
            cellNumber += 6;
        }

        foreach (GridViewRow Row in POLineItemList.Rows)
        {
            if (!Row.Cells[cellNumber].Text.Contains(FilterTB.Text))
            {
                Row.Visible = false;
            }
            else
            {
                Row.Visible = true;
            }
        }
    }

    public void PODocumentList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DownloadFile")
        {
            string[] cmdArgs = Convert.ToString(e.CommandArgument).Split(',');
            string fileName = cmdArgs[0];
            int docID = Convert.ToInt32(cmdArgs[1]);

            DataActions actions = new DataActions();

            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.TransmitFile(Server.MapPath(actions.GetPODocumentByID(docID).FilePath));
            Response.End();
        }
    }

    public void AddDocButton_Click(object sender, EventArgs e)
    {
        Boolean fileOK = false;
        String path = Server.MapPath("/Files/");
        string newFileName = Guid.NewGuid().ToString();
        if (POFile.HasFile)
        {
            String fileExtension = System.IO.Path.GetExtension(POFile.FileName).ToLower();

            //String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
            //for (int i = 0; i < allowedExtensions.Length; i++)
            //{
            //    if (fileExtension == allowedExtensions[i])
            //    {
            //        fileOK = true;
            //    }
            //}

            fileOK = true;
        }
        if (fileOK)
        {
            try
            {
                // Save to Images folder.

                POFile.PostedFile.SaveAs(path + newFileName);


            }
            catch (Exception ex)
            {
                LabelAddStatus.Text = ex.Message;
            }

            // Add product data to DB. 

            DataActions actions = new DataActions();

            PODocument newDoc = new PODocument() { POID = Convert.ToInt32(Request.QueryString["POID"]), Description = fileDescription.Text, FileName = POFile.FileName, FilePath = "/Files/" + newFileName };

            actions.AddPODocument(newDoc);
            PODocumentList.DataBind();
        }
        else
        {
            // LabelAddStatus.Text = "Unable to accept file type.";
        }
    }

    public void PODocumentList_DeleteItem(int PODocID)
    {
        DataActions actions = new DataActions();
        actions.DeletePODocument(PODocID);
    }

    // Filters PO Documents by the selected field
    public void FilterPODocumentList(object sender, EventArgs e)
    {
        int cellNumber = 3; // The first cell contains the view button so we start at Cell[1] to avoid the view button at [0]
        if (FilterDocsDDL.SelectedValue == "File Name")
        {
            cellNumber += 0;
        }
        else if (FilterDocsDDL.SelectedValue == "Description")
        {
            cellNumber += 1;
        }

        foreach (GridViewRow Row in PODocumentList.Rows)
        {
            if (!Row.Cells[cellNumber].Text.Contains(FilterDocsTB.Text))
            {
                Row.Visible = false;
            }
            else
            {
                Row.Visible = true;
            }
        }
    }

    // Export to Excel Section: **********************************************************************************************************************************************************************************************

    public void ExcelExport_Click(object sender, EventArgs e)
    {
        int key = ExportToExcel(Convert.ToInt16(ExportOption.SelectedValue));
        String startPath = @"C:\Users\rthomas\Documents\Material Tracking - Test Copy\Websites\MaterialManager\Files\Export";
        String zipPath = @"C:\Users\rthomas\Documents\Material Tracking - Test Copy\Websites\MaterialManager\Files\ExportArchives\PO" + key + ".zip";
        ZipFile.CreateFromDirectory(startPath, zipPath);
        ZipArchive archive = ZipFile.Open(@"C:\Users\rthomas\Documents\Material Tracking - Test Copy\Websites\MaterialManager\Files\ExportArchives\PO" + key + ".zip", ZipArchiveMode.Update);
        archive.CreateEntryFromFile(@"C:\Users\rthomas\Documents\Material Tracking - Test Copy\Websites\MaterialManager\Files\POExport" + key + ".xls", "PO.xls");

        IOrderedDictionary rowValues = new OrderedDictionary();
        DataActions actions = new DataActions();
        int docID;

        foreach (GridViewRow Row in ExportPODocumentList.Rows)
        {
            rowValues = GetValues(Row);

            if (((CheckBox)Row.FindControl("DownloadCB")).Checked)
            {
                docID = Convert.ToInt32(rowValues["PODocID"]);
                PODocument doc = actions.GetPODocumentByID(docID);
                if (doc != null)
                {
                    archive.CreateEntryFromFile(@"C:\Users\rthomas\Documents\Material Tracking - Test Copy\Websites\MaterialManager" + doc.FilePath, doc.FileName);
                }
            }
        }

        archive.Dispose();

        File.Delete(@"C:\Users\rthomas\Documents\Material Tracking - Test Copy\Websites\MaterialManager\Files\POExport" + key + ".xls");

        Response.Clear();
        Response.AppendHeader("Content-Disposition", "attachment; filename=PO" + Request.QueryString["POID"]);
        //Response.ContentType = "application/x-zip-compressed";
        //Response.WriteFile(zipPath);
        Response.TransmitFile(zipPath);
        Response.End();

        File.Delete(zipPath);
    }

    public int ExportToExcel(int orientation)
    {
        DataActions actions = new DataActions();

        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;

        xlApp = new Excel.ApplicationClass();
        xlWorkBook = xlApp.Workbooks.Add(misValue);
        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        int rowStart = 1;
        int rowShift = 0;
        int colStart = 1;
        int colShift = 0;
        string cellText = "";

        if (orientation == 1) // Vertical Alignment
        {
            // PO Details
            xlWorkSheet.Cells[rowShift + 1, colShift + 1] = "PO Details";
            rowShift++;
            rowShift += PODetails.Rows.Count;
            colShift += PODetails.Rows[0].Cells.Count;
            for (int i = 0; i < PODetails.Rows.Count; i++)
            {
                for (int j = 0; j < PODetails.Rows[i].Cells.Count; j++)
                {
                    cellText = PODetails.Rows[i].Cells[j].Text;
                    xlWorkSheet.Cells[i + 1 + rowStart, j + colStart] = cellText == "&nbsp;" ? "" : cellText;
                }
            }
            xlWorkSheet.Cells[PODetails.Rows.Count + rowStart, PODetails.Rows[0].Cells.Count - 1 + colStart] = ((Label)PODetails.FindControl("POTotal")).Text;
            xlWorkSheet.Range[xlWorkSheet.Cells[rowStart, colStart], xlWorkSheet.Cells[rowStart + rowShift - 1, colStart + colShift - 1]].AutoFormat();
            rowStart += rowShift + 1;
            rowShift = 0;
            colShift = 0;


            // PO Line Items
            xlWorkSheet.Cells[rowStart, colStart] = "Line Items";
            rowShift++;
            rowShift += POLineItemList.Rows.Count + 1; // + 1 for header row
            colShift += POLineItemList.Columns.Count - 3; // - 3 for unused columns
            
            for (int j = 1; j < POLineItemList.Columns.Count; j++)
            {
                xlWorkSheet.Cells[rowStart + 1, j + colStart - 1] = POLineItemList.Columns[j].HeaderText;
            }

            for (int i = 0; i < POLineItemList.Rows.Count; i++)
            {
                for (int j = 2; j < POLineItemList.Columns.Count; j++) // j = 2 because data doesn't start until 2
                {
                    cellText = POLineItemList.Rows[i].Cells[j].Text;
                    xlWorkSheet.Cells[i + 2 + rowStart, j + colStart - 2] = cellText == "&nbsp;" ? "" : cellText; // j - 2 since j starts at 2
                }
            }
            xlWorkSheet.Range[xlWorkSheet.Cells[rowStart, colStart], xlWorkSheet.Cells[rowStart + rowShift - 1, colStart + colShift]].AutoFormat();
            rowStart += rowShift + 1;
            rowShift = 0;
            colShift = 0;
        }
        else // Horizontal Orientation
        {
            // PO Details
            xlWorkSheet.Cells[rowShift + 1, colShift + 1] = "PO Details";
            rowShift++;
            rowShift += PODetails.Rows.Count;
            colShift += PODetails.Rows[0].Cells.Count;
            for (int i = 0; i < PODetails.Rows.Count; i++)
            {
                for (int j = 0; j < PODetails.Rows[i].Cells.Count; j++)
                {
                    cellText = PODetails.Rows[i].Cells[j].Text;
                    xlWorkSheet.Cells[i + 1 + rowStart, j + colStart] = cellText == "&nbsp;" ? "" : cellText;
                }
            }
            xlWorkSheet.Range[xlWorkSheet.Cells[rowStart, colStart], xlWorkSheet.Cells[rowStart + rowShift - 1, colStart + colShift - 1]].AutoFormat();
            colStart += colShift + 1;
            rowShift = 0;
            colShift = 0;


            // RFQ Line Items
            xlWorkSheet.Cells[rowStart, colStart] = "Line Items";
            rowShift++;
            rowShift += POLineItemList.Rows.Count + 1; // + 1 for header row
            colShift += POLineItemList.Columns.Count - 3; // - 3 for unused columns

            for (int j = 1; j < POLineItemList.Columns.Count; j++)
            {
                xlWorkSheet.Cells[rowStart + 1, j + colStart - 1] = POLineItemList.Columns[j].HeaderText;
            }

            for (int i = 0; i < POLineItemList.Rows.Count; i++)
            {
                for (int j = 1; j < POLineItemList.Columns.Count; j++) // j = 2 because data doesn't start until 2
                {
                    cellText = POLineItemList.Rows[i].Cells[j].Text;
                    xlWorkSheet.Cells[i + 2 + rowStart, j + colStart - 2] = cellText == "&nbsp;" ? "" : cellText; // j - 2 since j starts at 2
                }
            }
            xlWorkSheet.Cells[PODetails.Rows.Count + rowStart, PODetails.Rows[0].Cells.Count - 1 + colStart] = ((Label)PODetails.FindControl("POTotal")).Text;
            xlWorkSheet.Range[xlWorkSheet.Cells[rowStart, colStart], xlWorkSheet.Cells[rowStart + rowShift - 1, colStart + colShift - 1]].AutoFormat();
            colStart += colShift + 1;
            rowShift = 0;
            colShift = 0;
        }

        Random rng = new Random();
        int key = rng.Next(999999);

        xlWorkBook.SaveAs(@"C:\Users\rthomas\Documents\Material Tracking - Test Copy\Websites\MaterialManager\Files\POExport" + key + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        xlWorkBook.Close(true, misValue, misValue);
        xlApp.Quit();

        releaseObject(xlApp);
        releaseObject(xlWorkBook);
        releaseObject(xlWorkSheet);

        return key;
    }

    private static void releaseObject(object obj)
    {
        try
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            obj = null;
        }
        catch
        {
            obj = null;
        }
        finally
        {
            GC.Collect();
        }
    }

    public static IOrderedDictionary GetValues(GridViewRow row)
    {
        IOrderedDictionary values = new OrderedDictionary();
        foreach (DataControlFieldCell cell in row.Cells)
        {
            if (cell.Visible)
            {
                cell.ContainingField.ExtractValuesFromCell(values, cell, row.RowState, true);
            }
        }
        return values;
    }

    protected void PrepareForExportDetailsView(DetailsView dv)
    {
        // Change the Header Row to white
        dv.HeaderRow.Style.Add("background-color", "#FFFFFF");

        // Apply style to Individual Cells
        for (int k = 0; k < dv.HeaderRow.Cells.Count; k++)
        {
            dv.HeaderRow.Cells[k].Style.Add("background-color", "blue");
        }

        for (int i = 0; i < dv.Rows.Count; i++)
        {
            DetailsViewRow row = dv.Rows[i];

            // Change color back to white
            row.BackColor = System.Drawing.Color.White;

            // Apply text style to each Row
            row.Attributes.Add("class", "textmode");

            // Apply style to individual cells of alternating row
            if (i % 2 != 0)
            {
                for (int j = 0; j < dv.Rows[i].Cells.Count; j++)
                {
                    row.Cells[j].Style.Add("background-color", "#ADD8E6");
                }
            }

            row.Cells[0].Style.Add("background-color", "#5DADE2");
        }

        dv.Rows[dv.Rows.Count - 1].Visible = false;
    }

    protected void PrepareForExportGridView(GridView gv)
    {
        // Change the Header Row to white
        gv.HeaderRow.Style.Add("background-color", "#FFFFFF");

        // Apply style to Individual Cells
        for(int k = 0; k < gv.HeaderRow.Cells.Count; k++)
        {
            gv.HeaderRow.Cells[k].Style.Add("background-color", "#5DADE2");
        }

        for(int i = 0; i < gv.Rows.Count; i++)
        {
            GridViewRow row = gv.Rows[i];

            // Change color back to white
            row.BackColor = System.Drawing.Color.White;

            // Apply text style to each Row
            row.Attributes.Add("class", "textmode");

            // Apply style to individual cells of alternating row
            if(i%2 != 0)
            {
                for(int j = 0; j < gv.Rows[i].Cells.Count; j++)
                {
                    row.Cells[j].Style.Add("background-color", "#ADD8E6");
                }
            }
        }

        foreach(GridViewRow row in gv.Rows)
        {
            row.Cells[0].Visible = false;
            row.Cells[row.Cells.Count - 1].Visible = false;
        }

        GridViewRow Row = gv.HeaderRow;
        Row.Cells[0].Visible = false;
        Row.Cells[Row.Cells.Count - 1].Visible = false;
        DisableControls(gv);
    }

    private void DisableControls(GridView gv)

    {

        LinkButton lb = new LinkButton();


        string name = String.Empty;

        //for (int i = 0; i < gv.HeaderRow.Controls.Count; i++)

        //{
        //    if (gv.HeaderRow.Controls[i].GetType() == typeof(LinkButton))
        //    {
        //        l.Text = (gv.HeaderRow.Controls[i] as LinkButton).Text;

        //        gv.HeaderRow.Controls.Remove(gv.HeaderRow.Controls[i]);

        //        gv.HeaderRow.Controls.AddAt(i, l);
        //    }
        //}
        for(int i = 0; i< gv.HeaderRow.Cells.Count; i++)
        {
            if (gv.HeaderRow.Cells[i].Controls.Count != 0)
            {
                Literal l = new Literal();
                l.Text = (gv.HeaderRow.Cells[i].Controls[0] as LinkButton).Text;
                gv.HeaderRow.Cells[i].Controls.Clear();
                gv.HeaderRow.Cells[i].Controls.Add(l);
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void EditLinkButton_Load(object sender, EventArgs e)
    {
        if (!User.IsInRole("Creation"))
        {
            EditLinkButton.Visible = false;
        }
        else
        {
            EditLinkButton.Visible = true;
        }
    }

    protected void EditLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddPartsToPO.aspx?POID=" + Convert.ToInt32(Request.QueryString["POID"]));
    }

    protected void POLineItemList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!User.IsInRole("Creation"))
        {
            e.Row.Cells[0].Visible = false;
        }
    }

    protected void PODocumentList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!User.IsInRole("Creation"))
        {
            e.Row.Cells[0].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
            {
                ((LinkButton)e.Row.Cells[0].Controls[0]).Attributes["onclick"] = "if(!confirm('Are you sure to delete this document?'))return   false;";
            }
        }
    }
}
