using System.ComponentModel.DataAnnotations;

namespace WebNet23Online.Models.Steam
{
    public class GameReviewViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public bool IsRecommended { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public int GameId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
