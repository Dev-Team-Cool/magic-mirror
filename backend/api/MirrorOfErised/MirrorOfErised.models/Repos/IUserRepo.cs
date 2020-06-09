using System.Collections.Generic;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IUserRepo
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task<User> Update(User user);
    }
}