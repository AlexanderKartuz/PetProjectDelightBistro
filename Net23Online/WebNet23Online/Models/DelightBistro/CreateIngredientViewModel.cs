using System.ComponentModel.DataAnnotations;
using WebNet23Online.Models.CustomValidatioAttributes.DelightBistro;

namespace WebNet23Online.Models.DelightBistro
{
    public class CreateIngredientViewModel
    {
        public int Id { get; set; }
        [Required]
        [IsUniqueIngredient]
        public string Name { get; set; }
        public int Quantity { get; set; } = 10;
        public bool IsSelected { get; set; } = false;
    }
}
