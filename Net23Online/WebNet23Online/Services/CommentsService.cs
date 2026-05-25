using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;
using WebNet23Online.Models.Comments;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class CommentsService : ICommentsService
    {
        private IAuthService _authService;
        private ICommentsRepository _commentsRepository;
        private ICommentsMapper _commentsMapper;
        private IZooRepository _zooRepository;

        public CommentsService(IAuthService authService, ICommentsRepository commentsRepository, ICommentsMapper commentsMapper, IZooRepository zooRepository)
        {
            _authService = authService;
            _commentsRepository = commentsRepository;
            _commentsMapper = commentsMapper;
            _zooRepository = zooRepository;
        }

        public AllCommentsViewModel GetZooComments(int zooId)
        {
            var zoo = _zooRepository.Get(zooId);
            var comments = _commentsMapper.FromCommentsDataToCommnetsViewModel(_commentsRepository.GetZooComments(zooId));
            foreach (var comment in comments.Comments)
            {
                comment.AuthorDisplayName = GetAuthorDisplayName(comment.AuthorName, comment.AuthorFirstName, comment.AuthorLastName);
            }
            comments.EntityId = zooId;
            comments.DisplayName = zoo.ZooName;
            return comments;
        }

        public OneCommentViewModel AddZooComment(int zooId, string text)
        {
            var user = _authService.GetUser();
            var authorName = GetAuthorDisplayName(user.Name, user.FirstName, user.LastName);
            var createdAt = DateTime.UtcNow;
            var comment = new CommentData
            {
                AuthorId = user.Id,
                CommentType = EntityType.Zoo,
                Text = text,
                CreatedAt = createdAt,
                ZooId = zooId,
                Author = user,
                Zoo = _zooRepository.Get(zooId),
            };
            _commentsRepository.Add(comment);

            return new OneCommentViewModel
            {
                AuthorDisplayName = authorName,
                Text = text,
                CreatedAt = createdAt,
            };
        }

        private string GetAuthorDisplayName(string name, string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return name;
            }

            return $"{lastName} {firstName}";
        }
    }
}
