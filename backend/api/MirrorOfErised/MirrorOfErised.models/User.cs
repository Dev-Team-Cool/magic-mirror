using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MirrorOfErised.models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        public UserEntry Commute { get; set; }
        public UserSettings Settings { get; set; }
        public List<ImageEntry> Images { get; set; }
        public bool HasCompletedSignUp { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool ForcedPasswordReset { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
}