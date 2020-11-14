using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                            SignInManager<ApplicationUser> signInManager,
                                            ILogger<AccountController> logger,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider , string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider,properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null , string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if (remoteError != null)
            {
                ModelState.AddModelError("" ,$"Error from external login provider:{remoteError}");
                return View("Login", loginViewModel);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("","Error while loading external  login information");
                return View("Login", loginViewModel);
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = null;
            if (email != null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("","This email is not confirmed yet");
                    return View("Login", loginViewModel);
                }
            }
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                
                if (email != null)
                {
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        await _userManager.CreateAsync(user);
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                            new {userId = user.Id, token = token}, Request.Scheme);
                        _logger.Log(LogLevel.Warning, confirmationLink);
                        ViewBag.ErrorTitle = "Registration successful";
                        ViewBag.ErrorMessage =
                            "Before you login you must confirm your email by using the confirmation link we sent to your email";
                        return View("Error");
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                ViewBag.ErrorTitle = $"Email claim not received from:{info.LoginProvider}";
                ViewBag.ErrorMessage = $"Please contact support on Mahmoud.abdelsalam2016@gmail.com";
                return View("Error");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,  
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };  
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult>Login(LoginViewModel model,  string returnUrl)
        {
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    ModelState.AddModelError("", "This email is not confirmed yet");
                    return View(model);
                }
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, 
                                                                                        model.Password, 
                                                                                        model.RememberMe, 
                                                                                        true);
                if (result.Succeeded)
                {
                   
                    if (! string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "Home");
                    }
                   
                }

                if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                ModelState.AddModelError(string.Empty, "Username or password invalid");

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var userHasPassword = await _userManager.HasPasswordAsync(user);
            if (userHasPassword)
            {
                return View("ChangePassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _userManager.AddPasswordAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }

                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);
                return View("AddPasswordConfirmation");
            }

            return View(model);

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user =await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                        new {email = model.Email, token = token}, Request.Scheme);
                    _logger.Log(LogLevel.Warning,passwordResetLink);
                    return View("ForgetPasswordConfirmation");
                }

                return View("ForgetPasswordConfirmation");
            }

            return View(model); 

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email , string token)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("","Invalid reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        if (await  _userManager.IsLockedOutAsync(user))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        return View("ResetPasswordConfirmation");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
                return View("ResetPasswordConfirmation");
                
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var userHasPassword = await _userManager.HasPasswordAsync(user);
            if (!userHasPassword)
            {
                return RedirectToAction("AddPassword");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {

                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    City = model.City
                };
              var result = await _userManager.CreateAsync(user, model.Password);
              if (result.Succeeded)
              {
                  if (user.UserName == "Admin@gmail.com")
                  {
                      IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                      result = await _userManager.AddToRoleAsync(user, "Admin");
                  }
                    var token = await  _userManager.GenerateEmailConfirmationTokenAsync(user);
               var confirmationLink = Url.Action("ConfirmEmail", "Account", 
                    new {userId = user.Id, token = token}, Request.Scheme);
                _logger.Log(LogLevel.Warning, confirmationLink);
                  if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                  {
                      return RedirectToAction("UsersList", "Administration");
                  }

                  ViewBag.ErrorTitle = "Registration successful";
                  ViewBag.ErrorMessage =
                      "Please before login you must confirm your email by the confirmation link we sent to your email";
                  return View("Error");
              }

              foreach (var error in result.Errors)
              {
                  ModelState.AddModelError(string.Empty, error.Description);
              }
            }

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null && token == null)
            {
                return RedirectToAction("index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user id:{userId} is invalid";
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }
        [AcceptVerbs("Get" , "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {

          var user = await _userManager.FindByEmailAsync(email);

          if (user == null)
          {
              return Json(true);
          }
          else
          {
              return Json($"Email {email} is already in use");
          }

        }

    }
}