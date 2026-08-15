using DelightBistroMvc.Data.DataModels;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Models.DelightBistro;

namespace DelightBistroMvc.Services.Interfaces
{
    public interface IIngredientGenerator
    {
        void CreateIngredientData(CreateIngredientViewModel ingredient);
        List<CreateIngredientViewModel> GenerateIngredientsViewModelFromFoodItemData(FoodItemData? foodItemData = null);
        List<CreateIngredientViewModel> GetSelectedCreateIngredientViewModelFromIngredientsList(List<CreateIngredientViewModel> ingredientsViewModel);
        List<FoodItemIngredientData> GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(CreateFoodItemViewModel viewModel);
        List<CreateIngredientViewModel> MapSelectedIngredients(FoodItemData foodItemData);
    }
}