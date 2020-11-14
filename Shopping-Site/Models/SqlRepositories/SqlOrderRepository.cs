using System.Collections.Generic;
using System.Linq;
using ShoppingSite.Models.Interfaces;

namespace ShoppingSite.Models.SqlRepositories
{
    public class SqlOrderRepository : IOrderRepository
    {
        private readonly AppDbContext context;

        public SqlOrderRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Order GetOrder(int id)
        {
            return context.Orders.Find(id);
        }

        public Order AddOrder(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
            return order;
        }

        public Order DeleteOrder(int id)
        {
            Order order = context.Orders.Find(id);
            if (order != null)
            {
                context.Orders.Remove(order);
                context.SaveChanges();
            }

            return order;
        }

        public Order UpdateOrder(Order orderUpdates)
        {
            var order = context.Orders.Attach(orderUpdates);
            order.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return orderUpdates;
        }

        public Order FindOrder(string userId)
        {
            var order = context.Orders.FirstOrDefault(c => c.UserId == userId);
            return order;
        }

        public Order AddProductToOrder(Order order, Product product)
        {
            var productOrder = new ProductOrder()
            {
                OrderId = order.Id,
                ProductId = product.Id
            };

            context.ProductOrders.Add(productOrder);
            context.SaveChanges();
            return order;
        }

        public IEnumerable<Order> GettAllOrders()
        {
            return context.Orders;
        }

        public IEnumerable<Product> GetAllProductsInOrder(int orderId)
        {
            List<Product> products = new List<Product>();
            IEnumerable<ProductOrder> prodOrders = context.ProductOrders.Where(a=>a.OrderId== orderId).ToList();
            foreach (var prod in prodOrders)
            {
                products.Add(context.Products.Find(prod.ProductId));
            }

            return products;
        }
    }
}