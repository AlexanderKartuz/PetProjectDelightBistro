using DelightBistroMinimalApi.DbStuff.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DelightBistroMinimalApi.DbStuff.Repositories
{
    public class SeriLogRepository : ISeriLogRepository
    {
        private readonly MiniDbContext _context;
        private const int DAYS_TO_DELETE = -5;

        public SeriLogRepository(MiniDbContext context)
        {
            _context = context;
        }

        public async Task<int> CleanupAsync(CancellationToken ct)
        {
            var deleteTime = DateTime.UtcNow.AddDays(DAYS_TO_DELETE);

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
