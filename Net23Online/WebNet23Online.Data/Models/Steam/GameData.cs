using Microsoft.EntityFrameworkCore;

namespace WebNet23Online.Data.Models.Steam
{
    public class GameData : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int? PublisherId { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? ModifiedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public double? AverageRating { get; set; }
        public int? ReviewsCount { get; set; }
        public int? PositiveReviewsCount { get; set; }

        public virtual UserData CreatedByUser { get; set; }
        public virtual UserData ModifiedByUser { get; set; }
        public virtual PublisherData Publisher { get; set; }
        public virtual List<GameGenreData> GameGenres { get; set; }
        public virtual List<GameReviewData> GameReviews { get; set; }
    }
}
