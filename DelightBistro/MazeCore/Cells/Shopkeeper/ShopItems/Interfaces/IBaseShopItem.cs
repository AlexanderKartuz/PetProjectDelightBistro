using MazeCore.Cells.Shopkeeper.ShopMenuSystem;
using MazeCore.Characters.Interfaces;

namespace MazeCore.Cells.Shopkeeper.ShopItems.Interfaces
{
    public interface IBaseShopItem
    {
        string Name { get; set; }
        ShopMenu MenuForShop { get; }

        void Execute(IBaseCharacter character);
    }
}