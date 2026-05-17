using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;

namespace WebNet23Online.Data.Repositories
{
    public class FoodItemRepository : BaseRepository<FoodItemData>, IFoodItemRepository
    {
        public FoodItemRepository(WebContext context) : base(context) { }

        public List<FoodItemData> GetAllIncludeMenuAndIngredients()
        {
            var allFoods = _dbSet
                .Include(x => x.MenuData)
                .Include(x => x.IngredientsList);

            return allFoods.ToList();
        }

        public bool IsNameFree(string name)
        {
            return !_dbSet.Any(x => x.Name == name);
        }

        public FoodItemData? GetByIdIncludeMenuAndIngredientsLinks(int id)
        {
            var foodItemInclude = _dbSet
                .Include(x => x.MenuData)
                .Include(x => x.IngredientsList)
                .Include(fi => fi.FoodItemIngredientDatas) // Links
                .FirstOrDefault(x => x.Id == id);
            return foodItemInclude;
        }

        public List<FoodItemStatsDataModel> GetFoodItemStats()
        {
            var sql = @"SELECT 
            FI.[Name] as FoodItemName,
            COUNT (I.Id) as IngredientCount,
            FI.Price as FoodItemPrice,
            ISNULL (SUM(I.Price*FIID.QuantityOfIngredients/1000),0) as TotalPriceIngredient,
            FI.Price - ISNULL (SUM(I.Price*FIID.QuantityOfIngredients/1000),0) as Profit
            FROM FoodItemIngredientDatas as FIID
            LEFT JOIN FoodItems FI ON FIID.FoodItemDataId = FI.Id
            LEFT JOIN Ingredients I ON FIID.IngredientDataId = I.Id
            GROUP BY FI.[Name], FI.Id, FI.Price";

            var results = _context
                .Database
                .SqlQueryRaw<FoodItemStatsDataModel>(sql)
                .ToList();

            return results;
        }
    }
}
