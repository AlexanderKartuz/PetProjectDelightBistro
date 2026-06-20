using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
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
