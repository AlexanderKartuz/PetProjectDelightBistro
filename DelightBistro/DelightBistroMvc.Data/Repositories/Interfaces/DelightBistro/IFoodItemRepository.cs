using DelightBistroMvc.Data.DataModels;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro
{
    public interface IFoodItemRepository : IDelightBistroRepository<FoodItemData>, IBaseRepository<FoodItemData>
    {
        List<FoodItemData> GetAllIncludeMenuAndIngredients();
        FoodItemData? GetByIdIncludeMenuAndIngredientsLinks(int id);
        List<FoodItemStatsDataModel> GetFoodItemStats();
    }
}