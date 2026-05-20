using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data.Models;

namespace WebNet23Online.Models.RockLegendsPortal
{
    public class RockLegendsGenreItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public List<string> BandNames { get; set; } = new();
    }
}
