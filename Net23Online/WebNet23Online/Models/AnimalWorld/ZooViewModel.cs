using System.ComponentModel.DataAnnotations;
using WebNet23Online.Models.CustomValidatioAttributes.AnimalWorld;

namespace WebNet23Online.Models.AnimalWorld
{
    public class ZooViewModel
    {
        public int Id { get; set; }
        [Required]
        [ZooUniqueName]
        public string ZooName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        public List<string> AnimalFamilies { get; set; }
    }
}
