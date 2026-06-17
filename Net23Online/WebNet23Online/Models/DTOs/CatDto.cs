namespace WebNet23Online.Models.DTOs
{
    public class CatDto
    {
        public string id { get; set; }
        public List<string> tags { get; set; }
        public string mimetype { get; set; }
        public DateTime createdAt { get; set; }
    }
}
