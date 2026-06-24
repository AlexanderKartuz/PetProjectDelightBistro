
namespace WebNet23Online.Models.Steam
{
    public class RawgResponse
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<RawgGameDto> Results { get; set; }
    }
}
