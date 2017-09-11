using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

/// <summary>
/// Summary description for ProductContext
/// </summary>

namespace MaterialManager.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("MaterialManager")
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> ShoppingCartItems { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<RequestForQuote> RequestForQuotes { get; set; }
        public DbSet<RFQLineItem> RFQLineItems { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<RFQDocument> RFQDocuments { get; set; }
        public DbSet<POLineItem> POLineItems { get; set; }
        public DbSet<PartsCatalog> PartsCatalogs { get; set; }
        public DbSet<ProjectKitItem> ProjectKitItems { get; set; }
        public DbSet<ProjectKit> ProjectKits { get; set; }
        public DbSet<PODocument> PODocuments { get; set; }



    }
}