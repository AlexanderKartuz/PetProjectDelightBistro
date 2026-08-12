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

        public async Task<List<Drink>> GetDrinksAsync()
        {
            var drink = _dbContext.Drinks.ToListAsync();
            return await drink;
        }

        public async Task<Drink?> GetDrinkAsync(int id)
        {
            var drink = _dbContext.Drinks.FirstOrDefaultAsync(t => t.Id == id);
            return await drink;
        }

        public async Task CreateDrinkAsync(Drink tea)
        {
            await _dbContext.Drinks.AddAsync(tea);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Drink?> ChangeDrinkAsync(int id, Drink tea)
        {
            var changedDrink = await _dbContext.Drinks.FirstOrDefaultAsync(t => t.Id == id);

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
            var tea = await _dbContext.Drinks.FirstOrDefaultAsync(i => i.Id == id);

            if (tea == null)
            {
                return false;
            }
            _dbContext.Drinks.Remove(tea);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}