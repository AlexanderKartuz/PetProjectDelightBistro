using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using DelightBistroMvc.Controllers.CustomAuthAttribute;
using DelightBistroMvc.Hubs;
using DelightBistroMvc.Hubs.Interfaces;
using DelightBistroMvc.Models.DelightBistro;
using DelightBistroMvc.Services.Interfaces;


namespace DelightBistroMvc.Controllers
{
    public class DelightBistroController : Controller
    {
        private readonly IDelightBistroMainIndexGenerator _delightBistroMainIndexGenerator;
        private readonly IFoodItemGenerator _foodItemGenerator;
        private readonly IMenuTypeGenerator _menuTypeGenerator;
        private readonly IIngredientGenerator _ingredientGenerator;

        private readonly IHubContext<DeligtBistroHub, IDeligtBistroHub> _deligtBistroHub;


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

        public async Task<IActionResult> Index(string menuType,
            CancellationToken cancellationToken = default)
        {
            var viewModel = await _delightBistroMainIndexGenerator
                .GetMainIndexViewModelAsync(menuType, cancellationToken);

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
        public async Task<IActionResult> CreateMenu(CreateMenuViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            await _menuTypeGenerator.CreateMenuDataAsync(viewModel, cancellationToken);

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
        public async Task<IActionResult> CreateIngredient(CreateIngredientViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            await _ingredientGenerator.CreateIngredientDataAsync(viewModel, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        [IsModerator]
        public async Task<IActionResult> FoodBuilderData(int id, CancellationToken cancellationToken = default)
        {
            var createFoodItemVM = await _foodItemGenerator
                .GetCreateFoodItemViewModelAsync(id > 0 ? id : null, cancellationToken);

            return View(createFoodItemVM);
        }

        [HttpPost]
        [Authorize]
        [IsModerator]
        public async Task<IActionResult> FoodBuilderData(CreateFoodItemViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Menus = await _foodItemGenerator
                    .SelectMenuListAsync(cancellationToken);
                viewModel.IngredientsList = await _ingredientGenerator
                    .GenerateIngredientsViewModelFromFoodItemDataAsync(cancellation: cancellationToken);
                return View(viewModel);
            }

            if (viewModel.Id == 0)
            {
                await _foodItemGenerator
                    .CreateFoodItemDataAsync(viewModel, cancellationToken);

                await _deligtBistroHub.Clients.All.NewFoodWasCreated(viewModel.Name, viewModel.Price);

                return RedirectToAction(nameof(Index));
            }
            await _foodItemGenerator.ChangeFoodItemDataAsync(viewModel, cancellationToken);

            await _deligtBistroHub.Clients.All.NewFoodWasCreated(viewModel.Name, viewModel.Price);

            return RedirectToAction(nameof(AllFoodItems));
        }

        [Authorize]
        [IsEmployee]
        public async Task<IActionResult> AllFoodItems(CancellationToken cancellationToken = default)
        {
            var foodItemsWithPermissionVM = await _foodItemGenerator
                .GetAllFoodItemWithPermissionAsync(cancellationToken);

            return View(foodItemsWithPermissionVM);
        }

        [Authorize]
        [IsEmployee]
        [HttpPost]
        public async Task<IActionResult> DeleteFoodItem(int id = 0,
            CancellationToken cancellationToken = default)
        {
            await _foodItemGenerator.DeleteFoodItemAsync(id, cancellationToken);

            return RedirectToAction(nameof(AllFoodItems));
        }

        public async Task<IActionResult> GenerateTable(CancellationToken cancellationToken = default)
        {
            var fileStream = await _foodItemGenerator.GenerateTableAsync(cancellationToken);

            return File(fileStream, "text/csv");
        }
        public async Task<IActionResult> Stats(CancellationToken cancellationToken = default)
        {
            var viewModels = await _foodItemGenerator
                .GetFoodItemStatsViewModelsAsync(cancellationToken);

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
