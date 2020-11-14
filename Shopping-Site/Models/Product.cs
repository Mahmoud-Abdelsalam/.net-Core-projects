using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingSite.Models
{
    public class Product
    {
        public Product()
        {
            ProductOrders = new List<ProductOrder>();
            ProductCarts = new List<ProductCart>();
            Reviews = new List<Review>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "Product  name cannot exceed 25 characters")]
        [MinLength(5, ErrorMessage = "Product name cannot be less than 5 characters")]
        [Display(Name = "Product Name")]
        public string Product_Name { get; set; }

        [Required]
        public int Price { get; set; }

        public IList<ProductOrder> ProductOrders;
        public IList<ProductCart> ProductCarts;
        public IList<Review> Reviews { get; set; }
    }
}