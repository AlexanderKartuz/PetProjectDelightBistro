namespace WebNet23Online.Data.Models
{
    public class RockBandsData : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int? CreatedByUserId { get; set; }
        public int Likes { get; set; }
        public virtual UserData? CreatedByUser { get; set; }
        public virtual ICollection<RockBandGenreData> RockBandGenres { get; set; } = new List<RockBandGenreData>();
        public virtual ICollection<RockBandLikeData> RockBandLikes { get; set; } = new List<RockBandLikeData>();
    }
}
