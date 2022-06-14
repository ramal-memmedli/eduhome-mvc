using Common.Helpers;
using DAL.Models;
using EduhomeMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EduhomeMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            AppUser newUser = new AppUser()
            {
                Firstname = register.Firstname,
                Lastname = register.Lastname,
                UserName = register.Username,
                Email = register.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);

            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }

            if((await _userManager.GetUsersInRoleAsync(Enums.Roles.Admin.ToString())).Count == 0)
            {
                await _userManager.AddToRoleAsync(newUser, Enums.Roles.Admin.ToString());
            }else
            {
                await _userManager.AddToRoleAsync(newUser, Enums.Roles.Member.ToString());
            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            string route = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, token }, HttpContext.Request.Scheme);

            return RedirectToAction("EmailConfirmationPage", new {route});
        }

        public IActionResult EmailConfirmationPage(string route)
        {
            return View(model: route);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = await _userManager.FindByEmailAsync(login.Email);

            if(user is null)
            {
                ModelState.AddModelError("", "User does not exists");
                return View();
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistent, true);

            if(result.IsLockedOut)
            {
                ModelState.AddModelError("", "User locked. Please try again later");
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Entered email or password is wrong");
                return View();
            }

            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }

        public async Task CreateRoles()
        {
            foreach (string role in Enum.GetNames(typeof(Enums.Roles)))
            {
                if(!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(userId is null || token is null)
            {
                return NotFound();
            }

            AppUser user = await _userManager.FindByNameAsync(userId);

            if (user is null) return NotFound();

            await _userManager.ConfirmEmailAsync(user, token);
            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }
    }
}
