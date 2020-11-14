using System.Collections.Generic;

namespace ShoppingSite.Models
{
    public class Order
    {
        public Order()
        {
            ProductOrders = new List<ProductOrder>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<ProductOrder> ProductOrders;
    }
}