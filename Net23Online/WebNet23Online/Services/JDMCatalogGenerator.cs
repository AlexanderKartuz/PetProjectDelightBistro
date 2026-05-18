using WebNet23Online.Data.Models;
using WebNet23Online.Models.JapaneseDomesticMarket;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class JDMCatalogGenerator : IJDMCatalogGenerator
    {
        public List<JDMCatalogViewModels> GetManufacturerTypeFromJDMItems(List<JapaneseDomesticMarketViewModels> carsJDMItems, string sortManufacturerType)
        {
            var allCarsJDMTypes = new List<JDMCatalogViewModels>
            {
                new JDMCatalogViewModels()
                {
               ManufacturerType="Toyota",
               NameType="Тойота",
               CarsJDMItems = carsJDMItems.Where(x=>x.ManufacturerType=="Toyota").ToList()
                },
                new JDMCatalogViewModels()
                {
               ManufacturerType="Mazda",
               NameType="Мазда",
               CarsJDMItems = carsJDMItems.Where(x=>x.ManufacturerType=="Mazda").ToList()
                },
                new JDMCatalogViewModels()
                {
               ManufacturerType="Nissan",
               NameType="Ниссан",
               CarsJDMItems = carsJDMItems.Where(x=>x.ManufacturerType=="Nissan").ToList()
                },
                new JDMCatalogViewModels()
                {
               ManufacturerType="Honda",
               NameType="Хонда",
               CarsJDMItems = carsJDMItems.Where(x=>x.ManufacturerType=="Honda").ToList()
                },
                new JDMCatalogViewModels()
                {
               ManufacturerType="Acura",
               NameType="Акура",
               CarsJDMItems = carsJDMItems.Where(x=>x.ManufacturerType=="Acura").ToList()
                },
                 new JDMCatalogViewModels()
                {
               ManufacturerType="Mitsubishi",
               NameType="Митцубиси",
               CarsJDMItems = carsJDMItems.Where(x=>x.ManufacturerType=="Mitsubishi").ToList()
                }
            };
            var OneJDMCarsType = allCarsJDMTypes.Where(x => x.ManufacturerType == sortManufacturerType).ToList();
            if (string.IsNullOrEmpty(sortManufacturerType))
            {
                return allCarsJDMTypes;
            }
            return OneJDMCarsType;
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