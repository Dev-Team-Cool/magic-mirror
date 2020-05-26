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
        private readonly IUserEntryRepo userEntryrepo;

        public GoogleCalendarAPI GoogleCalendarAPI { get; }
        public IAuthTokenRepo AuthTokenRepo { get; }

        public UserEntryController(IUserEntryRepo userEntry, UserManager<IdentityUser> userManager, IHostingEnvironment hostingEnvironment , GoogleCalendarAPI googleCalendarAPI , IAuthTokenRepo authTokenRepo )   /*IUserEventRepo userEventRepo,*/
        {
            this.userEntryrepo = userEntry;

            _userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
            GoogleCalendarAPI = googleCalendarAPI;
            AuthTokenRepo = authTokenRepo;
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
        public async Task<ActionResult> Create(UserEntryCreateViewModel model )
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
                            Name = model.Name,
                            Image1Path = saveImage(model.Image1, identityUser),
                            Image2Path = saveImage(model.Image2, identityUser),
                            Image3Path = saveImage(model.Image3, identityUser),
                            UserId = identityUser.Id,
                            CommutingWay = model.CommutingWay


                        };

                        await userEntryrepo.AddImage(newEntry);
                    }


                    /*if (fileobj == null || fileobj.Length == 0)
                    {
                        ViewData["Message"] = "Please select atleast one image";
                    }

                    IdentityUser identityUser = await _userManager.GetUserAsync(User);
                    userEntry.UserId = identityUser.Id;*/


                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();


        }


        // settings page

        public ActionResult Settings()
        {
            return View();
        }



        // POST: UserEntry/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Settings(IFormCollection collection)
        {
            try
            {
               /* // Retrieve access token and refresh token from database
                IdentityUser user = await _userManager.GetUserAsync(User);

                AuthToken authToken = await AuthTokenRepo.GetTokensForNameAsync(user.UserName);



                // Get Key
                var filesResponse = await GoogleCalendarAPI.ListFiles(authToken.Token, authToken.RefreshToken, async token =>
                {
                    IdentityUser identityUser = await _userManager.GetUserAsync(User);

                    AuthToken Event = await AuthTokenRepo.GetTokensForNameAsync(identityUser.UserName);
                    dynamic Response = JsonConvert.DeserializeObject(token);
                    Event.Token = Response.access_token;
                    Event.ExpireDate = DateTime.Now.AddSeconds((int)Response.expires_in);

                    await AuthTokenRepo.UpdateTokenAsync(Event);

                });*/





                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
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