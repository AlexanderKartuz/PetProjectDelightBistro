using Microsoft.AspNetCore.SignalR;

namespace WebNet23Online.Hubs
{
    public class AnimalWorldHub : Hub<IAnimalWorldHub>
    {
    }

    public interface IAnimalWorldHub
    {
        Task NewAnimalInZooAppeared(string zooName, string animalSpeciesName);
    }
}
