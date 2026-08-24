using DelightBistroMvc.Models.DTOs.Chat;
using System.Security.Claims;

namespace DelightBistroMvc.Services.Chat.Interfaces
{
    public interface INewChatService
    {
        string ResolveDisplayName(ClaimsPrincipal? user, string connectionId);
        int? TryGetUserId(ClaimsPrincipal? user);
        Task<ChatMessageDto?> SaveMessageAsync(string senderName, string text, int? userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ChatMessageDto>> GetRecentMessageAsync(int count = 10, CancellationToken cancellationToken = default);
    }
}
