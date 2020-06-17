using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MirrorOfErised.models;
using MirrorOfErised.models.Repos;
using MirrorOfErised.ViewModels;

namespace MirrorOfErised.Controllers
{
    [Authorize]
    public class UserEntryController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserSettingsRepo _userSettingsRepo;
        private readonly PythonRunner _pythonRunner;
        private readonly IUserEntryRepo _userEntryRepo;
        private readonly IConfiguration _configuration;
        private readonly IImageEntryRepo _imageEntryRepo;
        private readonly IUserRepo _userRepo;

        public UserEntryController(IUserEntryRepo userEntry, UserManager<User> userManager, IUserSettingsRepo userSettingsRepo,
            PythonRunner pythonRunner, IConfiguration configuration, IImageEntryRepo imageEntryRepo, IUserRepo userRepo)
        {
            _userEntryRepo = userEntry;
            _userManager = userManager;
            _userSettingsRepo = userSettingsRepo;
            _pythonRunner = pythonRunner;
            _configuration = configuration;
            _imageEntryRepo = imageEntryRepo;
            _userRepo = userRepo;
        }
        
        // GET: UserEntry/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserEntry/Create
        private string SaveImage(IFormFile image, User user)
        {
            string uploadsFolder = _configuration["UploadConfig:UploadFolder"];
            string uniqueFileName = $"{Guid.NewGuid().ToString()}_{user.UserName}_{image.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            image.CopyTo(new FileStream(filePath, FileMode.Create));
            
            return uniqueFileName;
        }

        private async Task<ImageEntry> SaveAndProcessImage(IFormFile uploadedImage, User user)
        {
            string fileName = SaveImage(uploadedImage, user);
            ImageEntry image = new ImageEntry()
            {
                ImagePath = fileName,
                User = user
            };
            image.IsValid = await _pythonRunner.ValidateImage(image);
            if (!image.IsValid) return null;

            return image;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile image)
        {
            var user = await _userManager.GetUserAsync(User);
            ImageEntry savedImage = await SaveAndProcessImage(image, user);
            if (savedImage == null) return StatusCode(400);
            await _imageEntryRepo.AddImage(savedImage);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  
        public async Task<ActionResult> Create(UserEntryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User identityUser = await _userManager.GetUserAsync(User);
                    int linkedImagesCount = await _imageEntryRepo.CountImagesForUser(identityUser.Id);
                    if (linkedImagesCount > 2)
                    {
                        UserEntry entry = new UserEntry()
                        {
                            Address = model.Address,
                            CommutingWay = model.CommutingWay,
                            User = identityUser
                        };
                        
                        await _userEntryRepo.AddEntry(entry);
                        identityUser.HasCompletedSignUp = true;
                        await _userRepo.Update(identityUser);
                    }
                    else
                    {
                        int imageShortage = 3 - linkedImagesCount;
                        ViewBag.imageError =
                            $"We need at least 3 images of you. Upload {imageShortage} extra {(imageShortage == 1 ? "image" : "images")}.";
                        return View(model);
                    }
                   
                    return Redirect("/Home/index");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    ViewBag.error = "Something unexpected went wrong.";
                    return View(model);
                }
            }
            
            return View(model);
        }


        // settings page
        public async Task<ActionResult> Settings()
        {
            var id= _userManager.GetUserId(User);
            UserSettings setting = await _userSettingsRepo.GetSettingsForUserIdAsync(id);

            return View(setting);
        }

        // POST: UserEntry/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Settings(IFormCollection collection, UserSettings settings)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await _userManager.GetUserAsync(User);
                    settings.UserId = user.Id;
                    settings.User = user;

                    await _userSettingsRepo.UpdateSetting(settings);
                    return Redirect("/Home/index");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return View();
                }
            } 
            return View();
        }
    }
}