using System.Collections.Generic;

namespace ShoppingSite.Models.Interfaces
{
    public interface IProductRepository
    {
        Product GetProduct(int id);
        Product Add(Product product);
        Product Update(Product productUpdates);
        Product Delete(int id);
        IEnumerable<Product> GetAllProducts();
    }
}
