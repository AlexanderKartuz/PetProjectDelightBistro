using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using DelightBistroMvc.Controllers.CustomAuthAttribute;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Hubs;
using DelightBistroMvc.Hubs.Interfaces;
using DelightBistroMvc.Models.DelightBistro;
using DelightBistroMvc.Services.Interfaces;


namespace DelightBistroMvc.Controllers
{
    public class DelightBistroController : Controller
    {
        private IDelightBistroMainIndexGenerator _delightBistroMainIndexGenerator;
        private IFoodItemGenerator _foodItemGenerator;
        private IMenuTypeGenerator _menuTypeGenerator;
        private IIngredientGenerator _ingredientGenerator;

        private IHubContext<DeligtBistroHub, IDeligtBistroHub> _deligtBistroHub;


        public DelightBistroController(IFoodItemGenerator foodItemGenerator,
            IMenuTypeGenerator menuTypeGenerator,
            IIngredientGenerator ingredientGenerator,
            IHubContext<DeligtBistroHub,
            IDeligtBistroHub> deligtBistroHub,
            IDelightBistroMainIndexGenerator delightBistroMainIndexGenerator)
        {

            _foodItemGenerator = foodItemGenerator;
            _menuTypeGenerator = menuTypeGenerator;
            _ingredientGenerator = ingredientGenerator;

            _deligtBistroHub = deligtBistroHub;
            _delightBistroMainIndexGenerator = delightBistroMainIndexGenerator;
        }

        public async Task<IActionResult> Index(string menuType)
        {
            var viewModel = await _delightBistroMainIndexGenerator.GetMainIndexViewModelAsync(menuType);

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        [IsModerator]
        public IActionResult CreateMenu()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [IsModerator]
        public IActionResult CreateMenu(CreateMenuViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            _menuTypeGenerator.CreateMenuData(viewModel);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        [IsModerator]
        public IActionResult CreateIngredient()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [IsModerator]
        public IActionResult CreateIngredient(CreateIngredientViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            _ingredientGenerator.CreateIngredientData(viewModel);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        [IsModerator]
        public IActionResult FoodBuilderData(int id)
        {
            var createFoodItemVM = _foodItemGenerator.GetCreateFoodItemViewModel(id > 0 ? id : null);

            return View(createFoodItemVM);
        }

        [HttpPost]
        [Authorize]
        [IsModerator]
        public IActionResult FoodBuilderData(CreateFoodItemViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Menus = _foodItemGenerator.SelectMenuList();
                viewModel.IngredientsList = _ingredientGenerator.GenerateIngredientsViewModelFromFoodItemData();
                return View(viewModel);
            }

            if (viewModel.Id == 0)
            {
                _foodItemGenerator.CreateFoodItemData(viewModel);

                _deligtBistroHub.Clients.All.NewFoodWasCreated(viewModel.Name, viewModel.Price);

                return RedirectToAction(nameof(Index));
            }
            _foodItemGenerator.ChangeFoodItemData(viewModel);

            _deligtBistroHub.Clients.All.NewFoodWasCreated(viewModel.Name, viewModel.Price);

            return RedirectToAction(nameof(AllFoodItems));
        }

        [Authorize]
        [IsEmployee]
        public IActionResult AllFoodItems()
        {
            var foodItemsWithPermissionVM = _foodItemGenerator.GetAllFoodItemWithPermission();

            return View(foodItemsWithPermissionVM);
        }

        [Authorize]
        [IsEmployee]
        [HttpPost]
        public IActionResult DeleteFoodItem(int id = 0)
        {
            _foodItemGenerator.DeleteFoodItem(id);

            return RedirectToAction(nameof(AllFoodItems));
        }

        public IActionResult GenerateTable()
        {
            var fileStream = _foodItemGenerator.GenerateTable();

            return File(fileStream, "text/csv");
        }
        public IActionResult Stats()
        {
            var viewModels = _foodItemGenerator.GetFoodItemStatsViewModels();

            return View(viewModels);
        }

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult NewChat()
        {
            return View();
        }
    }
}
