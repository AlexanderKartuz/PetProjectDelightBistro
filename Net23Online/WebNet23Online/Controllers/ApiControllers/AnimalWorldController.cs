using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnimalWorldController : ControllerBase
    {
        private IZooRepository _zooRepository;
        private IAnimalSpeciesRepository _animalSpeciesRepository;

        public AnimalWorldController(IZooRepository zooRepository, IAnimalSpeciesRepository animalSpeciesRepository)
        {
            _zooRepository = zooRepository;
            _animalSpeciesRepository = animalSpeciesRepository;
        }

        [HttpGet]
        public bool IsZooNameFree([FromQuery] string zooName)
        {
            return _zooRepository.GetElementByName(zooName) == null;
        }

        [HttpGet]
        public List<string> GetAnimalSpeciesNames()
        {
            return _animalSpeciesRepository.GetAllAnimalSpeciesNames();
        }
    }
}
