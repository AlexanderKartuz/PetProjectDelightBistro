namespace WebNet23Online.Data.DataModels
{
    public class FoodItemStatsDataModel
    {
        public string FoodItemName { get; set; }
        public int IngredientCount { get; set; }
        public decimal FoodItemPrice { get; set; }
        public decimal TotalPriceIngredient { get; set; }
        public decimal Profit { get; set; }
    }
}
