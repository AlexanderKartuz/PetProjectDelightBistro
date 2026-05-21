using WebNet23Online.Data.Models;
using WebNet23Online.Models.Comments;

namespace WebNet23Online.Services
{
    public class CommentMapper
    {
        public AllCommentsViewModel FromCommentsDataToCommnetsViewModel(List<CommentData> comments)
        {
            return new AllCommentsViewModel
            {
                Comments = comments.Select(comment => new OneCommentViewModel
                {
                    Author = comment.AuthorName,
                    CreatedAt = comment.CreatedAt,
                    Text = comment.Text,
                }).ToList(),
            };
        }
    }
}
