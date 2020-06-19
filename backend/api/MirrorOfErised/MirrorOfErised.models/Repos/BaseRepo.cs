using System.Threading.Tasks;
using MirrorOfErised.models.Data;

namespace MirrorOfErised.models.Repos
{
    public class BaseRepo: IBaseRepo
    {
        protected readonly ApplicationDbContext _context;

        protected BaseRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}