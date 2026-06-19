using System.Text.Json.Serialization;

namespace WebNet23Online.Models.DTOs
{
    public class AnimalWorldRandomAnimalDto
    {
        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("fact")]
        public string Fact { get; set; }
    }
}
