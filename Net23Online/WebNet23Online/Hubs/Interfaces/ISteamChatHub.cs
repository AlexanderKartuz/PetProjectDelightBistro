namespace WebNet23Online.Hubs.Interfaces
{
    public interface ISteamChatHub
    {
        Task SendChatMessage(string userName, string message, int userId, DateTime timestamp);
    }
}
