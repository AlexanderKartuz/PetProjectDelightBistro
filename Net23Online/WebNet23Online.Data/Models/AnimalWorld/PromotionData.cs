namespace WebNet23Online.Data.Models.AnimalWorld
{
    public class PromotionData : BaseModel
    {
        public string PromotionName { get; set; }

        public string Description { get; set; }

        public DateTime EndDate { get; set; }

        public int VenueId { get; set; }

        public int CreatorId { get; set; }

        public virtual ZooData Venue { get; set; }

        public virtual UserData Creator { get; set; }
    }
}
