using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IBaseRepo
    {
        Task SaveAsync();
    }
}