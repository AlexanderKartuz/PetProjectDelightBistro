using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class SlayTheSpire2HeroesCardsRepository : BaseRepository<SlayTheSpire2HeroesCards>, ISlayTheSpire2HeroesCardsRepository
    {
        public SlayTheSpire2HeroesCardsRepository(WebContext webContext) : base(webContext) { }

        public List<SlayTheSpire2HeroesCards> GetByHeroId(int heroId) =>
            _dbSet.Where(c => c.HeroId == heroId).OrderBy(c => c.Name).ToList();
    }
}
