using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Models.RockLegendsPortal
{
    public class RockLegendsPortalViewModel
    {
        public int SelectedBandId { get; set; }
        public string BandName { get; set; }
        public string Biography { get; set; }
        public int PickTime { get; set; }
        public RockTrackDto RockHit { get; set; }

    }
}
