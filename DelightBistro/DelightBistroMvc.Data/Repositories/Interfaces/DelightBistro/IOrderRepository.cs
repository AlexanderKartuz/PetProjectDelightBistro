using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro
{
    public interface IOrderRepository : IBaseRepository<OrderData>
    {
        void DeleteExpiredOrderDatas();
    }
}