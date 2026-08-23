using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Data.Services.PasswordHasher;
using Microsoft.EntityFrameworkCore.Update;

namespace DelightBistroMvc.Data.Services.UserService
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IPasswordHasher _passwordHasher;

        public UserDataService(IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public bool IsNameUniq(string name)
        {
            var isNameUniq = _userRepository.IsNameUniq(name);
            return isNameUniq;
        }

        public async Task RegisterAsync(
            UserData user,
            CancellationToken cancellationToken = default)
        {
            if (!IsNameUniq(user.Name))
            {
                throw new InvalidOperationException($"Пользователь с имененм {user.Name} существует");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user.PasswordHash);
            user.Role = UserRole.User;
            user.Language = Language.English;

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateLanguageAsync(
            int userId,
            Language language,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetAsync(userId, cancellationToken)
                ?? throw new InvalidOperationException($"User {userId} not found");

            user.Language = language;
            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateProfileAsync(
            UserData userData,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetAsync(userData.Id, cancellationToken)
                ?? throw new InvalidOperationException($"User {userData.Id} not found");

            user.FirstName = userData.FirstName;
            user.LastName = userData.LastName;
            user.Mobilephone = userData.Mobilephone;
            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserData?> ValidateCredetialsAsync(
            string login,
            string password,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByNameAsync(login, cancellationToken);

            if (user == null)
            {
                return null;
            }

            return _passwordHasher.VerifyPassword(password, user.PasswordHash) ? user : null;
        }
    }
}
