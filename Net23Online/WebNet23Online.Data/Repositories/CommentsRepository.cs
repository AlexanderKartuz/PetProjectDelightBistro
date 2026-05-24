using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class CommentsRepository : BaseRepository<CommentData>, ICommentsRepository
    {
        public CommentsRepository(WebContext context) : base(context)
        {
        }

        public List<CommentData> GetZooComments(int zooId)
        {
            return _dbSet.Where(x => x.ZooId == zooId && x.CommentType == EntityType.Zoo).ToList();
        }
    }
}
