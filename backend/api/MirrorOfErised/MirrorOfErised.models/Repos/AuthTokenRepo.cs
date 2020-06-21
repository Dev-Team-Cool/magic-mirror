using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MirrorOfErised.models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public class AuthTokenRepo: BaseRepo, IAuthTokenRepo
    {
        private readonly UserManager<User> _userManager;

        public AuthTokenRepo(ApplicationDbContext context, UserManager<User> userManager): base(context)
        {
            _userManager = userManager;
        }
        
        public async Task<AuthToken> AddTokens(IEnumerable<AuthenticationToken> tokens, IEnumerable<Claim> claims)
        {
            try
            {
                string userName = "";

                foreach (var claim in claims)
                {
                    if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                    {
                        userName = claim.Value;
                    }
                }

                var newOrExistingToken = await GetTokensForNameAsync(userName);
                bool existingTokenFound = newOrExistingToken != null;

                if (!existingTokenFound)
                {
                    User user = await _userManager.FindByNameAsync(userName);
                    if (user == null) return null; // No user is found, no token can be made
                    newOrExistingToken = new AuthToken {UserId = user.Id, UserName = userName};
                }
                
                foreach (var token in tokens)
                {
                    if (token.Name == "refresh_token")
                    {
                        newOrExistingToken.RefreshToken = token.Value;
                        
                    }
                    if (token.Name == "access_token")
                    {
                        newOrExistingToken.Token = token.Value;

                    }
                    if (token.Name == "expires_at")
                    {
                        newOrExistingToken.ExpireDate = DateTime.Parse(token.Value.ToString());
                    }
                }

                if (existingTokenFound)
                    _context.Tokens.Update(newOrExistingToken);
                else
                    await _context.Tokens.AddAsync(newOrExistingToken);
                
                await _context.SaveChangesAsync();
                return newOrExistingToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Task<IEnumerable<AuthToken>> GetTokensAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<AuthToken> GetTokensForNameAsync(string username)
        {
            return await _context.Tokens
                .Where(e => e.UserName == username)
                .OrderByDescending(e => e.ExpireDate)
                .Take(1)
                .FirstOrDefaultAsync();
        }
        
        public async Task UpdateTokenAsync(AuthToken authToken)
        {
            _context.Tokens.Update(authToken);
            await _context.SaveChangesAsync();
        }
    }
}
