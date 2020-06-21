using MirrorOfErised.models.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MirrorOfErised.models.Repos
{
    public class UserSettingsRepo: BaseRepo, IUserSettingsRepo
    {
        public UserSettingsRepo(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<UserSettings> AddSetting(UserSettings settings)
        {
            await _context.UserSettings.AddAsync(settings);
            await _context.SaveChangesAsync();

            return settings;
        }

        public async Task<UserSettings> GetSettingsForUserIdAsync(string id)
        {
            try
            {
                return await _context.UserSettings.Where(e => e.UserId == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public UserSettings Update(UserSettings settings)
        {
            _context.UserSettings.Update(settings);
            return settings;
        }
    }
}
