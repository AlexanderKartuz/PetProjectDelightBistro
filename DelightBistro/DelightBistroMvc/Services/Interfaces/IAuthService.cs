using System.Security.Claims;
using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Services.Interfaces;

public interface IAuthService
{
    UserRole GetRole();
    UserData? GetUser();
    int GetUserId();
    string? GetUserName();
    bool IsAuthenticated();
    bool AtLeastModerator();
    bool IsCurrentUserAtLeastEmployee();
    bool IsUser();
    Language GetLanguage();
    Task SignIn(UserData user);
}