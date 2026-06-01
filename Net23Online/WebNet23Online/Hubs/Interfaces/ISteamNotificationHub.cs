namespace WebNet23Online.Hubs.Interfaces
{
    public interface ISteamNotificationHub
    {
        Task NewGameAdded(string gameName, string urlCover);
    }
}
