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
using MirrorOfErised.models.Repos;
using Project.API.Models;


namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private const string AuthSchemes = CookieAuthenticationDefaults.AuthenticationScheme + ",Identity.Application"; /* "CombinationScheme," + CookieAuthenticationDefaults.AuthenticationScheme + ","+ JwtBearerDefaults.AuthenticationScheme;*/
        private readonly IAuthTokenRepo AuthTokenRepo;

        public UserManager<IdentityUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }


        public EventsController(IAuthTokenRepo AuthTokenRepo, UserManager<IdentityUser> IdentitiyUserManager, RoleManager<IdentityRole> roleManager)
        {
            this.AuthTokenRepo = AuthTokenRepo;
            UserManager = IdentitiyUserManager;
            RoleManager = roleManager;
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

        // GET: api/AuthToken/id
        [HttpGet("{UserName}")]
        /*        [Authorize(AuthenticationSchemes = AuthSchemes, Roles = "Admin")]*/
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AuthToken>> GetEvent(string UserName)
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

    }
}
