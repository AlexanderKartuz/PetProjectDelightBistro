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
        public IActionResult GetGames([FromQuery] CatalogFilterViewModel? filter)
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
            var meta = catalog.PaginationMetadata;

            return Ok(new PaginatedGamesApiViewModel
            {
                Items = catalog.Games,
                TotalCount = meta.TotalCount,
                PageSize = filter.PageSize,
                CurrentPage = meta.CurrentPage,
                TotalPages = meta.TotalPages,
                HasPrevious = meta.HasPreviousPage,
                HasNext = meta.HasNextPage,
            });
        }

        [HttpGet]
        public IActionResult GetGameDetails([FromQuery] int id)
        {
            var game = _catalogService.GetGameDetails(id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(new SteamGameViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                Price = game.Price,
                AverageRating = game.AverageRating,
                ReviewsCount = game.ReviewsCount ?? 0,
                Genres = game.GameGenres?.Select(g => g.Name).ToList() ?? new(),
            });
        }

        [IsAdminApi]
        public bool Delete([FromQuery] List<int> gameIds)
        {
            _gameRepository.Delete(gameIds);
            return true;
        }
    }
}
