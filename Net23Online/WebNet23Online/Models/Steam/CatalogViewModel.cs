using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data.HelperModels.SteamPagination;

namespace WebNet23Online.Models.Steam
{
    public class CatalogViewModel
    {
        public bool IsUserAtLeastModerator { get; set; }
        public CatalogFilterViewModel Filter { get; set; }
        public List<SteamGameViewModel> Games { get; set; } = new();
        public List<SelectListItem> GameGenres { get; set; } = new();

        public PaginationMetadataViewModel PaginationMetadata { get; set; } = new();
    }
}   