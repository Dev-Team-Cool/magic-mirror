using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IUserEntryRepo: IBaseRepo
    {
        Task<UserEntry>AddEntry(UserEntry entry);
        Task<UserEntry> GetEntryForIdAsync(string username);

    }
}
