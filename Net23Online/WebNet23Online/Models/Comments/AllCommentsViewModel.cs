using WebNet23Online.Data.Enums;

namespace WebNet23Online.Models.Comments
{
    public class AllCommentsViewModel
    {
        public EntityType CommentsType { get; set; }
        public int EntityId { get; set; }
        public string DisplayName { get; set; }
        public List<OneCommentViewModel> Comments { get; set; }
    }
}
