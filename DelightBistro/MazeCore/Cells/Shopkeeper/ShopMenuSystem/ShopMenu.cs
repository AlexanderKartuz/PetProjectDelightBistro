using MazeCore.Cells.Shopkeeper.ShopItems;

namespace MazeCore.Cells.Shopkeeper.ShopMenuSystem
{
    public class ShopMenu
    {
        public List<BaseShopItem> MenuItems {  get; set; } = new ();
        public List<string> ShopHistory { get; set; } = new ();
        public char _cursor;
        public int _cursorPosition = 0;
    }
}
