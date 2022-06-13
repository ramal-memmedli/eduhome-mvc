using DAL.Models;
using EduhomeMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduhomeMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

            await _signInManager.SignInAsync(newUser, false);

            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }
    }
}
