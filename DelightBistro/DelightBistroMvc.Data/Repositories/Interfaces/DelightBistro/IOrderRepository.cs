using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro
{
    public interface IOrderRepository : IBaseRepository<OrderData>
    {
        Task<int> DeleteExpiredOrderDatasAsync(CancellationToken cancellationToken = default);
    }
}