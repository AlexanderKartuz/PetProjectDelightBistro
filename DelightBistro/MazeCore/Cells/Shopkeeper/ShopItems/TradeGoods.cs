using MazeCore.Characters.Interfaces;

namespace MazeCore.Cells.Shopkeeper.ShopItems
{

    public class TradeGoods : BaseShopItem
    {
        private int _unitPrice { get; set; }
        private int _count { get; set; }
        private Action<IBaseCharacter> _buyEffect;
        public int UnitPrice => _unitPrice;
        public int Count => _count;
        public override string PriceDisplay => $"Cost:{_unitPrice} (x{_count})";
        public Action<IBaseCharacter> BuyEffect => _buyEffect;
        public TradeGoods(string name, int unitPrice, int count, Action<IBaseCharacter> buyEffect)
        {
            Name = $"Buy {name}";
            _unitPrice = unitPrice;
            _count = count;
            _buyEffect = buyEffect;
        }
        public override void Execute(IBaseCharacter character)
        {
            if(_count <= 0)
            {
                MenuForShop.ShopHistory.Add($"{Name} is out of Stock.");
                return;
            }
            if(character.Coins < _unitPrice)
            {
                MenuForShop.ShopHistory.Add($"Not enough coins to buy {Name}");
                return;
            }
            character.Coins -= _unitPrice;
            _count--;
            _buyEffect(character);
            MenuForShop.ShopHistory.Add($"You bought {Name}");
        }

    }


}

