using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Services.Apis
{
    public class WaifuApi
    {
        private HttpClient _httpClient;

        public WaifuApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WaifuDtoRoot> GetWaifu()
        {
            return await _httpClient
                .GetFromJsonAsync<WaifuDtoRoot>("/images");
        }
    }
}
