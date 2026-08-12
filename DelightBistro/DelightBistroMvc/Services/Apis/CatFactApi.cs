using DelightBistroMvc.Models.DTOs;

namespace DelightBistroMvc.Services.Apis
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
