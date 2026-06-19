using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Services.Apis
{
    public class CatFactApi
    {
        private HttpClient _httpClient;

        public CatFactApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CatFactDto> GetCatFact()
        {
            return await _httpClient
                .GetFromJsonAsync<CatFactDto>("/fact");
        }
    }
}
