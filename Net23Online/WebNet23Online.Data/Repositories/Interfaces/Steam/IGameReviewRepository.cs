
using WebNet23Online.Data.Models.Steam;

namespace WebNet23Online.Data.Repositories.Interfaces.Steam
{
    public interface IGameReviewRepository : IBaseRepository<GameReviewData>
    {
        bool ExistsForUser(int gameId, int authorId);
        List<GameReviewData> GetByGameId(int gameId);
        GameReviewData? GetWithAuthor(int id);
    }
}
