namespace WebNet23Online.Models.DelightBistro
{
    public class FoodItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImgURL { get; set; }
        public string? MenuType { get; set; }
        public List<CreateIngredientViewModel> IngredientsList { get; set; } = new();

        public string? Creator { get; set; }
        public int? CreatorId { get; set; }
        public bool CanDelete { get; set; }
    }
}
