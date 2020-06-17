using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MirrorOfErised.models;
using MirrorOfErised.Models;

namespace MirrorOfErised.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;

        public HomeController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                User identityUser = await _userManager.GetUserAsync(User);
                if (identityUser.EmailConfirmed == false)
                {
                    ViewBag.verified = false;
                }
            }
            catch (Exception)
            {
                return View();
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
