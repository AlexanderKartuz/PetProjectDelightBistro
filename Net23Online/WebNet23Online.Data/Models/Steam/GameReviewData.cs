
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNet23Online.Data.Models.Steam
{
    public class GameReviewData : BaseModel
    {
        [Required, MinLength(3), MaxLength(5000)]
        public string Text { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int AuthorId { get; set; }
        public int GameId { get; set; }

        public virtual UserData Author { get; set; }
        public virtual GameData Game { get; set; }
    }
}
