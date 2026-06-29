using WebNet23Online.Data.Models;
using WebNet23Online.Models.Jdm;

namespace WebNet23Online.Services.Interfaces
{
    public interface IJdmCatalogGenerator
    {
        List<JdmCatalogViewModel> GetManufacturerTypeFromJDMItems(List<JdmViewModels> carsJDMItems, string sortManufacturerType);
        List<JdmCatalogViewModel> GetManufacturerType(List<JdmManufacturerData> manufactureTypes);
    }
}
