using WebNet23Online.Models.Steam;

namespace WebNet23Online.Services.Interfaces.Steam
{
    public interface IChatService
    {
        void AddChatMessage(string message);
        List<ChatMessageViewModel> GetMessages();
    }
}
