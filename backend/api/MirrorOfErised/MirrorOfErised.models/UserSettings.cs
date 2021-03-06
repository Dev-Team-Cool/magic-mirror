﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace MirrorOfErised.models
{
    public class UserSettings
    {
        [Display(Name = "Calendar"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public bool Calendar { get; set; }

        [Display(Name = "Google Assistant"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public bool Assistant { get; set; }

        [Display(Name = "Travel Data"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public bool Commuting { get; set; }

        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
