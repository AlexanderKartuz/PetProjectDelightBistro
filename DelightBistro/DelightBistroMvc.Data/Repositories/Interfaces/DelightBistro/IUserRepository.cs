using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro
{
    public interface IUserRepository : IBaseRepository<UserData>
    {
        UserData? GetByName(string login);
        bool IsNameUniq(string login);
    }
}