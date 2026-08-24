using Microsoft.AspNetCore.Mvc.Rendering;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Models.DelightBistro;

namespace DelightBistroMvc.Services.Interfaces
{
    public interface IFoodItemGenerator
    {

        Task CreateFoodItemDataAsync(CreateFoodItemViewModel foodItem, CancellationToken cancellationToken = default);
        Task ChangeFoodItemDataAsync(CreateFoodItemViewModel foodItem, CancellationToken cancellationToken = default);
        FoodItemViewModel ConvertToFoodItemVm(FoodItemData foodItemData);
        Task<CreateFoodItemViewModel> ConvertToCreateFoodItemVmAsync(FoodItemData? foodItemData = null, CancellationToken cancellationToken = default);
        Task<List<SelectListItem>> SelectMenuListAsync(CancellationToken cancellationToken = default);
        Task<AllFoodItemWithPermissionViewModel> GetFoodsWithPermissionAsync(List<FoodItemViewModel> foodItemsViewModel, CancellationToken cancellationToken = default);
        Task DeleteFoodItemAsync(int id, CancellationToken cancellationToken = default);
        Task<FileStream> GenerateTableAsync(CancellationToken cancellationToken = default);
        Task<List<FoodItemStatsViewModel>> GetFoodItemStatsViewModelsAsync(CancellationToken cancellationToken = default);
        Task<AllFoodItemWithPermissionViewModel> GetAllFoodItemWithPermissionAsync(CancellationToken cancellationToken = default);
        Task<CreateFoodItemViewModel> GetCreateFoodItemViewModelAsync(int? id = null, CancellationToken cancellationToken = default);
    }
}
