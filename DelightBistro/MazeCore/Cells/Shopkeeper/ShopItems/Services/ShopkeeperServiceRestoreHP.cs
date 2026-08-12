using MazeCore.Characters.Interfaces;

namespace MazeCore.Cells.Shopkeeper.ShopItems.Services
{
    public class ShopkeeperServiceRestoreHP : IBaseShopkeeperService
    {
        public ShopkeeperServiceRestoreHP(int unitPrice) : base(unitPrice)
        {
            Name = "Restore HP";
        }
        public override void Execute(IBaseCharacter character)
        {
            
            if (character.Coins > 0)
            {
                character.Coins--;
                character.Hp++;
                MenuForShop.ShopHistory.Add("You restored 1 HP");
            }
            else
            {
                MenuForShop.ShopHistory.Add("You don't have any coin");
            }
        }
    }
}
