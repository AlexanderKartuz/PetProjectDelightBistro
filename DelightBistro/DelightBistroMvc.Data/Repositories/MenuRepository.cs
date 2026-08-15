using Microsoft.EntityFrameworkCore;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

namespace DelightBistroMvc.Data.Repositories
{
    public class MenuRepository : BaseRepository<MenuData>, IMenuRepository
    {
        public MenuRepository(WebContext webContext) : base(webContext) { }

        public List<MenuData> GetAllIncludeFoodItemsWithIngredientsLinks(string filterMenuName)
        {
            var allMenus = _dbSet
            .AsNoTracking()
            .Include(x => x.Creator)
            .Include(x => x.FoodItems)
                .ThenInclude(f => f.Creator)
            .Include(x => x.FoodItems)
                .ThenInclude(x => x.FoodItemIngredientDatas)
                    .ThenInclude(x => x.IngredientData);
            
            if (!string.IsNullOrEmpty(filterMenuName))
            {
                var filterMenu = allMenus.Where(x => x.Name == filterMenuName).ToList();
                return filterMenu;
            }

            return allMenus.ToList();
        }

        public bool IsNameFree(string name)
        {
            return !_dbSet.Any(x => x.Name == name);
        }

    }
}
