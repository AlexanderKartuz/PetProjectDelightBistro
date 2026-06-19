using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Services.Apis
{
    public class AnimalWorldRandomAnimalApi
    {
        private HttpClient _httpClient;

        public AnimalWorldRandomAnimalApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> GetAnimalSpecies()
        {
            var endpoints = await _httpClient.GetFromJsonAsync<AnimalWorldRandomAnimalEndpointsDto>("/animal");
            return endpoints.Endpoints;
        }

        public async Task<AnimalWorldRandomAnimalDto> GetRandomAnimal(string type)
        {
            return await _httpClient.GetFromJsonAsync<AnimalWorldRandomAnimalDto>(type);
        }
    }
}
