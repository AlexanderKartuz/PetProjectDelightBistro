using System.ComponentModel.DataAnnotations;

namespace WebNet23Online.Models.JapaneseDomesticMarket
{
    public class AddJournalCommentViewModel
    {
        public int PostId { get; set; }
        public string Text { get; set; } = "";
    }
}