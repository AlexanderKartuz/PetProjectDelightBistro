using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Models.JapaneseDomesticMarket;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class JapaneseDomesticMarketGenerator : IJapaneseDomesticMarketGenerator
    {
        private List<JapaneseDomesticMarketViewModels> _jdmItems;
        private IJdmManufacturerRepository _jdmManufacturerRepository;
        public JapaneseDomesticMarketGenerator(IJdmManufacturerRepository jdmManufacturerRepository)
        {
            _jdmManufacturerRepository = jdmManufacturerRepository;
            _jdmItems = new List<JapaneseDomesticMarketViewModels>
            {
                new JapaneseDomesticMarketViewModels
                 {
                    ManufacturerType="Toyota",
                    Marka = "Toyota",
                    Model = "Supra",
                    Price = 21000,
                    Url = "/images/japanese-domestic-market/toyota-chaser.jpg"
                 },

                 new JapaneseDomesticMarketViewModels
                 {
                    ManufacturerType="Mitsubishi",
                    Marka = "Mitsubishi",
                    Model = "Evolution",
                    Price = 32000,
                    Url = "/images/japanese-domestic-market/mitsubishi_evo.jpg"
                 },

                 new JapaneseDomesticMarketViewModels
                 {
                    ManufacturerType="Honda",
                    Marka = "Honda",
                    Model = "NSX",
                    Price = 150000,
                    Url = "/images/japanese-domestic-market/honda-nsx.jpg"
                 },
                 new JapaneseDomesticMarketViewModels
                 {
                    ManufacturerType="Nissan",
                    Marka = "Nissan",
                    Model = "370Z",
                    Price = 27000,
                    Url = "/images/japanese-domestic-market/nissan-370z.jpg"
                 },
                 new JapaneseDomesticMarketViewModels
                 {
                    ManufacturerType="Acura",
                    Marka = "Acura",
                    Model = "RSX",
                    Price = 38000,
                    Url = "/images/japanese-domestic-market/acura-rsx.jpg"
                 },
                 new JapaneseDomesticMarketViewModels
                 {
                    ManufacturerType="Mazda",
                    Marka = "Mazda",
                    Model = "MX-5",
                    Price = 42000,
                    Url = "/images/japanese-domestic-market/mazda-mx-5.jpg"
                 }
            };
        }
        public void AddJDMItem(JapaneseDomesticMarketViewModels jdmItem)
        {
            _jdmItems.Add(jdmItem);
        }

        public List<JapaneseDomesticMarketViewModels> GenerateJDMCarsItems()
        {
            return _jdmItems;
        }
        public List<JapaneseDomesticMarketViewModels> GenerateJDMCarsItems(List<JdmCarsData> japaneseDomesticMarketCarsData)
        {
            var _jdmItems = japaneseDomesticMarketCarsData.Select(x => new JapaneseDomesticMarketViewModels
            {
                ManufacturerType = x.ManufacturerType,
                Marka = x.Marka,
                Model = x.Model,
                Price = x.Price,
                Url = x.Url,
                // ConnectedJdmTitles = string.Join(",", x.JdmManufacturerData.JapaneseDomesticMarketCarsDatas.Select(a => a.ManufacturerType!))
            });
            return _jdmItems.ToList();
        }
        public List<SelectListItem> GetListItemsJdmCars()
        {
            var manufactures = _jdmManufacturerRepository.GetAll();
            var manufacturesListItems = new List<SelectListItem>();

            manufacturesListItems.Add(new SelectListItem
            {
                Text = "Выбери производителя",
                Value = ""
            });
            manufacturesListItems.AddRange(manufactures.Select(x => new SelectListItem
            {
                Text = x.ManufacturerType,
                Value = x.Id.ToString()
            }));
            return manufacturesListItems;
        }
    }
}