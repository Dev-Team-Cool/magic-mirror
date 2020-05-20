using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MirrorOfErised.models.Repos;
using MirrorOfErised.Models;

namespace MirrorOfErised.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthTokenRepo authtokenrepo;

        public HomeController(ILogger<HomeController> logger ,IAuthTokenRepo authTokenRepo)
        {
            _logger = logger;
            this.authtokenrepo = authTokenRepo;
        }

        public async Task<IActionResult> Index()
        {
           ViewBag.accessToken = await HttpContext.GetTokenAsync("TicketCreated");

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
