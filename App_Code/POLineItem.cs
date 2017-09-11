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
    public class POLineItem
    {
        [Key, ScaffoldColumn(false)]
        public int POLineItemID { get; set; }

        [Required, Display(Name = "Purchase Order ID")]
        public int POID { get; set; }

        [Required, Display(Name = "Part ID")]
        public int PartID { get; set; }

        [Required, Display(Name = "Quantity")]
        public double Quantity { get; set; }

        [Required, Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get { return (int)Quantity * Price; } }
    }
}