using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Models.DelightBistro;

namespace DelightBistroMvc.Services.Interfaces
{
    public interface IMenuTypeGenerator
    {
        void CreateMenuData(CreateMenuViewModel viewModel);
        void FeelDataBase();
        List<MenuTypeViewModel> GetAllMenuViewModel(string sortMenuName="");
    }
}