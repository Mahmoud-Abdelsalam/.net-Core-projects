using System.Collections.Generic;

namespace ShoppingSite.Models.Interfaces
{
    public interface ICartRepository
    {
        Cart GetCart(int id);

        Cart AddCart(Cart cart);

        Cart DeleteCart(int id);

        Cart UpdateCart(Cart updateCart);

        Cart FindCart(string userId);

        Cart AddProductToCart(Cart cart, Product product);

        void DeleteProductInCard(int cardId, int prodId);

        IEnumerable<Product> GetAllProductInCart(int cartId);

        IEnumerable<Cart> GetAllCarts();
    }
}