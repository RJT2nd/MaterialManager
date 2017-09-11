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
    public class RequestForQuote
    {
        [Key, ScaffoldColumn(false)]
        public int RFQID { get; set; }

        public string JBReferenceCode { get { return "MaterialManager-" + RFQID; } }

        [Required, Display(Name = "Project")]
        public int ProjectID { get; set; }

        [Display(Name = "Date of Vendor RFQ")]
        public DateTime? RFQtoVendorDate { get; set; }

        [Display(Name = "Date of RFQ")]
        public DateTime? RFQDate { get; set; }

        [Display(Name = "Review Status")]
        public string ReviewStatus { get; set; }

        [Display(Name = "Approval Status")]
        public string ApprovalStatus { get; set; }

        public RequestForQuote()
        {
            ReviewStatus = "Not Reviewed";
            ApprovalStatus = "Not Approved";
        }
    }
}