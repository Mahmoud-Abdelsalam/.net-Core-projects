using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingSite.Models;
using ShoppingSite.Models.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ShoppingSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger,
            IProductRepository productRepository, IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IReviewRepository reviewRepository,
            UserManager<IdentityUser> userManager,
            AppDbContext context)
        {
            _logger = logger;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        public ViewResult Index()
        {
            var model = _productRepository.GetAllProducts();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ViewResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct(Product model)
        {
            if (ModelState.IsValid)
            {
                Product newProduct = new Product()
                {
                    Product_Name = model.Product_Name,
                    Price = model.Price,
                };
                _productRepository.Add(newProduct);
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(int id)
        {
            Product product = _productRepository.GetProduct(id);

            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ConfirmDelete(int id)
        {
            _productRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Product product = _productRepository.GetProduct(id);
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                Product product = _productRepository.GetProduct(model.Id);
                product.Id = model.Id;
                product.Price = model.Price;
                product.Product_Name = model.Product_Name;
                _productRepository.Update(product);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ViewResult Details(int id)
        {
            Product product = _productRepository.GetProduct(id);
            IList<Review> proReviews = _context.Reviews.Where(a => a.ProductId == id).Include(r => r.User).ToList();
            product.Reviews = proReviews;

            return View(product);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}