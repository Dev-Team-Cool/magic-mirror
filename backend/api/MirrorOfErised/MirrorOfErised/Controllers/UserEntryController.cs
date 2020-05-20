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

namespace MirrorOfErised.Controllers
{
    public class UserEntryController : Controller
    {


        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IUserEntryRepo userEntryrepo;

        public UserEntryController(IUserEntryRepo userEntry, UserManager<IdentityUser> userManager, IHostingEnvironment hostingEnvironment )   /*IUserEventRepo userEventRepo,*/
        {
            this.userEntryrepo = userEntry;

            _userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }
        // GET: UserEntry
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserEntry/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserEntry/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserEntry/Create
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
                    if (model.Image1 !=null)
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + identityUser.UserName + "_" + model.Image1.FileName ;
                        string FilePath = Path.Combine(uploadsFolder, uniqueFileName);
                        model.Image1.CopyTo(new FileStream(FilePath, FileMode.Create));
                    }

                    UserEntry newEntry = new UserEntry
                    {
                        Name = model.Name,
                        Image1Path = uniqueFileName,
                        UserId = identityUser.Id


                    };

                    userEntryrepo.AddImage(newEntry);
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