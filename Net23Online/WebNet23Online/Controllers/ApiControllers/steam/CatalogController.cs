using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Data.Repositories.Interfaces.Steam;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers.steam
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        private readonly IAuthService _authService;

        public CatalogController(IGameRepository gameRepository, IAuthService authService)
        {
            _gameRepository = gameRepository;
            _authService = authService;
        }

        [IsAdminApi]
        public bool Delete([FromQuery] List<int> gameIds)
        {
            _gameRepository.Delete(gameIds);
            return true;
        }
    }
}
