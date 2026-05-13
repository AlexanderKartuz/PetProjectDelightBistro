using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces
{
    public interface IAnimeRepository : IBaseRepository<AnimeData>
    {
        List<GirlNameAndAnimeNameDataModel> GetPopularGirlAndAnime();
    }
}