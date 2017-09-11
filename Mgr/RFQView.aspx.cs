using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MaterialManager.Logic;
using MaterialManager.Models;
using System.IO;
using System.Web.ModelBinding;
using System.IO.Compression;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Collections.Specialized;

public partial class Mgr_RFQView : System.Web.UI.Page
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

    public void RFQDetails_InsertItem()
    {
        var item = new MaterialManager.Models.RequestForQuote();
        TryUpdateModel(item);
        if (ModelState.IsValid)
        {
            DataActions actions = new DataActions();
            actions.SaveChanges();
        }
    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void RFQDetails_UpdateItem(int RFQID)
    {
        DataActions actions = new DataActions();
        MaterialManager.Models.RequestForQuote item = null;
        item = actions.GetRFQByID(Convert.ToInt32(Request.QueryString["RFQID"]));
        if (item == null)
        {
            // The item wasn't found
            ModelState.AddModelError("", String.Format("Item with id {0} was not found", RFQID));
            return;
        }
        TryUpdateModel(item);
        if (ModelState.IsValid)
        {
            actions.SaveChanges();

        }
    }

    // Review and Approval Buttons: *****************************************************************************************************************************************************************
    // OnLoad events which load the the button texts based on the user's permission level
    public void RevertStatus_OnLoad(object sender, EventArgs e)
    {
        string buttonText = "";
        if (User.IsInRole("Review")) // Sets the text of the button based on the role of the user, or makes it invisible
        {
            buttonText = "Not Reviewed";
        } else if (User.IsInRole("Approval"))
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
        
    // Returns the status of the RFQ to not reviewed or not approved depending on the user's permission level
    public void RevertStatus_Clicked(object sender, EventArgs e)
    {
        int rfqID = Convert.ToInt32(Request.QueryString["RFQID"]);
        DataActions actions = new DataActions();
        if (User.IsInRole("Review"))
        {
            actions.UndoRFQReview(rfqID);
        }else if (User.IsInRole("Approval"))
        {
            actions.UndoRFQApproval(rfqID);
        }
    }

    // Sets the value of the RFQ to Reviewed or Approved
    public void ChangeStatus_Clicked(object sender, EventArgs e)
    {   
        int rfqID = Convert.ToInt32(Request.QueryString["RFQID"]);
        DataActions actions = new DataActions();
        if (User.IsInRole("Review"))
        {
            actions.ReviewRFQ(rfqID);
        }
        else if (User.IsInRole("Approval"))
        {
            actions.ApproveRFQ(rfqID);
        }
    }

    // RFQLineItem Section: *****************************************************************************************************************************************************************
    // Sends list of RFQLineItems filtered by the ID of the RFQ
    public IQueryable<MaterialManager.Models.ExtendedLineItem> RFQLineItemList_GetData()
    {
        int RFQID = Convert.ToInt32(Request.QueryString["RFQID"]);
        DataActions actions = new DataActions();
        return actions.GetExtendedLineItemsList(RFQID).AsQueryable();
    }

    // The id parameter name should match the DataKeyNames value set on the control
    public void RFQLineItemList_DeleteItem(int RFQLineItemID)
    {
        DataActions action = new DataActions();
        action.DeleteRFQLineItem(RFQLineItemID);
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

    // Changes webpages over to ViewJobBossMaterialData and sends the item ID. It is then called for 
    protected void RFQLineItemList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewJobBossData")
        {
            //// Convert the row index stored in the CommandArgument
            //// property to an Integer.

            //int index = Convert.ToInt32(e.CommandArgument);

            //// Retrieve the row that contains the button clicked 
            //// by the user from the Rows collection.
            //GridViewRow row = RFQLineItemList.Rows[index];

            //string partID = row.Cells[3].Text;

            //Response.Redirect("ViewJobBossMaterialData.aspx?PartID=" + partID);

            LinkButton linkView = (LinkButton)e.CommandSource;
            String partID = linkView.CommandArgument;
            Response.Redirect("ViewJobBossMaterialData.aspx?PartID=" + partID);

        }

    }

    // RFQDocument Section: *****************************************************************************************************************************************************************
    // Sends list of RFQDocuments filtered by the ID of the RFQ
    public IQueryable<MaterialManager.Models.RFQDocument> RFQDocumentList_GetData()
    {
        int RFQID = Convert.ToInt32(Request.QueryString["RFQID"]);
        DataActions actions = new DataActions();
        return actions.GetRFQDocumentList(RFQID).AsQueryable();
    }

    // Makes the button to add documents invisible is the User's role is ReadOnly
    public void AddDocumentButton_Load(object sender, EventArgs e)
    {
        AddDocumentButton.Visible = !(User.IsInRole("ReadOnly"));
    }

    // Deletes RFQDocument from the database
    public void RFQDocumentList_DeleteItem(int RFQDocID)
    {
        DataActions action = new DataActions();
        action.DeleteRFQDocument(RFQDocID);
    }
    
    // Uploads the Document
    protected void AddDocButton_Click(object sender, EventArgs e)
    {
  
        Boolean fileOK = false;
        String path = Server.MapPath("../Files/");
        string newFileName = Guid.NewGuid().ToString();
        if (RFQFile.HasFile)
        {
            String fileExtension = System.IO.Path.GetExtension(RFQFile.FileName).ToLower();

            /*String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    fileOK = true;
                }
            }
            */
            fileOK = true;
        }
        if (fileOK)
        {
            try
            {
                // Save to Images folder.

                RFQFile.PostedFile.SaveAs(path + newFileName);


            }
            catch (Exception ex)
            {
                LabelAddStatus.Text = ex.Message;
            }

            // Add product data to DB. 

            DataActions actions = new DataActions();

            RFQDocument newDoc = new RFQDocument() { RFQID = Convert.ToInt32(Request.QueryString["RFQID"]), Description = fileDescription.Text, FileName = RFQFile.FileName, FilePath = "/Files/" + newFileName };

            actions.AddRFQDocument(newDoc);
            RFQDocumentList.DataBind();
        }
        else
        {
            // LabelAddStatus.Text = "Unable to accept file type.";
        }
    }

    // Filters the RFQDocumentList based on selected criteria
    public void FilterRFQDocumentList(object sender, EventArgs e)
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

        foreach (GridViewRow Row in RFQDocumentList.Rows)
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

    protected void RFQDocumentList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DownloadFile")
        {
            string[] cmdArgs = Convert.ToString(e.CommandArgument).Split(',');
            string fileName = cmdArgs[0];
            int docID = Convert.ToInt32(cmdArgs[1]);

            DataActions actions = new DataActions();

            Response.AppendHeader("Content-Disposition", "attachment; filename="+fileName);
            Response.TransmitFile(Server.MapPath(actions.GetRFQDocumentByID(docID).FilePath));
            Response.End();
        }
    }
    // Quotes Received Section: ****************************************************************************************************************************************************************
    // Filters the Quotes Received GridView
    public void FilterQuotesReceived(object sender, EventArgs e)
    {
        int cellNumber = 0; // The first cell
        if (FilterQuotesDDL.SelectedValue == "RFQ")
        {
            cellNumber += 0;
        }
        else if (FilterQuotesDDL.SelectedValue == "RFQ Reference")
        {
            cellNumber += 1;
        }
        else if (FilterQuotesDDL.SelectedValue == "Last Updated")
        {
            cellNumber += 2;
        }
        else if (FilterQuotesDDL.SelectedValue == "Quoted By")
        {
            cellNumber += 3;
        }
        else if (FilterQuotesDDL.SelectedValue == "Submitted Date")
        {
            cellNumber += 4;
        }
        else if (FilterQuotesDDL.SelectedValue == "Expired Date")
        {
            cellNumber += 5;
        }
        else if (FilterQuotesDDL.SelectedValue == "Comments")
        {
            cellNumber += 6;
        }
        else if (FilterQuotesDDL.SelectedValue == "Notes")
        {
            cellNumber += 7;
        }
        else if (FilterQuotesDDL.SelectedValue == "Status")
        {
            cellNumber += 8;
        }
        else if (FilterQuotesDDL.SelectedValue == "Status Date")
        {
            cellNumber += 9;
        }

        foreach (GridViewRow Row in QuotesReceived.Rows)
        {
            if (!Row.Cells[cellNumber].Text.Contains(FilterQuotesTB.Text))
            {
                Row.Visible = false;
            }
            else
            {
                Row.Visible = true;
            }
        }
    }

    protected void QuotesReceived_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    // The return type can be changed to IEnumerable, however to support
    // paging and sorting, the following parameters must be added:
    //     int maximumRows
    //     int startRowIndex
    //     out int totalRowCount
    //     string sortByExpression
    public IQueryable<JBXML.Respond.JBXMLJBXMLRespondQuoteListQueryRsQuote> QuotesReceived_GetData()
    {
        DataActions actions = new DataActions();
        RequestForQuote RFQ = actions.GetRFQByID(Convert.ToInt32(Request.QueryString["RFQID"]));
        return actions.getJBQuotes(RFQ.JBReferenceCode).AsQueryable();
    }

    // Purchase Order Section: *********************************************************************************************************************************************************************
    // Get purchase orders
    public IQueryable<MaterialManager.Models.PurchaseOrder> POList_GetData()
    {
        DataActions actions = new DataActions();
        return actions.GetPOList(Convert.ToInt32(Request.QueryString["RFQID"])).AsQueryable();
    }

    // Filters the list of Purchase Orders by any field
    public void FilterPOList(object sender, EventArgs e)
    {
        int cellNumber = 1; // The first cell contains the view button so we start at Cell[1] to avoid the view button at [0]
        if (FilterDDL.SelectedValue == "PO ID")
        {
            cellNumber += 0;
        }
        else if (FilterDDL.SelectedValue == "Vendor")
        {
            cellNumber += 1;
        }
        else if (FilterDDL.SelectedValue == "PO Sent to Vendor")
        {
            cellNumber += 2;
        }
        else if (FilterDDL.SelectedValue == "Required Delivery Date")
        {
            cellNumber += 3;
        }
        else if (FilterDDL.SelectedValue == "Justification")
        {
            cellNumber += 4;
        }
        else if (FilterDDL.SelectedValue == "Review Status")
        {
            cellNumber += 5;
        }
        else if (FilterDDL.SelectedValue == "Approval Status")
        {
            cellNumber += 6;
        }

        foreach (GridViewRow Row in POList.Rows)
        {
            if (!Row.Cells[cellNumber].Text.Contains(FilterTB.Text))
            {
                Row.Visible = false;
            }
            else
            {
                Row.Visible = true;
            }

            if (cellNumber == 6)
            {
                if (FilterTB.Text == "Reviewed")
                {
                    Row.Visible = Row.Cells[cellNumber].Text == "Reviewed";
                }
                else if (FilterTB.Text == "Not Reviewed")
                {
                    Row.Visible = Row.Cells[cellNumber].Text == "Not Reviewed";
                }
            }
            else if (cellNumber == 7)
            {
                if (FilterTB.Text == "Approved")
                {
                    Row.Visible = Row.Cells[cellNumber].Text == "Approved";
                }
                else if (FilterTB.Text == "Not Approved")
                {
                    Row.Visible = Row.Cells[cellNumber].Text == "Not Approved";
                }
            }
        }
    }

    public void POList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    public void POList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    // Export to Excel Section: **********************************************************************************************************************************************************************************************
    
    public void ExcelExport_Click(object sender, EventArgs e)
    {
        int key = ExportToExcel(Convert.ToInt16(ExportOption.SelectedValue));
        String startPath = Server.MapPath(@"..\Files\Export");
        String zipPath = Server.MapPath(@"..\Files\ExportArchives") + @"\RFQ" + key + ".zip";
        ZipFile.CreateFromDirectory(startPath, zipPath);
        ZipArchive archive = ZipFile.Open(Server.MapPath(@"..\Files\ExportArchives") + @"\RFQ" + key + ".zip", ZipArchiveMode.Update);
        archive.CreateEntryFromFile(Server.MapPath(@"..\Files") + @"\RFQExport" + key + ".xls", "RFQ.xls");

        IOrderedDictionary rowValues = new OrderedDictionary();
        DataActions actions = new DataActions();
        int docID;

        foreach (GridViewRow Row in ExportRFQDocumentList.Rows)
        {
            rowValues = GetValues(Row);

            if (((CheckBox)Row.FindControl("DownloadCB")).Checked)
            {
                docID = Convert.ToInt32(rowValues["RFQDocID"]);
                RFQDocument doc = actions.GetRFQDocumentByID(docID);
                if (doc != null)
                {
                    String temp = Server.MapPath("/" + doc.FilePath); //Server.MapPath(doc.FilePath);
                    archive.CreateEntryFromFile(temp, doc.FileName);
                }
            }
        }

        archive.Dispose();

        File.Delete(Server.MapPath(@"../Files/") + @"RFQExport" + key + ".xls");

        Response.Clear();
        Response.AppendHeader("Content-Disposition", "attachment; filename=RFQ" + Request.QueryString["RFQID"]);
        Response.ContentType = "application/x-zip-compressed";
        Response.WriteFile(zipPath);
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

        if(orientation == 1) // Vertical Alignment
        {
            // RFQ Details
            xlWorkSheet.Cells[rowShift + 1, colShift + 1] = "RFQ Details";
            rowShift++;
            rowShift += RFQDetails.Rows.Count;
            colShift += RFQDetails.Rows[0].Cells.Count;
            for (int i = 0; i < RFQDetails.Rows.Count; i++)
            {
                for (int j = 0; j < RFQDetails.Rows[i].Cells.Count; j++)
                {
                    cellText = RFQDetails.Rows[i].Cells[j].Text;
                    xlWorkSheet.Cells[i + 1 + rowStart, j + colStart] = cellText == "&nbsp;" ? "" : cellText;
                }
            }
            xlWorkSheet.Range[xlWorkSheet.Cells[rowStart, colStart], xlWorkSheet.Cells[rowStart + rowShift - 1, colStart + colShift - 1]].AutoFormat();
            rowStart += rowShift + 1;
            rowShift = 0;
            colShift = 0;


            // RFQ Line Items
            xlWorkSheet.Cells[rowStart, colStart] = "Line Items";
            rowShift++;
            rowShift += RFQLineItemList.Rows.Count + 1; // + 1 for header row
            colShift += RFQLineItemList.Columns.Count - 4; // - 4 for unused columns
            int partIdIndex = -1;

            for (int j = 0; j < RFQLineItemList.Columns.Count; j++)
            {
                if (RFQLineItemList.Columns[j].HeaderText == "Part ID")
                {
                    partIdIndex = j;
                    break;
                }
            }

            for (int j = 1; j < RFQLineItemList.Columns.Count; j++)
            {
                if (j < partIdIndex)
                {
                    xlWorkSheet.Cells[rowStart + 1, j + colStart - 1] = RFQLineItemList.Columns[j].HeaderText;
                }
                else if (j > partIdIndex)
                {
                    xlWorkSheet.Cells[rowStart + 1, j + colStart - 2 > 0 ? j + colStart - 2 : 1] = RFQLineItemList.Columns[j].HeaderText;
                }
            }
            partIdIndex++; // For some reason, there is one more column in Columns than there are cells in rows
            for (int i = 0; i < RFQLineItemList.Rows.Count; i++)
            {
                for (int j = 2; j < RFQLineItemList.Columns.Count; j++) // j = 2 because data doesn't start until 2
                {
                    cellText = RFQLineItemList.Rows[i].Cells[j].Text;
                    if (j < partIdIndex)
                    {
                        xlWorkSheet.Cells[i + 2 + rowStart, j + colStart - 2] = cellText == "&nbsp;" ? "" : cellText; // j - 2 since j starts at 2
                    }
                    else if (j > partIdIndex)
                    {
                        xlWorkSheet.Cells[i + 2 + rowStart, j + colStart - 3] = cellText == "&nbsp;" ? "" : cellText; // j - 3 since PartID is skipped
                    }
                }
            }
            xlWorkSheet.Range[xlWorkSheet.Cells[rowStart, colStart], xlWorkSheet.Cells[rowStart + rowShift - 1, colStart + colShift]].AutoFormat();
            rowStart += rowShift + 1;
            rowShift = 0;
            colShift = 0;

            // Quotes Received
            xlWorkSheet.Cells[rowStart, colStart] = "Quotes Received";
            rowShift++;
            rowShift += QuotesReceived.Rows.Count + 1; // + 1 for header row
            colShift += QuotesReceived.Columns.Count;
            
            for (int j = 0; j < QuotesReceived.Columns.Count; j++) // Header Row
            {
                xlWorkSheet.Cells[rowStart + 1, j + colStart] = QuotesReceived.Columns[j].HeaderText;
            }

            for (int i = 0; i < QuotesReceived.Rows.Count; i++)
            {
                for (int j = 0; j < QuotesReceived.Columns.Count; j++)
                {
                    cellText = QuotesReceived.Rows[i].Cells[j].Text;
                    xlWorkSheet.Cells[i + rowStart + 2, j + colStart] = cellText == "&nbsp;" ? "" : cellText;
                }
            }
            xlWorkSheet.Range[xlWorkSheet.Cells[rowStart, colStart], xlWorkSheet.Cells[rowStart + rowShift - 1, colStart + colShift - 1]].AutoFormat();
            rowStart += rowShift + 1;
            rowShift = 0;
            colShift = 0;

            // Purchase Orders
            xlWorkSheet.Cells[rowStart, colStart] = "Purchase Orders";
            rowShift++;
            rowShift += POList.Rows.Count + 1; // + 1 for header row
            colShift += POList.Columns.Count - 1; // - 1 for view button

            for (int j = 1; j < POList.Columns.Count; j++)
            {
                xlWorkSheet.Cells[rowStart + 1, j + colStart - 1] = POList.Columns[j].HeaderText; // j - 1 for the empty view button column header
            }

            for (int i = 0; i < POList.Rows.Count; i++)
            {
                for (int j = 1; j < POList.Columns.Count; j++)
                {
                    cellText = POList.Rows[i].Cells[j].Text;
                    xlWorkSheet.Cells[i + rowStart + 2, j + colStart - 1] = cellText == "&nbsp;" ? "" : cellText; // j - 1 for the view button column
                }
            }
            xlWorkSheet.Range[xlWorkSheet.Cells[rowStart, colStart], xlWorkSheet.Cells[rowStart + rowShift - 1, colStart + colShift - 1]].AutoFormat();
            rowStart += rowShift + 1;
            rowShift = 0;
            colShift = 0;
        }
        else // Horizontal Orientation
        {
            // RFQ Details
            xlWorkSheet.Cells[rowShift + 1, colShift + 1] = "RFQ Details";
            rowShift++;
            rowShift += RFQDetails.Rows.Count;
            colShift += RFQDetails.Rows[0].Cells.Count;
            for (int i = 0; i < RFQDetails.Rows.Count; i++)
            {
                for (int j = 0; j < RFQDetails.Rows[i].Cells.Count; j++)
                {
                    cellText = RFQDetails.Rows[i].Cells[j].Text;
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
            rowShift += RFQLineItemList.Rows.Count + 1; // + 1 for header row
            colShift += RFQLineItemList.Columns.Count - 3; // - 4 for unused columns
            int partIdIndex = -1;

            for (int j = 0; j < RFQLineItemList.Columns.Count; j++)
            {
                if (RFQLineItemList.Columns[j].HeaderText == "Part ID")
                {
                    partIdIndex = j;
                    break;
                }
            }

            for (int j = 1; j < RFQLineItemList.Columns.Count; j++)
            {
                if (j < partIdIndex)
                {
                    xlWorkSheet.Cells[rowStart + 1, j + colStart - 1] = RFQLineItemList.Columns[j].HeaderText;
                }
                else if (j > partIdIndex)
                {
                    xlWorkSheet.Cells[rowStart + 1, j + colStart - 2 > 0 ? j + colStart - 2 : 1] = RFQLineItemList.Columns[j].HeaderText;
                }
            }
            partIdIndex++;
            for (int i = 0; i < RFQLineItemList.Rows.Count; i++)
            {
                for (int j = 2; j < RFQLineItemList.Columns.Count; j++) // j = 2 because data doesn't start until 2
                {
                    cellText = RFQLineItemList.Rows[i].Cells[j].Text;
                    if (j < partIdIndex)
                    {
                        xlWorkSheet.Cells[i + 2 + rowStart, j + colStart - 2] = cellText == "&nbsp;" ? "" : cellText; // j - 2 since j starts at 2
                    }
                    else if (j > partIdIndex)
                    {
                        xlWorkSheet.Cells[i + 2 + rowStart, j + colStart - 3] = cellText == "&nbsp;" ? "" : cellText; // j - 3 since PartID is skipped
                    }
                }
            }
            xlWorkSheet.Range[xlWorkSheet.Cells[rowStart, colStart], xlWorkSheet.Cells[rowStart + rowShift - 1, colStart + colShift - 1]].AutoFormat();
            colStart += colShift + 1;
            rowShift = 0;
            colShift = 0;

            // Quotes Received
            xlWorkSheet.Cells[rowStart, colStart] = "Quotes Received";
            rowShift++;
            rowShift += QuotesReceived.Rows.Count + 1; // + 1 for header row
            colShift += QuotesReceived.Columns.Count;

            for (int j = 0; j < QuotesReceived.Columns.Count; j++) // Header Row
            {
                xlWorkSheet.Cells[rowStart + 1, j + colStart] = QuotesReceived.Columns[j].HeaderText;
            }

            for (int i = 0; i < QuotesReceived.Rows.Count; i++)
            {
                for (int j = 0; j < QuotesReceived.Columns.Count; j++)
                {
                    cellText = QuotesReceived.Rows[i].Cells[j].Text;
                    xlWorkSheet.Cells[i + rowStart + 2, j + colStart] = cellText == "&nbsp;" ? "" : cellText;
                }
            }
            xlWorkSheet.Range[xlWorkSheet.Cells[rowStart, colStart], xlWorkSheet.Cells[rowStart + rowShift - 1, colStart + colShift - 1]].AutoFormat();
            colStart += colShift + 1;
            rowShift = 0;
            colShift = 0;

            // Purchase Orders
            xlWorkSheet.Cells[rowStart, colStart] = "Purchase Orders";
            rowShift++;
            rowShift += POList.Rows.Count + 1; // + 1 for header row
            colShift += POList.Columns.Count - 1; // - 1 for view button

            for (int j = 1; j < POList.Columns.Count; j++)
            {
                xlWorkSheet.Cells[rowStart + 1, j + colStart - 1] = POList.Columns[j].HeaderText; // j - 1 for the empty view button column header
            }

            for (int i = 0; i < POList.Rows.Count; i++)
            {
                for (int j = 1; j < POList.Columns.Count; j++)
                {
                    cellText = POList.Rows[i].Cells[j].Text;
                    xlWorkSheet.Cells[i + rowStart + 2, j + colStart - 1] = cellText == "&nbsp;" ? "" : cellText; // j - 1 for the view button column
                }
            }
            xlWorkSheet.Range[xlWorkSheet.Cells[rowStart, colStart], xlWorkSheet.Cells[rowStart + rowShift - 1, colStart + colShift - 1]].AutoFormat();
            colStart += colShift + 1;
            rowShift = 0;
            colShift = 0;
        }

        Random rng = new Random();
        int key = rng.Next(999999);
        xlWorkBook.SaveAs(Server.MapPath(@"..\Files\") + "RFQExport" + key +".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
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

    private void PrepareForExportGridView(GridView gv)
    {
        // Change the Header Row to white
        gv.HeaderRow.Style.Add("background-color", "#FFFFFF");

        // Apply style to Individual Cells
        for (int k = 0; k < gv.HeaderRow.Cells.Count; k++)
        {
            gv.HeaderRow.Cells[k].Style.Add("background-color", "#5DADE2");
        }

        for (int i = 0; i < gv.Rows.Count; i++)
        {
            GridViewRow row = gv.Rows[i];

            // Change color back to white
            row.BackColor = System.Drawing.Color.White;

            // Apply text style to each Row
            row.Attributes.Add("class", "textmode");

            // Apply style to individual cells of alternating row
            if (i % 2 != 0)
            {
                for (int j = 0; j < gv.Rows[i].Cells.Count; j++)
                {
                    row.Cells[j].Style.Add("background-color", "#ADD8E6");
                }
            }
        }

        foreach (GridViewRow row in gv.Rows)
        {
            row.Cells[0].Visible = false;
            row.Cells[row.Cells.Count - 1].Visible = false;
        }

        GridViewRow Row = gv.HeaderRow;
        Row.Cells[0].Visible = false;
        Row.Cells[Row.Cells.Count - 1].Visible = false;
        DisableControls(gv);
    }

    private void PrepareForExportDetailsView(DetailsView dv)
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

        //dv.Rows[dv.Rows.Count - 1].Visible = false;
    }

    private void DisableControls(GridView gv)
    {
        for (int i = 0; i < gv.HeaderRow.Cells.Count; i++)
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
        Response.Redirect("AddPartsToRFQ.aspx?RFQID=" + Convert.ToInt32(Request.QueryString["RFQID"]));
    }

    protected void RFQLineItemList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!User.IsInRole("Creation"))
        {
            e.Row.Cells[0].Visible = false;
        }
    }

    protected void RFQDocumentList_RowDataBound(object sender, GridViewRowEventArgs e)
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