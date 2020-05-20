using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MirrorOfErised.models.Data;
using System;
using System.Collections.Generic;
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


                var result = await context.Tokens.AddAsync(selected); //cahngeTraking => iets wat in geheugen wordt bijgehouden
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
    }
}
