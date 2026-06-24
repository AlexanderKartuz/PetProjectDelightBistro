using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class AnimeGirlRepository : BaseRepository<AnimeGirlData>, IAnimeGirlRepository
    {
        public AnimeGirlRepository(WebContext webContext) : base(webContext) { }

        public List<AnimeGirlData> GetAllIncludeAnime()
        {
            return _dbSet
                .Include(g => g.Animes)
                .ToList();
        }

        public List<AnimeGirlData> GetAllIncludeAnime(string? sortBy)
        {
            var dataSource = _dbSet
                .Include(g => g.Animes)
                .AsQueryable();

            if (sortBy == "Id")
            {
                dataSource = dataSource.OrderBy(x => x.Id);
            }
            else if (sortBy == "Title")
            {
                dataSource = dataSource.OrderBy(x => x.Name);
            }
            else if (sortBy == "ConnectedAnimeTitles")
            {
                dataSource = dataSource.OrderBy(x => x.Animes.Count);
            }
            else if (sortBy == "Url")
            {
                dataSource = dataSource.OrderBy(x => x.Url);
            }

            return dataSource.ToList();
        }

        public override void Add(AnimeGirlData model)
        {
            if (model.Name == model.Description)
            {
                throw new Exception("Be more creative");
            }

            base.Add(model);
        }

        public bool IsNameFree(string name)
        {
            return !_dbSet.Any(x => x.Name == name);
        }

        public void Link(int animeId, int heroId)
        {
            var anime = _context.Animes.First(x => x.Id == animeId);
            var hero = _context.AnimeGirls.First(x => x.Id == heroId);
            anime.Heroes.Add(hero);
            _context.SaveChanges();
        }
    }
}
