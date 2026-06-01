namespace WebNet23Online.Models.Steam
{
    public class ChatMessageViewModel
    {
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserName { get; set; }
        public bool IsOwnMessage {  get; set; }
        public int UserId {  get; set; }
    }
}
