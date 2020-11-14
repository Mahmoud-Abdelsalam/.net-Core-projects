using ShoppingSite.Models.Interfaces;
using System.Collections.Generic;

namespace ShoppingSite.Models.SqlRepositories
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly AppDbContext context;

        public SqlProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Product GetProduct(int id)
        {
            return context.Products.Find(id);
        }

        public Product Add(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public Product Update(Product productUpdates)
        {
            var product = context.Products.Attach(productUpdates);
            product.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return productUpdates;
        }

        public Product Delete(int id)
        {
            Product product = context.Products.Find(id);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }

            return product;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return context.Products;
        }
    }
}