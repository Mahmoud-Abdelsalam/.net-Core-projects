using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingSite.Models;
using ShoppingSite.Models.Interfaces;

namespace ShoppingSite.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(IProductRepository productRepository, IOrderRepository orderRepository, IReviewRepository reviewRepository, ICartRepository cartRepository, UserManager<IdentityUser> userManager)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult CreateOrder()
        {
            var userId = _userManager.GetUserId(User);
            var cart = _cartRepository.FindCart(userId);
            var products = _cartRepository.GetAllProductInCart(cart.Id);
            var order = new Order()
            {
                UserId = cart.UserId
            };
            order = _orderRepository.AddOrder(order);
            foreach (var item in products)
            {
                _orderRepository.AddProductToOrder(order, item);
            }

            _cartRepository.DeleteCart(cart.Id);

            return RedirectToAction("ViewOrder", new { orderId = order.Id });
        }

        [HttpGet]
        public ViewResult ViewOrder(int orderId)
        {
            var model = _orderRepository.GetAllProductsInOrder(orderId);

            return View(model);
        }

        [HttpGet]
        public ViewResult AddReview(int id)
        {
            var pro = _productRepository.GetProduct(id);
            return View(pro);
        }

        [HttpPost]
        public IActionResult AddReview(int id, string comment)
        {
            var userId = _userManager.GetUserId(User);
            _reviewRepository.AddReviewToProduct(comment, id, userId);
            Review review = new Review()
            {
                UserReview = comment
            };
            Product product = _productRepository.GetProduct(id);
            product.Reviews.Add(review);
            return RedirectToAction("Details", "Home", new { id = id });
        }
    }
}