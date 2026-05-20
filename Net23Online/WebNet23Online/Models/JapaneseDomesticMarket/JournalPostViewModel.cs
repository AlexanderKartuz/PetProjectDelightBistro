using WebNet23Online.Data.Models;

namespace WebNet23Online.Models.JapaneseDomesticMarket
{
    public class JournalPostViewModel
    {
        public int PostId { get; set; }
        public List<JournalCommentsViewModel> Comments { get; set; } = new();
        public AddJournalCommentViewModel Form { get; set; } = new();
    }
}