using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Hubs;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnimeController : ControllerBase
    {
        private IAnimeRepository _animeRepository;
        private IHubContext<AnimeHub, IAnimeHub> _animeHub;

        public AnimeController(
            IAnimeRepository animeRepository, 
            IHubContext<AnimeHub, IAnimeHub> animeHub)
        {
            _animeRepository = animeRepository;
            _animeHub = animeHub;
        }

        public bool UpdateName([FromQuery] int id, [FromQuery] string name)
        {
            _animeRepository.Rename(id, name);
            return true;
        }

        public void NotifyAboutAnime(string name)
        {
            _animeHub.Clients.All.NewAnimeCreated(name, "https://avatars.mds.yandex.net/i?id=16b9b95bbc7879568c85de013a98ba67_l-5213440-images-thumbs&n=13");
        }
    }
}
