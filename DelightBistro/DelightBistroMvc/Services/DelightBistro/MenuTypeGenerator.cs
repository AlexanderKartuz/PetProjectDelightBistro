using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Models.DelightBistro;
using DelightBistroMvc.Services.Interfaces;

namespace DelightBistroMvc.Services.DelightBistro
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

        public void CreateMenuData(CreateMenuViewModel viewModel)
        {
            var menuData = new MenuData
            {
                Name = viewModel.Name,
                Creator = _authService.GetUserAsync()
            };

            _menuRepository.AddAsync(menuData);
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
            var menuListDatas = _menuRepository.GetAllIncludeFoodItemsWithIngredientsLinks(filterName);
            var menuVMList = menuListDatas.Select(ConvertMenuDataToViewModel).ToList();

            return menuVMList;
        }


    }
}
