using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Services.Apis
{
    public class RockApi
    {
        private readonly HttpClient _httpClient;

        public RockApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RockTrackDto> GetRandomRockHit()
        {
            var response = await _httpClient
                .GetFromJsonAsync<RockTrackRootDto>("/search?term=rock&media=music&limit=20");

            if (response?.Results != null && response.Results.Count > 0)
            {
                var random = new Random();
                int index = random.Next(response.Results.Count);
                return response.Results[index];
            }

            return null;
        }
    }
}
