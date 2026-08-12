using MazeCore.Cells;
using MazeCore.Cells.Shopkeeper.ShopItems;
using MazeCore.Cells.Shopkeeper.ShopItems.Services;
using MazeCore.Cells.Shopkeeper.ShopItems.Special;
using MazeCore.Cells.Shopkeeper.ShopMenuSystem;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells.Shopkeeper
{
    public class Shopkeeper : BaseCell
    {
        public IBaseCharacter Character { get; set; }
        private Random _random;
        public bool WantToTrade { get; set; }
        public List<BaseShopItem> GoodsAndServices { get; set; }
        public Shopkeeper(IMaze maze, Random random) : base(maze)
        {
            _random = random;
            WantToTrade = true;
        }
        public override char Symbol => '$';
        public override bool Interaction(IBaseCharacter character)
        {
            Character = character;
            Console.Clear();
            if (WantToTrade)
            {
                Maze.EventHistory.Add("Shopkeeper here!");
            }
            else
            {
                Maze.EventHistory.Add("Shopkeeper ignore you!");
                return true;
            }
            //нет способа остановить музыку лабиринта
            //MazeSoundPlayer SoundPlayer = new MazeSoundPlayer();
            //SoundPlayer.PlayMusic("Shopkeeper.mp3", 0.7f, true);
            GoodsAndServices = new List<BaseShopItem>
            {
                new TradeGoods(name: "Key", unitPrice: 2, count: 1, c => c.Keys++),
                new TradeGoods(name: "Speed Potion", unitPrice: 1, count: 3, c => c.Speed++),
                new TradeGoods(name: "Super Power", unitPrice: 3, count: 1, c => c.SuperPower++),
                new ShopkeeperServiceRestoreHP(unitPrice: 2),
                new TryStealCoins(this, _random)
            };
            var shopMenuController = new ShopMenuController(this);
            shopMenuController.StartShopMenu();
            return true;

        }


    }
}
