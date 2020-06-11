using System;
//using System.Text.Json.Serialization;

namespace MirrorOfErised.api.Models
{
    public class AuthTokenDto
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
