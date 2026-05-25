namespace WebNet23Online.Models.Comments
{
    public class OneCommentViewModel
    {
        public string AuthorName { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorDisplayName { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
