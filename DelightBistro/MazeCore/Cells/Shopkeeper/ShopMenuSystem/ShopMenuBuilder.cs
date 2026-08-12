using MazeCore.Cells.Shopkeeper.ShopItems;

namespace MazeCore.Cells.Shopkeeper.ShopMenuSystem
{
    public class ShopMenuBuilder
    {
        private ShopMenu _shopMenu;
        public ShopMenu Build(List<BaseShopItem> goodsAndServices, char cursor = '<')
        {
            var menuItems = new List<BaseShopItem>();
            foreach (BaseShopItem shopItem in goodsAndServices)
            {
                menuItems.Add(shopItem);
            }

            _shopMenu = new ShopMenu
            {
                MenuItems = menuItems,
                _cursor = cursor,
                ShopHistory = new List<string> {"A kind shopkeeper shows you his goods."}
            };

            foreach (var item in _shopMenu.MenuItems)
            {
                item.MenuForShop = _shopMenu;
            }

            return _shopMenu;
        }
    }
}
