namespace WebNet23Online.Hubs.Interfaces
{
    public interface IDeligtBistroHub
    {
        Task NewFoodWasCreated(string foodName, decimal price);
        Task ReceiveMessage(string senderName, string message);
        Task UserConnected(string userName);
        Task SetUserName(string userName);

    }
}