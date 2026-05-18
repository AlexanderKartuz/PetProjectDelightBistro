using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;
using WebNet23Online.Models.DelightBistro;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services.DelightBistro
{
    public class MenuTypeGenerator : IMenuTypeGenerator
    {
        private IMenuRepository _menuRepository;
        private IFoodItemGenerator _foodItemGenerator;
        private IAuthService _authService;

        public MenuTypeGenerator(IMenuRepository menuRepository, IFoodItemGenerator foodItemGenerator, IAuthService authService)
        {
            _menuRepository = menuRepository;
            _foodItemGenerator = foodItemGenerator;
            _authService = authService;
        }

        public void FeelDataBase()
        {
            if (_menuRepository.Any())
            {
                return;
            }

            _menuRepository.Add(new MenuData { Name = "Soups" });
            _menuRepository.Add(new MenuData { Name = "Hot" });
            _menuRepository.Add(new MenuData { Name = "Salads" });
        }

        public void CreateMenuData(CreateMenuViewModel viewModel)
        {
            var menuData = new MenuData
            {
                Name = viewModel.Name,
                Creator = _authService.GetUser()
            };

            _menuRepository.Add(menuData);
        }

        public MenuTypeViewModel ConvertMenuDataToViewModel(MenuData menuData)
        {

            return new MenuTypeViewModel
            {
                Id = menuData.Id,
                Name = menuData.Name,
                FoodItems = (menuData.FoodItems ?? new List<FoodItemData>())
                    .Select(_foodItemGenerator.ConvertToFoodItemVM)
                    .ToList(),
                Creator = menuData.Creator?.Name,
            };
        }

        public List<MenuTypeViewModel> GetAllMenuViewModel(string filterName)
        {
            var menuListDatas = _menuRepository.GetAllIncludeFoodItemsWithIngredients(filterName);
            var menuVMList = menuListDatas.Select(ConvertMenuDataToViewModel).ToList();

            return menuVMList;
        }


    }
}
