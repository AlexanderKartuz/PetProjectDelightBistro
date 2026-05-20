using WebNet23Online.Data.Models;
using WebNet23Online.Models.JapaneseDomesticMarket;

namespace WebNet23Online.Services.Interfaces
{
    public interface IJapaneseDomesticMarketGenerator
    {
        List<JapaneseDomesticMarketViewModels> GenerateJDMCarsItems();
        List<JapaneseDomesticMarketViewModels> GenerateJDMCarsItems(List<JdmCarsData> jdmCarsData);
        void AddJDMItem(JapaneseDomesticMarketViewModels _jdmItems);
    }
}