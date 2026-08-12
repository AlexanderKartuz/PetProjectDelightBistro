using Microsoft.EntityFrameworkCore;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro
{
    public interface IIngredientsRepository : IDelightBistroRepository<IngredientData>, IBaseRepository<IngredientData>
    {

    }
}