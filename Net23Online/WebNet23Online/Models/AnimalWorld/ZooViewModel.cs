using System.ComponentModel.DataAnnotations;
using WebNet23Online.Models.CustomValidatioAttributes.AnimalWorld;

namespace WebNet23Online.Models.AnimalWorld
{
    public class ZooViewModel
    {
        [Required]
        [ZooUniqueName]
        public string ZooName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        public List<AnimalFamilyViewModel> AnimalFamilies { get; set; } = new ();
        public List<AnimalSpeciesViewModel> AnimalSpecies { get; set; } = new ();
    }
}
