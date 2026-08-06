using Microsoft.EntityFrameworkCore;

namespace DelightBistroMinimalApi.DbStuff
{
    public class DrinkRepository : IDrinkRepository
    {
        private MiniDbContext _dbContext;

        public DrinkRepository(MiniDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Tea>> GetDrinksAsync()
        {
            var drink = _dbContext.Teas.ToListAsync();
            return await drink;
        }

        public async Task<Tea?> GetDrinkAsync(int id)
        {
            var drink = _dbContext.Teas.FirstOrDefaultAsync(t => t.Id == id);
            return await drink;
        }

        public async Task CreateDrinkAsync(Tea tea)
        {
            await _dbContext.Teas.AddAsync(tea);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Tea?> ChangeDrinkAsync(int id, Tea tea)
        {
            var changedDrink = await _dbContext.Teas.FirstOrDefaultAsync(t => t.Id == id);

            if (changedDrink == null)
            {
                return null;
            }

            changedDrink.Name = tea.Name;
            changedDrink.Price = tea.Price;
            changedDrink.Description = tea.Description;
            changedDrink.ImgUrl = tea.ImgUrl;

            await _dbContext.SaveChangesAsync();
            return changedDrink;
        }

        public async Task<bool> DeleteDrinkAsync(int id)
        {
            var tea = await _dbContext.Teas.FirstOrDefaultAsync(i => i.Id == id);

            if (tea == null)
            {
                return false;
            }
            _dbContext.Teas.Remove(tea);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}