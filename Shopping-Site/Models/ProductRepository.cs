using ShoppingSite.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingSite.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _productsList;

        public ProductRepository()
        {
            _productsList = new List<Product>
            {
                new Product() {Id = 1, Product_Name = "Rolex",Price = 550},
                new Product() {Id = 2, Product_Name = "Alpa",Price = 400},
                new Product() {Id = 3, Product_Name = "Calvin Klein",Price = 730}
            };
        }

        public Product GetProduct(int id)
        {
            return _productsList.Find(i => i.Id == id);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productsList;
        }

        public Product Add(Product product)
        {
            product.Id = _productsList.Max(e => e.Id) + 1;
            _productsList.Add(product);
            return product;
        }

        public Product Update(Product productUpdates)
        {
            Product product = GetProduct(productUpdates.Id);
            if (product != null)
            {
                product.Product_Name = productUpdates.Product_Name;
                product.Price = productUpdates.Price;
            }

            return product;
        }

        public Product Delete(int id)
        {
            Product product = GetProduct(id);
            if (product != null)
            {
                _productsList.Remove(product);
            }

            return product;
        }
    }
}