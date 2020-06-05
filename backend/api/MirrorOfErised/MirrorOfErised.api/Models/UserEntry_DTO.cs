using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MirrorOfErised.api.Models
{
    public class UserEntry_DTO
    {
/*        [Required(ErrorMessage = "Name is obligatory")]
        [Display(Name = "Display name")]
        public string Name { get; set; }*/

        [Required(ErrorMessage = "The image is obligatory")]
        [Display(Name = "Image")]

        public string Image1Path { get; set; }

        [Required(ErrorMessage = "The image is obligatory")]
        [Display(Name = "Image")]

        public string Image2Path { get; set; }

        [Required(ErrorMessage = "The image is obligatory")]
        [Display(Name = "Image")]

        public string Image3Path { get; set; }

        [Display(Name = "Fill in your address if you want traffic info otherwise leave blank")]
        public string Address { get; set; }

        [Display(Name = "Way you go to work")]
        public string CommutingWay { get; set; }


    }
}
