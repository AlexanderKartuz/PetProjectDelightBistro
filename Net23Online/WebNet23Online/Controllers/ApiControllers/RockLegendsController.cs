using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Hubs;

namespace WebNet23Online.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/rock-legends")]
    public class RockLegendsApiController : ControllerBase
    {
        private readonly IRockLegendsRepository _rockLegendsRepository;
        private readonly IRockLegendsGenresRepository _genreRepository;
        private IHubContext<RockLegendsHub, IRockLegendsHub> _rockLegendsHub;

        public RockLegendsApiController(
            IRockLegendsRepository rockLegendsRepository,
            IRockLegendsGenresRepository genreRepository,
            IHubContext<RockLegendsHub, IRockLegendsHub> rockLegendsHub)
        {
            _rockLegendsRepository = rockLegendsRepository;
            _genreRepository = genreRepository;
            _rockLegendsHub = rockLegendsHub;
        }

        [HttpPost("like/{id}")]
        [Authorize]
        public IActionResult LikeBand(int id)
        {
            if (HttpContext.Request.Cookies.ContainsKey("HasVotedInRockPoll"))
            {
                return BadRequest(new { success = false, message = "Вы уже отдавали свой голос! Рок-н-ролл за честные выборы 🤘" });
            }

            var targetBand = _rockLegendsRepository.GetById(id);
            if (targetBand == null)
            {
                return NotFound(new { message = "Группа не найдена" });
            } 
            targetBand.Likes++;
            _rockLegendsRepository.Update(targetBand);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                Secure = true
            };

            HttpContext.Response.Cookies.Append("HasVotedInRockPoll", "true", cookieOptions);

            return Ok(new { success = true, newLikes = targetBand.Likes });
        }

        [HttpGet("validate-genre")]
        public IActionResult ValidateGenreName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Ok(new { isValid = true });
            }
            var allGenres = _genreRepository.GetAll();
            var exists = allGenres.Any(g => g.Name.Trim().ToLower() == name.Trim().ToLower());

            if (exists)
            {
                return Ok(new { isValid = false, message = "Такой жанр уже гордо существует в нашей базе!" });
            }

            return Ok(new { isValid = true });
        }
        public void NotifyAboutGenre(string name)
        {
            _rockLegendsHub.Clients.All.NewGenreCreated(name, "https://www.pngall.com/wp-content/uploads/9/Rock-Music-Transparent.png");
        }
    }
}