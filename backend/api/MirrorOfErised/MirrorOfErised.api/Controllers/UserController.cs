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
        private readonly GoogleService _googleService;
        private readonly IUserRepo _userRepo;
        
        public UserController(IAuthTokenRepo authTokenRepo, IUserRepo userRepo,
            GoogleCalendarService googleCalendarApi, GoogleService googleService)
        {
            _authTokenRepo = authTokenRepo;
            _googleCalendarApi = googleCalendarApi;
            _googleService = googleService;
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
               
               var token = await _authTokenRepo.GetTokensForNameAsync(userName);
               if (token == null)
                   return NotFound("User not found");

               if (token.ExpireDate < DateTime.Now) // Token is invalid now, request a new one
                   token = await _googleService.RequestNewAccessToken(token);
               
               var user = await _userRepo.GetUserByUsername(userName);
               if (!user.HasCompletedSignUp) return NotFound("User not yet active.");
               return Ok(Mapper.ConvertToUserDto(ref user, ref token));
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

                // Get calender events
                string calendarEventsJson = await _googleCalendarApi.GetCalendarEvents(authToken);
                if (string.IsNullOrEmpty(calendarEventsJson))
                    return StatusCode(500, "Failed to get events.");
                
                return Ok(calendarEventsJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Unable to get user calendar");
            }
        }
    }
}
