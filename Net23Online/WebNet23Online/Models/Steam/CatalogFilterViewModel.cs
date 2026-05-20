namespace WebNet23Online.Models.Steam
{
    public class CatalogFilterViewModel
    {
        public int? GenreId { get; set; }
        public decimal? MaxPrice { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
