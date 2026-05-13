using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data.Models;
using WebNet23Online.Models.DelightBistro;

namespace WebNet23Online.Services.Interfaces
{
    public interface IFoodItemGenerator
    {

        void CreateFoodItemData(CreateFoodItemViewModel foodItem);
        void ChangeFoodItemData(CreateFoodItemViewModel foodItem);

        FoodItemViewModel ConvertToFoodItemVM(FoodItemData foodItemData);
        void FeelDataBase();
        CreateFoodItemViewModel ConvertToCreateFoodItemVM(FoodItemData foodItemData = null);
        List<SelectListItem> SelectMenuList();
        List<CreateIngredientViewModel> ChekBoxIngredients(FoodItemData foodItemData = null);
        AllFoodItemWithPermissionViewModel GetFoodsWithPermission(List<FoodItemViewModel> foodItemsViewModel);
        void DeleteFoodItem(int id);
        FileStream GenerateTable();
    }
}
