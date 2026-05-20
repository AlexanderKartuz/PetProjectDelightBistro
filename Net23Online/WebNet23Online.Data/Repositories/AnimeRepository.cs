using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class AnimeRepository : BaseRepository<AnimeData>, IAnimeRepository
    {
        public AnimeRepository(WebContext context) : base(context)
        {
        }

        public List<GirlNameAndAnimeNameDataModel> GetPopularGirlAndAnime()
        {
            var sql = @"SELECT AG.[Name] GirlName, A.[Name] AnimeName
FROM (
	SELECT AGD.HeroesId GirlId
	FROM AnimeDataAnimeGirlData AGD
	WHERE 1=1
	GROUP BY AGD.HeroesId
	HAVING 1=1
		AND COUNT(AGD.AnimesId) > 1
) PG 
	LEFT JOIN AnimeGirls AG ON AG.Id = PG.GirlId
	LEFT JOIN AnimeDataAnimeGirlData AGD ON AGD.HeroesId = AG.Id
	LEFT JOIN Animes A ON AGD.AnimesId = A.Id";

            var results = _context
                .Database
                .SqlQueryRaw<GirlNameAndAnimeNameDataModel>(sql)
                .ToList();

            return results;

        }
    }
}
