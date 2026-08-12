using MazeCore.Cells.Shopkeeper.ShopItems.Interfaces;

namespace MazeCore.Cells.Shopkeeper.ShopItems.Services.Interfaces
{
    public interface IIBaseShopkeeperService : IBaseShopItem
    {
        int UnitPrice { get; }
    }
}