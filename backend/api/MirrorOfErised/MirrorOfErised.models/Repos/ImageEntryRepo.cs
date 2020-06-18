using System;
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

        public async Task<bool> NeedsTraining()
        {
            int count = await _context.UserImages
                .Where(i => !i.IsProcessed && i.IsValid)
                .CountAsync();

            return count > 0;
        }

        public async Task<ImageEntry> GetImageById(int id)
        {
            return await _context.UserImages
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ImageEntry>> GetImagesForUserId(string userId)
        {
            return await _context.UserImages
                .Where(i => i.User.Id == userId)
                .ToListAsync();
        }

        public async Task<List<ImageEntry>> GetAllUnprocessedImages(DateTime? before = null)
        {
            //FIXME: there is probably a better way to this
            if (before != null)
            {
                return await _context.UserImages
                    .Where(i => !i.IsProcessed && i.IsValid && i.CreatedAt < before)
                    .ToListAsync();   
            }
            
            return await _context.UserImages
                .Where(i => !i.IsProcessed && i.IsValid)
                .ToListAsync();
        }

        public async Task<ImageEntry> AddImage(ImageEntry image)
        {
            await _context.UserImages.AddAsync(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public void Update(ImageEntry image)
        {
            _context.UserImages.Update(image);
        }

        public void UpdateAll(List<ImageEntry> images)
        {
            foreach (var image in images)
            {
                Update(image);
            }
        }
    }
}