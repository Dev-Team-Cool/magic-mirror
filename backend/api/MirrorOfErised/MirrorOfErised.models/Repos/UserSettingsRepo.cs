using Microsoft.AspNetCore.Identity;
using MirrorOfErised.models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public class UserSettingsRepo : IUserSettingsRepo
    {

        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> usermanager;

        //wel dependend van SchoolDbContext ( niet DbContext)
        public UserSettingsRepo(ApplicationDbContext Context, UserManager<IdentityUser> Usermanager)
        {
            this.context = Context;
            this.usermanager = Usermanager;
        }
        public async Task<UserSettings> AddSetting(UserSettings settings)
        {
            try
            {
                var result = context.UserSettings.Add(settings);  //cahngeTraking => iets wat in geheugen wordt bijgehouden
                await context.SaveChangesAsync();

                //return result != OK
                return settings;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public UserSettings GetSettingsForUserIdAsync(string id)
        {
            try
            {

            UserSettings entry = context.UserSettings.Where(e => e.UserId == id).FirstOrDefault();

            return entry;

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
                var result = context.UserSettings.Update(settings);  //cahngeTraking => iets wat in geheugen wordt bijgehouden
                await context.SaveChangesAsync();

                //return result != OK
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
