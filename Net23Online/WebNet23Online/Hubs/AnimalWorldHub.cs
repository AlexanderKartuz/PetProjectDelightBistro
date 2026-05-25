using Microsoft.AspNetCore.SignalR;

namespace WebNet23Online.Hubs
{
    public class AnimalWorldHub : Hub<IAnimalWorldHub>
    {
    }

    public interface IAnimalWorldHub
    {
        Task NewZooCreated(string zooName);
    }
}
