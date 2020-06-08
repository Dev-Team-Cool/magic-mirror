using Microsoft.AspNetCore.Identity;

namespace MirrorOfErised.models
{
    public class UserAddress
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Street { get; set; } //Contains house number
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}