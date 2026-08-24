using Microsoft.AspNetCore.Mvc.Rendering;
using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Models.DelightBistro;
using DelightBistroMvc.Services.Interfaces;
using DelightBistroMvc.Data.Repositories.Interfaces;

namespace DelightBistroMvc.Services.DelightBistro
{
    public class FoodItemGenerator : IFoodItemGenerator
    {
        private IFoodItemRepository _foodItemRepository;
        private IMenuRepository _menuRepository;
        private IIngredientGenerator _ingredientGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private IAuthService _authService;
        private IWebHostEnvironment _webHostEnvironment;

        public FoodItemGenerator(
            IFoodItemRepository foodItemRepository,
            IMenuRepository menuRepository,
            IIngredientGenerator ingredientGenerator,
            IAuthService authService,
            IWebHostEnvironment webHostEnvironment,
            IUnitOfWork unitOfWork)
        {
            _foodItemRepository = foodItemRepository;
            _menuRepository = menuRepository;
            _ingredientGenerator = ingredientGenerator;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateFoodItemDataAsync(CreateFoodItemViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            var selectedMenu = await GetSelectedMenuAsync(viewModel, cancellationToken);

            var links = _ingredientGenerator.GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(viewModel);

            var newFoodItemData = new FoodItemData()
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                ImgURL = viewModel.ImgUrl,

                MenuData = selectedMenu,

                FoodItemIngredientDatas = links,
                Creator = await _authService.GetUserAsync(cancellationToken)
            };

            await _foodItemRepository.AddAsync(newFoodItemData, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await GetImgFileAsync(viewModel, newFoodItemData, cancellationToken);
        }

        public async Task ChangeFoodItemDataAsync(CreateFoodItemViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            if (viewModel.Id <= 0)
            {
                return;
            }

            var links = _ingredientGenerator
                .GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(viewModel);

            var changedFoodItemData = await _foodItemRepository
                .GetByIdIncludeMenuAndIngredientsLinksAsync(viewModel.Id, cancellationToken)
                ?? throw new InvalidOperationException($"Блюдо с Id = {viewModel.Id} не найдено");

            changedFoodItemData.Name = viewModel.Name;
            changedFoodItemData.Price = viewModel.Price;
            changedFoodItemData.ImgURL = viewModel.ImgUrl;
            changedFoodItemData.MenuData = await GetSelectedMenuAsync(viewModel, cancellationToken);

            changedFoodItemData.FoodItemIngredientDatas.Clear();

            foreach (var item in links)
            {
                changedFoodItemData.FoodItemIngredientDatas.Add(item);
            }

            await _foodItemRepository.UpdateAsync(changedFoodItemData, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await GetImgFileAsync(viewModel, changedFoodItemData, cancellationToken);
        }

        public FoodItemViewModel ConvertToFoodItemVm(FoodItemData foodItemData)
        {
            var selectedIngredientsViewModel = _ingredientGenerator.MapSelectedIngredients(foodItemData);

            var foodItemViewModel = new FoodItemViewModel
            {
                Id = foodItemData.Id,
                Name = foodItemData.Name,
                Price = foodItemData.Price,
                ImgURL = foodItemData.ImgURL,
                MenuType = foodItemData.MenuData?.Name ?? "Общее меню",

                IngredientsList = selectedIngredientsViewModel,

                Creator = foodItemData.Creator?.Name,
                CreatorId = foodItemData.CreatorId,
            };

            return foodItemViewModel;
        }

        public async Task<CreateFoodItemViewModel> ConvertToCreateFoodItemVmAsync(
            FoodItemData? foodItemData = null,
            CancellationToken cancellationToken = default)
        {
            if (foodItemData == null)
            {
                var createFoodItemVM = new CreateFoodItemViewModel()
                {
                    Menus = await SelectMenuListAsync(cancellationToken),
                    IngredientsList = await _ingredientGenerator
                    .GenerateIngredientsViewModelFromFoodItemDataAsync(foodItemData, cancellationToken)
                };

                return createFoodItemVM;
            }

            var allIngredientsVM = await _ingredientGenerator.GenerateIngredientsViewModelFromFoodItemDataAsync(foodItemData, cancellationToken);

            var viewModel = new CreateFoodItemViewModel
            {
                Id = foodItemData.Id,
                Name = foodItemData.Name,
                Price = foodItemData.Price,
                ImgUrl = foodItemData.ImgURL,

                MenuId = foodItemData.MenuData?.Id,

                IngredientsList = allIngredientsVM,
                Menus = await SelectMenuListAsync(cancellationToken)
            };

            return viewModel;
        }

        public async Task<List<SelectListItem>> SelectMenuListAsync(CancellationToken cancellationToken = default)
        {
            var allMenuData = await _menuRepository.GetAllAsync(cancellationToken);
            var menuListItems = new List<SelectListItem>();
            menuListItems.AddRange(allMenuData.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }));
            return menuListItems;
        }

        private async Task<MenuData?> GetSelectedMenuAsync(
            CreateFoodItemViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            if (viewModel.MenuId == null)
            {
                return null;
            }
            var menuData = await _menuRepository
                .GetAsync(viewModel.MenuId.Value, cancellationToken);

            return menuData;
        }

        public async Task DeleteFoodItemAsync(int id, CancellationToken cancellationToken = default)
        {
            await _foodItemRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<AllFoodItemWithPermissionViewModel> GetFoodsWithPermissionAsync(
            List<FoodItemViewModel> foodItemsViewModel,
            CancellationToken cancellationToken = default)
        {
            var currentUser = await _authService.GetUserAsync(cancellationToken);
            var isAdmin = currentUser?.Role == UserRole.Admin;
            var currentUserId = currentUser?.Id;

            foreach (var item in foodItemsViewModel)
            {
                item.CanDelete = isAdmin || (currentUserId != null && item.CreatorId == currentUserId);
            }

            var viewModel = new AllFoodItemWithPermissionViewModel()
            {
                FoodItems = foodItemsViewModel,
                IsAdmin = isAdmin,
            };


            return viewModel;
        }

        private async Task GetImgFileAsync(
            CreateFoodItemViewModel viewModel,
            FoodItemData foodItemData,
            CancellationToken cancellationToken = default)
        {
            if (viewModel.Image != null)
            {
                var pathToWwwRotFolder = _webHostEnvironment.WebRootPath;
                var pathToFolder = "images\\delight-bistro\\";
                var fileName = $"fooditem-{foodItemData.Id}.jpg";
                var path = Path.Combine(pathToWwwRotFolder, pathToFolder, fileName);

                await using (var foodItemImgFile = new FileStream(path, FileMode.Create))
                {
                    await viewModel.Image.CopyToAsync(foodItemImgFile, cancellationToken);
                }

                foodItemData.ImgURL = $"/images/delight-bistro/{fileName}";

                await _foodItemRepository.UpdateAsync(foodItemData, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<FileStream> GenerateTableAsync(CancellationToken cancellationToken = default)
        {
            var path = Path.GetTempFileName();

            await using (var file = File.CreateText(path))
            {
                await file.WriteLineAsync($"Id,Name,Price,ImgUrl,MenuType,IngredientsList");

                var foodDatas = await _foodItemRepository
                    .GetAllIncludeMenuAndIngredientsAsync(cancellationToken);

                foreach (var foodItem in foodDatas)
                {
                    var foodName = ReplaceSeparateSymbols(foodItem.Name);
                    var foodItemName = string.Join(";",
                        foodItem.FoodItemIngredientDatas
                            .Select(x => x.IngredientData?.Name)
                            .Where(name => !string.IsNullOrEmpty(name)));

                    await file.WriteLineAsync($"{foodItem.Id},"
                        + $"{foodName},{foodItem.Price},"
                        + $"{foodItem.ImgURL ?? ""},"
                        + $"{foodItem.MenuData?.Name ?? ""},"
                        + $"{foodItemName}");
                }
            }
            var fileStream = new FileStream(path, FileMode.Open);

            return fileStream;
        }

        private string ReplaceSeparateSymbols(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "";
            }

            if (name.Contains(","))
            {
                var newName = name.Replace(",", ";");
                return newName;
            }
            return name;
        }

        public async Task<List<FoodItemStatsViewModel>> GetFoodItemStatsViewModelsAsync(CancellationToken cancellationToken = default)
        {
            var allFoodItemStatsDataModel = await _foodItemRepository
                .GetFoodItemStatsAsync(cancellationToken);

            var allFoodItemStatsViewModel = allFoodItemStatsDataModel
                .Select(x => new FoodItemStatsViewModel
                {
                    FoodItemName = x.FoodItemName,
                    IngredientCount = x.IngredientCount,
                    FoodItemPrice = x.FoodItemPrice,
                    TotalPriceIngredient = x.TotalPriceIngredient,
                    Profit = x.Profit,
                }).ToList();

            return allFoodItemStatsViewModel;
        }

        public async Task<AllFoodItemWithPermissionViewModel> GetAllFoodItemWithPermissionAsync(CancellationToken cancellationToken = default)
        {
            var foodItemsDatas = await _foodItemRepository
                .GetAllIncludeMenuAndIngredientsAsync(cancellationToken);
            var foodItemsViewModel = foodItemsDatas
                .Select(ConvertToFoodItemVm)
                .ToList();

            var foodsWithPermissionVM = await GetFoodsWithPermissionAsync(foodItemsViewModel, cancellationToken);

            return foodsWithPermissionVM;
        }

        public async Task<CreateFoodItemViewModel> GetCreateFoodItemViewModelAsync(
            int? id = null,
            CancellationToken cancellationToken = default)
        {
            if (id is null or <= 0)
            {
                return await ConvertToCreateFoodItemVmAsync(cancellationToken: cancellationToken);
            }

            var foodItemData = await _foodItemRepository
                .GetByIdIncludeMenuAndIngredientsLinksAsync(id.Value, cancellationToken);

            var createFoodVM = await ConvertToCreateFoodItemVmAsync(foodItemData, cancellationToken);
            return createFoodVM;
        }
    }
}
