using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MirrorOfErised.models.Data;

namespace MirrorOfErised.models.Repos
{
    public class UserRepo: BaseRepo, IUserRepo
    {
        public UserRepo(ApplicationDbContext context): base(context)
        {
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.
                OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users
                .Include(u => u.Commute)
                .ThenInclude(c => c.Address)
                .Include(u => u.Settings)
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}