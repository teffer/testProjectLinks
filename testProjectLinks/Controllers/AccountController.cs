using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using testProjectLinks.Models;
using testProjectLinks.ViewModels;

namespace testProjectLinks.Controllers
{
    public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) : Controller
    {
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Login!, model.Password!, true, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Ошибка при попытке входа");
            } 
            return View(model);
        }
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Login = model.Login,
                    Email = model.Email,
                    UserName = model.Login
                };
                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user,true);
                    return RedirectToAction(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            await signInManager.SignOutAsync();
            return RedirectToAction(returnUrl);
        }
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl)
                : RedirectToAction(nameof(HomeController.IndexAsync), nameof(HomeController));
        }
    }
}
