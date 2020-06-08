using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MirrorOfErised.models;
using MirrorOfErised.models.Repos;
using MirrorOfErised.Models;

namespace MirrorOfErised.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> userManager;

        public HomeController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                User identityUser = await userManager.GetUserAsync(User);
                if (identityUser.EmailConfirmed == false)
                {
                    ViewBag.verified = "NO";
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
