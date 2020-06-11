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
        private readonly IUserEntryRepo _userEntryRepo;
        private readonly IUserSettingsRepo _userSettings;
        private readonly UserManager<User> _userManager;
        
        public UserController(IAuthTokenRepo authTokenRepo, UserManager<User> userManager, IUserRepo userRepo,
            GoogleCalendarService googleCalendarApi, IUserEntryRepo userEntryRepo,IUserSettingsRepo userSettings)
        {
            _authTokenRepo = authTokenRepo;
            _userManager = userManager;
            _googleCalendarApi = googleCalendarApi;
            _userEntryRepo = userEntryRepo;
            _userSettings = userSettings;
            _userRepo = userRepo;
        }

        // GET: api/user/{username}
        [HttpGet("{userName}")]
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
        
        // GET: api/user/{username}/calendar
        [HttpGet("{userName}/calendar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetCalender(string userName)
        {
            try 
            {
                if (string.IsNullOrEmpty(userName))
                {
                    return BadRequest("Ongeldige EventId");
                }

                AuthToken authToken = await _authTokenRepo.GetTokensForNameAsync(userName);

                if (authToken == null)
                {
                    return NotFound("Geen tokens gevonden");
                }

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
        
        /*[HttpGet("Info/{UserName}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserEntry>> GetUserInfo(string userName)
        {
            try 
            {
                if (string.IsNullOrEmpty(userName))
                {
                    return BadRequest("Ongeldige user");
                }
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return NotFound("Geen user entry gevonden");

                }

                var userEntry = _userEntryRepo.GetEntryForIdAsync(user.Id);

                var userEntryDto = new UserEntryDto();
                Mapper.ConvertEntryTo_DTO(userEntry, ref userEntryDto);

                return Ok(userEntryDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Failed to get User");
            }
        }*/

        /*[HttpGet("Settings/{UserName}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult<UserSettings>> GetUserSettings(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    return BadRequest("Ongeldige user");
                }

                if (string.IsNullOrEmpty(userName))
                {
                    return BadRequest("Ongeldige user");
                }

                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return NotFound("Geen user entry gevonden");

                }
                
                var userEntry = _userSettings.GetSettingsForUserIdAsync(user.Id);
                
                var userSettingDto = new UserSettingDto();
                Mapper.ConvertSettingTo_DTO(userEntry, ref userSettingDto);

                return Ok(userSettingDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("failed to get setting");
            }
        }*/
    }
}
