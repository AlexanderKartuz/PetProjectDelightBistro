using DelightBistroMvc.Data.Repositories.Interfaces;

namespace DelightBistroMvc.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebContext _context;

        public UnitOfWork(WebContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
