using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;

namespace WebNet23Online.Data.Repositories
{
    public class FoodItemRepository : BaseRepository<FoodItemData>, IFoodItemRepository
    {
        public FoodItemRepository(WebContext context) : base(context) { }

        public List<FoodItemData> GetAllIncludeMenuAndIngredients()
        {
            var allFoods = _dbSet.Include(x => x.MenuData)
                .Include(x => x.IngredientsList);

            return allFoods.ToList();
        }

        public bool IsNameFree(string name)
        {
            return !_dbSet.Any(x => x.Name == name);
        }

        public FoodItemData? GetByIdIncludeMenuAndIngredients(int id)
        {
            var foodItemInclude = _dbSet
                .Include(x => x.MenuData)
                .Include(x => x.IngredientsList)
                .Include(fi => fi.FoodItemIngredientDatas) // Links
                .FirstOrDefault(x => x.Id == id);
            return foodItemInclude;
        }  
    }
}
