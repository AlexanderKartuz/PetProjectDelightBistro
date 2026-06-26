using Microsoft.AspNetCore.SignalR;

namespace WebNet23Online.Hubs
{
    public class AnimalWorldNotificationsHub : Hub<IAnimalWorldNotificationsHub>
    {
    }

    public interface IAnimalWorldNotificationsHub
    {
        Task ZoosPromotions(string message);
    }
}
