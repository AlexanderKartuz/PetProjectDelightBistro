using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebNet23Online.Models.AnimalWorld
{
    public class PromotionViewModel
    {
        public string PromotionName { get; set; }

        public string Description { get; set; }

        public int ZooId { get; set; }

        public string Place {  get; set; }

        public List<SelectListItem>? Zoos { get; set; }

        public DateTime EndDate { get; set; }
    }
}
