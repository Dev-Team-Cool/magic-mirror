using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MirrorOfErised.Areas.Identity.Pages.Account.Manage
{
    public class UploadPhotosModel : PageModel
    {
        public IActionResult OnGetAsync()
        {
            return Page();
        }
    }
}