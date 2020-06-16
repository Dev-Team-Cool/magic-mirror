using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MirrorOfErised.models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        
        public async Task<AuthToken> AddTokens(List<AuthenticationToken> tokens, List<Claim> claims)
        {
            try
            {
                AuthToken selected = new AuthToken();
                foreach (var token in tokens)
                {
                    if (token.Name == "refresh_token")
                    {
                        selected.RefreshToken = token.Value;
                        
                    }
                    if (token.Name == "access_token")
                    {
                        selected.Token = token.Value;

                    }
                    if (token.Name == "expires_at")
                    {
                        selected.ExpireDate = DateTime.Parse(token.Value.ToString());
                    }
                }
                
                foreach (var claim in claims)
                {
                    if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                    {
                        selected.UserName = claim.Value;
                    }
                }
                
                User user = await _userManager.FindByNameAsync(selected.UserName);
                selected.UserId = user.Id;

                try
                {
                    var result = await _context.Tokens.AddAsync(selected);
                }
                catch (Exception)
                {
                    var result = _context.Tokens.Update(selected);
                }
                
                await _context.SaveChangesAsync();
                return selected;
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
