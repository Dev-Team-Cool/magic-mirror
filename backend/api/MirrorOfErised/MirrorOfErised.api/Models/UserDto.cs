using MirrorOfErised.models;

namespace MirrorOfErised.api.Models
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public AuthTokenDto Tokens { get; set; }
        public SettingsDto Settings { get; set; }
        public UserEntryDto CommuteInfo { get; set; }
    }
}