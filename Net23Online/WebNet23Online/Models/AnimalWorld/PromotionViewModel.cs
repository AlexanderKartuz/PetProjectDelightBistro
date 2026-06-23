using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebNet23Online.Models.AnimalWorld
{
    public class PromotionViewModel
    {
        [Required]
        public string PromotionName { get; set; }

        [Required]
        public string Description { get; set; }

        public int ZooId { get; set; }

        public string? Place {  get; set; }

        public List<SelectListItem>? Zoos { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
