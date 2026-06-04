namespace WebNet23Online.Hubs.Interfaces
{
    public interface IJdmHub
    {
        Task NewJdmCarsCreated(string model, int price, string url);
    }
}
