using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MirrorOfErised.models.Repos;

namespace MirrorOfErised.models.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {


        }

        public virtual DbSet<AuthToken> Tokens { get; set; }
        public virtual DbSet<UserEntry> UserEntry { get; set; }
        public virtual DbSet<UserSettings> UserSettings { get; set; }
    }
}
