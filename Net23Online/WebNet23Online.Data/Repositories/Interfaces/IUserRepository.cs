using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces
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