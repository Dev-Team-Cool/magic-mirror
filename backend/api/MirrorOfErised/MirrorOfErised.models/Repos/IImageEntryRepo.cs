using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IImageEntryRepo
    {
        Task<ImageEntry> AddImage(ImageEntry image);
    }
}