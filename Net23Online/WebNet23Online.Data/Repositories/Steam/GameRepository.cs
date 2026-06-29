using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using WebNet23Online.Data.HelperModels;
using WebNet23Online.Data.HelperModels.SteamPagination;
using WebNet23Online.Data.Models.Steam;
using WebNet23Online.Data.Repositories.Interfaces.Steam;

namespace WebNet23Online.Data.Repositories.Steam
{
    public class GameRepository : BaseRepository<GameData>, IGameRepository
    {
        public const int SPECIAL_OFFERS_PREVIEW_COUNT = 6;

        public GameRepository(WebContext context) : base(context)
        {
        }

        public List<GameData> GetFeaturedForHomePage()
        {
            var featured = _dbSet
                .Include(g => g.GameGenres)
                .Skip(SPECIAL_OFFERS_PREVIEW_COUNT).ToList();

            return featured;
        }

        public List<GameData> GetSpecialOffersForHomePage()
        {
            var specialOffers = _dbSet
                .Include(g => g.GameGenres)
                .Take(SPECIAL_OFFERS_PREVIEW_COUNT).ToList();

            return specialOffers;
        }

        public GameData GetGameDetails(int id)
        {
            var gameData = _dbSet
                .Include(g => g.Publisher)
                .Include(g => g.GameGenres)
                .Include(g => g.GameReviews)
                    .ThenInclude(r => r.Author)
                .FirstOrDefault(g => g.Id == id);
            return gameData;
        }

        public GameData GetByTitle(string title)
        {
            return _dbSet.FirstOrDefault(g => g.Title == title);
        }

        public bool IsTitleFree(string title, int excludeGameId = 0)
        {
            return !_dbSet.Any(x => x.Title == title && x.Id != excludeGameId);
        }

        public List<GameData> GetAllWithReviews()
        {
            return _dbSet
                .Include(g => g.GameReviews)
                .ToList();
        }

        public PaginatedList<GameData> GetGames(GameFilter filter, int pageIndex, int pageSize)
        {
            var games = _dbSet
               .Include(g => g.GameGenres)
               .AsQueryable();

            if (filter.GenreId.HasValue)
            {
                games = games.Where(g => g.GameGenres.Any(gg => gg.Id == filter.GenreId.Value));
            }

            if (filter.MaxPrice.HasValue)
            {
                games = games.Where(g => g.Price <= filter.MaxPrice.Value);
            }

            var count = games.Count();
            var totalPages = count == 0 ? 1 : (int)Math.Ceiling(count / (double)pageSize);
            var safePageIndex = Math.Min(Math.Max(1, pageIndex), totalPages);

            var sortedGames = ApplySorting(games, filter.SortBy, filter.SortDirection);

            var pageItems = sortedGames
                .Skip((safePageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedList<GameData>(pageItems, safePageIndex, totalPages, count);
        }

        public IQueryable<T> ApplySorting<T>(
            IQueryable<T> query,
            string? sortBy,
            string? sortDirection)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return query;
            }

            var propertyInfo = typeof(T).GetProperty(sortBy,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                return query;
            }

            var parameter = Expression.Parameter(typeof(T), "entity");
            var property = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase)
                ? nameof(Queryable.OrderByDescending)
                : nameof(Queryable.OrderBy);

            var orderByMethod = typeof(Queryable)
                .GetMethods()
                .First(method => method.Name == methodName
                    && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), propertyInfo.PropertyType);

            return (IQueryable<T>)orderByMethod.Invoke(null, [query, lambda])!;
        }
    }
}
