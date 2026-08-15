using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderData>
    {
        void DeleteExpiredOrderDatas();
    }
}