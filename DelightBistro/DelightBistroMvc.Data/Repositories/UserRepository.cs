using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Data.Services.PasswordHasher;

namespace DelightBistroMvc.Data.Repositories
{
    public class UserRepository : BaseRepository<UserData>, IUserRepository
    {

        public UserRepository(WebContext context) : base(context)
        {

        }

        public UserData? GetByName(string login)
        {
            var user = _dbSet.FirstOrDefault(x => x.Name == login);
            return user;
        }

        public bool IsNameUniq(string login)
        {
            return !_dbSet.Any(x => x.Name == login);
        }
    }
}
