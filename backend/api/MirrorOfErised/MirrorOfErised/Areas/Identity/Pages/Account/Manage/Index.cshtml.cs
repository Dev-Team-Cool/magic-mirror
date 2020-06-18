using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MirrorOfErised.models;
using MirrorOfErised.models.Repos;
using MirrorOfErised.ViewModels;

namespace MirrorOfErised.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserEntryRepo _userEntryRepo;
        private readonly IUserSettingsRepo _userSettingsRepo;
        private readonly IUserRepo _userRepo;

        public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager,
            IUserEntryRepo userEntryRepo, IUserSettingsRepo userSettingsRepo, IUserRepo userRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userEntryRepo = userEntryRepo;
            _userSettingsRepo = userSettingsRepo;
            _userRepo = userRepo;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public UserEntryCreateViewModel Input { get; set; }

        private async Task LoadAsync(User user)
        {
            user = await _userRepo.GetUserByUsername(user.UserName);

            Username = user.UserName;

            Input = new UserEntryCreateViewModel()
            {
                Assistant = user.Settings.Assistant,
                Calendar = user.Settings.Calendar,
                City = user.Commute.Address.City,
                Street = user.Commute.Address.Street,
                ZipCode = user.Commute.Address.ZipCode,
                Commute = user.Settings.Commuting,
                CommutingWay = user.Commute.CommutingWay,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.IsActive = Input.AllowRecognition;

            UserEntry entry = await _userEntryRepo.GetEntryForIdAsync(user.Id);
            entry.CommutingWay = Input.CommutingWay;
            entry.Address.Street = Input.Street;
            entry.Address.City = Input.City;
            entry.Address.ZipCode = Input.ZipCode;

            UserSettings settings = await _userSettingsRepo.GetSettingsForUserIdAsync(user.Id);
            settings.Assistant = Input.Assistant;
            settings.Calendar = Input.Calendar;
            settings.Commuting = Input.Commute;

            _userRepo.Update(user);
            _userEntryRepo.Update(entry);
            _userSettingsRepo.Update(settings);
            await _userSettingsRepo.SaveAsync(); //Flush changes
            
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
