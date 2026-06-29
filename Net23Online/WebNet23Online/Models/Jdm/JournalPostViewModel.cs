using WebNet23Online.Data.Models;

namespace WebNet23Online.Models.Jdm
{
    public class JournalPostViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string UrlPicture { get; set; }
        public DateTime? PublishedDate { get; set; }
        public List<JournalCommentsViewModel> Comments { get; set; } = new();
        public AddJournalCommentViewModel Form { get; set; } = new();
    }
}