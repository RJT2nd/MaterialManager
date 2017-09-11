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
    public class RFQLineItem
    {
        [Key, ScaffoldColumn(false)]
        public int RFQLineItemID { get; set; }

        [Required, Display(Name = "RFQ ID")]
        public int RFQID { get; set; }

        [Required, Display(Name = "Part ID")]
        public int PartID { get; set; }

        [Required, Display(Name = "Quantity")]
        public double Quantity { get; set; }
    }
}