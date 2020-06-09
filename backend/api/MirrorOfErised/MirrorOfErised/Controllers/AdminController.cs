using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MirrorOfErised.models;
using MirrorOfErised.ViewModels;

namespace MirrorOfErised.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        
        public AdminController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }
        
        // GET admin
        public IActionResult Index()
        {
            return View();
        }
        
        // GET admin/login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loggedIn = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (loggedIn.Succeeded)
                    return RedirectToAction("Index");
                ModelState.AddModelError(string.Empty, "Unable to log you in with this combination of email and password.");
            }

            return View(model);
        }
    }
}