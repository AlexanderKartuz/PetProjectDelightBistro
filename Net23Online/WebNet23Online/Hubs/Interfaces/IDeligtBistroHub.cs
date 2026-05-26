namespace WebNet23Online.Hubs.Interfaces
{
    public interface IDeligtBistroHub
    {
        Task NewFoodWasCreated(string foodName, decimal price);
    }
}