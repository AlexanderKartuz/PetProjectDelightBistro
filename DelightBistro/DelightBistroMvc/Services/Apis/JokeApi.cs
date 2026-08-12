using DelightBistroMvc.Models.DTOs;

namespace DelightBistroMvc.Services.Apis
{
    public class JokeApi
    {
        private HttpClient _httpClient;

        public JokeApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JokeDto> GetJoke()
        {
            return await _httpClient
                .GetFromJsonAsync<JokeDto>("/jokes/random");
        }
    }
}
