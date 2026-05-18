using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data.Enums;

namespace WebNet23Online.Models.Steam
{
    public class SteamUserProfileViewModel
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
