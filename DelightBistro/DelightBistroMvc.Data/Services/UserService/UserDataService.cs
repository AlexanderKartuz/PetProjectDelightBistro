using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Data.Services.PasswordHasher;
using Microsoft.EntityFrameworkCore.Update;

namespace DelightBistroMvc.Data.Services.UserService
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserRepository _userRepository;
        private IPasswordHasher _passwordHasher;

        public UserDataService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public bool IsNameUniq(string name)
        {
            var isNameUniq = _userRepository.IsNameUniq(name);
            return isNameUniq;
        }

        public void Register(UserData user)
        {
            if (!IsNameUniq(user.Name))
            {
                throw new InvalidOperationException($"Пользователь с имененм {user.Name} существует");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user.PasswordHash);
            user.Role = UserRole.User;
            user.Language = Language.English;

            _userRepository.AddAsync(user);
        }

        public void UpdateLanguage(int userId, Language language)
        {
            var user = _userRepository.GetAsync(userId)
                ?? throw new InvalidOperationException($"User {userId} not found");

            user.Language = language;
            _userRepository.Update(user);
        }

        public void UpdateProfile(UserData userData)
        {
            var user = _userRepository.GetAsync(userData.Id)
                ?? throw new InvalidOperationException($"User {userData.Id} not found");

            user.FirstName = userData.FirstName;
            user.LastName = userData.LastName;
            user.Mobilephone = userData.Mobilephone;
            _userRepository.Update(user);
        }

        public UserData? ValidateCredetials(string login, string password)
        {
            var user = _userRepository.GetByName(login);

            if (user == null)
            {
                return null;
            }

            return _passwordHasher.VerifyPassword(password, user.PasswordHash) ? user : null;
        }
    }
}
