namespace WebNet23Online.Hubs.Interfaces
{
    public interface INotificationHub
    {
        Task NewMessage(string text);
    }
}
