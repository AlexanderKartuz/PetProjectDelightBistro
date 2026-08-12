using MazeCore.Cells.Shopkeeper.ShopItems.Interfaces;
using MazeCore.Cells.Shopkeeper.ShopMenuSystem;
using MazeCore.Characters.Interfaces;

namespace MazeCore.Cells.Shopkeeper.ShopItems
{
    public abstract class BaseShopItem : IBaseShopItem
    {
        public ShopMenu MenuForShop { get; internal set; }
        public string Name { get; set; }
        public virtual string PriceDisplay => "";
        public abstract void Execute(IBaseCharacter character);
    }
}
