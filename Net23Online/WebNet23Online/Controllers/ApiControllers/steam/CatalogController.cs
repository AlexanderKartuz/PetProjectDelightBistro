using Microsoft.AspNetCore.Mvc;

using WebNet23Online.Controllers.CustomAuthAttribute.Steam;
using WebNet23Online.Data.Repositories.Interfaces.Steam;

namespace WebNet23Online.Controllers.ApiControllers.steam
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;

        public CatalogController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;

        }

        [IsAdminApi]
        public bool Delete([FromQuery] List<int> gameIds)
        {
            _gameRepository.Delete(gameIds);
            return true;
        }
    }
}
