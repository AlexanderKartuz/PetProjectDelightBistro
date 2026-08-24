using DelightBistroMvc.Data.DataModels;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro
{
    public interface IFoodItemRepository : IDelightBistroRepository<FoodItemData>, IBaseRepository<FoodItemData>
    {
        Task<List<FoodItemData>> GetAllIncludeMenuAndIngredientsAsync(CancellationToken cancellation = default);
        Task<FoodItemData?> GetByIdIncludeMenuAndIngredientsLinksAsync(int id, CancellationToken cancellation = default);
        Task<List<FoodItemStatsDataModel>> GetFoodItemStatsAsync(CancellationToken cancellation = default);
    }
}