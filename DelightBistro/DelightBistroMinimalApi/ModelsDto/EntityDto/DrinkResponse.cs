using System.ComponentModel.DataAnnotations;

namespace DelightBistroMinimalApi.ModelsDto.EntityDto
{
    public class DrinkResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
    }
}
