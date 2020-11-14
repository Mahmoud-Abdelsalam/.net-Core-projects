using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingSite.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ShoppingSite.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (user.UserName=="Admin@gmail.com")
                    {
                        IdentityResult roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
                        result = await userManager.AddToRoleAsync(user, "Admin");
                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }



        ///Enable Cookie based authorization for MVC views

        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<IActionResult> Login(string Email, string password)
        //{
        //    if (!string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(password))
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    ClaimsIdentity identity = null;
        //    bool isAuthenticated = false;

        //    if (Email == "Admin" && password == "Admin_2020")
        //    {

        //        identity = new ClaimsIdentity(new[] {
        //            new Claim(ClaimTypes.Name, Email),
        //            new Claim(ClaimTypes.Role, "Admin")
        //        }, CookieAuthenticationDefaults.AuthenticationScheme);

        //        isAuthenticated = true;
        //    }

        //    if (Email == "User" && password == "User_2020")
        //    {

        //        identity = new ClaimsIdentity(new[] {
        //            new Claim(ClaimTypes.Name, Email),
        //            new Claim(ClaimTypes.Role, "User")
        //        }, CookieAuthenticationDefaults.AuthenticationScheme);

        //        isAuthenticated = true;
        //    }

        //    if (isAuthenticated)
        //    {
        //        var principal = new ClaimsPrincipal(identity);

        //         await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View();
        //}

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }
    }
}