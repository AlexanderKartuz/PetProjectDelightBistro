using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.ComponentModel.DataAnnotations;
using WebNet23Online.Models.CustomValidatioAttributes.AnimalWorld;

namespace WebNet23Online.Models.AnimalWorld
{
    public class AnimalSpeciesViewModel
    {
        [Required]
        [AnimalSpeciesUniqueName]
        public string AnimalSpeciesName { get; set; }

        public IFormFile AnimalSpeciesImage { get; set; }

        public string? Url { get; set; }

        [Required]
        public int AnimalFamilyId { get; set; }

        public List<SelectListItem>? AnimalFamilyNames { get; set; }

        [Required]
        public string NativeRange { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }
    }
}
