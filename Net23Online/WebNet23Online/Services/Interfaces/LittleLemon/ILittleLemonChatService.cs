using WebNet23Online.Data.Enums;

namespace WebNet23Online.Services.Interfaces.LittleLemon
{
    public interface ILittleLemonChatService
    {
        string AdminGroupName { get; }
        string GetUserGroupName(int userId);
        Task RegisterConnectionAsync(string connectionId, UserRole role, int userId);
        Task SendMessageToAdminAsync(string message);
        Task SendMessageToUserAsync(int targetUserId, string message);
    }
}
