using System.Collections.Generic;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IUserRepo
    {
        Task<List<User>> GetAllUsers();
    }
}