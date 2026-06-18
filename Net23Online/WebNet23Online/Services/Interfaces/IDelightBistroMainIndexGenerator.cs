using WebNet23Online.Models.DelightBistro;

namespace WebNet23Online.Services.Interfaces
{
    public interface IDelightBistroMainIndexGenerator
    {
        MainIndexViewModel GetMainIndexViewModel(string menuType);
    }
}