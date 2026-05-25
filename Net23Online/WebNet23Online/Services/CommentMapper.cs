using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Models.Comments;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class CommentMapper : ICommentsMapper
    {
        public AllCommentsViewModel FromCommentsDataToCommnetsViewModel(List<CommentData> comments)
        {
            return new AllCommentsViewModel
            {
                Comments = comments.Select(comment => new OneCommentViewModel
                {
                    AuthorName = comment.Author.Name,
                    AuthorFirstName = comment.Author.FirstName,
                    AuthorLastName = comment.Author.LastName,
                    CreatedAt = comment.CreatedAt,
                    Text = comment.Text,
                }).ToList(),
            };
        }
    }
}
