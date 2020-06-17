using System.ComponentModel.DataAnnotations;

namespace MirrorOfErised.api.Models
{
    public class LoginDto
    {
        //Basis voor CookieAutenticate 
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }


        //voor JWT
        public string Email { get; set; }
    }
}
