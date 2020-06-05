using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MirrorOfErised.ViewModels
{
    public class UserEntryCreateViewModel
    {
/*        [Required(ErrorMessage = "Name is obligatory")]
        [Display(Name = "Display name")]
        public string Name { get; set; }*/

        [Required(ErrorMessage = "The image is obligatory")]
        [Display(Name = "Image")]

        public IFormFile Image1 { get; set; }

        [Required(ErrorMessage = "The image is obligatory")]
        [Display(Name = "Image")]

        public IFormFile Image2 { get; set; }

        [Required(ErrorMessage = "The image is obligatory")]
        [Display(Name = "Image")]

        public IFormFile Image3 { get; set; }

        [Display(Name = "Fill in your address if you want traffic info otherwise leave blank")]

        public string Address { get; set; }

        [Display(Name = "Way you go to work")]
        public string CommutingWay { get; set; }


        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [JsonIgnore]
        public string UserId { get; set; }

        public IdentityUser identityUser { get; set; }
    }
}
