using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Models.DTOs.Chat;

namespace DelightBistroMvc.Hubs.Interfaces
{
    public interface INewChatHub
    {
        Task SetUserName(string userName);
        Task ReceiveHistory(IEnumerable<ChatMessageDto> messageDatas);
        Task ReceiveMessage(ChatMessageDto messages);
        Task ConnectedUsers(IEnumerable<ChatUserDto> users);
        Task UserConnected(string connetionId, string userName);
        Task UserDisconnected(string connetionId, string userName);
    }
}
