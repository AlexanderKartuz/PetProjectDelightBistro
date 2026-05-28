namespace WebNet23Online.Hubs.Interfaces
{
    public interface ISteamChatHub
    {
        void SendChatMessage(string userName, string message);
    }
}
