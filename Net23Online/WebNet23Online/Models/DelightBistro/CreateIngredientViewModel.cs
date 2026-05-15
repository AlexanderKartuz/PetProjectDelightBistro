using System.ComponentModel.DataAnnotations;
using WebNet23Online.Models.CustomValidatioAttributes.DelightBistro;

namespace WebNet23Online.Models.DelightBistro
{
    public class CreateIngredientViewModel
    {
        public int Id { get; set; }
        [Required]
        //[IsUniqueIngredient] // ошибка при передаче Name при создании/обновлении блюда
        public string Name { get; set; } 
        public decimal Quantity { get; set; } = 10;
        [Range(1, 1000)]
        public decimal Price { get; set; } = 1;
        public bool IsSelected { get; set; } = false;
    }
}
