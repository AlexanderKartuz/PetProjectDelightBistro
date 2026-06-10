using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class RockBandLikeRepository : BaseRepository<RockBandLikeData>, IRockBandLikeRepository
    {
        private readonly WebContext _context;
        private readonly DbSet<RockBandLikeData> _likes;
        private readonly DbSet<RockBandsData> _bands;

        public RockBandLikeRepository(WebContext context) : base(context)
        {
            _context = context;
            _likes = context.Set<RockBandLikeData>();
            _bands = context.RockBand;
        }

        public bool TryAddLike(int userId, int rockBandId)
        {
            if (userId <= 0 || rockBandId <= 0)
            {
                return false;
            }

            if (!_bands.Any(x => x.Id == rockBandId))
            {
                return false;
            }

            if (HasLike(userId, rockBandId))
            {
                return false;
            }

            _likes.Add(new RockBandLikeData
            {
                UserId = userId,
                RockBandId = rockBandId,
            });
            _context.SaveChanges();
            return true;
        }

        public bool HasLike(int userId, int rockBandId)
        {
            return _likes.Any(x => x.UserId == userId && x.RockBandId == rockBandId);
        }

        public HashSet<int> GetLikedRockBandIds(int userId, IReadOnlyCollection<int> rockBandIds)
        {
            if (userId <= 0 || rockBandIds == null || rockBandIds.Count == 0)
            {
                return new HashSet<int>();
            }

            return _likes
                .Where(x => x.UserId == userId && rockBandIds.Contains(x.RockBandId))
                .Select(x => x.RockBandId)
                .ToHashSet();
        }
    }
}
