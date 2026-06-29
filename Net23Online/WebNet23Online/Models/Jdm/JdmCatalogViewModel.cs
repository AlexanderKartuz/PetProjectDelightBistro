namespace WebNet23Online.Models.Jdm
{
    public class JdmCatalogViewModel
    {
        public int Id { get; set; }
        public string ManufacturerType { get; set; } = "";
        public string NameType { get; set; } = "";
        public List<JdmViewModels> CarsJDMItems { get; set; }

    }
}