using System.ComponentModel.DataAnnotations;

namespace MaterialManager.Models
{
    public class CartItem
    {

        [Key]
        public string ItemId { get; set; }

        public string CartId { get; set; }

        public int Quantity { get; set; }

        public int PartID { get; set; }
    }
}