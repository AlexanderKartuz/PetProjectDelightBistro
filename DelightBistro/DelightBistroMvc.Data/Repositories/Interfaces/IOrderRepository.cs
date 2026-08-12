using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderData>
    {
        List<OrderData> GetExpiredOrderDatas();
    }
}