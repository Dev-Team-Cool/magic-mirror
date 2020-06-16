using MirrorOfErised.models.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MirrorOfErised.models.Repos
{
    public class UserEntryRepo: BaseRepo, IUserEntryRepo
    {
        public UserEntryRepo(ApplicationDbContext context): base(context)
        {
        }
        
        public async Task<UserEntry> AddEntry(UserEntry entry)
        {
            await _context.UserEntry.AddAsync(entry); 
            await _context.SaveChangesAsync();

            return entry;
        }

        public UserEntry Update(UserEntry entry)
        {
            _context.UserEntry.Update(entry);
            return entry;
        }

        public async Task<UserEntry> GetEntryForIdAsync(string Id)
        { 
            return await _context.UserEntry
                .Include(e => e.Address)
                .Where(e => e.User.Id == Id)
                .FirstOrDefaultAsync();
        }
    }
}
