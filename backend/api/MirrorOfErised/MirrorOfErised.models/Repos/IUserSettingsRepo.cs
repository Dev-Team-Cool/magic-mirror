using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IUserSettingsRepo: IBaseRepo
    {
        Task<UserSettings>AddSetting(UserSettings settings);
        UserSettings Update(UserSettings settings);
        Task<UserSettings> GetSettingsForUserIdAsync(string id);



    }
}
