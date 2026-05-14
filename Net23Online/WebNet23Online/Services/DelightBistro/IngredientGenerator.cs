using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;
using WebNet23Online.Models.DelightBistro;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services.DelightBistro
{
    public class IngredientGenerator : IIngredientGenerator
    {
        private IIngredientsRepository _ingredientsRepository;
        private IAuthService _authService;

        public IngredientGenerator(IIngredientsRepository ingredientsRepository, IAuthService authService)
        {
            _ingredientsRepository = ingredientsRepository;
            _authService = authService;
        }
        public void FeelDataBase()
        {
            if (_ingredientsRepository.Any())
            {
                return;
            }
            _ingredientsRepository.Add(new IngredientData { Name = "Креветки" });
            _ingredientsRepository.Add(new IngredientData { Name = "Шампиньоны" });
            _ingredientsRepository.Add(new IngredientData { Name = "Лайм" });
            _ingredientsRepository.Add(new IngredientData { Name = "Паста" });
        }

        //public CreateIngredientViewModel ConvertDataToVM(IngredientData ingredientData)
        //{
        //    var ingredientViewModel = new CreateIngredientViewModel
        //    {
        //        Id = ingredientData.Id,
        //        Name = ingredientData.Name,
        //        //Quantity= ingredientData
        //    };
        //    return ingredientViewModel;
        //}

        //public List<CreateIngredientViewModel> GenerateIngredients(List<IngredientData> ingredientsData, FoodItemData foodItemData = null)
        //{
        //    var ingredientsViewModel = ingredientsData.Select(x => new CreateIngredientViewModel
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        IsSelected = foodItemData != null && foodItemData.IngredientsList.Any(i => i.Id == x.Id),
        //        Quantity = foodItemData?.FoodItemIngredientDatas
        //            .FirstOrDefault(fi => fi.IngredientDataId == x.Id)?
        //            .QuantityOfIngredients ?? 10
        //    }).ToList();

        //    return ingredientsViewModel;
        //}

        public void CreateIngredientData(CreateIngredientViewModel ingredient)
        {
            var ingredientData = new IngredientData
            {
                Name = ingredient.Name,
                Price=ingredient.Price,
                Creator = _authService.GetUser()
            };

            _ingredientsRepository.Add(ingredientData);
        }

        public List<CreateIngredientViewModel> GenerateIngredientsViewModelFromFoodItemData(FoodItemData foodItemData = null)
        {
            var ingredientsData = _ingredientsRepository.GetAll();

            var ingredientsViewModel = ingredientsData.Select(x => new CreateIngredientViewModel
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = foodItemData != null && foodItemData.IngredientsList.Any(i => i.Id == x.Id),
                Quantity = foodItemData?.FoodItemIngredientDatas
                .FirstOrDefault(fi => fi.IngredientDataId == x.Id)?
                .QuantityOfIngredients ?? 10
            }).ToList();

            return ingredientsViewModel;
        }

        //public List<CreateIngredientViewModel> GetAllCreateIngredientViewModel() // delete?
        //{
        //    var allIngredientViewModel = _ingredientsRepository
        //        .GetAll()
        //        .Select(x => new CreateIngredientViewModel
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            Quantity = 10,
        //            IsSelected = false
        //        })
        //        .ToList();
        //    return allIngredientViewModel;
        //}

        //private List<IngredientData> GetSelectedIngredientsDataFromFoodItemVM(CreateFoodItemViewModel viewModel)
        //{
        //    var selectedIngredients = new List<IngredientData>();

        //    var ingredientsIds = viewModel.IngredientsList // ids selected ingredients
        //        .Where(x => x.IsSelected)
        //        .Select(x => x.Id)
        //        .ToList();

        //    if (!viewModel.IngredientsList.IsNullOrEmpty())
        //    {
        //        selectedIngredients = _ingredientsRepository
        //            .GetAll()
        //            .Where(x => ingredientsIds.Contains(x.Id))
        //            .ToList();
        //    }

        //    return selectedIngredients;
        //}

        public List<CreateIngredientViewModel> GetSelectedCreateIngredientViewModelFromIngredientsList(List<CreateIngredientViewModel> ingredientsViewModel)
        {
            var selectedIngredientsViewModel = ingredientsViewModel.Where(x => x.IsSelected).ToList();

            return selectedIngredientsViewModel;
        }

        public List<FoodItemIngredientData> GetLinksFoodItemIngredientDataFromCreateFoodItemViewModel(CreateFoodItemViewModel viewModel)
        {
            var links = viewModel.IngredientsList
                .Where(x => x.IsSelected)
                .Select(x => new FoodItemIngredientData
                {   // ошибка при передаче Name при создании/обновлении блюда
                    IngredientDataId = x.Id,
                    QuantityOfIngredients = x.Quantity > 0 ? x.Quantity : 10
                })
                .ToList();

            return links;
        }
    }
}
