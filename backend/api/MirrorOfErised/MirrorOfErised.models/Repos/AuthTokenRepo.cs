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
    public class AuthTokenRepo : IAuthTokenRepo
    {

        private readonly ApplicationDbContext context;
        private readonly UserManager<User> usermanager;

        public AuthTokenRepo(ApplicationDbContext Context, UserManager<User> Usermanager)
        {
            this.context = Context;
            this.usermanager = Usermanager;
        }
        
        public async Task<AuthToken> Addtokens(List<AuthenticationToken> Tokens, List<Claim> claims)
        {
            try
            {
                AuthToken selected = new AuthToken();
                foreach (var token in Tokens)
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
                
                User user = await usermanager.FindByNameAsync(selected.UserName);
                selected.UserId = user.Id;

                try
                {
                    var result = await context.Tokens.AddAsync(selected);
                }
                catch (Exception)
                {
                    var result = context.Tokens.Update(selected);
                }
                
                await context.SaveChangesAsync();
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

            var user = context.Tokens.Where(e => e.UserName == username).OrderByDescending(e => e.ExpireDate).Take(1);

            return await user.FirstOrDefaultAsync();
        }
        
        public async Task UpdateTokenAsync(AuthToken authToken)
        {
            try
            {
                var result = context.Tokens.Update(authToken);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
