using WebNet23Online.Data.Models;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Models.DelightBistro
{
    public class FoodItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string ImgURL { get; set; }
        public string? MenuType { get; set; }
        public List<string> Ingredients { get; set; } // ?
        public List<CreateIngredientViewModel> IngredientsList { get; set; } = new(); // ?
        public Dictionary<int, int> IngredientQuantities { get; set; } = new(); // ?

        public string? Creator { get; set; }
        public int? CreatorId { get; set; }
        public bool CanDelete { get; set; }
    }
}
