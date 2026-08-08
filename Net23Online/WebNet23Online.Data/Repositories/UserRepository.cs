using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;
using WebNet23Online.Data.Services.PasswordHasher;

namespace WebNet23Online.Data.Repositories
{
    public class UserRepository : BaseRepository<UserData>, IUserRepository
    {
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(WebContext context,
            IPasswordHasher passwordHasher) : base(context)
        {
            _passwordHasher = passwordHasher;
            _passwordHasher = passwordHasher;
        }

        public UserData GetFirst()
        {
            return _dbSet.First();
        }

        public override void Add(UserData model)
        {
            throw new NotImplementedException("You can create new user only by using method Registration");
        }

        public UserData? GetByNameAndPassword(string login, string password)
        {
            var user = _dbSet.FirstOrDefault(x => x.Name == login);
            if (user == null)
            {
                return null;
            }
            var isValid = _passwordHasher.VerifyPassword(password, user.PasswordHash);

            return isValid ? user : null;
        }

        public bool IsNameUniq(string login)
        {
            return !_dbSet.Any(x => x.Name == login);
        }

        public void Registration(UserData user)
        {
            if (!IsNameUniq(user.Name))
            {
                throw new InvalidOperationException($"Пользователь с имененм {user.Name} существует");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user.PasswordHash);

            user.Role = Enums.UserRole.User;
            user.Language = Enums.Language.English;

            _dbSet.Add(user);
            _context.SaveChanges();
        }

        public void UpdateLanguage(int userId, Language language)
        {
            var user = _dbSet.First(x => x.Id == userId);
            user.Language = language;
            _context.SaveChanges();
        }

        public void UpdateProfile(UserData userData)
        {
            var user = _dbSet.First(x => x.Id == userData.Id);
            user.FirstName = userData.FirstName;
            user.LastName = userData.LastName;
            user.Mobilephone = userData.Mobilephone;
            _context.SaveChanges();
        }
    }
}
