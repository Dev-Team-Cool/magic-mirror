using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public class UserEntryRepo : IUserEntryRepo
    {
        public Task<AuthToken> AddImage(UserEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}
