namespace WebNet23Online.Models.SlayTheSpire2
{
    public class HeroesViewModel
    {
        public int HeroId { get; set; }

        public bool Found { get; set; }

        public string? Name { get; set; }

        public string? Color { get; set; }

        public List<HeroCardViewModel> Cards { get; set; } = new();
    }
}
