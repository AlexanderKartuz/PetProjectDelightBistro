using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebNet23Online.Models.CustomValidatioAttributes;

namespace WebNet23Online.Models.AnimeGirl
{
    public class CreateAnimeGirlViewModel
    {
        [IsUniqGirlName]
        [Required(
            ErrorMessageResourceType = typeof(Localizations.AnimeGirl),
            ErrorMessageResourceName = "Validation_NameRequired")]
        public string Name { get; set; }
        
        [DoubleCheck]
        public string DoubleCheckName { get; set; }

        public string Description { get; set; }

        public IFormFile? Image { get; set; }
        public string? Url { get; set; }

        [MinMaxCheck(1, 5,
            ErrorMessageResourceType = typeof(Localizations.AnimeGirl),
            ErrorMessageResourceName = "Validation_SizeRange")]
        public int Size { get; set; }

        public int? AnimeId { get; set; }

        public List<SelectListItem> Animes { get; set; } = new();
    }
}
