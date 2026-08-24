using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using Microsoft.EntityFrameworkCore;

namespace DelightBistroMvc.Data.Repositories
{
    public class OrderRepository : BaseRepository<OrderData>, IOrderRepository
    {
        private const int EXPIRED_MONTHS = -1;
        public OrderRepository(WebContext context) : base(context) { }

        public Task<int> DeleteExpiredOrderDatasAsync(CancellationToken cancellationToken = default)
        {
            var oneMonthAgo = DateTime.UtcNow.AddMonths(EXPIRED_MONTHS);

            return _dbSet
                .Where(x => x.CreatedDateTime < oneMonthAgo)
                .ExecuteDeleteAsync(cancellationToken: cancellationToken);
        }
    }
}
