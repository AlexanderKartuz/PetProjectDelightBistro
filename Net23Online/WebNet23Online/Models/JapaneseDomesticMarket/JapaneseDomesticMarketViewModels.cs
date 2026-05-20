using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Controllers;
using WebNet23Online.Models.CustomValidatioAttributes;

namespace WebNet23Online.Models.JapaneseDomesticMarket
{
    public class JapaneseDomesticMarketViewModels
    {
        public int Id { get; set; }
        public string Url { get; set; } = "";
        [CheckForEnglishLetters]
        public string Marka { get; set; } = string.Empty;
        [CheckForEnglishLetters]
        [CheckCivic]
        public string Model { get; set; } = string.Empty;
        public int Price { get; set; }

        public string ManufacturerType { get; set; } = "";
        public string ConnectedJdmTitles { get; set; } = string.Empty;
        public int? ManufactureId { get; set; }
        public List<SelectListItem> AllManufacturer { get; set; } = new();
        public bool IsAdminAuth { get; set; }
        public IFormFile? VehicleInspectionHistoryUrl { get; set; }

    }
}