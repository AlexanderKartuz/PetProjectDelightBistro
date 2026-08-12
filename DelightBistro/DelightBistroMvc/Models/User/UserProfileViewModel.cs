using Microsoft.AspNetCore.Mvc.Rendering;
using DelightBistroMvc.Data.Enums;

namespace DelightBistroMvc.Models.User
{
    public class UserProfileViewModel
    {
        public int UserId { get; set; }
        public string AvatarUrl { get; set; }
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobilephone { get; set; }
        public Language Language { get; set; }
        public List<SelectListItem> Languages { get; set; } = new();
    }
}
