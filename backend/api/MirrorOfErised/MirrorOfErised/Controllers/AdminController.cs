using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IImageEntryRepo _imageEntryRepo;
        private readonly ITrainJobService _trainJobService;
        private readonly ILogger<AdminController> _logger;
        
        public AdminController(SignInManager<User> signInManager, IUserRepo userRepo,
            ITrainJobService trainJobService, IImageEntryRepo imageEntryRepo,
            ILogger<AdminController> logger)
        {
            _signInManager = signInManager;
            _userRepo = userRepo;
            _trainJobService = trainJobService;
            _imageEntryRepo = imageEntryRepo;
            _logger = logger;
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
                _userRepo.Update(user);
                await _userRepo.SaveAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Run()
        {
            TrainJobsViewModel jobs = new TrainJobsViewModel()
            {
                IsTrainable = await _imageEntryRepo.NeedsTraining(),
                Jobs = await _trainJobService.GetAllJobs()
            };
            return View(jobs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Run(string _)
        {
            try
            {
                if (!await _imageEntryRepo.NeedsTraining())
                    return RedirectToAction("Run");
                
                RunnerResult result = await _trainJobService.StartJob();
                if (!result.Failed)
                {
                    List<ImageEntry> images = await _imageEntryRepo.GetAllUnprocessedImages(result.TrainJob.StartedAt);
                    foreach (var image in images)
                    {
                        image.IsProcessed = true;
                        _imageEntryRepo.Update(image);
                    }
                    await _imageEntryRepo.SaveAsync();
                }
                
                return Ok(string.IsNullOrEmpty(result.Errors) ? result.Output : result.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Ok("Unable to start a job.");
            }
        }
    }
}