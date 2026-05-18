using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class RockLegendsGenresRepository : BaseRepository<RockLegendsGenres>, IRockLegendsGenresRepository
    {
        public RockLegendsGenresRepository(WebContext webContext) : base(webContext) { }

        public List<RockLegendsGenres> GetAllWithGroups()
        {
            return _dbSet.Include(x => x.Groups).ToList();
        }

        public bool IsNameFree(string name)
        {
            return !_dbSet.Any(x => x.Name == name);
        }
        public List<RockLegendsGenreStatsDataModel> GetGenreStatsSql()
        {
            var sql = @"
                SELECT 
                    g.Id AS Id, 
                    g.Name AS Name, 
                    COUNT(b.Id) AS BandsCount
                FROM 
                    RockLegendsGenres g
                LEFT JOIN 
                    RockLegends b ON g.Id = b.RockLegendsGenresId
                GROUP BY 
                    g.Id, g.Name";

            var results = _context
                .Database
                .SqlQueryRaw<RockLegendsGenreStatsDataModel>(sql)
                .ToList();

            return results;
        }
    }
}