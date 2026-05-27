namespace WebNet23Online.Models.Steam
{
    public class AddGameReviewRequest
    {
        public int GameId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }
}