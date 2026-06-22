using WebNet23Online.Data.Models.AnimalWorld;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;

namespace WebNet23Online.Data.Repositories.AnimalWorld
{
    public class PromotionRepository : BaseRepository<PromotionData>, IPromotionRepository
    {
        public PromotionRepository(WebContext context) : base(context)
        {
        }
    }
}
