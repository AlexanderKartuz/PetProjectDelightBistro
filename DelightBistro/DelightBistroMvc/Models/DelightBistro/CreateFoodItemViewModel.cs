using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using DelightBistroMvc.Models.CustomValidatioAttributes.DelightBistro;

namespace DelightBistroMvc.Models.DelightBistro
{
    public class CreateFoodItemViewModel
    {
        public int Id { get; set; }
        [IsUniqueFoodItem]
        public string Name { get; set; }

        [Range(1, 150)]
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }

        public List<CreateIngredientViewModel> IngredientsList { get; set; } = new();

        public int? MenuId { get; set; }
        public List<SelectListItem> Menus { get; set; } = new();
        public IFormFile? Image { get; set; }

    }
}
