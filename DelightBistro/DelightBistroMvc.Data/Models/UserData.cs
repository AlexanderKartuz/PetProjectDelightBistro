using DelightBistroMvc.Data.Enums;

namespace DelightBistroMvc.Data.Models
{
    public class UserData : BaseModel
    {
        public string Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobilephone { get; set; }
        public string PasswordHash { get; set; }

        public UserRole Role { get; set; }
        public Language Language { get; set; }
        public string? AvatarUrl { get; set; }
        public int? UserProfileId { get; set; }

        public virtual List<UserData> MyFriends { get; set; }
        public virtual List<UserData> WhoIsMyFriends { get; set; }
        public virtual UserProfileData? UserProfile { get; set; }

        public virtual List<IngredientData> CreatedIngredients { get; set; } = new();
        public virtual List<FoodItemData> CreatedFoodItems { get; set; } = new();
        public virtual List<MenuData> CreatedMenus { get; set; } = new();
        public virtual List<OrderData> Orders { get; set; } = new();

        public virtual List<NotificationData> Notifications { get; set; }

    }
}
