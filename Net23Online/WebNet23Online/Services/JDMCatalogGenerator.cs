using WebNet23Online.Data.Models;
using WebNet23Online.Models.JapaneseDomesticMarket;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class JDMCatalogGenerator : IJDMCatalogGenerator
    {
        public List<JDMCatalogViewModels> GetManufacturerTypeFromJDMItems(List<JapaneseDomesticMarketViewModels> carsJDMItems, string sortManufacturerType)
        {
            var allCarsJdmTypes = carsJDMItems
        .Where(x => !string.IsNullOrWhiteSpace(x.ManufacturerType))
        .GroupBy(x => x.ManufacturerType)
        .Select(g => new JDMCatalogViewModels
        {
            ManufacturerType = g.Key,
            NameType = g.Key,
            CarsJDMItems = g.ToList()
        })
        .OrderBy(x => x.ManufacturerType)
        .ToList();
            if (string.IsNullOrWhiteSpace(sortManufacturerType))
            {
                return allCarsJdmTypes;
            }
            return allCarsJdmTypes
                .Where(x => x.ManufacturerType == sortManufacturerType)
                .ToList();
        }
        public List<JDMCatalogViewModels> GetManufacturerType(List<JdmManufacturerData> manufactureTypes)
        {
            return manufactureTypes.Select(x => new JDMCatalogViewModels
            {
                Id = x.Id,
                ManufacturerType = x.ManufacturerType,
            }).ToList();
        }
    }
}