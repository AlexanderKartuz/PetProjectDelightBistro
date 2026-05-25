using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class OrderRepository : BaseRepository<OrderData>, IOrderRepository
    {
        public OrderRepository(WebContext context) : base(context) { }
    }
}
