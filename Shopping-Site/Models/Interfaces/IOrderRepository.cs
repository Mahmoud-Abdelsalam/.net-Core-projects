using System.Collections.Generic;

namespace ShoppingSite.Models.Interfaces
{
    public interface IOrderRepository
    {
        Order GetOrder(int id);
        Order AddOrder(Order order);
        Order DeleteOrder(int id);
        Order UpdateOrder(Order orderUpdates);
        Order FindOrder(string userId);
        Order AddProductToOrder(Order order, Product product);
        IEnumerable<Order> GettAllOrders();
        IEnumerable<Product> GetAllProductsInOrder(int orderId);
    }
}
