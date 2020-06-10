using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MirrorOfErised.models
{
    public class UserEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserId { get; set; }
        public User User { get; set; }
        public UserAddress Address { get; set; }
        public CommutingOption CommutingWay { get; set; }
    }

    public enum CommutingOption
    {
        Train,
        Car,
        Other
    }
}
