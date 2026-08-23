using Microsoft.EntityFrameworkCore;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces;

namespace DelightBistroMvc.Data.Repositories
{
    public abstract class BaseRepository<DataModel>
        : IBaseRepository<DataModel> where DataModel : BaseModel
    {
        protected WebContext _context;
        protected DbSet<DataModel> _dbSet;

        public BaseRepository(WebContext context)
        {
            _context = context;
            _dbSet = _context.Set<DataModel>();
        }

        public virtual Task AddAsync(DataModel model, CancellationToken cancellationToken = default)
        {
            return _dbSet.AddAsync(model, cancellationToken).AsTask();
            //_context.SaveChanges();
        }

        public virtual Task RemoveAsync(DataModel model, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(model);
            return Task.CompletedTask;
            //_context.SaveChanges();
        }

        public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                //_context.SaveChanges();
            }
        }

        public virtual async Task DeleteAsync(List<int> ids, CancellationToken cancellationToken = default)
        {
            var models = await _dbSet.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
            if (models.Count > 0)
            {
                _dbSet.RemoveRange(models);
                //_context.SaveChanges();
            }
        }

        public virtual Task DeleteRangeAsync(List<DataModel> models, CancellationToken cancellationToken = default)
        {
            _dbSet.RemoveRange(models);
            return Task.CompletedTask;
            //_context.SaveChanges();
        }

        public virtual Task<DataModel?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

        public virtual Task<List<DataModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _dbSet.ToListAsync(cancellationToken: cancellationToken);
        }

        public virtual Task<List<DataModel>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken = default)
        {
            return _dbSet.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        }
        public virtual Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return _dbSet.AnyAsync(cancellationToken: cancellationToken);
        }

        public virtual Task UpdateAsync(DataModel model, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(model);
            return Task.CompletedTask;
            //_context.SaveChanges();
        }

        public virtual Task UpdateAsync(List<DataModel> models, CancellationToken cancellationToken = default)
        {
            _dbSet.UpdateRange(models);
            return Task.CompletedTask;
            //_context.SaveChanges();
        }
    }
}
