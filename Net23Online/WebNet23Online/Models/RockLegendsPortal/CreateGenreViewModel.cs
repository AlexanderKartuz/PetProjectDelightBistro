using System.ComponentModel.DataAnnotations;
using WebNet23Online.Models.CustomValidatioAttributes;

namespace WebNet23Online.Models.RockLegendsPortal
{
    public class CreateGenreViewModel
    {
        [IsUniqRockLegendsGenre]
        [Required(ErrorMessage = "Name can not be empty")]
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
        public string? CoverUrl { get; set; }
    }
}