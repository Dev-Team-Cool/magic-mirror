using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MirrorOfErised.api.Models
{
    public class UserSetting_DTO
    {
        [Display(Name = "Calendar")]
        public bool Calendar { get; set; }

        [Display(Name = "Google Assistant")]
        public bool Assistant { get; set; }

        [Display(Name = "Travel Data")]
        public bool Commuting { get; set; }
    }
}
