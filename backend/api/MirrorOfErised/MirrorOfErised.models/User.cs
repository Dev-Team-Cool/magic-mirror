using Microsoft.AspNetCore.Identity;

namespace MirrorOfErised.models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool HasCompletedSignUp { get; set; } = false;
    }
}