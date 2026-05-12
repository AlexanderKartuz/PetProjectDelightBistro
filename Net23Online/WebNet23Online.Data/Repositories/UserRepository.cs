using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class UserRepository : BaseRepository<UserData>, IUserRepository
    {
        public UserRepository(WebContext context) : base(context) { }

        public UserData GetFirst()
        {
            return _dbSet
                .First();
        }

        public override void Add(UserData model)
        {
            throw new NotImplementedException("You can create new user only by using method Registration");
        }

        public UserData? GetByNameAndPassword(string login, string password)
        {
            var hash = GetHashOfPassword(password);
            return _dbSet
                .FirstOrDefault(x => x.Name == login && x.Password == hash);
        }

        public bool IsNameUniq(string login)
        {
            return !_dbSet.Any(x => x.Name == login);
        }

        public void Registration(UserData user)
        {
            var hash = GetHashOfPassword(user.Password);
            user.Password = hash;
            user.Role = Enums.UserRole.User;
            user.Language = Enums.Language.English;

            _dbSet.Add(user);
            _context.SaveChanges();
        }

        private string GetHashOfPassword(string password)
        {
            // "Password"
            // "Possword"
            // "Posswor"

            password = password.Replace("a", "o");
            return password.Substring(0, password.Length - 1);
        }

        public void UpdateLanguage(int userId, Language language)
        {
            var user = _dbSet.First(x => x.Id == userId);
            user.Language = language;
            _context.SaveChanges();
        }

        public void UpdateProfile(UserData userData)
        {
            //var user = _dbSet.First(x => x.Id == userData.Id);
            //user.Language = userData.Language;
            //user.FirstName = userData.FirstName;
            //user.LastName = userData.LastName;
            //user.Mobilephone = userData.Mobilephone;
            Update(userData);
            //_dbSet.Update(user);
            //_context.SaveChanges();
        }
    }
}
