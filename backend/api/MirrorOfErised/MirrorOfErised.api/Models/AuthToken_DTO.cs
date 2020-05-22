using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
//using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Project.API.Models
{
    public class AuthToken_DTO
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
