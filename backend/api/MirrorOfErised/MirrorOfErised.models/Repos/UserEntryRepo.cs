using Microsoft.AspNetCore.Identity;
using MirrorOfErised.models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public class UserEntryRepo : IUserEntryRepo
    {

        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> usermanager;

        //wel dependend van SchoolDbContext ( niet DbContext)
        public UserEntryRepo(ApplicationDbContext Context, UserManager<IdentityUser> Usermanager)
        {
            this.context = Context;
            this.usermanager = Usermanager;
        }
        public async Task<UserEntry> AddEntry(UserEntry entry)
        {
            try
            {
                var result = context.UserEntry.Add(entry);  //cahngeTraking => iets wat in geheugen wordt bijgehouden
                await context.SaveChangesAsync();

                //return result != OK
                return entry;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public UserEntry GetEntryForIdAsync(string Id)
        {
            UserEntry entry = context.UserEntry.Where(e => e.UserId == Id).FirstOrDefault();

            return entry;
            
        }
    }
}
