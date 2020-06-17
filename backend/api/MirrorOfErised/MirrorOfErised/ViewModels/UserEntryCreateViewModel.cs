using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MirrorOfErised.models;

namespace MirrorOfErised.ViewModels
{
    public class UserEntryCreateViewModel
    {
        [Display(Name = "Fill in your address if you want traffic info otherwise leave blank")]
        public UserAddress Address { get; set; }

        [Display(Name = "How do you go to work?")]
        public CommutingOption CommutingWay { get; set; }


        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
