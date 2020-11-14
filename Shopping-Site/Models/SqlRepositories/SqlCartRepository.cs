using ShoppingSite.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingSite.Models.SqlRepositories
{
    public class SqlCartRepository : ICartRepository
    {
        private readonly AppDbContext context;

        public SqlCartRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Cart GetCart(int id)
        {
            return context.Carts.Find(id);
        }

        public Cart AddCart(Cart cart)
        {
            context.Carts.Add(cart);
            context.SaveChanges();
            return cart;
        }

        public Cart DeleteCart(int id)
        {
            Cart cart = context.Carts.Find(id);
            if (cart != null)
            {
                context.Carts.Remove(cart);
                context.SaveChanges();
            }

            return cart;
        }

        public Cart UpdateCart(Cart updateCart)
        {
            var cart = context.Carts.Attach(updateCart);
            cart.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return updateCart;
        }

        public Cart FindCart(string userId)
        {
            var cart = context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                Cart newCart = new Cart()
                {
                    UserId = userId
                };
                cart = AddCart(newCart);
            }
            return cart;
        }

        public Cart AddProductToCart(Cart cart, Product product)
        {
            var productCart = new ProductCart
            {
                CartId = cart.Id,
                ProductId = product.Id
            };

            context.ProductCarts.Add(productCart);
            context.SaveChanges();
            return cart;
        }

        public void DeleteProductInCard(int cartId, int prodId)
        {
            IEnumerable<ProductCart> prodCarts = context.ProductCarts.Where(a => a.CartId == cartId).ToList();
            context.ProductCarts.Remove(prodCarts.SingleOrDefault(a => a.ProductId == prodId));
            context.SaveChanges();
        }

        public IEnumerable<Product> GetAllProductInCart(int cartId)
        {
            List<Product> products = new List<Product>();
            IEnumerable<ProductCart> prodCarts = context.ProductCarts.Where(a => a.CartId == cartId).ToList();
            foreach (var prod in prodCarts)
            {
                products.Add(context.Products.Find(prod.ProductId));
            }

            return products;
        }

        public IEnumerable<Cart> GetAllCarts()
        {
            return context.Carts;
        }
    }
}