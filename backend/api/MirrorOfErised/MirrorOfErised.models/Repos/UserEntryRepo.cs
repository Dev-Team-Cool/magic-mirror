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
    public class UserEntryRepo : IUserEntryRepo
    {
        private readonly ApplicationDbContext _context;

        public UserEntryRepo(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<UserEntry> AddEntry(UserEntry entry)
        {
            try
            { 
                await _context.UserEntry.AddAsync(entry);
                await _context.SaveChangesAsync();

                return entry;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<UserEntry> GetEntryForIdAsync(string Id)
        { 
            return await _context.UserEntry.Where(e => e.User.Id == Id).FirstOrDefaultAsync();
        }
    }
}
