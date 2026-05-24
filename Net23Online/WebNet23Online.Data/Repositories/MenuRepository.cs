using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;

namespace WebNet23Online.Data.Repositories
{
    public class MenuRepository : BaseRepository<MenuData>, IMenuRepository
    {
        public MenuRepository(WebContext webContext) : base(webContext) { }

        public List<MenuData> GetAllIncludeFoodItemsWithIngredients(string filterMenuName)
        {
            var allMenus = _dbSet
           .Include(x => x.Creator)
           .Include(x => x.FoodItems)
               .ThenInclude(f => f.Creator)
           .Include(x => x.FoodItems) // delete?
               .ThenInclude(x => x.IngredientsList) // delete?
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
