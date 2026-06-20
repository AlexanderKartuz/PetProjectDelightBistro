using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderData>
    {
        List<OrderData> GetExpiredOrderDatas();
    }
}