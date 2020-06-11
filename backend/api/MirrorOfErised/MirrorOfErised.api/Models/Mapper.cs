using System;
using MirrorOfErised.models;

namespace MirrorOfErised.api.Models
{
    public static class Mapper
    {
        public static UserDto ConvertToUserDto(ref User user, ref AuthToken userTokens)
        {
            return new UserDto()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Settings = ConvertToUserSettingsDto(user.Settings),
                CommuteInfo = ConvertToEntryDto(user.Commute),
                Tokens = ConvertToAuthTokensDto(userTokens)
            };
        }

        public static UserEntryDto ConvertToEntryDto(UserEntry entry)
        {
            if (entry == null) return null;
            return new UserEntryDto()
            {
                Address = ConvertToUserAddressDto(entry.Address),
                CommutingWay = Enum.GetName(typeof(CommutingOption), entry.CommutingWay)
            };
        }

        public static UserAddressDto ConvertToUserAddressDto(UserAddress address)
        {
            if (address == null) return null;
            return new UserAddressDto()
            {
                City = address.City,
                Street = address.Street,
                ZipCode = address.ZipCode
            };
        }
 
        public static AuthTokenDto ConvertToAuthTokensDto(AuthToken tokens)
        {
            if (tokens == null) return null;
            return new AuthTokenDto()
            {
                Token = tokens.Token,
                ExpireDate = tokens.ExpireDate
            };
        }

        public static SettingsDto ConvertToUserSettingsDto(UserSettings settings)
        {
            if (settings == null) return null;
            return new SettingsDto()
            {
                Assistant = settings.Assistant,
                Calendar = settings.Calendar,
                Commute = settings.Commuting
            };
        }
    }
}