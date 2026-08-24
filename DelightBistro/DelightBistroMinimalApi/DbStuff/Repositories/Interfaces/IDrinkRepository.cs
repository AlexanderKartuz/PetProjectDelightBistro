using DelightBistroMinimalApi.DbStuff.Models;

namespace DelightBistroMinimalApi.DbStuff.Repositories.Interfaces
{
    public interface IDrinkRepository
    {
        Task<Drink?> ChangeDrinkAsync(int id, Drink tea);
        Task CreateDrinkAsync(Drink tea);
        Task<bool> DeleteDrinkAsync(int id);
        Task<Drink?> GetDrinkAsync(int id);
        Task<List<Drink>> GetDrinksAsync();
    }
}