namespace DelightBistroMinimalApi.DbStuff
{
    public class Tea
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }

        public string? ImgUrl { get; set; }
    }
}
