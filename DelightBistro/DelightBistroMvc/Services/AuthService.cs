using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.RelfectionTools;
using DelightBistroMvc.Services.Interfaces;

namespace DelightBistroMvc.Services
{
    public class AuthService : IAuthService
    {
        public const string AUTH_KEY = "AuthDelightBistro";
        public const string COOCKIE_ID_KEY = "Id";
        public const string COOCKIE_ROLE_KEY = "Role";
        public const string COOCKIE_NAME_KEY = "UserName";
        public const string COOCKIE_LANGUAGE_KEY = "Language";

        private IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        [AutoRegister]
        public AuthService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public int GetUserId()
        {
            var userIdStr = _httpContextAccessor.HttpContext!.User?.Claims
                .FirstOrDefault(x => x.Type == COOCKIE_ID_KEY)
                ?.Value;
            if (userIdStr is null)
            {
                return 0;
            }

            var userId = int.Parse(userIdStr);
            return userId;
        }

        public async Task<UserData?> GetUserAsync()
        {
            var userId = GetUserId();
            if (userId <= 0)
            {
                return null;
            }

            return await _userRepository.GetAsync(userId);
        }

        public string? GetUserName()
        {
            var userName = _httpContextAccessor.HttpContext!.User?.Claims
                .FirstOrDefault(x => x.Type == COOCKIE_NAME_KEY)
                ?.Value;

            return userName;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }

        /// <summary>
        /// You can't call this method if user not authenticated
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public UserRole GetRole()
        {
            if (!IsAuthenticated())
            {
                throw new InvalidOperationException();
            }
            var roleStr = _httpContextAccessor.HttpContext!.User.Claims
                .First(x => x.Type == COOCKIE_ROLE_KEY)
                .Value;
            var role = Enum.Parse<UserRole>(roleStr);
            return role;
        }

        public bool AtLeastModerator()
        {
            if (!IsAuthenticated())
            {
                return false;
            }

            var role = GetRole();
            return role == UserRole.Moderator || role == UserRole.Admin;
        }

        public Language GetLanguage()
        {
            if (!IsAuthenticated())
            {
                return Language.English;
            }

            var languageStr = _httpContextAccessor.HttpContext!.User.Claims
                .First(x => x.Type == COOCKIE_LANGUAGE_KEY)
                .Value;
            var language = Enum.Parse<Language>(languageStr);
            return language;
        }

        public async Task SignInAsync(UserData user)
        {
            var claims = new List<Claim>
            {
                new Claim(COOCKIE_ID_KEY, user.Id.ToString()),
                new Claim(COOCKIE_ROLE_KEY, user.Role.ToString()),
                new Claim(COOCKIE_NAME_KEY, user.Name),
                new Claim(COOCKIE_LANGUAGE_KEY, user.Language.ToString()),
                new Claim(ClaimTypes.AuthenticationMethod, AUTH_KEY)
            };

            var identity = new ClaimsIdentity(claims, AUTH_KEY);

            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor
                 .HttpContext!
                 .SignInAsync(AUTH_KEY, principal);
        }

        public bool IsCurrentUserAtLeastEmployee()
        {
            if (!IsAuthenticated())
            {
                return false;
            }
            var role = GetRole();
            return role == UserRole.Admin
                || role == UserRole.Moderator
                || role == UserRole.Employee;
        }

        public bool IsUser()
        {
            if (!IsAuthenticated() || GetRole() != UserRole.User)
            {
                return false;
            }

            return true;
        }

    }
}
