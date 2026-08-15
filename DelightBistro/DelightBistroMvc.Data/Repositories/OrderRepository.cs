using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DelightBistroMvc.Data.Repositories
{
    public class OrderRepository : BaseRepository<OrderData>, IOrderRepository
    {
        private const int EXPIRED_MONTHS = -1;
        public OrderRepository(WebContext context) : base(context) { }

        public void DeleteExpiredOrderDatas()
        {
            var oneMonthAgo = DateTime.UtcNow.AddMonths(EXPIRED_MONTHS);

            var expiredOrders = _dbSet
                .Where(x => x.CreatedDateTime < oneMonthAgo)
                .ExecuteDelete();
        }
    }
}
