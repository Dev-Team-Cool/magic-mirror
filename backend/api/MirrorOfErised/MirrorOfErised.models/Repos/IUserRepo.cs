using System.Collections.Generic;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IUserRepo: IBaseRepo
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserById(string id);
        User Update(User user);
    }
}