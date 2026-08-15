using Microsoft.AspNetCore.Mvc.Rendering;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Models.DelightBistro;

namespace DelightBistroMvc.Services.Interfaces
{
    public interface IFoodItemGenerator
    {

        void CreateFoodItemData(CreateFoodItemViewModel foodItem);
        void ChangeFoodItemData(CreateFoodItemViewModel foodItem);

        FoodItemViewModel ConvertToFoodItemVM(FoodItemData foodItemData);
        CreateFoodItemViewModel ConvertToCreateFoodItemVM(FoodItemData? foodItemData = null);
        List<SelectListItem> SelectMenuList();
        AllFoodItemWithPermissionViewModel GetFoodsWithPermission(List<FoodItemViewModel> foodItemsViewModel);
        void DeleteFoodItem(int id);
        FileStream GenerateTable();
        List<FoodItemStatsViewModel> GetFoodItemStatsViewModels();

        AllFoodItemWithPermissionViewModel GetAllFoodItemWithPermission();
        CreateFoodItemViewModel GetCreateFoodItemViewModel(int? id = null);
    }
}
