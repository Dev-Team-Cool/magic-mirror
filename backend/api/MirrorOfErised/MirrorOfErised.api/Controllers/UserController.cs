using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MirrorOfErised.api.Models;
using MirrorOfErised.models;
using MirrorOfErised.models.Repos;
using MirrorOfErised.models.Services;
using Newtonsoft.Json;

namespace MirrorOfErised.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthTokenRepo _authTokenRepo;
        private readonly GoogleCalendarService _googleCalendarApi;
        private readonly IUserRepo _userRepo;
        
        public UserController(IAuthTokenRepo authTokenRepo, IUserRepo userRepo,
            GoogleCalendarService googleCalendarApi)
        {
            _authTokenRepo = authTokenRepo;
            _googleCalendarApi = googleCalendarApi;
            _userRepo = userRepo;
        }

        // GET: api/user/{username}
        [HttpGet("{userName}")]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUser(string userName)
        {
           try
           {
               if (string.IsNullOrEmpty(userName))
                   return BadRequest("Username is required");
               var userTokens = await _authTokenRepo.GetTokensForNameAsync(userName);
               if (userTokens == null)
                   return NotFound("User not found");
               var user = await _userRepo.GetUserByUsername(userName);
               return Ok(Mapper.ConvertToUserDto(ref user, ref userTokens));
           }
           catch (Exception ex)
           {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
           }
        }
        
        //TODO: Enable the Authorization

        // GET: api/user/{username}/calendar
        [HttpGet("{userName}/calendar")]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetCalender(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    return BadRequest("Username is required");

                AuthToken authToken = await _authTokenRepo.GetTokensForNameAsync(userName);
                if (authToken == null)
                    return NotFound("User not found");

                // Get Key
                var filesResponse = await _googleCalendarApi.ListFiles(authToken.Token, authToken.RefreshToken, async token =>
                {
                    AuthToken @event = await _authTokenRepo.GetTokensForNameAsync(userName);
                    dynamic response = JsonConvert.DeserializeObject(token);
                    @event.Token = response.access_token;
                    @event.ExpireDate = DateTime.Now.AddSeconds((int)response.expires_in);

                    await _authTokenRepo.UpdateTokenAsync(@event);

                });
                
                return Ok(filesResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Unable to get user calendar");
            }
        }
    }
}
