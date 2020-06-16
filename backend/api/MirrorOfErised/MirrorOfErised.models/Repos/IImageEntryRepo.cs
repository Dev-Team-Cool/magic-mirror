using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IImageEntryRepo: IBaseRepo
    {
        Task<int> CountImagesForUser(string userId);
        Task<ImageEntry> AddImage(ImageEntry image);
    }
}