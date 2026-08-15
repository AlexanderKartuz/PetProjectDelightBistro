using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Services.UserService
{
    public interface IUserDataService
    {
        UserData? ValidateCredetials(string login, string password);
        bool IsNameUniq(string name);
        void Register(UserData user);
        void UpdateLanguage(int UserId, Language language);
        void UpdateProfile(UserData user);
    }
}