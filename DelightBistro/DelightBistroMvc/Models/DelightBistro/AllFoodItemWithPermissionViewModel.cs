namespace DelightBistroMvc.Models.DelightBistro
{
    public class AllFoodItemWithPermissionViewModel
    {
        public List<FoodItemViewModel> FoodItems { get; set; }
        public bool IsAdmin { get; set; }
    }
}
