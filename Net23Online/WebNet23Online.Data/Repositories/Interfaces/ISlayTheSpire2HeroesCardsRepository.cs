using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces
{
    public interface ISlayTheSpire2HeroesCardsRepository : IBaseRepository<SlayTheSpire2HeroesCards>
    {
        List<SlayTheSpire2HeroesCards> GetByHeroId(int heroId);
    }
}
