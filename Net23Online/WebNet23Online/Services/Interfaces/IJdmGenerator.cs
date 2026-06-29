using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data.Models;
using WebNet23Online.Models.Jdm;

namespace WebNet23Online.Services.Interfaces
{
    public interface IJdmGenerator
    {
        List<JdmViewModels> GenerateJDMCarsItems();
        List<JdmViewModels> GenerateJDMCarsItems(List<JdmCarsData> jdmCarsData);
        void AddJDMItem(JdmViewModels _jdmItems);
        List<SelectListItem> GetListItemsJdmCars();
    }
}