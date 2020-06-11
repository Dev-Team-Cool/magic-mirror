using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MirrorOfErised.models;

namespace MirrorOfErised.api.Models
{
    public class UserEntryDto
    {
        public UserAddressDto Address { get; set; }
        public string CommutingWay { get; set; }
    }
}
