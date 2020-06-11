using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MirrorOfErised.api.Models;

public class JWTServices<TEntity> where TEntity : IdentityUser
{
    private readonly ILogger _logger;
    private readonly UserManager<TEntity> _userManager;
    private readonly IPasswordHasher<TEntity> _hasher;
    private readonly IConfiguration _configuration;

    public JWTServices(IConfiguration configuration, ILogger logger, UserManager<TEntity> userManager, IPasswordHasher<TEntity> hasher)
    {
        _logger = logger;
        _userManager = userManager;
        _hasher = hasher;
        _configuration = configuration;
    }

    //returnen van een anoniem object
    public async Task<object> GenerateJwtToken(LoginDto identityDto)
    {
        //0. Geen gebruik van de signin manager voor het token-> deze signin manager 
        //maakt een autorisatie cookie aan . De signin manager gebruiken (Cookie 
        // authenticatie) zou kunnen als backup voor een falend token.

        //=> Token aanmaken na loggedin user  met extra claims + verplichte JWT claims.

        //1. Gebruiker opzoeken in de database met async UserManager en hash vergelijking

        try
        {
            var user = await _userManager.FindByNameAsync(identityDto.UserName);
            var roles = await _userManager.GetRolesAsync(user);

            if (user != null)
            {
                if (_hasher.VerifyHashedPassword(user, user.PasswordHash, identityDto.Password) == PasswordVerificationResult.Success)
                {
                    //generateToken code hier indien paswoord controle.
                    Console.Write("password verified: {0}", user.PasswordHash);
                }
                else
                {
                    return new { error = "Unknown user or password" };
                }
            }
            else
            {
                return new { error = "Unknown user or password" };
            }

            //2. claims (key/value) toevoegen
            //2.1 Customised of extra claims, komende van het Identity system (vb. rollen):
            var userClaims = await _userManager.GetClaimsAsync(user);
            await _userManager.RemoveClaimsAsync(user, userClaims);

            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
            //combined string van roles kan niet => ClaimTypes.Role
            foreach (var role in roles)
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
            }

            userClaims = await _userManager.GetClaimsAsync(user);


            //2.2. Noodzakelijke claims komende vd JWD spec
            var claims = new List<Claim>
             {
                //JWT claims zijn ingebouwd in de JWT spec: "sub"scriber, JWT Id
                 new Claim(JwtRegisteredClaimNames.Sub, identityDto.UserName),  //subscriber
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                /* //extra claims (waardoor toch datastore info beschikbaar wordt)
               new Claim(JwtRegisteredClaimNames.Birthdate, identityDTO.Birthdate.ToString())*/
             }.Union(userClaims);   //nog de extra userClaims toevoegen.

            //3. Sigin credentials met de symmetric key & encryptie methode
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //key en protocol

            //4. aanmaken van het token
            var token = new JwtSecurityToken(
             issuer: _configuration["Tokens:Issuer"],  //onze website
             audience: _configuration["Tokens:Audience"],//gebruikers
             claims: claims,
             expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Tokens: Expires"])),
             signingCredentials: creds //controleert token v
             );

            //5. user info returnen (vervaldatum als additionele info)
            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token), //token generator
                expiration = token.ValidTo
            };
        }
        catch (Exception exc)
        {
            _logger.LogError($"Exception thrown when creating JWT: {exc}");
        }
        return new { error = "Failed to generate JWT token" }; //minimale info ->meer in de logger
    }
}