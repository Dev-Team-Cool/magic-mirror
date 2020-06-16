using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MirrorOfErised.models;
using MirrorOfErised.models.Repos;

namespace MirrorOfErised.Areas.Identity.Pages.Account.Manage
{
    public partial class PhotosModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IImageEntryRepo _imageEntryRepo;
        private readonly IConfiguration _configuration;
        
        [BindProperty]
        public List<ImageEntry> Images { get; set; }

        public string ImagesPath { get; set; }

        public User LoggedInUser { get; set; }
        
        public PhotosModel(UserManager<User> userManager, IImageEntryRepo imageEntryRepo,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _imageEntryRepo = imageEntryRepo;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("No user found.");
            }

            LoggedInUser = user;
            ImagesPath = _configuration["UploadConfig:ProcessedFolder"];
            Images = await _imageEntryRepo.GetImagesForUserId(user.Id);

            return Page();
        }
    }
}