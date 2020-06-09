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
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUserSettingsRepo _userSettingsRepo;
        private readonly FacePython _facePython;
        private readonly IUserEntryRepo _userEntryrepo;
        private readonly IConfiguration _configuration;

        public UserEntryController(IUserEntryRepo userEntry, UserManager<User> userManager, IHostingEnvironment hostingEnvironment, IUserSettingsRepo userSettingsRepo, FacePython facePython, IConfiguration configuration)
        {
            this._userEntryrepo = userEntry;
            this._userManager = userManager;
            this._hostingEnvironment = hostingEnvironment;
            this._userSettingsRepo = userSettingsRepo;
            this._facePython = facePython;
            this._configuration = configuration;
        }
        
        // GET: UserEntry/Create
        public async Task<ActionResult> Create()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);
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

        private List<ImageEntry> SaveAndProcessImages(IFormFile[] uploadedImages, ref UserEntry userEntry)
        {
            List<ImageEntry> images = new List<ImageEntry>();
            foreach (IFormFile uploadedImage in uploadedImages)
            {
                string fileName = SaveImage(uploadedImage, userEntry.User);
                ImageEntry image = new ImageEntry()
                {
                    ImagePath = fileName,
                    User = userEntry
                };
                image.IsValid = _facePython.ValidateImage(ref image);
                if (!image.IsValid) throw new Exception($"Image with filename: {uploadedImage.FileName} is invalid.");
                images.Add(image);
            }
            return images;
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
                    if (model.Images.Length > 2)
                    {
                        UserEntry entry = new UserEntry()
                        {
                            Address = model.Address,
                            CommutingWay = model.CommutingWay,
                            User = identityUser
                        };
                        try
                        {
                            entry.Images = SaveAndProcessImages(model.Images, ref entry);
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("Images", e.Message);
                            return View(model);
                        }

                        await _userEntryrepo.AddEntry(entry);
                    }
                    else
                    {
                        ModelState.AddModelError("Images", "We need at least 3 images of you.");
                        return View(model);
                    }
                   
                    return Redirect("/Home/index");
                }
                catch
                {
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