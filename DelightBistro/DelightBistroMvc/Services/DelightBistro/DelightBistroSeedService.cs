using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Services.Interfaces;

namespace DelightBistroMvc.Services.DelightBistro
{
    public class DelightBistroSeedService : IDelightBistroSeedService
    {
        private readonly IFoodItemRepository _foodItemRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IIngredientsRepository _ingredientsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DelightBistroSeedService(
            IFoodItemRepository foodItemRepository,
            IMenuRepository menuRepository,
            IIngredientsRepository ingredientsRepository,
            IUnitOfWork unitOfWork)
        {
            _foodItemRepository = foodItemRepository;
            _menuRepository = menuRepository;
            _ingredientsRepository = ingredientsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task EnsureSeedAsync(CancellationToken cancellationToken = default)
        {
            await FillFoodItemDataAsync(cancellationToken);
            await FillIngredientDataAsync(cancellationToken);
            await FillMenuDataAsync(cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task FillFoodItemDataAsync(CancellationToken cancellationToken = default)
        {
            if (await _foodItemRepository.AnyAsync(cancellationToken))
            {
                return;
            }

            var foodItemData = new FoodItemData
            {
                Name = "Вода",
                Price = 5m,
                ImgURL = "https://png.klev.club/uploads/posts/2024-03/png-klev-club-p-stakan-vodi-png-9.png",

            };
            await _foodItemRepository.AddAsync(foodItemData, cancellationToken);

            var cesarSalad = new FoodItemData
            {
                Name = "Цезарь",
                Price = 15m,
                ImgURL = "/images/delight-bistro/CesarSalad.jpg",
            };
            await _foodItemRepository.AddAsync(cesarSalad, cancellationToken);
        }

        private async Task FillIngredientDataAsync(CancellationToken cancellationToken = default)
        {
            if (await _ingredientsRepository.AnyAsync(cancellationToken))
            {
                return;
            }
            await _ingredientsRepository.AddAsync(new IngredientData { Name = "Креветки", Price = 40 }, cancellationToken);
            await _ingredientsRepository.AddAsync(new IngredientData { Name = "Шампиньоны", Price = 12 }, cancellationToken);
            await _ingredientsRepository.AddAsync(new IngredientData { Name = "Лайм", Price = 9 }, cancellationToken);
            await _ingredientsRepository.AddAsync(new IngredientData { Name = "Паста", Price = 8 }, cancellationToken);
        }

        private async Task FillMenuDataAsync(CancellationToken cancellationToken = default)
        {
            if (await _menuRepository.AnyAsync(cancellationToken))
            {
                return;
            }

            await _menuRepository.AddAsync(new MenuData { Name = "Soups" }, cancellationToken);
            await _menuRepository.AddAsync(new MenuData { Name = "Hot" }, cancellationToken);
            await _menuRepository.AddAsync(new MenuData { Name = "Salads" }, cancellationToken);
        }
    }
}
