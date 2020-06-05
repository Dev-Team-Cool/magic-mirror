using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IUserEntryRepo
    {
        Task<UserEntry>AddEntry(UserEntry entry);
        UserEntry GetEntryForIdAsync(string username);

    }
}
