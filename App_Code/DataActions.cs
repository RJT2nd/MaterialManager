using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MaterialManager.Models;
using JBInterface;
using System.Xml.Serialization;
using Microsoft.AspNet.Identity;
using MaterialManager;
using System.IO;
using System.Xml;
using JBXML.Respond;

/// <summary>
/// Summary description for DataActions
/// </summary>
namespace MaterialManager.Logic
{
    public class DataActions : IDisposable
    {
        private ProductContext db = new ProductContext();
        private static string ProjectSessionKey = "CurrentProjectID";

        private int ProjectID { get; set; }

        public DataActions()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //public string GetUserRole(string id)
        //{
        //    var _db = new MaterialManager.ApplicationDbContext();
        //    return HttpContext.Current.User.Identity.GetUserId().;
        //}

        public List<PurchaseOrder> GetPOList()
        {
            int projID = GetCurrentProject();
            return db.PurchaseOrders.Where(p => p.ProjectID == projID).ToList();
        }

        public List<PurchaseOrder> GetPOList(int rfqID)
        {
            int projID = GetCurrentProject();
            return db.PurchaseOrders.Where(p => p.ProjectID == projID && p.RFQID == rfqID).ToList();
        }

        public RequestForQuote GetRFQByID(int rfqID)
        {
            return db.RequestForQuotes.Find(rfqID);
        }

        // Part Actions: ********************************************************************************************************************************************************************************

        public int AddPart(Part newPart)
        {
            db.Parts.Add(newPart);

            db.SaveChanges();

            return newPart.PartID;
        }

        // RFQLineItem Actions: ******************************************************************************************************************************************************************

        public void AddPartToRFQ(Part newPart, int currentRFQID, int quantity)
        {
            int partID = AddPart(newPart);

            db.RFQLineItems.Add(new RFQLineItem()
            {
                RFQID = currentRFQID,
                PartID = partID,
                Quantity = quantity,
            });

            db.SaveChanges();
        }

        public void AddPartToRFQ(int currentRFQID, string jbMaterialID, int quantity)
        {
            if (db.Parts.Where(p => p.JBMaterialID == jbMaterialID).Count() > 0)
            {
                Part newPart = db.Parts.Where(p => p.JBMaterialID == jbMaterialID).FirstOrDefault();
                db.Parts.Add(newPart);
                db.SaveChanges();
                if (db.RFQLineItems.Where(p => p.PartID == newPart.PartID && p.RFQID == currentRFQID).Count() == 0)
                {
                    db.RFQLineItems.Add(new RFQLineItem { PartID = newPart.PartID, RFQID = currentRFQID, Quantity = quantity });
                    db.SaveChanges();
                }
            }
            else
            {
                Part newPart = db.Parts.Add(new Part { PartNumber = jbMaterialID, JBMaterialID = jbMaterialID, NSN = "0000-00-000-0000", Description = "Default description text." });
                db.SaveChanges();
                db.RFQLineItems.Add(new RFQLineItem { PartID = newPart.PartID, RFQID = currentRFQID, Quantity = quantity });
                db.SaveChanges();
            }
        }

        // POLineItem Actions: ********************************************************************************************************************************************************
        public List<POLineItem> GetPOLineItems(int POID)
        {
            return db.POLineItems.Where(p => p.POID == POID).ToList();
        }

        public void DeletePOLineItem(int itemID)
        {
            if (GetPOLineItemByID(itemID) != null)
            {
                db.POLineItems.Remove(GetPOLineItemByID(itemID));
                db.SaveChanges();
            }
        }

        public void AddPartToPO(Part newPart, int currentPOID, int quantity, decimal price)
        {
            int partID = AddPart(newPart);

            db.POLineItems.Add(new POLineItem()
            {
                POID = currentPOID,
                PartID = partID,
                Quantity = quantity,
                Price = price
            });

            db.SaveChanges();
        }

        public void AddPartToPO(int currentPOID, string jbMaterialID, int quantity, decimal price)
        {
            if (db.Parts.Where(p => p.JBMaterialID == jbMaterialID).Count() > 0)
            {
                Part newPart = db.Parts.Where(p => p.JBMaterialID == jbMaterialID).FirstOrDefault();
                db.Parts.Add(newPart);
                db.SaveChanges();
                if (db.POLineItems.Where(p => p.PartID == newPart.PartID && p.POID == currentPOID).Count() == 0)
                {
                    db.POLineItems.Add(new POLineItem { PartID = newPart.PartID, POID = currentPOID, Quantity = quantity, Price = price });
                    db.SaveChanges();
                }
            }
            else
            {
                Part newPart = db.Parts.Add(new Part { PartNumber = jbMaterialID, JBMaterialID = jbMaterialID, NSN = "0000-00-000-0000", Description = "Default description text." });
                db.SaveChanges();
                db.POLineItems.Add(new POLineItem { PartID = newPart.PartID, POID = currentPOID, Quantity = quantity, Price = price });
                db.SaveChanges();
            }
        }

        public List<JBXMLJBXMLRespondMaterialListQueryRsMaterial> getJBMaterialListFilteredByVendor(int POID)
        {
            //try
            //{
            //    int partID = db.POLineItems.Where(lineItem => lineItem.POID == POID).ToList().FirstOrDefault().PartID;

            //    // Interesting note: MaterialListQueryRsMaterial's Material is equivalent to MaterialQueryRs's ID
            //    string materialID = getJBMaterialDetails(partID).ID;
            //    JBXMLJBXMLRespondMaterialListQueryRsMaterial material = getJBMaterialList().Where(item => item.Material == materialID).FirstOrDefault();
            //    return getJBMaterialList().Where(item => item.Primary_Vendor == material.Primary_Vendor).ToList();
            //}
            //catch
            //{
            return getJBMaterialList();
            //}
        }

        public POLineItem GetPOLineItemByID(int POID)
        {
            return db.POLineItems.Find(POID);
        }

        public decimal GetExpendedFunds()
        {
            List<PurchaseOrder> poList = GetPOList();

            List<POLineItem> lineItems = new List<POLineItem>();

            foreach(PurchaseOrder po in poList)
            {
                lineItems.AddRange(GetPOLineItems(po.PurchaseOrderID));
            }

            return lineItems.Sum(l => l.Price);
        }

        public decimal GetPOTotal(int POID)
        {
            return GetPOLineItems(POID).Sum(l => l.TotalPrice);
        }

        // Project Sections: ************************************************************************************************************************************************************************

        public List<Project> GetProjectList()
        {

            return db.Projects.ToList();
        }

        public Project GetProjectByID(int projID)
        {
            return db.Projects.Where(proj => proj.ProjectID == projID).FirstOrDefault();
        }

        public void UpdateProject(Dictionary<string, dynamic> UpdatedInfo)
        {
            Project project = db.Projects.Find(Convert.ToInt32(UpdatedInfo["ProjectID"]));
            project.ProjectName = UpdatedInfo["ProjectName"];
            project.ContractNumber = UpdatedInfo["ContractNumber"];
            project.FundsAllocated = UpdatedInfo["FundsAllocated"];
            db.SaveChanges();
        }

        public void DeleteProject(int ProjectID)
        {
            Project ProjectToDelete = db.Projects.Find(ProjectID);
            db.Projects.Remove(ProjectToDelete);
            db.SaveChanges();
        }

        public void AddProject(Dictionary<string, dynamic> inputs)
        {
            db.Projects.Add(new Project
            {
                ProjectName = inputs["ProjectName"],
                ContractNumber = inputs["ContractNumber"],
                FundsAllocated = inputs["FundsAllocated"]
            });

            db.SaveChanges();
        }

        public List<RequestForQuote> GetRFQList()
        {
            int projID = GetCurrentProject();
            return db.RequestForQuotes.Where(r => r.ProjectID == projID).ToList();
        }

        public List<RFQLineItem> GetRFQLineItemsList(int rfqID)
        {

            return db.RFQLineItems.Where(r => r.RFQID == rfqID).ToList();
        }

        public List<POLineItem> GetPOLineItemsList(int poID)
        { 
            return db.POLineItems.Where(p => p.POID == poID).ToList();
        }

        // Purchase Order Actions: ****************************************************************************************************************************************************************
        // Updates the purchase order
        public void UpdatePurchaseOrder(Dictionary<int, dynamic> purchaseOrderValues)
        {
            // 0. Purchase Order ID
            // 1. POVendor
            // 2. Vendor Date
            // 3. Required Delivery Date
            // 4. Expected Delivery Date
            // 5. Actual Delivery Date
            // 6. Delivery Address
            // 7. Mark For
            // 8. Justification
            PurchaseOrder po = db.PurchaseOrders.Find(Convert.ToInt32(purchaseOrderValues[0]));
            po.POVendor = purchaseOrderValues[1];
            po.POtoVendorDate = purchaseOrderValues[2];
            po.RequiredDeliveryDate = purchaseOrderValues[3];
            po.ExpectedDeliveryDate = purchaseOrderValues[4];
            po.ActualDeliveryDate = purchaseOrderValues[5];
            po.DeliveryAddress = purchaseOrderValues[6];
            po.MarkFor = purchaseOrderValues[7];
            po.Justification = purchaseOrderValues[8];
            db.SaveChanges();
        }

        public int AddPO(int rfqID)
        {
            PurchaseOrder po = new PurchaseOrder { RFQID = rfqID, ProjectID = GetCurrentProject(), ReviewStatus = "Not Reviewed", ApprovalStatus = "Not Approved" };
            db.PurchaseOrders.Add(po);
            db.SaveChanges();
            return po.PurchaseOrderID;
        }

        public int AddPO()
        {
            PurchaseOrder po = new PurchaseOrder { ProjectID = GetCurrentProject(), ReviewStatus = "Not Reviewed", ApprovalStatus = "Not Approved" };
            db.PurchaseOrders.Add(po);
            db.SaveChanges();
            return po.PurchaseOrderID;
        }

        public PurchaseOrder GetPOByID(int poID)
        {
            return db.PurchaseOrders.Where(po => po.PurchaseOrderID == poID).FirstOrDefault();
        }

        // Review and Approval of PO Sections
        public void UndoPOReview(int poID)
        {
            PurchaseOrder PO = db.PurchaseOrders.Where(po => po.PurchaseOrderID == poID).FirstOrDefault();
            PO.ReviewStatus = "Not Reviewed";
            db.SaveChanges();
        }

        public void UndoPOApproval(int poID)
        {
            PurchaseOrder PO = db.PurchaseOrders.Where(po => po.PurchaseOrderID == poID).FirstOrDefault();
            PO.ApprovalStatus = "Not Approved";
            db.SaveChanges();
        }

        public void ReviewPO(int poID)
        {
            PurchaseOrder PO = db.PurchaseOrders.Where(po => po.PurchaseOrderID == poID).FirstOrDefault();
            PO.ReviewStatus = "Reviewed";
            db.SaveChanges();
        }

        public void ApprovePO(int poID)
        {
            PurchaseOrder PO = db.PurchaseOrders.Where(po => po.PurchaseOrderID == poID).FirstOrDefault();
            PO.ApprovalStatus = "Approved";
            db.SaveChanges();
        }

        public List<MaterialManager.Models.PODocument> GetPODocumentListByID(int POID)
        {
            return db.PODocuments.Where(doc => doc.POID == POID).ToList();
        }

        public PODocument GetPODocumentByID(int PODocID)
        {
            return db.PODocuments.Where(d => d.PODocID == PODocID).FirstOrDefault();
        }

        public List<ExtendedLineItem> GetExtendedLineItemsList(int rfqID)
        {
            List<RFQLineItem> lineItems = GetRFQLineItemsList(rfqID);
            List<ExtendedLineItem> extendedLineItems = new List<ExtendedLineItem>();
            
            foreach(RFQLineItem lineItem in lineItems)
            {
                extendedLineItems.Add(new ExtendedLineItem(lineItem, getPart(lineItem.PartID)));
            }

            return extendedLineItems;
        }

        public void AddPODocument(PODocument doc)
        {
            db.PODocuments.Add(doc);
            db.SaveChanges();
        }

        public void DeletePODocument(int PODocID)
        {
            if (GetPODocumentByID(PODocID) != null)
            {
                PODocument doc = db.PODocuments.Where(d => d.PODocID == PODocID).FirstOrDefault();
                db.PODocuments.Remove(doc);
                db.SaveChanges();
            }
        }

        public List<ExtendedPOLineItem> GetExtendedPOLineItemsList(int poID)
        {

            List<POLineItem> lineItems = GetPOLineItemsList(poID);
            List<ExtendedPOLineItem> extendedLineItems = new List<ExtendedPOLineItem>();

            foreach (POLineItem lineItem in lineItems)
            {
                extendedLineItems.Add(new ExtendedPOLineItem(lineItem, getPart(lineItem.PartID)));
            }

            return extendedLineItems;
        }

        // RFQLineItem Actions: ******************************************************************************************************************************************************************
        public void DeleteRFQLineItem(int itemID)
        {
            if (GetRFQLineItemByID(itemID) != null)
            {
                db.RFQLineItems.Remove(GetRFQLineItemByID(itemID));
                db.SaveChanges();
            }
        }

        public RFQLineItem GetRFQLineItemByID(int itemID)
        {
            return db.RFQLineItems.Find(itemID);
        }

        public RFQLineItem GetRFQLineItem(int itemID)
        {
            return db.RFQLineItems.Find(itemID);
        }

        // RFQDocument Actions: ******************************************************************************************************************************************************************
        // GetFRQDocumentList is called by RFQView 
        public List<RFQDocument> GetRFQDocumentList(int rfqID)
        {
            return db.RFQDocuments.Where(r => r.RFQID == rfqID).ToList();
        }

        // AddRFQDocument is called by RFQView when 'Add Document' is clicked
        // AddFRQDocument will add a document and save it into the MaterialsManager Database in the RFQDocuments Table
        public void AddRFQDocument(RFQDocument doc)
        {
            db.RFQDocuments.Add(doc);
            db.SaveChanges();
        }

        // DeleteRFQDocument is called by RFQView when 'delete' is clicked
        // DeleteRFQDocument will delete a document and save the changes into the database
        public void DeleteRFQDocument(int id)
        {
            if (GetRFQDocumentByID(id) != null)
            {
                db.RFQDocuments.Remove(GetRFQDocumentByID(id));
                db.SaveChanges();
            }
        }

        public RFQDocument GetRFQDocumentByID(int docID)
        {
            return db.RFQDocuments.Find(docID);
            
        }

        // Kits: ****************************************************************************************************************************************************************
        // Called by KitList.aspx to bring a list of kits tied to the project
        public List<ProjectKit> GetKitList()
        {
            int projID = GetCurrentProject();
            return db.ProjectKits.Where(k => k.ProjectID == projID).ToList();
        }

        public void DeleteProjectKitItem(int projectKitItemID)
        {
            ProjectKitItem kitItem = db.ProjectKitItems.Where(k => k.ProjectKitItemID == projectKitItemID).FirstOrDefault();
            if (kitItem != null)
            {
                db.ProjectKitItems.Remove(kitItem);
                db.SaveChanges();
            }
        }

        public ProjectKitItem GetProjectKitItemByID(int projectKitItemID)
        {
            return db.ProjectKitItems.Where(k => k.ProjectKitItemID == projectKitItemID).FirstOrDefault();
        }

        public ProjectKit GetProjectKitByID(int projectKitID)
        {
            return db.ProjectKits.Where(k => k.ProjectKitID == projectKitID).FirstOrDefault();
        }
        
        public void DeleteProjectKit(int projectKitID)
        {
            // Deletes all of the ProjectKit's Items
            IEnumerable<ProjectKitItem> list = db.ProjectKitItems.Where(item => item.ProjectKitID == projectKitID);
            db.ProjectKitItems.RemoveRange(list);

            // Delete Project Kit
            ProjectKit kitToRemove = db.ProjectKits.Where(k => k.ProjectKitID == projectKitID).FirstOrDefault();
            db.ProjectKits.Remove(kitToRemove);
            db.SaveChanges();
        }

        public void AddPartToProjectKit(int currentProjectKitID, string jbMaterialID, int quantity)
        {
            if (db.Parts.Where(p => p.JBMaterialID == jbMaterialID).Count() > 0)
            {
                Part newPart = db.Parts.Where(p => p.JBMaterialID == jbMaterialID).FirstOrDefault();
                db.Parts.Add(newPart);
                db.SaveChanges();
                if (db.ProjectKitItems.Where(p => p.PartID == newPart.PartID && p.ProjectKitID == currentProjectKitID).Count() == 0)
                {
                    db.ProjectKitItems.Add(new ProjectKitItem { PartID = newPart.PartID, ProjectKitID = currentProjectKitID, Quantity = quantity });
                    db.SaveChanges();
                }
            }
            else
            {
                Part newPart = db.Parts.Add(new Part { PartNumber = jbMaterialID, JBMaterialID = jbMaterialID, NSN = "0000-00-000-0000", Description = "Default description text." });
                db.SaveChanges();
                db.ProjectKitItems.Add(new ProjectKitItem { PartID = newPart.PartID, ProjectKitID = currentProjectKitID, Quantity = quantity });
                db.SaveChanges();
            }
        }

        public int AddProjectKit(string projectKitDescription)
        {
            ProjectKit kit = new ProjectKit { ProjectID = GetCurrentProject(), Description = projectKitDescription };
            db.ProjectKits.Add(kit);
            db.SaveChanges();
            return kit.ProjectKitID;
        }

        public List<ProjectKitItem> GetKitItemList(int projectKitID)
        {
            return db.ProjectKitItems.Where(i => i.ProjectKitID == projectKitID).ToList();
        }

        public List<ExtendedProjectKitItem> GetExtendedProjectKitItemsList(int projectKitID)
        {

            List<ProjectKitItem> kitItems = GetKitItemList(projectKitID);
            List<ExtendedProjectKitItem> extendedKitItems = new List<ExtendedProjectKitItem>();

            foreach (ProjectKitItem kitItem in kitItems)
            {
                extendedKitItems.Add(new ExtendedProjectKitItem(kitItem, getPart(kitItem.PartID)));
            }

            return extendedKitItems;
        }

        public List<Part> GetPartsCatalogList()
        {
            ProjectID = GetCurrentProject();
            List<Part> partsList = new List<Part>();

            List<PartsCatalog> catalog = db.PartsCatalogs.Where(p => p.ProjectID == ProjectID).ToList();
            
            foreach(PartsCatalog pc in catalog)
            {
                partsList.Add(db.Parts.Find(pc.PartID));
            }

            return partsList;
        }

        public List<Part> GetPartsNotInCatalogList()
        {
            ProjectID = GetCurrentProject();
            List<Part> partsList = new List<Part>();

            List<PartsCatalog> catalog = db.PartsCatalogs.Where(p => p.ProjectID == ProjectID).ToList();

            partsList.AddRange(db.Parts);

            foreach (PartsCatalog pc in catalog)
            {

                partsList.Remove(partsList.Find(p => p.PartID == pc.PartID));
            }

            return partsList;
        }

        public void AddPartToCatalog(int newPartId)
        {
            db.PartsCatalogs.Add(new PartsCatalog { PartID = newPartId, ProjectID = GetCurrentProject() });
            db.SaveChanges();
        }

        // RFQ Actions: ******************************************************************************************************************************************************************
        // Calls for the required DateTimes to create the RFQ and returns the RFQID so that it can be used to link to the next step of the RFQ creation.
        public int AddRFQ(DateTime rfqDT, DateTime vendorDT)
        {
            RequestForQuote rfq = new RequestForQuote { RFQDate = rfqDT, RFQtoVendorDate = vendorDT, ProjectID = GetCurrentProject(), ReviewStatus = "Not Reviewed", ApprovalStatus = "Not Approved" };
            db.RequestForQuotes.Add(rfq);
            db.SaveChanges();
            return rfq.RFQID;
        }

        public void DeleteRFQ(int rfqID)
        {
            // Deletes RFQ's line items
            IEnumerable<RFQLineItem> lineItemsToDelete = db.RFQLineItems.Where(item => item.RFQID == rfqID);
            if (lineItemsToDelete.Count() != 0)
            {
                db.RFQLineItems.RemoveRange(lineItemsToDelete);
            }

            // Deletes RFQ's attached documents
            IEnumerable<RFQDocument> documentsToDelete = db.RFQDocuments.Where(doc => doc.RFQID == rfqID);
            if (documentsToDelete.Count() != 0)
            {
                db.RFQDocuments.RemoveRange(documentsToDelete);
            }

            // Deletes the RFQ
            RequestForQuote rfqToDelete = db.RequestForQuotes.Where(rfq => rfq.RFQID == rfqID).FirstOrDefault();
            if (rfqToDelete != null)
            {
                db.RequestForQuotes.Remove(rfqToDelete);
            }

            db.SaveChanges();
        }

        // Review and Approval of RFQ Sections
        public void UndoRFQReview(int rfqID)
        {
            RequestForQuote RFQ = db.RequestForQuotes.Where(rfq => rfq.RFQID == rfqID).FirstOrDefault();
            RFQ.ReviewStatus = "Not Reviewed";
            db.SaveChanges();
        }

        public void UndoRFQApproval(int rfqID)
        {
            RequestForQuote RFQ = db.RequestForQuotes.Where(rfq => rfq.RFQID == rfqID).FirstOrDefault();
            RFQ.ApprovalStatus = "Not Approved";
            db.SaveChanges();
        }

        public void ReviewRFQ(int rfqID)
        {
            RequestForQuote RFQ = db.RequestForQuotes.Where(rfq => rfq.RFQID == rfqID).FirstOrDefault();
            RFQ.ReviewStatus = "Reviewed";
            db.SaveChanges();
        }

        public void ApproveRFQ(int rfqID)
        {
            RequestForQuote RFQ = db.RequestForQuotes.Where(rfq => rfq.RFQID == rfqID).FirstOrDefault();
            RFQ.ApprovalStatus = "Approved";
            db.SaveChanges();
        }

        public void AddJBPartToCatalog(string jbMaterialID)
        {


            if (db.Parts.Where(p => p.JBMaterialID == jbMaterialID).Count() > 0)
            {
                Part newPart = db.Parts.Where(p => p.JBMaterialID == jbMaterialID).FirstOrDefault();
                int projectID = GetCurrentProject();
                if (db.PartsCatalogs.Where(p => p.PartID == newPart.PartID && p.ProjectID == projectID).Count() == 0)
                { 
                    db.PartsCatalogs.Add(new PartsCatalog { PartID = newPart.PartID, ProjectID = projectID });
                    db.SaveChanges();
                }
            }
            else
            {
                Part newPart = db.Parts.Add(new Part { PartNumber = jbMaterialID, JBMaterialID = jbMaterialID, NSN = "0000-00-000-0000", Description="Default description text." });
                db.SaveChanges();
                db.PartsCatalogs.Add(new PartsCatalog { PartID = newPart.PartID, ProjectID = GetCurrentProject() });
                db.SaveChanges();
            }

        }

        public void DeletePartsCatalogItem(int id)
        {

            int projID = GetCurrentProject();
                PartsCatalog deletePart = db.PartsCatalogs.Where(p => p.PartID == id && p.ProjectID == projID).FirstOrDefault();
                db.PartsCatalogs.Remove(deletePart);
                db.SaveChanges();
           
            
        }

        public Part getPart(int id)
        {
            return db.Parts.Find(id);
        }

        public List<JBXML.Respond.JBXMLJBXMLRespondQuoteListQueryRsQuote> getJBQuotes(string rfqReferenceID)
        {
            UserManager manager = new UserManager();
            var user = manager.FindById(HttpContext.Current.User.Identity.GetUserId());

            JBXML.Request.JBXML jbx = new JBXML.Request.JBXML();
            JBXML.Request.JBXMLJBXMLRequest request = new JBXML.Request.JBXMLJBXMLRequest();

            JBXML.Request.QueryFilterType filter = new JBXML.Request.QueryFilterType();
            filter.NameFilter = "%";


            JBXML.Request.JBXMLJBXMLRequestQuoteListQueryRq materialRequest = new JBXML.Request.JBXMLJBXMLRequestQuoteListQueryRq();
            materialRequest.QueryFilter = filter;
            JBXML.Request.JBXMLJBXMLRequestQuoteListQueryRq[] outerMaterialRequest = { materialRequest };

            JBRequestProcessor processor = new JBRequestProcessor();

            string err = "";
            string session = processor.CreateSession(user.JBUser, user.JBPassword, ref err);

            request.QuoteListQueryRq = outerMaterialRequest;
            request.Session = session;
            jbx.Item = request;

            XmlSerializer serializer = new XmlSerializer(typeof(JBXML.Request.JBXML));

            StringWriter sw = new StringWriter();

            serializer.Serialize(sw, jbx);

            string serializedRequest = sw.ToString();

            string serializedResponse = processor.ProcessRequest(serializedRequest);
            processor.CloseSession(session);

            XmlReader reader = XmlReader.Create(new StringReader(serializedResponse));

            XmlSerializer deserializer = new XmlSerializer(typeof(JBXML.Respond.JBXML));

            JBXML.Respond.JBXML response = (JBXML.Respond.JBXML)deserializer.Deserialize(reader);
            
            List<JBXML.Respond.JBXMLJBXMLRespondQuoteListQueryRsQuote> quoteList = response.JBXMLRespond.QuoteListQueryRs[0].Quote.ToList();

            quoteList.RemoveAll(q => q.Reference != rfqReferenceID);

            

            return quoteList;

        }

        public List<JBXML.Respond.JBXMLJBXMLRespondMaterialListQueryRsMaterial> getJBMaterialList()
        {
            UserManager manager = new UserManager();
            var user = manager.FindById(HttpContext.Current.User.Identity.GetUserId());

            JBXML.Request.JBXML jbx = new JBXML.Request.JBXML();
            JBXML.Request.JBXMLJBXMLRequest request = new JBXML.Request.JBXMLJBXMLRequest();

            JBXML.Request.QueryFilterType filter = new JBXML.Request.QueryFilterType();
            filter.NameFilter = "%";
            

            JBXML.Request.JBXMLJBXMLRequestMaterialListQueryRq materialRequest = new JBXML.Request.JBXMLJBXMLRequestMaterialListQueryRq();
            materialRequest.QueryFilter = filter;
            JBXML.Request.JBXMLJBXMLRequestMaterialListQueryRq[] outerMaterialRequest = { materialRequest };

            JBRequestProcessor processor = new JBRequestProcessor();

            string err = "";
            string session = processor.CreateSession(user.JBUser, user.JBPassword, ref err);

            request.MaterialListQueryRq = outerMaterialRequest;
            request.Session = session;
            jbx.Item = request;

            XmlSerializer serializer = new XmlSerializer(typeof(JBXML.Request.JBXML));

            StringWriter sw = new StringWriter();

            serializer.Serialize(sw, jbx);

            string serializedRequest = sw.ToString();

            string serializedResponse = processor.ProcessRequest(serializedRequest);
            processor.CloseSession(session);

            XmlReader reader = XmlReader.Create(new StringReader(serializedResponse));

            XmlSerializer deserializer = new XmlSerializer(typeof(JBXML.Respond.JBXML));

            JBXML.Respond.JBXML response = (JBXML.Respond.JBXML)deserializer.Deserialize(reader);

            return response.JBXMLRespond.MaterialListQueryRs[0].Material.ToList();

        }

        public JBXML.Respond.JBXMLJBXMLRespondMaterialQueryRs getJBMaterialDetails(int PartID)
        {
            return getJBMaterialDetails(db.Parts.Find(PartID).JBMaterialID);
        }

        public JBXML.Respond.JBXMLJBXMLRespondMaterialQueryRs getJBMaterialDetails(string JBMaterialID)
        {
            UserManager manager = new UserManager();
            var user = manager.FindById(HttpContext.Current.User.Identity.GetUserId());

            JBXML.Request.JBXML jbx = new JBXML.Request.JBXML();
            JBXML.Request.JBXMLJBXMLRequest request = new JBXML.Request.JBXMLJBXMLRequest();

            JBXML.Request.JBXMLJBXMLRequestMaterialQueryRqMaterialQueryFilter filter = new JBXML.Request.JBXMLJBXMLRequestMaterialQueryRqMaterialQueryFilter();
            filter.ID = JBMaterialID;
            filter.IncludeCustomerParts = true;
            filter.IncludeMaterialLocations = true;
            filter.IncludePriceBreaks = true;

            JBXML.Request.JBXMLJBXMLRequestMaterialQueryRq materialRequest = new JBXML.Request.JBXMLJBXMLRequestMaterialQueryRq();
            materialRequest.MaterialQueryFilter = filter;
            JBXML.Request.JBXMLJBXMLRequestMaterialQueryRq[] outerMaterialRequest = { materialRequest };

            JBRequestProcessor processor = new JBRequestProcessor();

            string err = "";
            string session = processor.CreateSession(user.JBUser, user.JBPassword, ref err);

            request.MaterialQueryRq = outerMaterialRequest;
            request.Session = session;
            jbx.Item = request;

            XmlSerializer serializer = new XmlSerializer(typeof(JBXML.Request.JBXML));

            StringWriter sw = new StringWriter();

            serializer.Serialize(sw, jbx);

            string serializedRequest = sw.ToString();

            string serializedResponse = processor.ProcessRequest(serializedRequest);
            processor.CloseSession(session);

            XmlReader reader = XmlReader.Create(new StringReader(serializedResponse));

            XmlSerializer deserializer = new XmlSerializer(typeof(JBXML.Respond.JBXML));

            JBXML.Respond.JBXML response = (JBXML.Respond.JBXML)deserializer.Deserialize(reader);

            return response.JBXMLRespond.MaterialQueryRs[0];
        }



        public Project getProject(int ProjectID)
        {
            return db.Projects.Find(ProjectID);
        }

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();

                db = null;
            }
        }

        public void SetCurrentProject(int id)
        {
            HttpContext.Current.Session[ProjectSessionKey] = id;

         }

        public int GetCurrentProject()
        {
            if (HttpContext.Current.Session[ProjectSessionKey] == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(HttpContext.Current.Session[ProjectSessionKey]);
            }
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}