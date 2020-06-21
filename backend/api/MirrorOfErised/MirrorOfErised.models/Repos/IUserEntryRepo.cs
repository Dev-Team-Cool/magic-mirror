using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IUserEntryRepo: IBaseRepo
    {
        Task<UserEntry> GetEntryForIdAsync(string username);

        Task<UserEntry> AddEntry(UserEntry entry);
        UserEntry Update(UserEntry entry);
    }
}
