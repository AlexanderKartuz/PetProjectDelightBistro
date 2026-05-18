using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
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

        public virtual void Add(DataModel model)
        {
            _dbSet.Add(model);
            _context.SaveChanges();
        }

        public virtual void Remove(DataModel model)
        {
            _dbSet.Remove(model);
            _context.SaveChanges();
        }

        public virtual DataModel? Get(int id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public virtual bool Any()
        {
            return _dbSet.Any();
        }

        public virtual List<DataModel> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual void Update(DataModel model)
        {
            _dbSet.Update(model);
            _context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var user = _dbSet.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                _dbSet.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
