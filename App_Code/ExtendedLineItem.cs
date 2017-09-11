using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ExtendedLineItem
/// </summary>
namespace MaterialManager.Models
{
    public class ExtendedLineItem
    {
        private RFQLineItem lineItem;
        private Part lineItemPart;

        public ExtendedLineItem(RFQLineItem item, Part part)
        {
            lineItem = item;
            lineItemPart = part;
        }


        public int RFQLineItemID { get { return lineItem.RFQLineItemID; } }


        public int RFQID { get { return lineItem.RFQID; } }


        public int PartID { get { return lineItem.PartID; } }


        public double Quantity { get { return lineItem.Quantity; } set { lineItem.Quantity = value; } }


        public string JBMaterialID { get { return lineItemPart.JBMaterialID; } }


        public string NSN { get { return lineItemPart.NSN; } set { lineItemPart.NSN = value; } }

        public string PartNumber { get { return lineItemPart.PartNumber; } set { lineItemPart.PartNumber = value; } }

        public string Description { get { return lineItemPart.Description; } set { lineItemPart.Description = value; } }

        public Part ItemPart { get { return lineItemPart; } }

    }
}