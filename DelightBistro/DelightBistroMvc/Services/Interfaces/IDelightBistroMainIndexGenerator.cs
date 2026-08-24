using DelightBistroMvc.Models.DelightBistro;

namespace DelightBistroMvc.Services.Interfaces
{
    public interface IDelightBistroMainIndexGenerator
    {
        Task<MainIndexViewModel> GetMainIndexViewModelAsync(string menuType, CancellationToken cancellationToken = default);
    }
}