using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
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
        private IIngredientsRepository _ingredientsRepository;
        private IIngredientGenerator _ingredientGenerator;
        private IAuthService _authService;
        private IWebHostEnvironment _webHostEnvironment;

        public FoodItemGenerator(IFoodItemRepository foodItemRepository, IMenuRepository menuRepository,
            IIngredientsRepository ingredientsRepository, IIngredientGenerator ingredientGenerator,
            IAuthService authService, IWebHostEnvironment webHostEnvironment)
        {
            _foodItemRepository = foodItemRepository;
            _menuRepository = menuRepository;
            _ingredientsRepository = ingredientsRepository;
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
                Price = 5,
                ImgURL = "https://png.klev.club/uploads/posts/2024-03/png-klev-club-p-stakan-vodi-png-9.png",

            };
            _foodItemRepository.Add(foodItemData);
        }

        public void CreateFoodItemData(CreateFoodItemViewModel viewModel)
        {
            var selectedIngredients = GetSelectedIngredients(viewModel);
            var selectedMenu = GetSelectedMenu(viewModel);

            var newFoodItemData = new FoodItemData()
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                ImgURL = viewModel.ImgURL,

                MenuData = selectedMenu,
                IngredientsList = selectedIngredients,
                Creator = _authService.GetUser()
            };

            _foodItemRepository.Add(newFoodItemData);

            GetImgFile(viewModel, newFoodItemData);

        }

        public void ChangeFoodItemData(CreateFoodItemViewModel viewModel)
        {
            var selectedIngredients = GetSelectedIngredients(viewModel);
            var selectedMenu = GetSelectedMenu(viewModel);

            var changedFoodItemData = _foodItemRepository.GetByIdIncludeMenuAndIngredients(viewModel.Id);

            changedFoodItemData.Name = viewModel.Name;
            changedFoodItemData.Price = viewModel.Price;
            changedFoodItemData.ImgURL = viewModel.ImgURL;
            changedFoodItemData.MenuData = selectedMenu;
            changedFoodItemData.IngredientsList = selectedIngredients;

            _foodItemRepository.Update(changedFoodItemData);
            GetImgFile(viewModel, changedFoodItemData);

        }

        public FoodItemViewModel ConvertToFoodItemVM(FoodItemData foodItemData)
        {
            var foodItemViewModel = new FoodItemViewModel
            {
                Id = foodItemData.Id,
                Name = foodItemData.Name,
                Price = foodItemData.Price,
                ImgURL = foodItemData.ImgURL,
                MenuType = foodItemData.MenuData?.Name ?? "Общее меню",
                Ingredients = foodItemData.IngredientsList
                .Select(fi => fi.Name).ToList(),
                Creator = foodItemData.Creator?.Name,
                CreatorId = foodItemData.CreatorId,
            };

            return foodItemViewModel;
        }

        public CreateFoodItemViewModel ConvertToCreateFoodItemVM(FoodItemData foodItemData = null)
        {
            if (foodItemData == null)
            {
                var createFoodItemVM = new CreateFoodItemViewModel()
                {
                    Menus = SelectMenuList(),
                    Ingredients = ChekBoxIngredients()
                };

                return createFoodItemVM;
            }

            var viewModel = new CreateFoodItemViewModel
            {
                Id = foodItemData.Id,
                Name = foodItemData.Name,
                Price = foodItemData.Price,
                ImgURL = foodItemData.ImgURL,

                MenuId = foodItemData.MenuData?.Id,
                SelectedIngredientsId = foodItemData.IngredientsList
                .Select(x => x.Id).ToList(),

                Ingredients = ChekBoxIngredients(foodItemData),
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

        public List<CreateIngredientViewModel> ChekBoxIngredients(FoodItemData foodItemData = null)
        {
            var allIngredientsDatas = _ingredientsRepository.GetAll();
            var allIngredientVM = _ingredientGenerator.GenerateIngredients(allIngredientsDatas, foodItemData);

            return allIngredientVM;
        }

        private List<IngredientData> GetSelectedIngredients(CreateFoodItemViewModel viewModel)
        {
            var selectedIngredients = new List<IngredientData>();
            if (!viewModel.SelectedIngredientsId.IsNullOrEmpty())
            {
                selectedIngredients = _ingredientsRepository.GetAll()
                    .Where(x => viewModel.SelectedIngredientsId.Contains(x.Id)).ToList();
            }

            return selectedIngredients;
        }

        private MenuData GetSelectedMenu(CreateFoodItemViewModel viewModel)
        {
            MenuData menuData = null;
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
                file.WriteLine($"Id,Name,Price,ImgUrl,MenyType,Ingredients");

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
    }
}
