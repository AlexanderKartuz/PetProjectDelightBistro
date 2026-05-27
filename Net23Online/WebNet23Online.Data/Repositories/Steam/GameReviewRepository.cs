using Microsoft.EntityFrameworkCore;

using WebNet23Online.Data.Models.Steam;
using WebNet23Online.Data.Repositories.Interfaces.Steam;

namespace WebNet23Online.Data.Repositories.Steam
{
    public class GameReviewRepository : BaseRepository<GameReviewData>, IGameReviewRepository
    {
        public GameReviewRepository(WebContext context) : base(context)
        {
        }

        public bool ExistsForUser(int gameId, int authorId)
        {
            return _dbSet.Any(r => r.GameId == gameId && r.AuthorId == authorId);
        }

        public List<GameReviewData> GetByGameId(int gameId)
        {
            return _dbSet
                .Where(r => r.GameId == gameId)
                .ToList();
        }

        public GameReviewData? GetWithAuthor(int id)
        {
            return _dbSet
                .Include(r => r.Author)
                .FirstOrDefault(r => r.Id == id);
        }
    }
}
