namespace WebNet23Online.Models.SlayTheSpire2
{
    public class HeroCardViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Rarity { get; set; } = string.Empty;

        public int ManaCost { get; set; }

        public string TypeOfCard { get; set; } = string.Empty;

        public bool Upgraded { get; set; }

        public string? ImageUrl { get; set; }
    }
}
