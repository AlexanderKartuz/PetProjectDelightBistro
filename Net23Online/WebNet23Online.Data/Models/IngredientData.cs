using WebNet23Online.Data.DataModels;

namespace WebNet23Online.Data.Models
{
    public class IngredientData : BaseModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public virtual List<FoodItemData> FoodItems { get; set; } = new();
        public virtual List<FoodItemIngredientData> FoodItemIngredientDatas { get; set; } = new();
        public int? CreatorId { get; set; }
        public virtual UserData? Creator { get; set; }
    }
}
