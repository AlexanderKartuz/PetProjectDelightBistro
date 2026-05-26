namespace WebNet23Online.Models.Steam.ApiResponses
{
    public class AddGameReviewApiResponse : BaseApiResponse
    {
        public string Author { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
