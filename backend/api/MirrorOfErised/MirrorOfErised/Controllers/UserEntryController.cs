using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MirrorOfErised.models;
using MirrorOfErised.models.Repos;
using MirrorOfErised.ViewModels;

namespace MirrorOfErised.Controllers
{
    public class UserEntryController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUserSettingsRepo _userSettingsRepo;
        private readonly FacePython _facePython;
        private readonly IUserEntryRepo _userEntryrepo;

        public UserEntryController(IUserEntryRepo userEntry, UserManager<User> userManager, IHostingEnvironment hostingEnvironment, IUserSettingsRepo userSettingsRepo, FacePython facePython )
        {
            this._userEntryrepo = userEntry;
            this._userManager = userManager;
            this._hostingEnvironment = hostingEnvironment;
            this._userSettingsRepo = userSettingsRepo;
            this._facePython = facePython;
        }
        
        // GET: UserEntry
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserEntry/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return Redirect("/Error/400");
            }

            return View();
        }

        // GET: UserEntry/Create
        public async Task<ActionResult> Create()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);
            if (identityUser.EmailConfirmed == false)
            {
                return Redirect("/Error/403");
            }
            return View();
        }

        // POST: UserEntry/Create
        private string saveImage(IFormFile image, IdentityUser identityUser)
        {
            string uniqueFileName = null;
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + identityUser.UserName + "_" + image.FileName;
            string FilePath = Path.Combine(uploadsFolder, uniqueFileName);
            image.CopyTo(new FileStream(FilePath, FileMode.Create));
            var result = _facePython.validateImage(uniqueFileName);
            if (result.Contains("NOK")| result.Contains("Traceback"))
            {
                return "false";
            }
            return uniqueFileName;
        }

        private List<ImageEntry> processImages(IFormFile[] uploadedImages, ref UserEntry user)
        {
            List<ImageEntry> images = new List<ImageEntry>();
            foreach (IFormFile uploadedImage in uploadedImages)
            {
                images.Add(new ImageEntry()
                {
                    ImagePath = uploadedImage.FileName,
                    User = user
                });
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
                    if (identityUser.EmailConfirmed == false)
                    {
                        return Redirect("/Error/403");
                    }

                    if (model.Images.Length > 2)
                    {
                        UserEntry entry = new UserEntry()
                        {
                            Address = model.Address,
                            CommutingWay = model.CommutingWay,
                            User = identityUser
                        };
                        entry.Images = processImages(model.Images, ref entry);

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
                    return View(model);
                }
            }
            return View();
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
        
        // GET: UserEntry/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserEntry/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserEntry/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserEntry/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}