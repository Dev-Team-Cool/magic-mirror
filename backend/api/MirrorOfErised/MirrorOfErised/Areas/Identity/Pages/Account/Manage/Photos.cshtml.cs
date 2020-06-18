using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MirrorOfErised.models;
using MirrorOfErised.models.Repos;

namespace MirrorOfErised.Areas.Identity.Pages.Account.Manage
{
    [ValidateAntiForgeryToken]
    [Authorize]
    public partial class PhotosModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IImageEntryRepo _imageEntryRepo;
        private readonly IConfiguration _configuration;
        
        [BindProperty]
        public List<ImageEntry> Images { get; set; }

        public string StatusMessage { get; set; }
        public string ImagesPath { get; set; }
        [BindProperty]
        public int SelectedImageId { get; set; }

        public User LoggedInUser { get; set; }
        
        public PhotosModel(UserManager<User> userManager, IImageEntryRepo imageEntryRepo,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _imageEntryRepo = imageEntryRepo;
            _configuration = configuration;
        }

        private async Task LoadImages(User user)
        {
            LoggedInUser = user;
            ImagesPath = _configuration["UploadConfig:ProcessedFolder"];
            Images = await _imageEntryRepo.GetImagesForUserId(user.Id);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("No user found.");
            }
            
            await LoadImages(user);
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                var image = await _imageEntryRepo.GetImageById(SelectedImageId);

                if (image == null || image.User.Id != user.Id)
                {
                    // User wants to remove an image that is not his/hers.
                    StatusMessage = "Cannot remove this image.";
                    await LoadImages(user);
                    return Page();
                }

                image.IsValid = false;
                _imageEntryRepo.Update(image);
                await _imageEntryRepo.SaveAsync();
                return RedirectToPage();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                StatusMessage = "Something unexpected happened";
                await LoadImages(user);
                return Page();
            }
        }
    }
}