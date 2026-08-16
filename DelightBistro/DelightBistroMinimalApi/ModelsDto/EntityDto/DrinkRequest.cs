using System.ComponentModel.DataAnnotations;

namespace DelightBistroMinimalApi.ModelsDto.EntityDto
{
    public class DrinkRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Range(0.1, 500, ErrorMessage = "Price must be between 0.1 and 500")]
        public decimal Price { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        [MaxLength(500)]
        public string? ImgUrl { get; set; }
    }
}
