using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Data.Services.PasswordHasher;
using Microsoft.EntityFrameworkCore;

namespace DelightBistroMvc.Data.Repositories
{
    public class UserRepository : BaseRepository<UserData>, IUserRepository
    {

        public UserRepository(WebContext context) : base(context)
        {

        }

        public Task<UserData?> GetByNameAsync(string login, CancellationToken cancellationToken = default)
        {
            return _dbSet.FirstOrDefaultAsync(x => x.Name == login, cancellationToken);
        }

        public bool IsNameUniq(string login)
        {
            return !_dbSet.Any(x => x.Name == login);
        }
    }
}
