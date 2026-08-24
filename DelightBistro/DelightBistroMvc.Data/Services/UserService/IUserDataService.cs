using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Services.UserService
{
    public interface IUserDataService
    {
        Task<UserData?> ValidateCredetialsAsync(string login, string password, CancellationToken cancellationToken = default);
        bool IsNameUniq(string name);
        Task RegisterAsync(UserData user, CancellationToken cancellationToken = default);
        Task UpdateLanguageAsync(int UserId, Language language, CancellationToken cancellationToken = default);
        Task UpdateProfileAsync(UserData user, CancellationToken cancellationToken = default);
    }
}