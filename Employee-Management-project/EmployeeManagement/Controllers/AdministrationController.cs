using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    [Authorize(Policy = "AdminRolePolicy")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdministrationController> _logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                                        UserManager<ApplicationUser> userManager,
                                                        ILogger<AdministrationController> logger )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }
        [HttpGet]
        [Authorize(Policy = "CreateRolePolicy")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CreateRolePolicy")]
        public async  Task<IActionResult> CreateRole( CreateRoleModelView model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result= await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList", "Administration");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult RolesList()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessages = $" Role with id : {id}  cannot be found";
                return View("NotFound");
            }
            var  model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            foreach (var user in _userManager.Users )
            {
                if (await _userManager.IsInRoleAsync(user , role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessages = $" Role with id : {model.Id}  cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList");
                }

                foreach (var error in result.Errors)
                {
                   ModelState.AddModelError("", error.Description); 
                }
                return View(model);
            }

          
        }
        [HttpGet]
        public async Task<IActionResult>  EditUser(string id)
        {
           var user = await _userManager.FindByIdAsync(id);
           if (user == null)
           {
               ViewBag.ErrorMessages = $" User with id : {id}  cannot be found";
               return View("NotFound");
           }

           var userClaims = await _userManager.GetClaimsAsync(user);
           var userRoles = await  _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Roles = userRoles,
                Claims = userClaims.Select(c=>c.Type + " : " + c.Value).ToList()

            };

           return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"This user id :{model.Id} can not be found";
                return View("NotFound");
            }
            else
            {
                user.Id = model.Id;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;

              var result =   await _userManager.UpdateAsync(user);
                 if (result.Succeeded)
                 {
                     return RedirectToAction("UsersList");
                 }
                 else
                 {
                     foreach (var error in result.Errors)
                     {
                         ModelState.AddModelError("",error.Description);
                     }
                 }
            }

            return View(model);

        }
        [HttpGet]
        public async  Task<IActionResult> EditRoleUser(string roleId)
        {
            ViewBag.roleId = roleId;
            var role= await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id  : {roleId} it can not be found";
                return View("NotFound");
            }

            var model = new List<EditRoleUserViewModel>();
            foreach (var user in _userManager.Users)
            {
                var editRoleUserViewModel = new EditRoleUserViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    editRoleUserViewModel.IsSelected = true;
                }
                else
                {
                    editRoleUserViewModel.IsSelected = false;
                }
                model.Add(editRoleUserViewModel);
              
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRoleUser(string roleId, List<EditRoleUserViewModel> model)
        {
             var role = await _roleManager.FindByIdAsync(roleId);
             if (role == null)
             {
                 ViewBag.ErrorMessage = $"Role with id  : {roleId} it can not be found";
                 return View("NotFound");
             }
            
             for (int i = 0; i < model.Count;  i++)
             {
                 var user = await _userManager.FindByIdAsync(model[i].UserId);
                 IdentityResult result = null;

                 if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user , role.Name)))
                 {
                   result =  await  _userManager.AddToRoleAsync(user, role.Name);
                 }
                 else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                 {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                 }
                 else
                 {
                     continue;
                 }

                 if (result.Succeeded)
                 {
                     if (i < (model.Count-1))
                     {
                         continue;
                     }
                     else
                     {
                         return RedirectToAction( "EditRole" , new {Id = roleId});
                     }
                 }
             }

             return RedirectToAction("EditRole", new {Id = roleId});
        }
        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditUserRoles(string userId, List<EditUserRolesViewModel> model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id  : {userId} it can not be found";
                return View("NotFound");
            }

            var roles =await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("","Cannot remove roles from this user");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).
                                                                                                                        Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("","The selected roles cannot be added to this user");
                return View(model);
            }
                return RedirectToAction("EditUser", new { Id = userId });
        }
        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id  : {userId} it can not be found";
                return View("NotFound");
            }

            var model = new List<EditUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var editUserRolesViewModel = new EditUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user,role.Name))
                {
                    editUserRolesViewModel.IsSelected = true;
                }
                else
                {
                    editUserRolesViewModel.IsSelected = false;
                }
                model.Add(editUserRolesViewModel);

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserClaims(string userId)

        {
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = ( $"User with id: {userId} cannot be found");
                return View("NotFound");
            }

            var model = new EditUserClaimsViewModel { UserId = userId};
            var existingUserClaims= await _userManager.GetClaimsAsync(user);
            foreach (Claim claim  in ClaimStore.AllClaims)
            {
             UserClaim userClaim = new UserClaim
             {
                 ClaimType = claim.Type
             };
             if (existingUserClaims.Any(c=>c.Type == claim.Type && c.Value == "true"))
             {
                 userClaim.IsSelected = true;   
             }
             model.Claims.Add(userClaim);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserClaims(EditUserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = ($"User with id: {model.UserId} cannot be found");
                return View("NotFound");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result =await  _userManager.RemoveClaimsAsync(user, claims);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("","Claims cannot be removed from this user");
                return View(model);
            }

            var claimsSelected = model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false"));

            result = await _userManager.AddClaimsAsync(user, claimsSelected);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("","The selected claims cannot be added to this user");
            }

            return RedirectToAction("EditUser", new{id = model.UserId});
        }

        [HttpGet]
        public IActionResult UsersList()
        {
             var users =_userManager.Users;
             return View(users);
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public  async Task<IActionResult> DeleteUser(string id)
        {
            var user= await _userManager.FindByIdAsync(id);
           if (user ==null)
           {
               ViewBag.ErrorMessage = $"This user with id{id} is not found";
               return View("NotFound");
           }
           else
           {
              var result= await _userManager.DeleteAsync(user);
              if (result.Succeeded)
              {
                  return RedirectToAction("UsersList");
              }
              else
              {
                  foreach (var error in result.Errors)
                  {
                      ModelState.AddModelError("",error.Description);
                  }

                  return View("UsersList");
              }
           }
        }
        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {

                ViewBag.ErrorMessage = $"This role with id{id} is not found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("RolesList");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View("RolesList");
                    }
                }
                catch (DbUpdateException exp)
                {
                    _logger.LogError($"Exception occured : {exp}");
                    ViewBag.ErrorTitle = $"{role.Name} role in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role. " +
                                           $"If you want to delete this role," +
                                           $" please remove the users from the role and then try to delete";
                    return View("Error");

                }
              
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}