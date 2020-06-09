using System.ComponentModel.DataAnnotations;

namespace MirrorOfErised.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required] 
        public string Password { get; set; }
    }
}