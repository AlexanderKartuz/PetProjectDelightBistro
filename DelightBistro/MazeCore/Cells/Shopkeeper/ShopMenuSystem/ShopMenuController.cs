using MazeCore.Cells.Shopkeeper;

namespace MazeCore.Cells.Shopkeeper.ShopMenuSystem
{
    public class ShopMenuController
    {
        private ShopMenu _shopMenu;
        private Shopkeeper _shopkeeper { get; set; }
        public ShopMenuController(Shopkeeper shopkeeper)
        {
            _shopkeeper = shopkeeper;
        }
        public void StartShopMenu()
        {
            var shopMenuBuilder = new ShopMenuBuilder();
            _shopMenu = shopMenuBuilder.Build(_shopkeeper.GoodsAndServices, '<');
            var shopMenuDrawer = new ShopMenuDrawer();
            shopMenuDrawer.Draw(_shopMenu, _shopkeeper);
            
            var continueShoping = true;
            do
            {
                continueShoping = DoOneMenuAction();
                shopMenuDrawer.Draw(_shopMenu, _shopkeeper);
            } while (continueShoping);
        }
        private bool DoOneMenuAction()
        {
            var currentCursorPosition = _shopMenu._cursorPosition;
            var currentMenuItem = _shopMenu.MenuItems[currentCursorPosition];
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.W:
                    {
                        if (currentCursorPosition != 0)
                        {
                            currentCursorPosition--;
                        }
                        break;
                    }
                case ConsoleKey.S:
                    {
                        if (currentCursorPosition != _shopMenu.MenuItems.Count - 1)
                        {
                            currentCursorPosition++;
                        }
                        break;
                    }
                case ConsoleKey.Enter:
                    {
                        currentMenuItem.Execute(_shopkeeper.Character);
                        break;
                    }
                case ConsoleKey.Escape:
                    {
                        return false;
                    }
            }
            _shopMenu._cursorPosition = currentCursorPosition;
            if(!_shopkeeper.WantToTrade)
            {
                return false;
            }
            return true;
        }
       
    }
}
