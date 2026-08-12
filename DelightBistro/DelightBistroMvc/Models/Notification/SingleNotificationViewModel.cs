namespace DelightBistroMvc.Models.Notification
{
    public class SingleNotificationViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeToPublish { get; set; }
        public string AuthorName { get; set; }
    }
}
