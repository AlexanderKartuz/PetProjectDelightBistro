using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces.DelightBistro
{
    public interface IFoodItemRepository : IDelightBistroRepository<FoodItemData>, IBaseRepository<FoodItemData>
    {
        List<FoodItemData> GetAllIncludeMenuAndIngredients();
        FoodItemData? GetByIdIncludeMenuAndIngredientsLinks(int id);
        List<FoodItemStatsDataModel> GetFoodItemStats();
    }
}