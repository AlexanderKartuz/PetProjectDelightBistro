namespace WebNet23Online.Models.DTOs
{
    public class RockTrackDto
    {
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
        public string CollectionName { get; set; }
        public string ArtworkUrl100 { get; set; }
        public string PreviewUrl { get; set; }
    }

    public class RockTrackRootDto
    {
        public List<RockTrackDto> Results { get; set; }
    }
}
