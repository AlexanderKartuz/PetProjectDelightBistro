using Microsoft.AspNetCore.Mvc;

using WebNet23Online.Controllers.CustomAuthAttribute.Steam;
using WebNet23Online.Data.Repositories.Interfaces.Steam;
using WebNet23Online.Models.Steam;
using WebNet23Online.Services.Interfaces.Steam;

namespace WebNet23Online.Controllers.ApiControllers.steam
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        private readonly ICatalogService _catalogService;

        public CatalogController(IGameRepository gameRepository, ICatalogService catalogService)
        {
            _gameRepository = gameRepository;
            _catalogService = catalogService;
        }

        [HttpGet]
        public ActionResult<List<SteamGameViewModel>> GetGames([FromQuery] CatalogFilterViewModel? filter)
        {
            filter ??= new CatalogFilterViewModel();

            if (filter.Page < 1)
            {
                filter.Page = 1;
            }

            if (filter.PageSize < 1)
            {
                filter.PageSize = 12;
            }

            var catalog = _catalogService.GetCatalog(filter);
            return Ok(catalog.Games);
        }

        [IsAdminApi]
        public bool Delete([FromQuery] List<int> gameIds)
        {
            _gameRepository.Delete(gameIds);
            return true;
        }
    }
}
