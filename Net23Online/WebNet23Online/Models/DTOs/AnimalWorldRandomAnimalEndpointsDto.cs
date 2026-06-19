using System.Text.Json.Serialization;

namespace WebNet23Online.Models.DTOs
{
    public class AnimalWorldRandomAnimalEndpointsDto
    {
        [JsonPropertyName("endpoints")]
        public List<string> Endpoints { get; set; }
    }
}
