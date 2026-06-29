using WebNet23Online.Data.Models;
using WebNet23Online.Models.Jdm;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class JdmCatalogGenerator : IJdmCatalogGenerator
    {
        public List<JdmCatalogViewModel> GetManufacturerTypeFromJDMItems(List<JdmViewModels> carsJDMItems, string sortManufacturerType)
        {
            var allCarsJdmTypes = carsJDMItems
        .Where(x => !string.IsNullOrWhiteSpace(x.ManufacturerType))
        .GroupBy(x => x.ManufacturerType)
        .Select(g => new JdmCatalogViewModel
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

        public List<JdmCatalogViewModel> GetManufacturerType(List<JdmManufacturerData> manufactureTypes)
        {
            return manufactureTypes.Select(x => new JdmCatalogViewModel
            {
                Id = x.Id,
                ManufacturerType = x.ManufacturerType,
            }).ToList();
        }
    }
}