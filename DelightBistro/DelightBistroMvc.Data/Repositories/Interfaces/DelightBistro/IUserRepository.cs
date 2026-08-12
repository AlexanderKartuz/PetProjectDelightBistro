using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro
{
    public interface IUserRepository : IBaseRepository<UserData>
    {
        UserData GetFirst();
        UserData? GetByNameAndPassword(string login, string password);
        bool IsNameUniq(string login);
        void Registration(UserData user);
        void UpdateLanguage(int userId, Language language);
        void UpdateProfile(UserData userData);
    }
}