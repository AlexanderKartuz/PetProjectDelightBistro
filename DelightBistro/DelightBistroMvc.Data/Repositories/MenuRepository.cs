using Microsoft.EntityFrameworkCore;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

namespace DelightBistroMvc.Data.Repositories
{
    public class MenuRepository : BaseRepository<MenuData>, IMenuRepository
    {
        public MenuRepository(WebContext webContext) : base(webContext) { }

        public Task<List<MenuData>> GetAllIncludeFoodItemsWithIngredientsLinksAsync(string filterMenuName, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
            .AsNoTracking()
            .Include(x => x.Creator)
            .Include(x => x.FoodItems)
                .ThenInclude(f => f.Creator)
            .Include(x => x.FoodItems)
                .ThenInclude(x => x.FoodItemIngredientDatas)
                    .ThenInclude(x => x.IngredientData)
            .AsQueryable();

            if (!string.IsNullOrEmpty(filterMenuName))
            {
                query = query.Where(x => x.Name == filterMenuName);
            }

            return query.ToListAsync(cancellationToken);
        }

        public bool IsNameFree(string name)
        {
            return !_dbSet.Any(x => x.Name == name);
        }

    }
}
