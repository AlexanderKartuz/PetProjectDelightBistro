using System.Security.Claims;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;

namespace WebNet23Online.Services.Interfaces;

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
    void SignIn(UserData user);
    bool IsBlockedInTracker();
}