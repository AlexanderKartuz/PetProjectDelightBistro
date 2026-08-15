using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Services.Interfaces;

namespace DelightBistroMvc.Services.DelightBistro
{
    public class DelightBistroSeedService : IDelightBistroSeedService
    {
        private readonly IFoodItemRepository _foodItemRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IIngredientsRepository _ingredientsRepository;

        public DelightBistroSeedService(IFoodItemRepository foodItemRepository,
            IMenuRepository menuRepository,
            IIngredientsRepository ingredientsRepository)
        {
            _foodItemRepository = foodItemRepository;
            _menuRepository = menuRepository;
            _ingredientsRepository = ingredientsRepository;
        }

        public void EnsureSeed()
        {
            FillFoodItemData();
            FillIngredientData();
            FillMenuData();
        }

        private void FillFoodItemData()
        {
            if (_foodItemRepository.Any())
            {
                return;
            }

            var foodItemData = new FoodItemData
            {
                Name = "Вода",
                Price = 5m,
                ImgURL = "https://png.klev.club/uploads/posts/2024-03/png-klev-club-p-stakan-vodi-png-9.png",

            };
            _foodItemRepository.Add(foodItemData);

            var cesarSalad = new FoodItemData
            {
                Name = "Цезарь",
                Price = 15m,
                ImgURL = "/images/delight-bistro/CesarSalad.jpg",
            };
            _foodItemRepository.Add(cesarSalad);
        }

        private void FillIngredientData()
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

        public void FillMenuData()
        {
            if (_menuRepository.Any())
            {
                return;
            }

            _menuRepository.Add(new MenuData { Name = "Soups" });
            _menuRepository.Add(new MenuData { Name = "Hot" });
            _menuRepository.Add(new MenuData { Name = "Salads" });
        }
    }
}
