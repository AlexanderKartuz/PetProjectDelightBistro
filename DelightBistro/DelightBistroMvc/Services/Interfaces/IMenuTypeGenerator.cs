using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Models.DelightBistro;

namespace DelightBistroMvc.Services.Interfaces
{
    public interface IMenuTypeGenerator
    {
        Task CreateMenuDataAsync(CreateMenuViewModel viewModel, CancellationToken cancellationToken = default);
        Task<List<MenuTypeViewModel>> GetAllMenuViewModelAsync(string sortMenuName = "", CancellationToken cancellationToken = default);
    }
}