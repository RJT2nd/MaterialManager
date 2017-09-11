using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for RequestForQuote
/// </summary>
namespace MaterialManager.Models
{
    public class Quote
    {
        [Key, ScaffoldColumn(false)]
        public int QuoteID { get; set; }
        
        [Required, Display(Name = "RFQ Line Item ID")]
        public int RFQLineItemID { get; set; }

        [Display(Name = "RFQ Line Item ID")]
        public int POLineItemID { get; set; }

        [StringLength(255), Display(Name = "JobBoss Quote ID")]
        public string JBQuoteID { get; set; }

        [StringLength(255), Display(Name = "JobBoss Quote Line Item ID")]
        public string JBQuoteLineItemID { get; set; }

        [StringLength(255), Display(Name = "JobBoss Vendor ID")]
        public string JBVendorID { get; set; }

        [Display(Name = "Quoted Unit Price")]
        public decimal QuotedPrice { get; set; }

    }
}