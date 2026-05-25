using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RockBandsController : ControllerBase
    {
        private readonly IRockBandsRepository _rockBandsRepository;
        private readonly IRockBandsService _rockBandsService;
        private readonly IAuthService _authService;

        public RockBandsController(
            IRockBandsRepository rockBandsRepository,
            IRockBandsService rockBandsService,
            IAuthService authService)
        {
            _rockBandsRepository = rockBandsRepository;
            _rockBandsService = rockBandsService;
            _authService = authService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddLike(int bandId)
        {
            var userId = _authService.GetUserId();
            if (userId <= 0)
            {
                return Unauthorized();
            }

            var result = _rockBandsService.AddLike(bandId, userId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                likeCount = result.LikeCount,
                liked = result.Liked,
                alreadyLiked = result.AlreadyLiked,
            });
        }

        public bool IsBandNameFree(string name)
        {
            Thread.Sleep(1000);
            if (string.IsNullOrWhiteSpace(name))
            {
                return true;
            }

            return !_rockBandsRepository.IsBandNameTaken(name.Trim());
        }
    }
}
