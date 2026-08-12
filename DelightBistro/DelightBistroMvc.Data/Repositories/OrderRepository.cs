using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces;

namespace DelightBistroMvc.Data.Repositories
{
    public class OrderRepository : BaseRepository<OrderData>, IOrderRepository
    {
        private const int EXPIRED_MONTHS = -1;
        public OrderRepository(WebContext context) : base(context) { }

        public List<OrderData> GetExpiredOrderDatas()
        {
            var oneMonthAgo = DateTime.UtcNow.AddMonths(EXPIRED_MONTHS);

            var expiredOrders = _dbSet
                .Where(x => x.CreatedDateTime < oneMonthAgo)
                .ToList();

            return expiredOrders;
        }
    }
}
