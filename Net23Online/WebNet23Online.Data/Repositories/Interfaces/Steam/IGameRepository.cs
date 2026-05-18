using WebNet23Online.Data.HelperModels;
using WebNet23Online.Data.HelperModels.SteamPagination;
using WebNet23Online.Data.Models.Steam;

namespace WebNet23Online.Data.Repositories.Interfaces.Steam
{
    public interface IGameRepository : IBaseRepository<GameData>
    {
        List<GameData> GetFeaturedForHomePage();
        List<GameData> GetSpecialOffersForHomePage();
        GameData GetGameWithPublisherAndGenres(int id);
        bool IsTitleFree(string title, int excludeGameId = 0);
        PaginatedList<GameData> GetGames(GameFilter filter, int pageIndex, int pageSize);
    }
}
