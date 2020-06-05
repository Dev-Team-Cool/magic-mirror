using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Project.API.Models;
namespace Project.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private  readonly IConfiguration configuration;
        IPasswordHasher<IdentityUser> hasher;
        UserManager<IdentityUser> IdentityUserManager;
        ILogger<AuthController> logger;


        public AuthController(SignInManager<IdentityUser> signInMgr, IPasswordHasher<IdentityUser> hasher, UserManager<IdentityUser> IdentityUserManager, ILogger<AuthController> logger , IConfiguration configuration)
        {
            this._signInManager = signInMgr;
            this.hasher = hasher;
            this.IdentityUserManager = IdentityUserManager;
            this.logger = logger;
            this.configuration = configuration;

        }

       

        /*[HttpPost]
        [Route("login")] //vult de controller basis route aan
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            //LoginViewModel met (Required) IdentityUserName en Password aanbrengen.
            var returnMessage = "";
            if (!ModelState.IsValid)
                return BadRequest("Onvolledige gegevens.");
            try
            {
                //geen persistence, geen lockout -> via false, false
                var result = await
               _signInManager.PasswordSignInAsync(loginDTO.UserName,
               loginDTO.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok("Welkom " + loginDTO.UserName);
*//*                    return Ok("Welkom " + identityDTO.IdentityUserName);
*//*                }
                throw new Exception("IdentityUser of paswoord niet gevonden.");
                //zo algemeen mogelijk response. Vertelt niet dat het pwd niet juist is.
            }
            catch (Exception exc)
            {
                returnMessage = $"Foutief of ongeldig request: {exc.Message}";
                ModelState.AddModelError("", returnMessage);
            }
            return BadRequest(returnMessage); //zo weinig mogelijk (hacker) info
        }*/

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateJwtToken([FromBody]LoginDTO
 identityDTO)
        {
            try
            {
                var jwtsvc = new JWTServices<IdentityUser>(configuration,
                logger, IdentityUserManager, hasher);
                var token = await jwtsvc.GenerateJwtToken(identityDTO);
                return Ok(token);
            }
            catch (Exception exc)
            {
                logger.LogError($"Exception thrown when creating JWT: {exc}");
            }
            //Bij niet succesvolle authenticatie wordt een Badrequest (=zo weinig mogelijke info) teruggeven.
return BadRequest("Failed to generate JWT token");
        }



    }
}