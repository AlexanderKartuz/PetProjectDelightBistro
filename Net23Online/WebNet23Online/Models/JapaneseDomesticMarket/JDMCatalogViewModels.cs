namespace WebNet23Online.Models.JapaneseDomesticMarket
{
    public class JDMCatalogViewModels
    {
        public int Id { get; set; }
        public string ManufacturerType { get; set; } = "";
        public string NameType { get; set; } = "";
        public List<JapaneseDomesticMarketViewModels> CarsJDMItems { get; set; }

    }
}