using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro
{
    public interface IUserRepository : IBaseRepository<UserData>
    {
        Task<UserData?> GetByNameAsync(string login, CancellationToken cancellationToken = default);
        bool IsNameUniq(string login);
    }
}