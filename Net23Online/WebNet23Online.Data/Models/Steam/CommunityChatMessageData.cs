
namespace WebNet23Online.Data.Models.Steam
{
    public class CommunityChatMessageData : BaseModel
    {
        public string MessageText { get; set; }
        public DateTime CreatedAt {  get; set; }
        public int UserId { get; set; }

        public virtual UserData CreatedByUser { get; set; }
    }
}
