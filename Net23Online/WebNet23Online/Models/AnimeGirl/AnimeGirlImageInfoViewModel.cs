namespace WebNet23Online.Models.AnimeGirl
{
    public class AnimeGirlImageInfoViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string ConnectedAnimeTitles { get; set; } = string.Empty;
        public int Likes { get; set; }
    }
}
