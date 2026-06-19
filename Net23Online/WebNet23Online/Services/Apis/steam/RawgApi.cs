
using WebNet23Online.Models.Steam;

namespace WebNet23Online.Services.Apis.steam
{
    public class RawgApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public RawgApi(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["RAWG:ApiKey"];
        }

        public async Task<RawgResponse?> SearchGames(string query, int pageSize = 10) =>
            await _httpClient.GetFromJsonAsync<RawgResponse>(
                $"games?key={_apiKey}&search={Uri.EscapeDataString(query)}&page_size={pageSize}");

        public async Task<RawgGameDto?> GetGameDetails(string slug) =>
            await _httpClient.GetFromJsonAsync<RawgGameDto>(
                $"games/{slug}?key={_apiKey}");

        public async Task<RawgResponse?> GetGameSeries(string gameId, int pageSize = 6) =>
            await _httpClient.GetFromJsonAsync<RawgResponse>(
                $"games/{gameId}/game-series?key={_apiKey}&page_size={pageSize}");

        public async Task<RawgResponse?> GetPopularGames(int pageSize = 12) =>
            await _httpClient.GetFromJsonAsync<RawgResponse>(
                $"games?key={_apiKey}&ordering=-rating&page_size={pageSize}");

        public async Task<RawgResponse?> GetNewReleases(int pageSize = 12) =>
            await _httpClient.GetFromJsonAsync<RawgResponse>(
                $"games?key={_apiKey}&ordering=-released&page_size={pageSize}");
    }
}