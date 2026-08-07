namespace DelightBistroMinimalApi.DbStuff
{
    public interface IDrinkRepository
    {
        Task<Tea?> ChangeDrinkAsync(int id, Tea tea);
        Task CreateDrinkAsync(Tea tea);
        Task<bool> DeleteDrinkAsync(int id);
        Task<Tea?> GetDrinkAsync(int id);
        Task<List<Tea>> GetDrinksAsync();
    }
}