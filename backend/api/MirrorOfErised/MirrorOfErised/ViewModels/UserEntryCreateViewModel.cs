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
        [Display(Name = "City")]
        public string City { get; set; }
        
        [Display(Name = "City")]
        public string TrainCity { get; set; }
        
        [Display(Name = "Street")]
        public string Street { get; set; }
        
        [Display(Name="Zip code"), MaxLength(4)]
        public string ZipCode { get; set; }

        public string ActualCity => string.IsNullOrEmpty(TrainCity) ? City : TrainCity;

        [Display(Name="The mirror is allowed to recognize me and process your details.")]
        public bool AllowRecognition { get; set; } = true;

        [Display(Name="Use your Google Assistant")]
        public bool Assistant { get; set; }
        [Display(Name="Show commute info")]
        public bool Commute { get; set; }
        [Display(Name="Show calendar events")]
        public bool Calendar { get; set; }

        [Display(Name = "How do you go to work?")]
        public CommutingOption CommutingWay { get; set; }
    }
}
