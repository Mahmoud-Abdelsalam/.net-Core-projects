using System.Collections.Generic;

namespace ShoppingSite.Models
{
    public class Cart
    {
        public Cart()
        {
            ProductCarts = new List<ProductCart>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<ProductCart> ProductCarts;
    }
}