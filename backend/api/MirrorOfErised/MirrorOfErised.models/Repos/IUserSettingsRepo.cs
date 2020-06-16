using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IUserSettingsRepo: IBaseRepo
    {
        Task<UserSettings>AddSetting(UserSettings settings);
        Task<UserSettings>UpdateSetting(UserSettings settings);
        Task<UserSettings> GetSettingsForUserIdAsync(string id);



    }
}
