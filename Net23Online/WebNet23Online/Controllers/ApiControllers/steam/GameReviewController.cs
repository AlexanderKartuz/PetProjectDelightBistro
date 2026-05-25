using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Models.Steam;
using WebNet23Online.Data.Repositories.Interfaces.Steam;
using WebNet23Online.Models.Steam;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers.steam
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameReviewController : ControllerBase
    {
        private readonly IGameReviewRepository _gameReviewRepository;
        private readonly IAuthService _authService;

        public GameReviewController(IGameReviewRepository reviews, IAuthService auth)
        {
            _gameReviewRepository = reviews;
            _authService = auth;
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddGameReviewRequest request)
        {
            if (!_authService.IsAuthenticated())
            {
                return Unauthorized(new { error = "Login required." });
            }

            if (request == null || request.GameId <= 0 || string.IsNullOrWhiteSpace(request.Text) || request.Rating < 1 || request.Rating > 10)
            {
                return BadRequest(new { error = "Invalid data." });
            }

            var userId = _authService.GetUserId();

            if (_gameReviewRepository.ExistsForUser(request.GameId, userId))
            {
                return Conflict(new { error = "You already reviewed this game." });
            }

            var review = new GameReviewData
            {
                GameId = request.GameId,
                AuthorId = userId,
                Text = request.Text.Trim(),
                Rating = request.Rating,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null
            };

            _gameReviewRepository.Add(review);

            return Ok(new
            {
                author = _authService.GetUserName(),
                text = review.Text,
                rating = review.Rating,
                createdAt = review.CreatedAt
            });
        }
    }
}