using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnimeController : ControllerBase
    {
        private IAnimeRepository _animeRepository;

        public AnimeController(IAnimeRepository animeRepository)
        {
            _animeRepository = animeRepository;
        }

        public bool UpdateName([FromQuery] int id, [FromQuery] string name)
        {
            _animeRepository.Rename(id, name);
            return true;
        }
    }
}
