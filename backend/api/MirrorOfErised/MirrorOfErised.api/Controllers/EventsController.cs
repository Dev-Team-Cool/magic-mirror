using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MirrorOfErised.api.Models;
using MirrorOfErised.models;
using MirrorOfErised.models.Repos;
using Newtonsoft.Json;
using Project.API.Models;


namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
/*        private const string AuthSchemes = CookieAuthenticationDefaults.AuthenticationScheme + ",Identity.Application"; *//* "CombinationScheme," + CookieAuthenticationDefaults.AuthenticationScheme + ","+ JwtBearerDefaults.AuthenticationScheme;*/
        private readonly IAuthTokenRepo AuthTokenRepo;
        private readonly GoogleCalendarAPI googleCalendarAPI;
        private readonly IUserEntryRepo userEntryRepo;
        private readonly IUserSettingsRepo userSettings;

        public UserManager<IdentityUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }


        public EventsController(IAuthTokenRepo AuthTokenRepo, UserManager<IdentityUser> IdentitiyUserManager, RoleManager<IdentityRole> roleManager,GoogleCalendarAPI googleCalendarAPI, IUserEntryRepo userEntryRepo,IUserSettingsRepo userSettings)
        {
            this.AuthTokenRepo = AuthTokenRepo;
            UserManager = IdentitiyUserManager;
            RoleManager = roleManager;
            this.googleCalendarAPI = googleCalendarAPI;
            this.userEntryRepo = userEntryRepo;
            this.userSettings = userSettings;
        }



        /*        // GET: api/Events
                [HttpGet]
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


                public async Task<ActionResult<IEnumerable<AuthToken>>> GetEvents()
                {
                    //1. entiteiten ophalen
                    var model = await AuthTokenRepo.GetAllEventsAsync();

                    //2. Mapping naar DTO
                    List<AuthToken_DTO> model_DTO = new List<AuthToken_DTO>();

                    foreach (AuthToken item in model)
                    {
                        var result = new AuthToken_DTO();
                        model_DTO.Add(Mapper.ConvertTo_DTO(item, ref result));
                    }

                    //3. DTO returnen ( en niet de entiteiten)
                    return Ok(model_DTO);

                }*/

        // GET: api/AuthToken/username

        /*        [Authorize(AuthenticationSchemes = AuthSchemes, Roles = "Admin")]*/
        [HttpGet("{UserName}")]
/*        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
*/        public async Task<ActionResult<AuthToken>> GetEvent(string UserName)
        {
            try 
            { 
                if (string.IsNullOrEmpty(UserName))
                {
                    return BadRequest("Ongeldige EventId");
                }

                var Event = await AuthTokenRepo.GetTokensForNameAsync(UserName);

                if (Event == null)
                {
                    return NotFound("Geen tokens gevonden");
                }
                var Event_DTO = new AuthToken_DTO();
                Mapper.ConvertTo_DTO(Event, ref Event_DTO);

                return Ok(Event_DTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Unable to get data");
            }
}



        /*        [Authorize(AuthenticationSchemes = AuthSchemes, Roles = "Admin")]*/
        // GET: api/AuthToken/id
        [HttpGet("Calendar/{UserName}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetCalender(string UserName)
        {
            try 
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    return BadRequest("Ongeldige EventId");
                }

                AuthToken authToken = await AuthTokenRepo.GetTokensForNameAsync(UserName);

                if (authToken == null)
                {
                    return NotFound("Geen tokens gevonden");
                }

                // Get Key
                var filesResponse = await googleCalendarAPI.ListFiles(authToken.Token, authToken.RefreshToken, async token =>
                {
                    AuthToken Event = await AuthTokenRepo.GetTokensForNameAsync(UserName);
                    dynamic Response = JsonConvert.DeserializeObject(token);
                    Event.Token = Response.access_token;
                    Event.ExpireDate = DateTime.Now.AddSeconds((int)Response.expires_in);

                    await AuthTokenRepo.UpdateTokenAsync(Event);

                });


            

                return Ok(filesResponse);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Unable to get user calendar");
            }
        }



        [HttpGet("Info/{UserName}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserEntry>> GetUserInfo(string UserName)
        {
            try 
            {
                IdentityUser user = new IdentityUser();
                if (string.IsNullOrEmpty(UserName))
                {
                    return BadRequest("Ongeldige user");
                }
                user = await UserManager.FindByNameAsync(UserName);
                if (user == null)
                {
                    return NotFound("Geen user entry gevonden");

                }

                var userEntry = userEntryRepo.GetEntryForIdAsync(user.Id);

                var UserEntry_DTO = new UserEntry_DTO();
                Mapper.ConvertEntryTo_DTO(userEntry, ref UserEntry_DTO);

                return Ok(UserEntry_DTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Failed to get User");
            }
        }



        
        [HttpGet("Settings/{UserName}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult<UserSettings>> GetUserSettings(string UserName)
        {
            try
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    return BadRequest("Ongeldige user");
                }

                IdentityUser user = new IdentityUser();
                if (string.IsNullOrEmpty(UserName))
                {
                    return BadRequest("Ongeldige user");
                }

                user = await UserManager.FindByNameAsync(UserName);
                if (user == null)
                {
                    return NotFound("Geen user entry gevonden");

                }




                var userEntry = userSettings.GetSettingsForUserIdAsync(user.Id);


                var UserSetting_DTO = new UserSetting_DTO();
                Mapper.ConvertSettingTo_DTO(userEntry, ref UserSetting_DTO);

                return Ok(UserSetting_DTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("failed to get setting");
            }
        }


    }
}

