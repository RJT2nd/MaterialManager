using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ExtendedLineItem
/// </summary>
namespace MaterialManager.Models
{
    public class ExtendedCartItem
    {
        private CartItem cartItem;
        private Part lineItemPart;

        public ExtendedCartItem(CartItem item, Part part)
        {
            cartItem = item;
            lineItemPart = part;
        }


        public string ItemId { get { return cartItem.ItemId; } }


        public string CartId { get { return cartItem.CartId; } }


        public int PartID { get { return cartItem.PartID; } }


        public int Quantity { get { return cartItem.Quantity; } set { cartItem.Quantity = value; } }


        public string JBMaterialID { get { return lineItemPart.JBMaterialID; } }


        public string NSN { get { return lineItemPart.NSN; } set { lineItemPart.NSN = value; } }

        public string PartNumber { get { return lineItemPart.PartNumber; } set { lineItemPart.PartNumber = value; } }

        public string Description { get { return lineItemPart.Description; } set { lineItemPart.Description = value; } }

        public Part ItemPart { get { return lineItemPart; } }

    }
}