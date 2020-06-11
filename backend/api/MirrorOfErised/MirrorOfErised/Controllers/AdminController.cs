using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MirrorOfErised.models;
using MirrorOfErised.models.Repos;
using MirrorOfErised.models.Services;
using MirrorOfErised.ViewModels;

namespace MirrorOfErised.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepo _userRepo;
        private readonly ITrainJobService _trainJobService;
        
        public AdminController(SignInManager<User> signInManager, IUserRepo userRepo, ITrainJobService trainJobService)
        {
            _signInManager = signInManager;
            _userRepo = userRepo;
            _trainJobService = trainJobService;
        }
        
        // GET admin
        public async Task<IActionResult> Index()
        {
            UserViewModel userViewModel = new UserViewModel()
            {
                Users = await _userRepo.GetAllUsers()
            };
            return View(userViewModel);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable(UserViewModel model)
        {
            var user = await _userRepo.GetUserById(model.changedUser.Id);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                await _userRepo.Update(user);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Run()
        {
            TrainJobsViewModel jobs = new TrainJobsViewModel()
            {
                Jobs = await _trainJobService.GetAllJobs()
            };
            return View(jobs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Run(string _)
        {
            string result = await _trainJobService.StartJob();
            return Ok(result);
        }
    }
}