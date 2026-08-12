using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.DataModels
{
    public class FoodItemIngredientData
    {
        public int FoodItemDataId { get; set; }
        public FoodItemData? FoodItemData { get; set; }

        public int IngredientDataId { get; set; }
        public IngredientData? IngredientData { get; set; }

        public decimal QuantityOfIngredients { get; set; }

    }
}
