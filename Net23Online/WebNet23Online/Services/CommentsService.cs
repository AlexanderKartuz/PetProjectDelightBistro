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
            comments.EntityId = zooId;
            comments.DisplayName = zoo.ZooName;
            return comments;
        }

        public OneCommentViewModel AddZooComment(int zooId, string text)
        {
            var user = _authService.GetUser();
            var authorName = GetAuthorDisplayName(user);
            var createdAt = DateTime.UtcNow;
            var comment = new CommentData
            {
                AuthorId = user.Id,
                AuthorName = authorName,
                CommentType = EntityType.Zoo,
                Text = text,
                CreatedAt = createdAt,
                ZooId = zooId,
            };
            _commentsRepository.Add(comment);

            return new OneCommentViewModel
            {
                Author = authorName,
                Text = text,
                CreatedAt = createdAt,
            };
        }

        private string GetAuthorDisplayName(UserData user)
        {
            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
            {
                return user.Name;
            }

            return $"{user.LastName} {user.FirstName}";
        }
    }
}
