using Microsoft.EntityFrameworkCore;
using DelightBistroMvc.Data.Migrations;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro
{
    public interface IMenuRepository : IDelightBistroRepository<MenuData>, IBaseRepository<MenuData>
    {
        List<MenuData> GetAllIncludeFoodItemsWithIngredients(string filterMenuName);

    }
}