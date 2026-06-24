using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Services.Apis
{
    public class DogApi
    {
        private HttpClient _httpClient;

        public DogApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DogDto> GetDog()
        {
            return await _httpClient
                .GetFromJsonAsync<DogDto>("/api/breeds/image/random");
        }
    }
}
