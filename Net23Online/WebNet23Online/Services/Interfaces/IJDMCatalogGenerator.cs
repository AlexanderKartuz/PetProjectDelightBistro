using WebNet23Online.Data.Models;
using WebNet23Online.Models.JapaneseDomesticMarket;

namespace WebNet23Online.Services.Interfaces
{
    public interface IJDMCatalogGenerator
    {
        List<JDMCatalogViewModels> GetManufacturerTypeFromJDMItems(List<JapaneseDomesticMarketViewModels> carsJDMItems, string sortManufacturerType);
        List<JDMCatalogViewModels> GetManufacturerType(List<JdmManufacturerData> manufactureTypes);
    }
}
