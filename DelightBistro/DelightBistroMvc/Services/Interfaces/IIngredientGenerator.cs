using DelightBistroMvc.Data.DataModels;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Models.DelightBistro;

namespace DelightBistroMvc.Services.Interfaces
{
    public interface IIngredientGenerator
    {
        Task CreateIngredientDataAsync(CreateIngredientViewModel ingredient, CancellationToken cancellationToken = default);
        Task<List<CreateIngredientViewModel>> GenerateIngredientsViewModelFromFoodItemDataAsync(FoodItemData? foodItemData = null, CancellationToken cancellationToken = default);
        List<CreateIngredientViewModel> GetSelectedCreateIngredientViewModelFromIngredientsList(List<CreateIngredientViewModel> ingredientsViewModel);
        List<FoodItemIngredientData> GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(CreateFoodItemViewModel viewModel);
        List<CreateIngredientViewModel> MapSelectedIngredients(FoodItemData foodItemData);
    }
}