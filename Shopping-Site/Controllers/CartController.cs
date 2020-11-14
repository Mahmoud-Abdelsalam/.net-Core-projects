using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingSite.Models;
using ShoppingSite.Models.Interfaces;

namespace ShoppingSite.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(IProductRepository productRepository, IOrderRepository orderRepository, ICartRepository cartRepository, UserManager<IdentityUser> userManager)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            Product product = _productRepository.GetProduct(id);
            var userId = _userManager.GetUserId(User);
            var cart = _cartRepository.FindCart(userId);
            _cartRepository.AddProductToCart(cart, product);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public ViewResult ShowCart()
        {
            var userId = _userManager.GetUserId(User);
            var cart = _cartRepository.FindCart(userId);
            var cartId = cart.Id;
            var model = _cartRepository.GetAllProductInCart(cartId);
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteItem(int id)
        {
            var userId = _userManager.GetUserId(User);
            var cart = _cartRepository.FindCart(userId);
            var cartId = cart.Id;
            _cartRepository.DeleteProductInCard(cartId, id);
            return Ok();
        }
    }
}