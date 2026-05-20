using WebNet23Online.Data.DataModels;

namespace WebNet23Online.Data.Models
{
    public class FoodItemData : BaseModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImgURL { get; set; }

        public virtual MenuData? MenuData { get; set; }
        public virtual List<IngredientData> IngredientsList { get; set; } = new();
        public virtual List<FoodItemIngredientData> FoodItemIngredientDatas { get; set; } = new();

        public int? CreatorId { get; set; }
        public virtual UserData? Creator { get; set; }
    }
}
