using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces
{
    public interface IBaseRepository<DataModel>
        where DataModel : BaseModel
    {
        Task AddAsync(DataModel model, CancellationToken cancellationToken = default);
        Task<List<DataModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task RemoveAsync(DataModel model, CancellationToken cancellationToken = default);
        Task<DataModel?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task UpdateAsync(DataModel model, CancellationToken cancellationToken = default);
        Task UpdateAsync(List<DataModel> models, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(List<DataModel> models, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(List<int> ids, CancellationToken cancellationToken = default);
        Task<List<DataModel>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken = default);
    }
}