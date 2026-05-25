using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models.AnimalWorld;

namespace WebNet23Online.Data.Models
{
    public class TicketData : BaseModel
    {
        public string UniqueKey { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public DateTime EventDate { get; set; }
        public EntityType TicketType { get; set; }
        public bool IsUsed { get; set; } = false;
        public int UserId { get; set; }
        public int? ZooId { get; set; }
        public virtual UserData User { get; set; }
        public virtual ZooData Zoo { get; set; }
    }
}
