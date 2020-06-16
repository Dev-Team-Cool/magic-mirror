using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MirrorOfErised.models.Data;

namespace MirrorOfErised.models.Repos
{
    public class ImageEntryRepo: BaseRepo, IImageEntryRepo
    {
        public ImageEntryRepo(ApplicationDbContext context): base(context)
        {
        }

        public async Task<int> CountImagesForUser(string userId)
        {
            return await _context.UserImages
                .Where(i => i.User.Id == userId )
                .CountAsync();
        }

        public async Task<List<ImageEntry>> GetImagesForUserId(string userId)
        {
            return await _context.UserImages
                .Where(i => i.User.Id == userId)
                .ToListAsync();
        }

        public async Task<ImageEntry> AddImage(ImageEntry image)
        {
            await _context.UserImages.AddAsync(image);
            await _context.SaveChangesAsync();

            return image;
        }
    }
}