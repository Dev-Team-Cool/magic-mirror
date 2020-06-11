using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MirrorOfErised.api.Models;
using MirrorOfErised.models;

namespace MirrorOfErised.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private  readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _hasher;
        private readonly UserManager<User> _identityUserManager;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(IPasswordHasher<User> hasher, UserManager<User> userManager, 
            ILogger<AuthController> logger , IConfiguration configuration)
        {
            _hasher = hasher;
            _identityUserManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }
        
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateJwtToken([FromBody]LoginDto identityDto)
        {
            try
            {
                var jwtsvc = new JWTServices<User>(_configuration,
                _logger, _identityUserManager, _hasher);
                var token = await jwtsvc.GenerateJwtToken(identityDto);
                return Ok(token);
            }
            catch (Exception exc)
            {
                _logger.LogError($"Exception thrown when creating JWT: {exc}");
            }
            
            return BadRequest("Failed to generate JWT token");
        }
    }
}