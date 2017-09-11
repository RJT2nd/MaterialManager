using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for ExtendedPOLineItem
/// </summary>

namespace MaterialManager.Models
{

    public class ExtendedPOLineItem
    {
        private POLineItem lineItem;
        private Part lineItemPart;

        public ExtendedPOLineItem(POLineItem item, Part part)
        {
            lineItem = item;
            lineItemPart = part;
        }

        
        public int PartID { get { return lineItem.PartID; } }

        public string JBMaterialID { get { return lineItemPart.JBMaterialID; } }

        public string NSN { get { return lineItemPart.NSN; } }

        public string PartNumber { get { return lineItemPart.PartNumber; } }

        public string Description { get { return lineItemPart.Description; } }

        public int POLineItemID { get { return lineItem.POLineItemID; } }

        public int POID { get { return lineItem.POID; } }

        public double Quantity { get { return lineItem.Quantity; } }

        public decimal Price { get { return lineItem.Price; } }

        public decimal TotalPrice { get { return lineItem.TotalPrice; } }
    }
}