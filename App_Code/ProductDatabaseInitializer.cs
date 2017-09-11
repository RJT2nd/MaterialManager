using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

/// <summary>
/// Summary description for ProductDatabaseInitializer
/// </summary>

namespace MaterialManager.Models
{
    public class ProductDatabaseInitializer : DropCreateDatabaseIfModelChanges<ProductContext>
    {
        protected override void Seed(ProductContext context)
        {
            GetCategories().ForEach(c => context.Categories.Add(c));
            GetProducts().ForEach(p => context.Products.Add(p));
            GetProjects().ForEach(p => context.Projects.Add(p));
            GetRFQs().ForEach(r => context.RequestForQuotes.Add(r));
            GetRFQLineItems().ForEach(r => context.RFQLineItems.Add(r));
            GetParts().ForEach(r => context.Parts.Add(r));
            GetRFQDocuments().ForEach(r => context.RFQDocuments.Add(r));
            GetPODocuments().ForEach(r => context.PODocuments.Add(r));
            GetQuotes().ForEach(r => context.Quotes.Add(r));
            GetPurchaseOrders().ForEach(r => context.PurchaseOrders.Add(r));
            GetPOLineItems().ForEach(r => context.POLineItems.Add(r));
            GetPartsCatalogs().ForEach(r => context.PartsCatalogs.Add(r));
            GetProjectKits().ForEach(r => context.ProjectKits.Add(r));
            GetProjectKitItems().ForEach(r => context.ProjectKitItems.Add(r));
        }

        private static List<Project> GetProjects()
        {
            var Projects = new List<Project>
            {
                new Project { ProjectID = 1, ProjectName = "Default Project", ContractNumber = "FA999-17-F-9999", FundsAllocated=1000000.00M }
            };

            return Projects;
        }

        private static List<RequestForQuote> GetRFQs()
        {
            var RFQs = new List<RequestForQuote>
            {
                new RequestForQuote { RFQID=1 ,ProjectID = 1, RFQDate = DateTime.Parse("1/15/2017"), RFQtoVendorDate = DateTime.Parse("1/16/2017"),
                    ReviewStatus = "Not Reviewed", ApprovalStatus="Not Approved" },
                new RequestForQuote { RFQID=2 ,ProjectID = 1, RFQDate = DateTime.Parse("1/20/2017"), RFQtoVendorDate = DateTime.Parse("1/25/2017"),
                    ReviewStatus = "Not Reviewed", ApprovalStatus="Not Approved" },
            };

            return RFQs;
        }

        private static List<RFQLineItem> GetRFQLineItems()
        {
            var RFQLineItems = new List<RFQLineItem>
            {
                new RFQLineItem { RFQLineItemID = 1, PartID = 1, RFQID = 1, Quantity = 1.0 },
                new RFQLineItem { RFQLineItemID = 2, PartID = 2, RFQID = 1, Quantity = 2 },
                new RFQLineItem { RFQLineItemID = 3, PartID = 3, RFQID = 2, Quantity = 1.0 }
            };

            return RFQLineItems;
        }

        private static List<Part> GetParts()
        {
            var Parts = new List<Part>
            {
                new Part { PartID = 1, NSN = "0000-00-000-0000", PartNumber = "0000K000", Description = "Dummy part A", JBMaterialID="160D414006-41" },
                new Part { PartID = 2, NSN = "0000-00-000-0011", PartNumber = "0000K001", Description = "Dummy part B" , JBMaterialID="160D414006-41"},
                new Part { PartID = 3, NSN = "0000-00-000-0111", PartNumber = "0000K002", Description = "Dummy part C" , JBMaterialID="160D414006-41"}
            };

            return Parts;
        }

        private static List<RFQDocument> GetRFQDocuments()
        {
            var RFQDocuments = new List<RFQDocument>
            {
                new RFQDocument { RFQDocID = 1, RFQID = 1, FilePath = @"C:\Users\agrimm\Documents\Visual Studio 2015\WebSites\MaterialManager\Files\Test.txt", FileName="Test.txt",
                    Description ="Test text file" },
                new RFQDocument { RFQDocID = 1, RFQID = 1, FilePath = @"C:\Users\agrimm\Documents\Visual Studio 2015\WebSites\MaterialManager\Files\Test.xlsx", FileName="Test.xlsx",
                    Description ="Test Excel file" },
                new RFQDocument { RFQDocID = 1, RFQID = 2, FilePath = @"C:\Users\agrimm\Documents\Visual Studio 2015\WebSites\MaterialManager\Files\Test.txt", FileName="Test.txt",
                    Description ="Test text file" }
            };

            return RFQDocuments;
        }

        private static List<PODocument> GetPODocuments()
        {
            var PODocuments = new List<PODocument>
            {
                new PODocument { PODocID = 1, POID = 1, FilePath = @"C:\Users\agrimm\Documents\Visual Studio 2015\WebSites\MaterialManager\Files\Test.txt", FileName="Test.txt",
                    Description ="Test text file" },
                new PODocument { PODocID = 1, POID = 1, FilePath = @"C:\Users\agrimm\Documents\Visual Studio 2015\WebSites\MaterialManager\Files\Test.xlsx", FileName="Test.xlsx",
                    Description ="Test Excel file" },
                new PODocument { PODocID = 1, POID = 2, FilePath = @"C:\Users\agrimm\Documents\Visual Studio 2015\WebSites\MaterialManager\Files\Test.txt", FileName="Test.txt",
                    Description ="Test text file" }
            };

            return PODocuments;
        }

        private static List<Quote> GetQuotes()
        {
            var Quotes = new List<Quote>
            {
                new Quote { QuoteID = 1, RFQLineItemID = 1, QuotedPrice = 1532.50M },
                new Quote { QuoteID = 2, RFQLineItemID = 1, QuotedPrice = 1428.50M },
                new Quote { QuoteID = 3, RFQLineItemID = 2, QuotedPrice = 255.50M }
            };

            return Quotes;
        }

        private static List<PurchaseOrder> GetPurchaseOrders()
        {
            var PurchaseOrders = new List<PurchaseOrder>
            {
                new PurchaseOrder {PurchaseOrderID = 1, ProjectID = 1, RFQID = 1, RequiredDeliveryDate = DateTime.Parse("3/31/2017"),
                    ExpectedDeliveryDate = DateTime.Parse("2/28/2017"), DeliveryAddress = "1200 N Herndon St, Arlington, VA 22201", MarkFor="Adam Grimm",
                    Justification ="Some kind of justification", POtoVendorDate=DateTime.Parse("1/15/2017"), ReviewStatus = "Not Reviewed", ApprovalStatus="Not Approved" },
                new PurchaseOrder {PurchaseOrderID = 2, ProjectID = 1, RFQID = 1, RequiredDeliveryDate = DateTime.Parse("3/31/2017"),
                    ExpectedDeliveryDate = DateTime.Parse("4/28/2017"), DeliveryAddress = "1200 N Herndon St, Arlington, VA 22201", MarkFor="Adam Grimm",
                    Justification ="Some other kind of justification", POtoVendorDate=DateTime.Parse("1/1/2017"), ReviewStatus = "Not Reviewed", ApprovalStatus="Not Approved" }
            };

            return PurchaseOrders;
        }

        private static List<POLineItem> GetPOLineItems()
        {
            var POLineItems = new List<POLineItem>
            {
                new POLineItem { POLineItemID = 1, POID = 1, PartID = 1, Quantity = 1.0, Price = 1250.50M },
                new POLineItem { POLineItemID = 2, POID = 1, PartID = 1, Quantity = 2.0, Price = 3500M  }

            };

            return POLineItems;
        }

        private static List<PartsCatalog> GetPartsCatalogs()
        {
            var PartsCatalogs = new List<PartsCatalog>
            {
                new PartsCatalog { PartsCatalogID = 1, ProjectID = 1, PartID = 2 },
                new PartsCatalog { PartsCatalogID = 2, ProjectID = 1, PartID = 1}

            };

            return PartsCatalogs;
        }

        private static List<ProjectKit> GetProjectKits()
        {
            var ProjectKits = new List<ProjectKit>
            {
                new ProjectKit { ProjectKitID = 1, ProjectID = 1, Description = "First project kit of some items" },
                new ProjectKit { ProjectKitID = 2, ProjectID = 1, Description = "Second project kit of some other items"}

            };

            return ProjectKits;
        }

        private static List<ProjectKitItem> GetProjectKitItems()
        {
            var ProjectKitItems = new List<ProjectKitItem>
            {
                new ProjectKitItem {ProjectKitItemID = 1, ProjectKitID = 1, PartID = 1, Quantity = 2 },
                new ProjectKitItem {ProjectKitItemID = 2, ProjectKitID = 1, PartID = 2, Quantity = 1 },
                new ProjectKitItem {ProjectKitItemID = 3, ProjectKitID = 1, PartID = 3, Quantity = 1 },
                new ProjectKitItem {ProjectKitItemID = 4, ProjectKitID = 2, PartID = 2, Quantity = 2 },
                new ProjectKitItem {ProjectKitItemID = 5, ProjectKitID = 2, PartID = 3, Quantity = 2 },
            };

            return ProjectKitItems;
        }

        private static List<Category> GetCategories()
        {
            var categories = new List<Category> {
                new Category { CategoryID = 1, CategoryName = "Cars" },
                new Category { CategoryID = 2, CategoryName = "Planes" },
                new Category { CategoryID = 3, CategoryName = "Trucks" },
                new Category { CategoryID = 4, CategoryName = "Boats" },
                new Category { CategoryID = 5, CategoryName = "Rockets" },
            };

            return categories;
        }

        private static List<Product> GetProducts()
        {
            var products = new List<Product> {
                new Product {
                    ProductID = 1,
                    ProductName = "Convertible Car",
                    Description = "This convertible car is fast! The engine is powered by a neutrino based battery (not included)." + "Power it up and let it go!",
                    ImagePath ="carconvert.png",
                    UnitPrice = 22.50,
                    CategoryID = 1 },
                new Product {
                    ProductID = 2,
                    ProductName = "Old-time Car",
                    Description = "There's nothing old about this toy car, except it's looks. Compatible with other old toy cars.",
                    ImagePath ="carearly.png",
                    UnitPrice = 15.95,
                    CategoryID = 1 },
                new Product {
                    ProductID = 3,
                    ProductName = "Fast Car",
                    Description = "Yes this car is fast, but it also floats in water.",
                    ImagePath ="carfast.png",
                    UnitPrice = 32.99,
                    CategoryID = 1 },
                new Product {
                    ProductID = 4,
                    ProductName = "Super Fast Car",
                    Description = "Use this super fast car to entertain guests. Lights and doors work!",
                    ImagePath ="carfaster.png",
                    UnitPrice = 8.95,
                    CategoryID = 1 },
                new Product {
                    ProductID = 5,
                    ProductName = "Old Style Racer",
                    Description = "This old style racer can fly (with user assistance). Gravity controls flight duration." + "No batteries required.",
                    ImagePath ="carracer.png",
                    UnitPrice = 34.95,
                    CategoryID = 1 },
                new Product {
                    ProductID = 6,
                    ProductName = "Ace Plane",
                    Description = "Authentic airplane toy. Features realistic color and details.",
                    ImagePath ="planeace.png",
                    UnitPrice = 95.00,
                    CategoryID = 2 },
                new Product {
                    ProductID = 7,
                    ProductName = "Glider",
                    Description = "This fun glider is made from real balsa wood. Some assembly required.",
                    ImagePath ="planeglider.png",
                    UnitPrice = 4.95,
                    CategoryID = 2 },
                new Product {
                    ProductID = 8,
                    ProductName = "Paper Plane",
                    Description = "This paper plane is like no other paper plane. Some folding required.",
                    ImagePath ="planepaper.png",
                    UnitPrice = 2.95,
                    CategoryID = 2 },
                new Product {
                    ProductID = 9, ProductName = "Propeller Plane", Description = "Rubber band powered plane features two wheels.", ImagePath="planeprop.png", UnitPrice = 32.95, CategoryID = 2 },
                new Product { ProductID = 10, ProductName = "Early Truck", Description = "This toy truck has a real gas powered engine. Requires regular tune ups.", ImagePath="truckearly.png", UnitPrice = 15.00, CategoryID = 3 },
                new Product {ProductID = 11, ProductName = "Fire Truck", Description = "You will have endless fun with this one quarter sized fire truck.", ImagePath="truckfire.png", UnitPrice = 26.00, CategoryID = 3 },
                new Product { ProductID = 12, ProductName = "Big Truck", Description = "This fun toy truck can be used to tow other trucks that are not as big.", ImagePath="truckbig.png", UnitPrice = 29.00, CategoryID = 3 },
                new Product { ProductID = 13, ProductName = "Big Ship", Description = "Is it a boat or a ship. Let this floating vehicle decide by using its " + "artifically intelligent computer brain!", ImagePath="boatbig.png", UnitPrice = 95.00, CategoryID = 4 },
                new Product { ProductID = 14, ProductName = "Paper Boat", Description = "Floating fun for all! This toy boat can be assembled in seconds. Floats for minutes!" + "Some folding required.", ImagePath="boatpaper.png", UnitPrice = 4.95, CategoryID = 4 },
                new Product { ProductID = 15, ProductName = "Sail Boat", Description = "Put this fun toy sail boat in the water and let it go!", ImagePath="boatsail.png", UnitPrice = 42.95, CategoryID = 4 },
                new Product { ProductID = 16, ProductName = "Rocket", Description = "This fun rocket will travel up to a height of 200 feet.", ImagePath="rocket.png", UnitPrice = 122.95, CategoryID = 5 }
            };
            return products;
        }
    }
}