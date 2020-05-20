﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace MirrorOfErised.models
{
   public class UserEntry
    {
        [Required(ErrorMessage = "Name is obligatory")]
        [Display(Name = "Display name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The image is obligatory")]
        [Display(Name = "Image")]

        public string Image1Path { get; set; }

        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [JsonIgnore]
        public string UserId { get; set; }

        public IdentityUser identityUser { get; set; }

    }
}
