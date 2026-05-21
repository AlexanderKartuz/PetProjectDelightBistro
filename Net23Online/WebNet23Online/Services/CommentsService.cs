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
            var comments = _commentsMapper.FromCommentsDataToCommnetsViewModel(_commentsRepository.GetZooComments(zooId));
            comments.DisplayName = _zooRepository.Get(zooId).ZooName;
            return comments;
        }

        public bool AddZooComment(int zooId, string text)
        {
            var user = _authService.GetUser();
            var comment = new CommentData
            {
                Author = user,
                AuthorName = user.FirstName,
                CommentType = EntityType.Zoo,
                Text = text,
                CreatedAt = DateTime.UtcNow,
                Zoo = _zooRepository.Get(zooId)
            };
            _commentsRepository.Add(comment);
            return true;
        }
    }
}
