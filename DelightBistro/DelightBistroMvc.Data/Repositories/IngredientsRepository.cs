using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

namespace DelightBistroMvc.Data.Repositories
{
    public class IngredientsRepository : BaseRepository<IngredientData>, IIngredientsRepository
    {
        public IngredientsRepository(WebContext webContex) : base(webContex) { }
        public bool IsNameFree(string name)
        {
            return !_dbSet.Any(x => x.Name == name);
        }
    }
}
