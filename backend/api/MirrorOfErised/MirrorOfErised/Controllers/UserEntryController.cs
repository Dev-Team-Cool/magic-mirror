using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MirrorOfErised.models;
using MirrorOfErised.models.Repos;
using MirrorOfErised.ViewModels;
using Newtonsoft.Json;

namespace MirrorOfErised.Controllers
{
    public class UserEntryController : Controller
    {


        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IUserSettingsRepo userSettingsRepo;
        private readonly IUserEntryRepo userEntryrepo;

        public GoogleCalendarAPI GoogleCalendarAPI { get; }
        public IAuthTokenRepo AuthTokenRepo { get; }

        public UserEntryController(IUserEntryRepo userEntry, UserManager<IdentityUser> userManager, IHostingEnvironment hostingEnvironment , GoogleCalendarAPI googleCalendarAPI , IAuthTokenRepo authTokenRepo ,IUserSettingsRepo userSettingsRepo )   /*IUserEventRepo userEventRepo,*/
        {
            this.userEntryrepo = userEntry;

            _userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
            GoogleCalendarAPI = googleCalendarAPI;
            AuthTokenRepo = authTokenRepo;
            this.userSettingsRepo = userSettingsRepo;
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
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + identityUser.UserName + "_" + image.FileName;
            string FilePath = Path.Combine(uploadsFolder, uniqueFileName);
            image.CopyTo(new FileStream(FilePath, FileMode.Create));
            return uniqueFileName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  
        public async Task<ActionResult> Create(UserEntryCreateViewModel model)
        {
 
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = null;
                    IdentityUser identityUser = await _userManager.GetUserAsync(User);
                    if (identityUser.EmailConfirmed == false)
                    {
                        return Redirect("/Error/403");
                    }

                    if (model.Image1 != null & model.Image2 != null & model.Image3 != null)
                    {
                        UserEntry newEntry = new UserEntry
                        {
                            Address = model.Address,
/*                            Name = model.Name,
*/                            Image1Path = saveImage(model.Image1, identityUser),
                            Image2Path = saveImage(model.Image2, identityUser),
                            Image3Path = saveImage(model.Image3, identityUser),
                            UserId = identityUser.Id,
                            CommutingWay = model.CommutingWay


                        };

                        await userEntryrepo.AddEntry(newEntry);


                        if (userSettingsRepo.GetSettingsForUserIdAsync(identityUser.Id) == null)
                        {
                            UserSettings userSettings = new UserSettings
                            {
                                Assistant = true,
                                Calendar = true,
                                Commuting = true,
                                UserId = identityUser.Id
                            };

                            var UpdatetSetting = await userSettingsRepo.AddSetting(userSettings);
                        }


                    }


                    /*if (fileobj == null || fileobj.Length == 0)
                    {
                        ViewData["Message"] = "Please select atleast one image";
                    }

                    IdentityUser identityUser = await _userManager.GetUserAsync(User);
                    userEntry.UserId = identityUser.Id;*/


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

        public ActionResult Settings()
        {
            var id= _userManager.GetUserId(User);
            UserSettings setting = userSettingsRepo.GetSettingsForUserIdAsync(id);

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

                var user = await _userManager.GetUserAsync(User);
                settings.UserId = user.Id;
                settings.identityUser = user;

                var UpdatetSetting = await userSettingsRepo.UpdateSetting(settings);


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