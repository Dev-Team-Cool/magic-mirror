using System.Threading.Tasks;
using MirrorOfErised.models.Data;

namespace MirrorOfErised.models.Repos
{
    public class ImageEntryRepo: IImageEntryRepo
    {
        private readonly ApplicationDbContext _context;

        public ImageEntryRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<ImageEntry> AddImage(ImageEntry image)
        {
            await _context.UserImages.AddAsync(image);
            await _context.SaveChangesAsync();

            return image;
        }
    }
}