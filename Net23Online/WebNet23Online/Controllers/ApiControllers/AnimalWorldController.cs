using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnimalWorldController : ControllerBase
    {
        private IZooRepository _zooRepository;

        public AnimalWorldController(IZooRepository zooRepository)
        {
            _zooRepository = zooRepository;
        }

        [HttpGet]
        public bool IsZooNameFree([FromQuery] string zooName)
        {
            return _zooRepository.GetElementByName(zooName) == null;
        }
    }
}
