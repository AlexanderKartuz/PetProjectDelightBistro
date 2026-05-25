using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnimeGirlController : ControllerBase
    {
        public IAnimeGirlRepository _animeGirlRepository;

        public AnimeGirlController(IAnimeGirlRepository animeGirlRepository)
        {
            _animeGirlRepository = animeGirlRepository;
        }

        public bool Delete([FromQuery]List<int> ids)
        {
            _animeGirlRepository.Delete(ids);
            return true;
        }
    }
}
