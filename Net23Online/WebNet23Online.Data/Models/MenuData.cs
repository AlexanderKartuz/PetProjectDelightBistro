namespace WebNet23Online.Data.Models
{
    public class MenuData : BaseModel
    {
        public string Name { get; set; }

        public virtual List<FoodItemData> FoodItems { get; set; } = new();
        public int? CreatorId { get; set; }
        public virtual UserData? Creator { get; set; }

    }
}
