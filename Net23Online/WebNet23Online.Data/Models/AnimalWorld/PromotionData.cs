namespace WebNet23Online.Data.Models.AnimalWorld
{
    public class PromotionData : BaseModel
    {
        public string PromotionName { get; set; }

        public string Description { get; set; }

        public DateTime EndDate { get; set; }

        public int ZooId { get; set; }

        public int UserId { get; set; }

        public virtual ZooData Venue { get; set; }

        public virtual UserData Creator { get; set; }
    }
}
