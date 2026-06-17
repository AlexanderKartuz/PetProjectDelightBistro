namespace WebNet23Online.Models.DTOs
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class WaifuDtoArtist
    {
        public int id { get; set; }
        public string name { get; set; }
        public object patreon { get; set; }
        public string pixiv { get; set; }
        public string twitter { get; set; }
        public object deviantArt { get; set; }
        public string reviewStatus { get; set; }
        public object creatorId { get; set; }
        public int imageCount { get; set; }
    }

    public class WaifuDtoItem
    {
        public int id { get; set; }
        public string perceptualHash { get; set; }
        public string extension { get; set; }
        public string dominantColor { get; set; }
        public string source { get; set; }
        public List<WaifuDtoArtist> artists { get; set; }
        public object uploaderId { get; set; }
        public DateTime uploadedAt { get; set; }
        public bool isNsfw { get; set; }
        public bool isAnimated { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int byteSize { get; set; }
        public string url { get; set; }
        public List<WaifuDtoTag> tags { get; set; }
        public string reviewStatus { get; set; }
        public int favorites { get; set; }
        public object likedAt { get; set; }
        public object addedToAlbumAt { get; set; }
        public List<object> albums { get; set; }
    }

    public class WaifuDtoRoot
    {
        public List<WaifuDtoItem> items { get; set; }
        public int pageNumber { get; set; }
        public int totalPages { get; set; }
        public int totalCount { get; set; }
        public int maxPageSize { get; set; }
        public int defaultPageSize { get; set; }
        public bool hasPreviousPage { get; set; }
        public bool hasNextPage { get; set; }
    }

    public class WaifuDtoTag
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string description { get; set; }
        public string reviewStatus { get; set; }
        public object creatorId { get; set; }
        public int imageCount { get; set; }
    }


}
