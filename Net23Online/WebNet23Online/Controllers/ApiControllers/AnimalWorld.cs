using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalWorld : ControllerBase
    {
        private IZooRepository _zooRepository;

        public AnimalWorld(IZooRepository zooRepository)
        {
            _zooRepository = zooRepository;
        }

        public bool IsZooNameFree(string zooName)
        {
            return _zooRepository.GetElementByName(zooName) == null;
        }
    }
}
