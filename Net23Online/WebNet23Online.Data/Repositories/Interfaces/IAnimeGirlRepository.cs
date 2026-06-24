using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces
{
    public interface IAnimeGirlRepository : IBaseRepository<AnimeGirlData>
    {
        bool IsNameFree(string name);
        void Link(int animeId, int heroId);
        List<AnimeGirlData> GetAllIncludeAnime();
        List<AnimeGirlData> GetAllIncludeAnime(string? sortBy);
    }
}