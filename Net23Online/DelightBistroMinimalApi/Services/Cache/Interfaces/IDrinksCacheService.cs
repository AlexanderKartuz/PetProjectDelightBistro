using DelightBistroMinimalApi.DbStuff;

namespace DelightBistroMinimalApi.Services.Cache.Interfaces
{
    public interface IDrinksCacheService
    {
        Task<List<Drink>> GetDrinksAsync();
        Task<Drink?> GetDrinkAsync(int id);
        Task CreateDrinkAsync(Drink tea);
        Task<Drink?> ChangeDrinkAsync(int id, Drink tea);
        Task<bool> DeleteDrinkAsync(int id);
    }
}
