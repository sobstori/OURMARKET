using Microsoft.AspNetCore.Mvc;
using OurMarketBackend.ViewModels;
using Microsoft.AspNetCore.Identity;
using OurMarketBackend.Models;
using System.Threading.Tasks;

namespace OurMarketBackend.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !string.IsNullOrEmpty(user.UserName))
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, false);
                    if (result.Succeeded)
                    {
                        TempData["LoginMessage"] = "Login successful!";
                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["LoginMessage"] = "Login failed. Please check your credentials.";
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["LoginMessage"] = "Registration successful! You are now logged in.";
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Account()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Account(string FirstName, string LastName, string PhoneNumber, string State)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.FirstName = FirstName;
                user.LastName = LastName;
                user.PhoneNumber = PhoneNumber;
                user.State = State;
                await _userManager.UpdateAsync(user);
                TempData["AccountMessage"] = "Profile updated successfully.";
            }
            return View(user);
        }
    }
}