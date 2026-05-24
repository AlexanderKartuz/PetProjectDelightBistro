using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;
using WebNet23Online.Models.DelightBistro;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services.DelightBistro
{
    public class IngredientGenerator : IIngredientGenerator
    {
        private IIngredientsRepository _ingredientsRepository;
        private IAuthService _authService;

        public IngredientGenerator(IIngredientsRepository ingredientsRepository, IAuthService authService)
        {
            _ingredientsRepository = ingredientsRepository;
            _authService = authService;
        }
        public void FeelDataBase()
        {
            if (_ingredientsRepository.Any())
            {
                return;
            }
            _ingredientsRepository.Add(new IngredientData { Name = "Креветки", Price = 40 });
            _ingredientsRepository.Add(new IngredientData { Name = "Шампиньоны", Price = 12 });
            _ingredientsRepository.Add(new IngredientData { Name = "Лайм", Price = 9 });
            _ingredientsRepository.Add(new IngredientData { Name = "Паста", Price = 8 });
        }


        public void CreateIngredientData(CreateIngredientViewModel ingredient)
        {
            var ingredientData = new IngredientData
            {
                Name = ingredient.Name,
                Price = ingredient.Price,
                Creator = _authService.GetUser()
            };

            _ingredientsRepository.Add(ingredientData);
        }

        public List<CreateIngredientViewModel> GenerateIngredientsViewModelFromFoodItemData(FoodItemData? foodItemData = null)
        {
            var ingredientsData = _ingredientsRepository.GetAll();

            var ingredientsViewModel = ingredientsData.Select(x => new CreateIngredientViewModel
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = foodItemData != null && foodItemData.IngredientsList.Any(i => i.Id == x.Id),
                Quantity = foodItemData?.FoodItemIngredientDatas
                .FirstOrDefault(fi => fi.IngredientDataId == x.Id)?
                .QuantityOfIngredients ?? 10
            }).ToList();

            return ingredientsViewModel;
        }

        public List<CreateIngredientViewModel> GetSelectedCreateIngredientViewModelFromIngredientsList(List<CreateIngredientViewModel> ingredientsViewModel)
        {
            var selectedIngredientsViewModel = ingredientsViewModel.Where(x => x.IsSelected).ToList();

            return selectedIngredientsViewModel;
        }

        public List<FoodItemIngredientData> GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(CreateFoodItemViewModel viewModel)
        {
            var links = viewModel.IngredientsList
                .Where(x => x.IsSelected)
                .Select(x => new FoodItemIngredientData
                {
                    IngredientDataId = x.Id,
                    QuantityOfIngredients = x.Quantity > 0 ? x.Quantity : 10
                })
                .ToList();

            return links;
        }
    }
}
