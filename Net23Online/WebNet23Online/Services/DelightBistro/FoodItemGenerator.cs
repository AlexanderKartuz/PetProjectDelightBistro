using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;
using WebNet23Online.Models.DelightBistro;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services.DelightBistro
{
    public class FoodItemGenerator : IFoodItemGenerator
    {
        private IFoodItemRepository _foodItemRepository;
        private IMenuRepository _menuRepository;
        private IIngredientGenerator _ingredientGenerator;
        private IAuthService _authService;
        private IWebHostEnvironment _webHostEnvironment;

        public FoodItemGenerator(
            IFoodItemRepository foodItemRepository
            , IMenuRepository menuRepository
            , IIngredientGenerator ingredientGenerator
            , IAuthService authService
            , IWebHostEnvironment webHostEnvironment)
        {
            _foodItemRepository = foodItemRepository;
            _menuRepository = menuRepository;
            _ingredientGenerator = ingredientGenerator;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
        }
        public void FeelDataBase()
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

        public void CreateFoodItemData(CreateFoodItemViewModel viewModel)
        {
            var selectedMenu = GetSelectedMenu(viewModel);

            var links = _ingredientGenerator.GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(viewModel);

            var newFoodItemData = new FoodItemData()
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                ImgURL = viewModel.ImgURL,

                MenuData = selectedMenu,

                FoodItemIngredientDatas = links,
                Creator = _authService.GetUser()
            };

            _foodItemRepository.Add(newFoodItemData);

            GetImgFile(viewModel, newFoodItemData);

        }

        public void ChangeFoodItemData(CreateFoodItemViewModel viewModel)
        {

            if (viewModel.Id > 0)
            {
                var links = _ingredientGenerator.GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(viewModel);

                var changedFoodItemData = _foodItemRepository.GetByIdIncludeMenuAndIngredientsLinks(viewModel.Id);
                if (changedFoodItemData == null)
                {
                    throw new InvalidOperationException($"Блюдо с Id = {viewModel.Id} не найдено");
                }

                changedFoodItemData.Name = viewModel.Name;
                changedFoodItemData.Price = viewModel.Price;
                changedFoodItemData.ImgURL = viewModel.ImgURL;
                changedFoodItemData.MenuData = GetSelectedMenu(viewModel);

                changedFoodItemData.FoodItemIngredientDatas = links;


                _foodItemRepository.Update(changedFoodItemData);
                GetImgFile(viewModel, changedFoodItemData);
            }
        }

        public FoodItemViewModel ConvertToFoodItemVM(FoodItemData foodItemData)
        {
            var allIngredientsVM = _ingredientGenerator.GenerateIngredientsViewModelFromFoodItemData(foodItemData);
            var selectedIngredientsViewModel = _ingredientGenerator.GetSelectedCreateIngredientViewModelFromIngredientsList(allIngredientsVM);

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

        public CreateFoodItemViewModel ConvertToCreateFoodItemVM(FoodItemData? foodItemData = null)
        {
            if (foodItemData == null)
            {
                var createFoodItemVM = new CreateFoodItemViewModel()
                {
                    Menus = SelectMenuList(),
                    IngredientsList = _ingredientGenerator.GenerateIngredientsViewModelFromFoodItemData(foodItemData)
                };

                return createFoodItemVM;
            }

            var allIngredientsVM = _ingredientGenerator.GenerateIngredientsViewModelFromFoodItemData(foodItemData);

            var viewModel = new CreateFoodItemViewModel
            {
                Id = foodItemData.Id,
                Name = foodItemData.Name,
                Price = foodItemData.Price,
                ImgURL = foodItemData.ImgURL,

                MenuId = foodItemData.MenuData?.Id,

                IngredientsList = allIngredientsVM,
                Menus = SelectMenuList()
            };

            return viewModel;
        }

        public List<SelectListItem> SelectMenuList()
        {
            var allMenuData = _menuRepository.GetAll();
            var menuListItems = new List<SelectListItem>();
            menuListItems.AddRange(allMenuData.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }));
            return menuListItems;
        }

        private MenuData? GetSelectedMenu(CreateFoodItemViewModel viewModel)
        {
            MenuData? menuData = null;
            if (viewModel.MenuId != null)
            {
                menuData = _menuRepository.Get(viewModel.MenuId.Value);
            }
            return menuData;
        }

        public void DeleteFoodItem(int id)
        {
            var fooditem = _foodItemRepository.Get(id);
            if (fooditem != null)
            {
                _foodItemRepository.Remove(fooditem);
            }
        }

        public AllFoodItemWithPermissionViewModel GetFoodsWithPermission(List<FoodItemViewModel> foodItemsViewModel)
        {
            var currentUser = _authService.GetUser()!;
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

        private void GetImgFile(CreateFoodItemViewModel viewModel, FoodItemData foodItemData)
        {
            if (viewModel.Image != null)
            {
                var pathToWwwRotFolder = _webHostEnvironment.WebRootPath;
                var pathToFolder = "images\\delight-bistro\\";
                var fileName = $"fooditem-{foodItemData.Id}.jpg";
                var path = Path.Combine(pathToWwwRotFolder, pathToFolder, fileName);

                using (var foodItemImgFile = new FileStream(path, FileMode.Create))
                {
                    viewModel.Image.CopyTo(foodItemImgFile);
                }

                foodItemData.ImgURL = $"/images/delight-bistro/{fileName}";
                _foodItemRepository.Update(foodItemData);
            }
        }

        public FileStream GenerateTable()
        {
            var path = Path.GetTempFileName();

            using (var file = File.CreateText(path))
            {
                file.WriteLine($"Id,Name,Price,ImgUrl,MenyType,IngredientsList");

                var foodDatas = _foodItemRepository.GetAllIncludeMenuAndIngredients();

                foreach (var foodItem in foodDatas)
                {
                    var foodName = ReplaceSeparateSymbols(foodItem.Name);
                    var foodItemName = string.Join(";",
                        (foodItem.IngredientsList
                        .Select(x => x.Name)));

                    file.WriteLine($"{foodItem.Id},"
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

        public List<FoodItemStatsViewModel> GetFoodItemStatsViewModels()
        {
            var allFoodItemStatsDataModel = _foodItemRepository.GetFoodItemStats();

            var allFoodItemStatsViewModel = allFoodItemStatsDataModel.Select(x => new FoodItemStatsViewModel
            {
                FoodItemName = x.FoodItemName,
                IngredientCount = x.IngredientCount,
                FoodItemPrice = x.FoodItemPrice,
                TotalPriceIngredient = x.TotalPriceIngredient,
                Profit = x.Profit,
            }).ToList();

            return allFoodItemStatsViewModel;
        }
    }
}
