namespace WebNet23Online.Data.Models
{
    public class RockBandLikeData : BaseModel
    {
        public int UserId { get; set; }
        public int RockBandId { get; set; }

        public virtual UserData User { get; set; } = null!;
        public virtual RockBandsData RockBand { get; set; } = null!;
    }
}
