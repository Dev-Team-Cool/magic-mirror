using Microsoft.AspNetCore.Identity;
using MirrorOfErised.models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MirrorOfErised.models.Repos
{
    public class UserSettingsRepo : IUserSettingsRepo
    {

        private readonly ApplicationDbContext context;

        public UserSettingsRepo(ApplicationDbContext Context, UserManager<User> Usermanager)
        {
            this.context = Context;
        }
        public async Task<UserSettings> AddSetting(UserSettings settings)
        {
            try
            {
                var result = context.UserSettings.Add(settings);
                await context.SaveChangesAsync();

                return settings;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<UserSettings> GetSettingsForUserIdAsync(string id)
        {
            try
            {
                return await context.UserSettings.Where(e => e.UserId == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<UserSettings> UpdateSetting(UserSettings settings)
        {
            try
            {
                var result = context.UserSettings.Update(settings);
                await context.SaveChangesAsync();

                return settings;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
