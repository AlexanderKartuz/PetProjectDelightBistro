using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Services.Apis
{
    public class FakeRestaurantApi
    {
        private HttpClient _httpClient;

        public FakeRestaurantApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<FakeRestaurantDto>> GetFakeMenuItems()
        {
            return await _httpClient
                .GetFromJsonAsync<List<FakeRestaurantDto>>("/api/Restaurant/5/menu");
        }
    }
}
