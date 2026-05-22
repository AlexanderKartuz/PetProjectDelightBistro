using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Models.Comments;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class CommentMapper : ICommentsMapper
    {
        public AllCommentsViewModel FromCommentsDataToCommnetsViewModel(List<CommentData> comments, int zooId)
        {
            return new AllCommentsViewModel
            {
                EntityId = zooId,
                CommentsType = EntityType.Zoo,
                Comments = comments
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(comment => new OneCommentViewModel
                    {
                        Author = comment.AuthorName,
                        CreatedAt = comment.CreatedAt,
                        Text = comment.Text,
                    })
                    .ToList(),
            };
        }
    }
}
