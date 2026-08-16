namespace DelightBistroMvc.Data.Models
{
    public class ChatMessageData : BaseModel
    {
        public string SenderName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }

        public int? UserId { get; set; }
        public virtual UserData User { get; set; }
    }
}
