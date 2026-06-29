namespace WebNet23Online.Models.Jdm
{
    public class CatalogCarsPermissionViewModel
    {
        public List<JdmCatalogViewModel> CatalogAuto { get; set; } = new();
        public List<VehicleInspectionHistoryItemViewModel> CarsWithoutInspection { get; set; } = new();
        public bool IsAdmin { get; set; }
    }
}