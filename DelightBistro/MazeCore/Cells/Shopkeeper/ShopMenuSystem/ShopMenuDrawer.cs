using MazeCore.Cells.Shopkeeper;

namespace MazeCore.Cells.Shopkeeper.ShopMenuSystem
{
    public class ShopMenuDrawer
    {
        private ShopMenu _shopMenu;
        private Shopkeeper _shopkeeper;
        private const int SHOP_HISTORY_LENGHT = 3;
        public void Draw(ShopMenu shopMenu, Shopkeeper shopkeeper)
        {
            _shopMenu = shopMenu;
            _shopkeeper = shopkeeper;
            Console.Clear();
            for (var menuItemIndex = 0; menuItemIndex < shopMenu.MenuItems.Count(); menuItemIndex++)
            {
                var currentMenuItem = shopMenu.MenuItems[menuItemIndex];
                Console.Write($"{menuItemIndex + 1}.");
                Console.Write($"{currentMenuItem.Name.PadRight(20)} ");
                if (currentMenuItem.PriceDisplay != "")
                {
                    Console.Write($" {currentMenuItem.PriceDisplay.PadRight(11)}");
                }
                if(shopMenu._cursorPosition == menuItemIndex)
                {
                    Console.Write($"{shopMenu._cursor}");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"\nCoins: {_shopkeeper.Character.Coins}");
            Console.WriteLine("\nShop History:");
            DrawShopMenuHistory();
        }
        private void DrawShopMenuHistory()
        {
            var messages = _shopMenu
                .ShopHistory
                .TakeLast(SHOP_HISTORY_LENGHT);

            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
        }
    }
}
