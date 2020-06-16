using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MirrorOfErised.models.Repos;

namespace MirrorOfErised.models.Data
{
    public class ApplicationDbContext: IdentityDbContext<User, IdentityRole, string, IdentityUserClaim<string>, IdentityUserRole<string>,IdentityUserLogin<string>,IdentityRoleClaim<string>,IdentityUserToken<string>>
    {
        public virtual DbSet<AuthToken> Tokens { get; set; }
        public virtual DbSet<UserEntry> UserEntry { get; set; }
        public virtual DbSet<UserSettings> UserSettings { get; set; }
        public virtual DbSet<ImageEntry> UserImages { get; set; }
        public virtual DbSet<TrainJob> TrainJobs { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ImageEntry>(entity =>
            {
                entity.Property(i => i.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<User>(user =>
            {
                user.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<TrainJob>(job =>
            {
                job.Property(j => j.StartedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<UserSettings>(settings =>
            {
                settings.Property(s => s.Assistant)
                    .HasDefaultValue(false);

                settings.Property(s => s.Calendar)
                    .HasDefaultValue(false);

                settings.Property(s => s.Commuting)
                    .HasDefaultValue(false);
            });
        }
    }
}
