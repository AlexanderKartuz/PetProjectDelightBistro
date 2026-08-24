using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Models.DelightBistro;
using DelightBistroMvc.Services.Interfaces;

namespace DelightBistroMvc.Services.DelightBistro
{
    public class MenuTypeGenerator : IMenuTypeGenerator
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IFoodItemGenerator _foodItemGenerator;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public MenuTypeGenerator(IMenuRepository menuRepository, IFoodItemGenerator foodItemGenerator, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _menuRepository = menuRepository;
            _foodItemGenerator = foodItemGenerator;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateMenuDataAsync(CreateMenuViewModel viewModel
            , CancellationToken cancellationToken = default)
        {
            var menuData = new MenuData
            {
                Name = viewModel.Name,
                Creator = await _authService.GetUserAsync(cancellationToken)
            };

            await _menuRepository.AddAsync(menuData, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public MenuTypeViewModel ConvertMenuDataToViewModel(MenuData menuData)
        {

            return new MenuTypeViewModel
            {
                Id = menuData.Id,
                Name = menuData.Name,
                FoodItems = (menuData.FoodItems ?? new List<FoodItemData>())
                    .Select(_foodItemGenerator.ConvertToFoodItemVm)
                    .ToList(),
                Creator = menuData.Creator?.Name,
            };
        }

        public async Task<List<MenuTypeViewModel>> GetAllMenuViewModelAsync(string filterName,
            CancellationToken cancellationToken = default)
        {
            var menuListDatas = await _menuRepository
                .GetAllIncludeFoodItemsWithIngredientsLinksAsync(filterName, cancellationToken);
            var menuVMList = menuListDatas.Select(ConvertMenuDataToViewModel).ToList();

            return menuVMList;
        }


    }
}
