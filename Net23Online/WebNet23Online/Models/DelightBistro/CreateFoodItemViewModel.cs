using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebNet23Online.Models.CustomValidatioAttributes.DelightBistro;

namespace WebNet23Online.Models.DelightBistro
{
    public class CreateFoodItemViewModel
    {
        public int Id { get; set; }
        [IsUniqueFoodItem]
        public string Name { get; set; }

        //[Range(1, 150)]
        public int Price { get; set; }
        public string? ImgURL { get; set; }

        //checkBoxId
        public List<int> SelectedIngredientsId { get; set; } = new();
        public List<CreateIngredientViewModel> Ingredients { get; set; } = new();
        public int? MenuId { get; set; }
        public List<SelectListItem> Menus { get; set; } = new();
        public IFormFile? Image { get; set; }

    }
}
