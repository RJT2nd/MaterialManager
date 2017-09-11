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
    public class PurchaseOrder
    {
        [Key, ScaffoldColumn(false)]
        public int PurchaseOrderID { get; set; }

        [Required, Display(Name = "Project ID")]
        public int ProjectID { get; set; }

        [Display(Name = "RFQ ID")]
        public int RFQID { get; set; }

        [StringLength(255), Display(Name = "Vendor")]
        public string POVendor { get; set; }

        [Display(Name = "Date of Purchase Order to Vendor")]
        public DateTime? POtoVendorDate { get; set; }

        [Display(Name = "Required Delivery Date")]
        public DateTime? RequiredDeliveryDate { get; set; }

        [Display(Name = "Expected Delivery Date")]
        public DateTime? ExpectedDeliveryDate { get; set; }

        [Display(Name = "Actual Delivery Date")]
        public DateTime? ActualDeliveryDate { get; set; }

        [StringLength(10000), Display(Name = "Delivery Address"), DataType(DataType.MultilineText)]
        public string DeliveryAddress { get; set; }

        [StringLength(255), Display(Name = "Mark For")]
        public string MarkFor { get; set; }

        [StringLength(10000), Display(Name = "Justification"), DataType(DataType.MultilineText)]
        public string Justification { get; set; }

        [Display(Name = "Review Status")]
        public string ReviewStatus { get; set; }

        [Display(Name = "Approval Status")]
        public string ApprovalStatus { get; set; }

        public PurchaseOrder()
        {
            ReviewStatus = "Not Reviewed";
            ApprovalStatus = "Not Approved";
        }
    }
}