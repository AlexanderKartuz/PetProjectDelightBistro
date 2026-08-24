using Microsoft.EntityFrameworkCore;
using DelightBistroMvc.Data.DataModels;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

namespace DelightBistroMvc.Data.Repositories
{
    public class FoodItemRepository : BaseRepository<FoodItemData>, IFoodItemRepository
    {
        public FoodItemRepository(WebContext context) : base(context) { }

        public Task<List<FoodItemData>> GetAllIncludeMenuAndIngredientsAsync(CancellationToken cancellation = default)
        {
            var allFoods = _dbSet
                .AsNoTracking()
                .Include(x => x.MenuData)
                .Include(x => x.Creator)
                .Include(x => x.FoodItemIngredientDatas)
                    .ThenInclude(x => x.IngredientData);

            return allFoods.ToListAsync(cancellationToken: cancellation);
        }

        public bool IsNameFree(string name)
        {
            return !_dbSet.Any(x => x.Name == name);
        }

        public Task<FoodItemData?> GetByIdIncludeMenuAndIngredientsLinksAsync(int id, CancellationToken cancellation = default)
        {
            // Без AsNoTracking — сущность потом обновляется
            var foodItemInclude = _dbSet
                .Include(x => x.MenuData)
                .Include(fi => fi.FoodItemIngredientDatas)
                    .ThenInclude(x => x.IngredientData)
                .FirstOrDefaultAsync(x => x.Id == id, cancellation);

            return foodItemInclude;
        }

        public Task<List<FoodItemStatsDataModel>> GetFoodItemStatsAsync(CancellationToken cancellation = default)
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
                .ToListAsync(cancellation);

            return results;
        }
    }
}
