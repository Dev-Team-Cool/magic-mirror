using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace MirrorOfErised.models
{
    public class AuthToken
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [JsonIgnore]
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public User User { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
