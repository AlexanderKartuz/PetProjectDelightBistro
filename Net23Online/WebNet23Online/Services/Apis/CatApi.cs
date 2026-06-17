using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Services.Apis
{
    public class CatApi
    {
        private HttpClient _httpClient;

        public CatApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatDto>> GetCats()
        {
            return await _httpClient
                .GetFromJsonAsync<List<CatDto>>("/api/cats");
        }
    }
}
