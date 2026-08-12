namespace DelightBistroMvc.Hubs.Interfaces
{
    public interface INotificationHub
    {
        Task NewMessage(string text);
    }
}
