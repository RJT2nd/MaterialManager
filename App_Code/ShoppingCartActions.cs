using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MaterialManager.Models;

/// <summary>
/// Summary description for ShoppingCartActions
/// </summary>

namespace MaterialManager.Logic
{
    public class ShoppingCartActions : IDisposable
    {
        public string ShoppingCartId { get; set; }

        private ProductContext db = new ProductContext();

        public const string CartSessionKey = "CartId";

        public void AddToCart(int id, int quantity)
        {
            // Retrieve the product from the database. 
            ShoppingCartId = GetCartId();

            var cartItem = db.ShoppingCartItems.SingleOrDefault(
                c => c.CartId == ShoppingCartId && c.PartID == id);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists. 
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    PartID = id,
                    CartId = ShoppingCartId,
                    Quantity = quantity,
                };

                db.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();

                db = null;
            }
        }

        public string GetCartId()
        {
            if(HttpContext.Current.Session[CartSessionKey] == null)
            {
                if(!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionKey] = tempCartId.ToString();

                }
            }
            return HttpContext.Current.Session[CartSessionKey].ToString();
        }

        public IQueryable<CartItem> GetCartItems()
        {
            ShoppingCartId = GetCartId();

            return db.ShoppingCartItems.Where(c => c.CartId == ShoppingCartId).ToList().AsQueryable();
        }

        public IQueryable<ExtendedCartItem> GetExtendedCartItems()
        {
            DataActions actions = new DataActions();

            IQueryable<CartItem> lineItems = GetCartItems();
            List<ExtendedCartItem> extendedCartItems = new List<ExtendedCartItem>();

            foreach (CartItem lineItem in lineItems)
            {
                extendedCartItems.Add(new ExtendedCartItem(lineItem, actions.getPart(lineItem.PartID)));
            }

            return extendedCartItems.AsQueryable();
        }

        //public decimal GetTotal()
        //{
        //    ShoppingCartId = GetCartId();

        //    decimal? total = decimal.Zero;

        //    total = (decimal?)(from cartItems in db.ShoppingCartItems
        //                       where cartItems.CartId == ShoppingCartId
        //                       select (int?)cartItems.Quantity * cartItems.Product.UnitPrice).Sum();
        //    return total ?? decimal.Zero;
        //}

        public ShoppingCartActions GetCart(HttpContext context)
        {
            using (var cart = new ShoppingCartActions())
            {
                cart.ShoppingCartId = cart.GetCartId();
                return cart;
            }
        }

        public void UpdateShoppingCartDatabase(String cartId, ShoppingCartUpdates[] CartItemUpdates)
        {
            using (var db = new MaterialManager.Models.ProductContext())
            {
                try
                {
                    int CartItemCount = CartItemUpdates.Count();
                    IQueryable<CartItem> myCart = GetCartItems();
                    foreach (var cartItem in myCart)
                    {
                        for(int i = 0; i < CartItemCount; i++)
                        {
                            if(cartItem.PartID == CartItemUpdates[i].ProductId)
                            {
                                if((CartItemUpdates[i].PurchaseQuantity < 1) || CartItemUpdates[i].RemoveItem==true)
                                {
                                    RemoveItem(cartId, cartItem.PartID);
                                }
                                else
                                {
                                    UpdateItem(cartId, cartItem.PartID, CartItemUpdates[i].PurchaseQuantity);
                                }
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Update Cart Database - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void RemoveItem(string removeCartID, int removeProductID)
        {
            using (var db = new MaterialManager.Models.ProductContext())
            {
                try
                {
                    var myItem = (from c in db.ShoppingCartItems where c.CartId == removeCartID && c.PartID == removeProductID select c).FirstOrDefault();
                    if(myItem != null)
                    {
                        db.ShoppingCartItems.Remove(myItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Remove Cart Item - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void UpdateItem(string updateCartID, int updateProductID, int quantity)
        {
            using (var db = new MaterialManager.Models.ProductContext())
            {
                try
                {
                    var myItem = (from c in db.ShoppingCartItems where c.CartId == updateCartID && c.PartID == updateProductID select c).FirstOrDefault();
                    if(myItem != null)
                    {
                        myItem.Quantity = quantity;
                        db.SaveChanges();
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Update Cart Item - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void EmptyCart()
        {
            ShoppingCartId = GetCartId();
            var cartItems = db.ShoppingCartItems.Where(c => c.CartId == ShoppingCartId);
            foreach(var cartItem in cartItems)
            {
                db.ShoppingCartItems.Remove(cartItem);
                db.SaveChanges();
            }
        }

        public int GetCount()
        {
            ShoppingCartId = GetCartId();

            int? count = (from cartItems in db.ShoppingCartItems
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Quantity).Sum();

            return count ?? 0;
        }


        public struct ShoppingCartUpdates
        {
            public int ProductId;
            public int PurchaseQuantity;
            public bool RemoveItem;
        }
        

    }
}