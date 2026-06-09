namespace MovieMinimalApi.DbStuff
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int Rating { get; set; }
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
