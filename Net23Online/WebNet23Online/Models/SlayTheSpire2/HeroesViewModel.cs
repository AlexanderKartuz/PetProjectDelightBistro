using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebNet23Online.Models.SlayTheSpire2
{
    public class HeroesViewModel
    {
        public int HeroId { get; set; }

        public bool Found { get; set; }

        public string? Name { get; set; }

        public string? Color { get; set; }

        public List<HeroCardViewModel> Cards { get; set; } = new();

        public AddHeroCardFormViewModel AddCardForm { get; set; } = new();

        public List<SelectListItem> HeroOptions { get; set; } = new();

        public List<SelectListItem> RarityOptions { get; set; } = new();

        public List<SelectListItem> TypeOfCardOptions { get; set; } = new();
    }
}
