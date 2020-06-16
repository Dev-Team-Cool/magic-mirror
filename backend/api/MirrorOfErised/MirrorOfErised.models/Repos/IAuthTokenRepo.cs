using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IAuthTokenRepo: IBaseRepo
    {
        Task<AuthToken> AddTokens(List<AuthenticationToken> tokens, List<Claim> claims);
        Task<IEnumerable<AuthToken>> GetTokensAsync();
        Task<AuthToken> GetTokensForNameAsync(string name);
        Task UpdateTokenAsync(AuthToken authToken);
        
    }
}
