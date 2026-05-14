using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Models;
using WebNet23Online.Models.DelightBistro;

namespace WebNet23Online.Services.Interfaces
{
    public interface IIngredientGenerator
    {
        void CreateIngredientData(CreateIngredientViewModel ingredient);
        void FeelDataBase();
        List<CreateIngredientViewModel> GenerateIngredientsViewModelFromFoodItemData(FoodItemData? foodItemData = null);
        List<CreateIngredientViewModel> GetSelectedCreateIngredientViewModelFromIngredientsList(List<CreateIngredientViewModel> ingredientsViewModel);
        List<FoodItemIngredientData> GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(CreateFoodItemViewModel viewModel);
    }
}