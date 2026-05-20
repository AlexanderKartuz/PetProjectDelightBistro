using WebNet23Online.Data.Models;

namespace WebNet23Online.Models.JapaneseDomesticMarket
{
    public class JournalCommentsViewModel
    {
        public int PostsId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AuthorName { get; set; } = "";
    }
}