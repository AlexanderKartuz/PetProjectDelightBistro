using DelightBistroMinimalApi.DbStuff;

namespace DelightBistroMinimalApi.Services.Cache.Interfaces
{
    public interface IDrinksCacheService
    {
        Task<List<Tea>> GetDrinksAsync();
        Task<Tea?> GetDrinkAsync(int id);
        Task CreateDrinkAsync(Tea tea);
        Task<Tea?> ChangeDrinkAsync(int id, Tea tea);
        Task<bool> DeleteDrinkAsync(int id);
    }
}
