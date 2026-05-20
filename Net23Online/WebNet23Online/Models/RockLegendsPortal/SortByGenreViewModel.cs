using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Models;

namespace WebNet23Online.Models.RockLegendsPortal
{
    public class SortByGenreViewModel
    {
        public List<RockLegendsGenreItemViewModel> Genres { get; set; } = new();
        public List<SelectListItem> Bands { get; set; } = new();
        public int SelectedBandId { get; set; }
        public int SelectedGenreId { get; set; }
        public bool IsCurrentUserAdmin { get; set; }
        public List<RockLegendsGenreStatsDataModel> AdminSqlStats { get; set; } = new();

    }
}
