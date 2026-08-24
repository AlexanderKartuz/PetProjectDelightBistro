using DelightBistroMvc.Data.DataModels;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Models.DelightBistro;
using DelightBistroMvc.Services.Interfaces;

namespace DelightBistroMvc.Services.DelightBistro
{
    public class IngredientGenerator : IIngredientGenerator
    {
        private readonly IIngredientsRepository _ingredientsRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public IngredientGenerator(IIngredientsRepository ingredientsRepository, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _ingredientsRepository = ingredientsRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateIngredientDataAsync(CreateIngredientViewModel ingredient,
            CancellationToken cancellationToken = default)
        {
            var ingredientData = new IngredientData
            {
                Name = ingredient.Name,
                Price = ingredient.Price,
                Creator = await _authService.GetUserAsync(cancellationToken)
            };

            await _ingredientsRepository.AddAsync(ingredientData, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<CreateIngredientViewModel>> GenerateIngredientsViewModelFromFoodItemDataAsync(
            FoodItemData? foodItemData = null,
            CancellationToken cancellationToken = default)
        {
            var ingredientsData = await _ingredientsRepository.GetAllAsync(cancellationToken);

            var ingredientsViewModel = ingredientsData.Select(i => new CreateIngredientViewModel
            {
                Id = i.Id,
                Name = i.Name,
                IsSelected = foodItemData != null
                    && foodItemData.FoodItemIngredientDatas
                        .Any(links => links.IngredientDataId == i.Id),
                Quantity = foodItemData?.FoodItemIngredientDatas
                    .FirstOrDefault(fi => fi.IngredientDataId == i.Id)?
                    .QuantityOfIngredients ?? 10
            }).ToList();

            return ingredientsViewModel;
        }

        public List<CreateIngredientViewModel> GetSelectedCreateIngredientViewModelFromIngredientsList(List<CreateIngredientViewModel> ingredientsViewModel)
        {
            return ingredientsViewModel.Where(x => x.IsSelected).ToList();
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

        /// <summary>
        /// Карточки Index/AllFoodItems: только выбранные ингредиенты из уже загруженных FoodItemIngredientDatas.
        /// </summary>
        public List<CreateIngredientViewModel> MapSelectedIngredients(FoodItemData foodItemData)
        {
            return foodItemData.FoodItemIngredientDatas
                .Select(links => new CreateIngredientViewModel
                {
                    Id = links.IngredientDataId,
                    Name = links.IngredientData?.Name ?? string.Empty,
                    IsSelected = true,
                    Quantity = links.QuantityOfIngredients,
                })
                .ToList();
        }
    }
}
