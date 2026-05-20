namespace WebNet23Online.Models.JapaneseDomesticMarket
{
    public class CatalogCarsPermissionViewModel
    {
        public List<JDMCatalogViewModels> CatalogAuto { get; set; } = new();
        public List<VehicleInspectionHistoryItemViewModel> CarsWithoutInspection { get; set; } = new();
        public bool IsAdmin { get; set; }
    }
}