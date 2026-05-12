using Microsoft.AspNetCore.Mvc;
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

        public CreateIngredientViewModel ConvertDataToVM(IngredientData ingredientData)
        {
            var ingredientViewModel = new CreateIngredientViewModel
            {
                Id = ingredientData.Id,
                Name = ingredientData.Name,
                //Quantity=
            };
            return ingredientViewModel;
        }

        public List<CreateIngredientViewModel> GenerateIngredients(List<IngredientData> ingredientsData, FoodItemData foodItemData = null)
        {
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

        public void CreateIngredientData(CreateIngredientViewModel ingredient)
        {
            var ingredientData = new IngredientData
            {
                Name = ingredient.Name,
                Creator = _authService.GetUser()
            };

            _ingredientsRepository.Add(ingredientData);
        }

    }
}
