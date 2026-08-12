using DelightBistroMvc.Models.DelightBistro;

namespace DelightBistroMvc.Services.Interfaces
{
    public interface IDelightBistroMainIndexGenerator
    {
        MainIndexViewModel GetMainIndexViewModel(string menuType);
    }
}