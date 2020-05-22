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
        private readonly UserManager<IdentityUser> usermanager;

        //wel dependend van SchoolDbContext ( niet DbContext)
        public AuthTokenRepo(ApplicationDbContext Context, UserManager<IdentityUser> Usermanager)
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
                IdentityUser user = await usermanager.FindByNameAsync(selected.UserName);
                selected.UserId = user.Id;

                /*                if (SelectedWaardering.WaarderingId is null)
                                {
                                    SelectedWaardering.WaarderingId = Guid.NewGuid().ToString();
                                }*/
                try
                {
                    var result = await context.Tokens.AddAsync(selected);
                }
                catch (Exception)
                {
                    var result = context.Tokens.Update(selected);
                }
                 //cahngeTraking => iets wat in geheugen wordt bijgehouden
                 //cahngeTraking => iets wat in geheugen wordt bijgehouden
                await context.SaveChangesAsync();

                

                /*return result != OK*/
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

        public async Task<AuthToken> GetTokensForNameAsync(string Username)
        {

            var User = context.Tokens.Where(e => e.UserName == Username).OrderByDescending(e => e.ExpireDate).Take(1);

            return User.FirstOrDefault();
        }
    }
}
