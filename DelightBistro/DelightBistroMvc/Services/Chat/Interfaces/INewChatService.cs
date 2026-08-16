using DelightBistroMvc.Models.DTOs.Chat;
using System.Security.Claims;

namespace DelightBistroMvc.Services.Chat.Interfaces
{
    public interface INewChatService
    {
        string ResolveDisplayName(ClaimsPrincipal? user, string connectionId);
        int? TryGetUserId(ClaimsPrincipal? user);
        ChatMessageDto? SaveMessage(string senderName, string text, int? userId);
        IReadOnlyList<ChatMessageDto> GetRecentMessage(int count = 10);
    }
}
