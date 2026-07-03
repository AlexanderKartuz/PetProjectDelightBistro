namespace WebNet23Online.Data.Models
{
    public class AnimeGirlData : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int Likes { get; set; }

        public virtual List<AnimeData> Animes { get; set; } = new();
    }
}
