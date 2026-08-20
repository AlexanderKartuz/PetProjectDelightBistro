using DelightBistroMinimalApi.DbStuff.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DelightBistroMinimalApi.DbStuff.Repositories
{
    public class SeriLogRepository : ISeriLogRepository
    {
        private readonly MiniDbContext _context;

        public SeriLogRepository(MiniDbContext context)
        {
            _context = context;
        }

        public async Task<int> CleanupAsync(CancellationToken ct)
        {
            var deleteTime = DateTime.UtcNow.AddDays(-5);

            var query = _context.LogEntries
                .Where(e => e.TimeStamp != null && e.TimeStamp < deleteTime);

            var oldLogEntries = await query.ExecuteDeleteAsync(ct);

            if (!(oldLogEntries > 0))
            {
                return 0;
            }

            return oldLogEntries;
        }
    }
}
