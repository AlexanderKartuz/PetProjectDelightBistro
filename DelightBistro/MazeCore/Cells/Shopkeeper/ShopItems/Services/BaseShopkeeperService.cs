using MazeCore.Cells.Shopkeeper.ShopItems;
using MazeCore.Cells.Shopkeeper.ShopItems.Services.Interfaces;

namespace MazeCore.Cells.Shopkeeper.ShopItems.Services
{
    public abstract class IBaseShopkeeperService : BaseShopItem, IIBaseShopkeeperService
    {
        protected int _unitPrice;
        public int UnitPrice => _unitPrice;
        public override string PriceDisplay => $"Cost:{_unitPrice}";
        protected IBaseShopkeeperService(int unitPrice)
        {
            _unitPrice = unitPrice;
        }

    }
}
